# WinGet Publishing Setup - Summary

This PR adds a complete automated workflow for publishing Steffi to the Windows Package Manager (WinGet).

## What's Included

### 1. Main Publishing Workflow
**File**: `.github/workflows/winget-publish.yml`

A fully automated workflow that:
- Builds AOT-compiled binaries for Windows (x64 and ARM64)
- Creates ZIP archives with SHA256 checksums
- Uploads artifacts to GitHub Releases
- Automatically submits to WinGet package repository

**Triggers**:
- Automatically on new GitHub releases
- Manually via workflow dispatch

### 2. Test Build Workflow
**File**: `.github/workflows/build-test.yml`

A validation workflow that:
- Runs on every push and PR
- Tests the AOT build process
- Runs unit tests
- Verifies executable creation
- Does NOT publish to WinGet (testing only)

### 3. Documentation

#### Quick Start Guide
**File**: `.github/workflows/QUICKSTART.md`
- Step-by-step instructions for publishing new versions
- Common workflows and use cases
- Troubleshooting guide

#### Comprehensive Documentation
**File**: `.github/workflows/WINGET_README.md`
- Detailed workflow explanation
- Setup requirements
- Configuration options
- Manual submission fallback

### 4. WinGet Manifest Templates
**Directory**: `.github/winget-manifests/`

Reference manifest files:
- `MichalCzerwinski.Steffi.yaml` - Main package manifest
- `MichalCzerwinski.Steffi.installer.yaml` - Installer configuration
- `MichalCzerwinski.Steffi.locale.en-US.yaml` - Package metadata
- `README.md` - Template usage guide

### 5. Updated Project README
**File**: `readme.md`

Added installation section with:
- WinGet installation command
- Alternative installation methods
- Links to releases

## Prerequisites to Use

### Required: GitHub Personal Access Token

To publish to WinGet, you must create a GitHub Personal Access Token and add it to repository secrets:

1. **Create Token**:
   - Go to GitHub Settings → Developer settings → Personal access tokens → Fine-grained tokens
   - Create token with permissions:
     - Contents: Read and write
     - Pull requests: Read and write

2. **Add to Repository**:
   - Go to repository Settings → Secrets and variables → Actions
   - Create secret named `WINGET_TOKEN`
   - Paste the token value

See `.github/workflows/WINGET_README.md` for detailed instructions.

## How to Publish

### Quick Method

1. Create and push a tag:
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

2. Create a GitHub release from that tag

3. The workflow automatically:
   - Builds binaries
   - Uploads to release
   - Submits to WinGet

### See `.github/workflows/QUICKSTART.md` for complete instructions

## Benefits

✅ **Automated Publishing** - No manual steps required after setup  
✅ **Multi-Architecture** - Supports x64 and ARM64 Windows  
✅ **AOT Compilation** - Fast startup, no .NET runtime required  
✅ **Checksum Validation** - SHA256 hashes for security  
✅ **Professional Distribution** - Available via `winget install`  
✅ **Version Control** - Automatic version detection from tags  
✅ **Testing Pipeline** - Validates builds before release  

## Package Information

Once published, users can install Steffi via:

```powershell
winget install MichalCzerwinski.Steffi
```

Package details:
- **Identifier**: MichalCzerwinski.Steffi
- **Type**: Portable (ZIP extraction)
- **Architectures**: x64, ARM64
- **Executable**: `steffi.exe`

## Files Changed

### Added Files
```
.github/
├── workflows/
│   ├── winget-publish.yml          (Main publishing workflow)
│   ├── build-test.yml              (Test build workflow)
│   ├── WINGET_README.md            (Detailed documentation)
│   └── QUICKSTART.md               (Quick start guide)
└── winget-manifests/
    ├── README.md                    (Templates guide)
    ├── MichalCzerwinski.Steffi.yaml
    ├── MichalCzerwinski.Steffi.installer.yaml
    └── MichalCzerwinski.Steffi.locale.en-US.yaml
```

### Modified Files
```
readme.md                            (Added installation section)
```

## Next Steps

1. **Set up WINGET_TOKEN** (see Prerequisites above)
2. **Create a test release** to validate the workflow
3. **Monitor the Actions tab** to see the workflow in action
4. **Check the WinGet PR** at https://github.com/microsoft/winget-pkgs

## Support

- **Workflow Issues**: See `.github/workflows/WINGET_README.md`
- **Publishing Help**: See `.github/workflows/QUICKSTART.md`
- **WinGet Questions**: https://github.com/microsoft/winget-pkgs

---

**Ready to publish?** See [QUICKSTART.md](.github/workflows/QUICKSTART.md) to get started!
