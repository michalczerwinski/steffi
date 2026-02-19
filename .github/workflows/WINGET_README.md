# WinGet Publishing Workflow

This workflow automatically publishes AOT (Ahead-of-Time) compiled binaries of Steffi to WinGet (Windows Package Manager).

## Overview

The workflow performs the following tasks:

1. **Build AOT Binaries**: Compiles Steffi for Windows x64 and ARM64 architectures using .NET AOT compilation
2. **Create Release Artifacts**: Packages binaries into ZIP archives with SHA256 checksums
3. **Upload to GitHub Release**: Attaches the artifacts to the GitHub release
4. **Submit to WinGet**: Automatically creates a PR to the winget-pkgs repository

## Trigger Conditions

The workflow runs in two scenarios:

### 1. Automatic on Release
When you create a new GitHub release:
```bash
# Create and push a tag
git tag v1.0.0
git push origin v1.0.0

# Then create a release on GitHub from that tag
```

### 2. Manual Dispatch
You can manually trigger the workflow from the Actions tab with a specific version:
1. Go to Actions → "Publish to WinGet"
2. Click "Run workflow"
3. Enter the version (e.g., `1.0.0`)

## Setup Requirements

### 1. WinGet Token (Required)

To submit packages to WinGet, you need to set up a GitHub Personal Access Token:

1. **Create a Personal Access Token**:
   - Go to GitHub Settings → Developer settings → Personal access tokens → Fine-grained tokens
   - Click "Generate new token"
   - Set permissions:
     - Repository access: Public repositories (or specific repo)
     - Permissions: `Contents: Read and write`, `Pull requests: Read and write`
   - Generate and copy the token

2. **Add Token to Repository Secrets**:
   - Go to your repository → Settings → Secrets and variables → Actions
   - Click "New repository secret"
   - Name: `WINGET_TOKEN`
   - Value: Paste the token you created
   - Click "Add secret"

### 2. .NET 10 SDK

The workflow uses .NET 10 with preview quality. Ensure your project is compatible:
- Target framework: `net10.0`
- AOT compilation enabled: `<PublishAot>true</PublishAot>`

## Build Configurations

The workflow builds for two architectures:

- **win-x64**: Windows 64-bit (Intel/AMD)
- **win-arm64**: Windows ARM64 (for ARM-based Windows devices)

Each build produces:
- A ZIP archive containing the AOT-compiled executable
- A SHA256 checksum file for verification

## WinGet Package Details

- **Package Identifier**: `MichalCzerwinski.Steffi`
- **Package Type**: Portable (ZIP extraction)
- **Architectures**: x64, ARM64

## Output Artifacts

For version `1.0.0`, the workflow creates:
- `steffi-1.0.0-win-x64.zip`
- `steffi-1.0.0-win-x64.zip.sha256`
- `steffi-1.0.0-win-arm64.zip`
- `steffi-1.0.0-win-arm64.zip.sha256`

## Installation (End Users)

Once published to WinGet, users can install Steffi using:

```powershell
winget install MichalCzerwinski.Steffi
```

Or search for it:

```powershell
winget search steffi
```

## Troubleshooting

### Workflow Fails at "Submit to WinGet"

**Issue**: Missing or invalid `WINGET_TOKEN`

**Solution**: 
1. Verify the token exists in repository secrets
2. Ensure the token has the correct permissions
3. Check if the token has expired

### Build Fails for .NET 10

**Issue**: .NET 10 SDK not available

**Solution**: 
- The workflow uses `dotnet-quality: 'preview'` to access preview versions
- Ensure the .NET 10 SDK is available in the setup-dotnet action
- You may need to update the `dotnet-version` if the version changes

### WinGet PR Rejected

**Issue**: Manifest validation fails

**Solution**:
- Check the WinGet PR for validation errors
- Common issues:
  - Incorrect package identifier format
  - Missing required manifest fields
  - Invalid SHA256 hashes
  - URL accessibility issues

## Manual WinGet Submission

If the automated workflow fails, you can submit manually:

1. Download the release artifacts
2. Fork the [winget-pkgs](https://github.com/microsoft/winget-pkgs) repository
3. Create manifest files in `manifests/m/MichalCzerwinski/Steffi/<version>/`
4. Submit a PR to the winget-pkgs repository

Refer to the [WinGet documentation](https://github.com/microsoft/winget-pkgs#contributing) for detailed instructions.

## Version Naming Convention

- Git tags should use semantic versioning with `v` prefix: `v1.0.0`, `v1.2.3`, etc.
- The workflow automatically strips the `v` prefix for WinGet package versions
- WinGet package versions follow semantic versioning: `1.0.0`, `1.2.3`, etc.

## Workflow Permissions

The workflow requires the following permissions:
- `contents: write` - To upload release artifacts
- `pull-requests: write` - To create PRs in the winget-pkgs repository (via token)

## Additional Resources

- [WinGet Package Manager](https://github.com/microsoft/winget-cli)
- [WinGet Package Repository](https://github.com/microsoft/winget-pkgs)
- [.NET AOT Compilation](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/)
- [winget-releaser Action](https://github.com/vedantmgoyal9/winget-releaser)
