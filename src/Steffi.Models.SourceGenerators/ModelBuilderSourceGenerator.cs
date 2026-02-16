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

				// Generate SetXProperty methods for all classes with [GenerateModelBuilderSetter]
					var setterMethods = new StringBuilder();
					foreach (var typeSymbol in modelBuilderTypes.OfType<INamedTypeSymbol>())
					{
						var setterProps = GetAllMembers(typeSymbol).OfType<IPropertySymbol>()
							.Where(p => p.GetAttributes().Any(a => a.AttributeClass?.Name == "GenerateModelBuilderSetterAttribute"));
					if (!setterProps.Any()) continue;
					setterMethods.AppendLine($"        private static bool Set{typeSymbol.Name}Property(SteffiObject steffiObject, ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)");
					setterMethods.AppendLine("        {");
					setterMethods.AppendLine($"            if (steffiObject is {typeSymbol.Name} obj)");
					setterMethods.AppendLine("            {");
					foreach (var prop in setterProps)
					{
						var type = prop.Type.ToDisplayString();
						string parseExpr;
						if (type == "int" || type == "int?")
						{
							parseExpr = "int.Parse(value)";
						}
						else if (type == "bool" || type == "bool?")
						{
							parseExpr = "bool.Parse(value)";
						}
						else
						{
							parseExpr = "value.ToString()";
						}
						setterMethods.AppendLine($"                if (propertyName.Equals(nameof({typeSymbol.Name}.{prop.Name}), StringComparison.InvariantCultureIgnoreCase))");
						setterMethods.AppendLine("                {");
						setterMethods.AppendLine($"                    obj.{prop.Name} = {parseExpr};");
						setterMethods.AppendLine("                    return true;");
						setterMethods.AppendLine("                }");
					}
					setterMethods.AppendLine("            }");
					setterMethods.AppendLine("            return false;");
					setterMethods.AppendLine("        }");
				}

				var fullSource = $$"""
				{{usingDirectives}}

				#nullable enable

				namespace Steffi.Models.Builder
				{
				    public static partial class ModelBuilder
				    {
				        public static Steffi.Models.SteffiObject? CreateObjectFactory(ReadOnlySpan<char> tokenType, ReadOnlySpan<char> name, Steffi.Models.Interfaces.IParentObject parentObject) => tokenType switch
				        {
				{{switchCases}}
				            _ => null,
				        };

				{{setterMethods}}
				    }
				}

				""";

				sourceProductionContext.AddSource("ModelBuilder.SourceGen.g.cs", SourceText.From(fullSource, Encoding.UTF8));
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

	private static IEnumerable<ISymbol> GetAllMembers(INamedTypeSymbol typeSymbol)
	{
		var current = typeSymbol;
		while (current != null)
		{
			foreach (var member in current.GetMembers())
			{
				yield return member;
			}
			current = current.BaseType;
		}
	}
}