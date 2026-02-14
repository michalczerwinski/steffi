# Test Naming Convention

## Test Structure

### Parser Tests (SteffiParserTests.cs)
Tests that validate parsing behavior using inline strings.

**Naming Pattern:**
- Method: `Compiles{Category}{NN}` or `Fails{Category}{NN}`
- Use `DisplayName` attribute to describe the test case
- Examples:
  - `CompilesCase01` - "Compiles comments whitespace and single object without name"
  - `FailsCase01` - "Fails when nested closing missing"

### Output Generation Tests (RenderingTests.cs)
Tests that validate end-to-end SVG rendering from STF files.

**Naming Pattern:**
- Method: `Renders{Description}` - e.g., `RendersBasicCanvasWithShapes`
- Test files: `{TestCategory}{NN}.{type}.{ext}`
  - Input: `SvgGeneration01.input.stf`
  - Expected: `SvgGeneration01.expected.svg`

**Numbering Convention:**
- Use two-digit numbers: 01, 02, 03, etc.
- Numbers are sequential within each category
- Allows for up to 99 test cases per category

## File Organization

```
Steffi.UnitTests/
├── SteffiParserTests.cs           # Inline string-based parsing tests
├── SteffiParserTestsBase.cs       # Shared test utilities
├── RenderingTests.cs          # File-based SVG rendering tests
├── RenderingTests/
│   ├── SvgGeneration01.input.stf      # Test input file
│   ├── SvgGeneration01.expected.svg   # Expected output file
│   ├── SvgGeneration02.input.stf      # Next test case...
│   └── ...
└── TestNamingConvention.md
```

## Adding New Tests

### Parser Test (inline)
```csharp
[Test, DisplayName("Your test description")]
public async Task CompilesCase{NN}() => await CompilesWithoutError(
    """
    Your STF code here
    """);
```

### SVG Generation Test (file-based)
1. Create input file: `RenderingTests\SvgGeneration{NN}.input.stf`
2. Create expected output: `RenderingTests\SvgGeneration{NN}.expected.svg`
3. Add test method:
```csharp
[Test, DisplayName("Your test description")]
public async Task Renders{Description}() => await RendersSvgCorrectly("SvgGeneration{NN}");
```

The project file uses wildcards to automatically copy all `RenderingTests\*.input.stf` and `RenderingTests\*.expected.svg` files to the output directory.
