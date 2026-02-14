using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

[Generator]
public class ModelBuilderSourceGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterSourceOutput(
			context.CompilationProvider,
			(sourceProductionContext, compilation) =>
			{
				var source = """
				namespace Steffi.Models.Builder
				{
				    public static partial class ModelBuilder
				    {
				        public static string HelloFromSourceGenerator() => "Hello Source Generator";
				    }
				}
				""";
				sourceProductionContext.AddSource("ModelBuilder.SourceGen.g.cs", SourceText.From(source, Encoding.UTF8));
			});
	}
}
