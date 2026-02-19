# WinGet Manifest Templates

This directory contains template files for WinGet package manifests. These templates are for reference and manual submission if needed.

The automated workflow (winget-publish.yml) uses the `vedantmgoyal9/winget-releaser` action which automatically generates these manifests based on your GitHub releases.

## Manifest Structure

WinGet packages require three manifest files:

1. **version.yaml** - Main manifest with package metadata
2. **installer.yaml** - Installer-specific details (URLs, hashes, architectures)
3. **defaultLocale.yaml** - Localized package information

## Automated Generation

The workflow automatically:
- Detects version from git tags (e.g., `v1.0.0` → `1.0.0`)
- Extracts SHA256 hashes from build artifacts
- Generates installer URLs from GitHub releases
- Creates all required manifest files
- Submits a PR to microsoft/winget-pkgs

## Manual Submission

If you need to submit manually:

1. Copy the templates from this directory
2. Update `<VERSION>`, `<X64_HASH>`, and `<ARM64_HASH>` placeholders
3. Create a directory in your fork of winget-pkgs:
   ```
   manifests/m/MichalCzerwinski/Steffi/<VERSION>/
   ```
4. Place the three manifest files in that directory
5. Submit a PR to microsoft/winget-pkgs

## Validation

Before submitting, validate your manifests:

```powershell
# Install WinGet development tools
winget install Microsoft.WingetCreate

# Validate manifests
wingetcreate validate manifests/m/MichalCzerwinski/Steffi/<VERSION>/
```

## References

- [WinGet Manifest Schema](https://github.com/microsoft/winget-pkgs/tree/master/doc/manifest)
- [WinGet Submission Guidelines](https://github.com/microsoft/winget-pkgs/blob/master/AUTHORING_MANIFESTS.md)
