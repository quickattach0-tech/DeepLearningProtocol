# Release Distribution Policy - v3.2

**Effective Date:** January 25, 2026  
**Version:** 3.1  
**Status:** Active

---

## Summary of Changes

As of version 3.1, Deep Learning Protocol releases now distribute **installers** instead of source code. This change improves user experience, security, and maintainability.

## What Changed

### Previous Release Distribution (v1.2.0)
- GitHub releases contained compiled binaries
- Source code was publicly available via git
- Users needed to build from source for some platforms
- No standardized installation paths

### New Release Distribution (v3.2+)
- GitHub releases contain **platform-specific installers**
- **SignalR & Docker:** Adds a built-in SignalR server endpoint (`/hub/notifications`) and Docker runtime updates (ASP.NET runtime, port 80 exposed)
- Self-contained binaries for each platform
- Automated installation with system integration
- Standardized installation directories and service management
- Source code remains available in the repository

## Release Contents

Each release now includes:

### Installers (Primary Distribution)
1. **Windows Installer** (`install-windows.bat`)
   - Automated installation to Program Files
   - Start Menu and Desktop shortcuts
   - Registry entries for uninstall
   - Admin privileges required

2. **Linux Installer** (`install-linux.sh`)
   - System-wide installation to `/opt/`
   - Systemd service integration
   - User account creation
   - Configuration in `/etc/`

3. **macOS Installer** (`install-macos.sh`)
   - Application installation to `/Applications/`
   - Launch agent for auto-start
   - User Library directories
   - Command-line tool in `/usr/local/bin/`

### Binary Archives (Backup)
- `deeplearning-protocol-linux-x64.tar.gz`
- `deeplearning-protocol-win-x64.zip`
- `deeplearning-protocol-osx-x64.tar.gz`
- `deeplearning-protocol-framework-dependent.tar.gz`

For users who prefer manual installation without the installer scripts.

## Installation Paths

### Windows
- **Installation:** `C:\Program Files\DeepLearningProtocol\`
- **Config:** `%APPDATA%\DeepLearningProtocol\config\`
- **Data:** `%APPDATA%\DeepLearningProtocol\`
- **Logs:** `%APPDATA%\DeepLearningProtocol\logs\`

### Linux
- **Installation:** `/opt/deep-learning-protocol/`
- **Config:** `/etc/deep-learning-protocol/`
- **Data:** `/var/lib/deep-learning-protocol/`
- **Logs:** `/var/log/deep-learning-protocol/`
- **Service:** `deep-learning-protocol.service`
- **Binary Link:** `/usr/local/bin/deep-learning-protocol`

### macOS
- **Installation:** `/Applications/DeepLearningProtocol/`
- **Config:** `~/Library/Application Support/DeepLearningProtocol/config/`
- **Data:** `~/Library/Application Support/DeepLearningProtocol/`
- **Logs:** `~/Library/Application Support/DeepLearningProtocol/logs/`
- **Launch Agent:** `com.quickattach0tech.deeplearningprotocol.plist`
- **Binary Link:** `/usr/local/bin/deep-learning-protocol`

## Benefits

### For Users
✅ One-click installation  
✅ Automatic system integration  
✅ Service management support  
✅ Standardized locations  
✅ Easy uninstallation  
✅ No build tools required  

### For Development
✅ Cleaner release artifacts  
✅ Better version management  
✅ Easier updates/upgrades  
✅ Consistent deployment across platforms  
✅ Source code separation from releases  

## Backward Compatibility

- Source code remains available in the GitHub repository
- Users can still build from source if needed
- Legacy binary packages available for older versions
- No breaking changes to application functionality

## Source Code Access

The source code is **always available** in the GitHub repository:
- Main branch: https://github.com/quickattach0-tech/DeepLearningProtocol
- Release tags: https://github.com/quickattach0-tech/DeepLearningProtocol/releases
- Build instructions: See [Getting-Started.md](./docs/Getting-Started.md)

Users can still clone and build the project:
```bash
git clone https://github.com/quickattach0-tech/DeepLearningProtocol.git
cd DeepLearningProtocol
dotnet build
dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj
```

## Installation Methods (Priority Order)

### Recommended: Use Installers
```bash
# Windows
install-windows.bat  # Run as Administrator

# Linux
sudo bash install-linux.sh

# macOS
sudo bash install-macos.sh
```

### Alternative: Extract Binaries
For advanced users who prefer manual setup, use the self-contained binary archives.

### Advanced: Build from Source
For developers and customization:
```bash
git clone <repo>
dotnet publish -c Release -r linux-x64 --self-contained
```

## Migration from Previous Versions

Users upgrading from v1.2.0 to v3.2:

1. **Uninstall** the previous version (optional)
2. **Download** the v3.2 installer
3. **Run** the installer for your platform
4. **Configuration** will be in the new standard location
5. **Data** can be migrated if needed

Installers will not overwrite existing data unless specifically configured.

## Support for Multiple Platforms

The release distribution strategy ensures:
- **Single codebase** builds for multiple platforms
- **Consistent behavior** across Windows, Linux, macOS
- **Platform-specific integration** (services, shortcuts, etc.)
- **Unified documentation** for all platforms

## Future Enhancements

Planned improvements:
- Signed installers with code signing certificates
- Automatic update checking and installation
- GUI installer for Windows (WiX)
- Package manager support (apt, brew, choco)
- Docker container distribution
- Cloud deployment options

## Policy Changes

| Aspect | v1.2.0 | v3.2+ |
|--------|--------|-------|
| Release Format | Source/Binaries | Installers |
| Install Location | User-defined | Standard |
| Service Integration | Manual | Automatic |
| Configuration | Application dir | Standard location |
| Uninstall | Manual | Automated |
| Update Process | Manual rebuild | Installer update |

## Questions?

See [INSTALLATION_GUIDE.md](./INSTALLATION_GUIDE.md) for detailed installation instructions.

For issues: https://github.com/quickattach0-tech/DeepLearningProtocol/issues
