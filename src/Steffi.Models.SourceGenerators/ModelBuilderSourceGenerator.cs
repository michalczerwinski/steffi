using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Steffi.Models.SourceGenerators;

[Generator]
public class ModelBuilderSourceGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterSourceOutput(
			context.CompilationProvider,
			(sourceProductionContext, compilation) =>
			{
				List<ISymbol> modelBuilderTypes = GetModelBuilderTypes(compilation);

				var usingDirectives = string.Join("\n", modelBuilderTypes
					.Select(type => type.ContainingNamespace?.ToDisplayString())
					.Distinct()
					.Where(ns => !string.IsNullOrEmpty(ns))
					.Select(ns => $"using {ns};"));

				var switchCases = string.Join("\n", modelBuilderTypes.Select(typeName =>
					$"			nameof({typeName}) => new {typeName} {{ Name = name.ToString(), Parent = parentObject, ParentProperties = parentObject.CreateContainerProperties() }},"));

				var source = $$"""
				{{usingDirectives}}
				namespace Steffi.Models.Builder
				{
				    public static partial class ModelBuilder
				    {
				        public static Steffi.Models.SteffiObject? CreateObjectFactory(ReadOnlySpan<char> tokenType, ReadOnlySpan<char> name, Steffi.Models.Interfaces.IParentObject parentObject) => tokenType switch
				        {
				{{switchCases}}
				            _ => null,
				        };
				    }
				}
				""";

				sourceProductionContext.AddSource("ModelBuilder.SourceGen.g.cs", SourceText.From(source, Encoding.UTF8));
			});
	}

	private static List<ISymbol> GetModelBuilderTypes(Compilation compilation)
	{
		var modelBuilderTypes = new List<ISymbol>();

		foreach (var tree in compilation.SyntaxTrees)
		{
			var semanticModel = compilation.GetSemanticModel(tree);
			var classes = tree.GetRoot().DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax>();

			foreach (var @class in classes)
			{
				var symbol = semanticModel.GetDeclaredSymbol(@class);

				if (symbol == null) continue;
				foreach (var attr in symbol.GetAttributes())
				{
					if (attr.AttributeClass?.Name == "GenerateModelBuilderAttribute")
					{
						modelBuilderTypes.Add(symbol);
						break;
					}
				}
			}
		}

		return modelBuilderTypes;
	}
}