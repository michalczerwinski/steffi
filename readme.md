# Steffi

Steffi is a modern graph visualization and analysis toolkit built around a small domain-specific language (DSL). Describe documents in `.stf` files, parse them into strongly typed objects, and render them in the console using Spectre.Console panels.

## Highlights

- **Expressive DSL** – Model graphs, nodes, and nested structures with a clean, comment-friendly syntax.
- **Robust parsing pipeline** – Lexer and parser layers are designed for extension, so you can evolve the language safely.
- **Rich console UX** – The CLI uses Spectre.Console to render color-coded panels for documents, graphs, and nodes.
- **Testable core** – A dedicated unit test suite exercises every parsing rule and regression scenario.

## Development

### Running the CLI
Parse a document and render its structure:
```bash
dotnet run --project .\src\Steffi.Cli\Steffi.Cli.csproj -- structure .\samples\simple.stf
```

For iterative development, keep the command hot-reloading:
```bash
dotnet watch run --project .\src\Steffi.Cli\Steffi.Cli.csproj -- structure .\samples\simple.stf
```

### Running tests
```bash
cd .\src\Steffi.UnitTests\
dotnet watch test
```
