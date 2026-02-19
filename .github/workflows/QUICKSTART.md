# Quick Start Guide: Publishing to WinGet

This guide helps you quickly publish a new version of Steffi to WinGet.

## Prerequisites

Before your first release, ensure you have:

1. **GitHub Personal Access Token** set up as a repository secret named `WINGET_TOKEN`
   - See [WINGET_README.md](WINGET_README.md#1-winget-token-required) for detailed setup instructions

## Publishing a New Version

### Method 1: Create a GitHub Release (Recommended)

1. **Tag the version locally**:
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

2. **Create a GitHub Release**:
   - Go to https://github.com/michalczerwinski/steffi/releases/new
   - Select the tag you just created (v1.0.0)
   - Fill in the release title and description
   - Click "Publish release"

3. **Automatic Processing**:
   - The workflow automatically triggers when you publish the release
   - It builds AOT binaries for Windows x64 and ARM64
   - Uploads artifacts to the GitHub release
   - Submits a PR to the WinGet package repository

4. **Monitor Progress**:
   - Go to the Actions tab to see the workflow progress
   - The workflow creates two jobs:
     - `build-aot`: Builds and uploads binaries
     - `publish-winget`: Submits to WinGet

### Method 2: Manual Workflow Dispatch

If you need to publish without creating a release:

1. **Go to Actions tab**: https://github.com/michalczerwinski/steffi/actions
2. **Select "Publish to WinGet"** workflow
3. **Click "Run workflow"**
4. **Enter version** (e.g., `1.0.0` without the `v` prefix)
5. **Click "Run workflow"** button

Note: This method builds the binaries but does not create a GitHub release automatically.

## After Publishing

### Check WinGet PR

1. The workflow creates a PR in https://github.com/microsoft/winget-pkgs
2. The PR is automatically validated by WinGet's CI
3. If validation passes, a WinGet maintainer will review and merge
4. Once merged, your package is available via `winget install MichalCzerwinski.Steffi`

### Common Wait Times

- **Build**: 5-10 minutes
- **WinGet PR creation**: Immediate after build
- **WinGet validation**: 1-5 minutes
- **WinGet maintainer review**: Hours to days (varies)

## Testing Locally

Before publishing, you can test the AOT build locally:

```bash
# Build for Windows x64
dotnet publish src/Steffi.Cli/Steffi.Cli.csproj `
  -c Release `
  -r win-x64 `
  --self-contained true `
  -p:PublishAot=true `
  -o ./publish/win-x64

# Test the executable
./publish/win-x64/steffi.exe --help
```

## Troubleshooting

### Build Fails

- **Check .NET version**: Ensure .NET 10 SDK preview is available
- **Check project file**: Verify `PublishAot` is set to `true`
- **Review logs**: Go to Actions tab and check the failed job logs

### WinGet PR Fails Validation

Common issues:
- **Invalid SHA256**: Ensure the hash matches the actual file
- **URL not accessible**: GitHub release must be public
- **Version format**: Use semantic versioning (1.0.0, not v1.0.0)

### No WINGET_TOKEN

If you see "Error: Input required and not supplied: token":
1. Create a GitHub Personal Access Token
2. Add it to repository secrets as `WINGET_TOKEN`
3. Re-run the workflow

## Version Numbering

Follow [Semantic Versioning](https://semver.org/):
- **Major**: Breaking changes (1.0.0 → 2.0.0)
- **Minor**: New features, backward compatible (1.0.0 → 1.1.0)
- **Patch**: Bug fixes (1.0.0 → 1.0.1)

Examples:
- `v1.0.0` - First stable release
- `v1.1.0` - Added new shape types
- `v1.1.1` - Fixed rendering bug
- `v2.0.0` - Breaking DSL syntax change

## Release Checklist

- [ ] Update version in code if needed
- [ ] Run tests: `dotnet test src/Steffi.UnitTests/`
- [ ] Test AOT build locally
- [ ] Write release notes
- [ ] Create and push tag
- [ ] Create GitHub release
- [ ] Monitor workflow in Actions tab
- [ ] Verify artifacts uploaded to release
- [ ] Check WinGet PR status
- [ ] Test installation: `winget install MichalCzerwinski.Steffi` (after merge)

## Getting Help

- **Workflow Issues**: Check [WINGET_README.md](WINGET_README.md)
- **WinGet Issues**: See [microsoft/winget-pkgs](https://github.com/microsoft/winget-pkgs)
- **Steffi Issues**: Open an issue at [steffi/issues](https://github.com/michalczerwinski/steffi/issues)
