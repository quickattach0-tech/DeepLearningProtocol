# Installers Directory

This directory contains platform-specific installer scripts and configurations for Deep Learning Protocol.

## Contents

### Scripts

#### `install-windows.bat` - Windows Installer
Batch script for automated installation on Windows systems.

**Features:**
- Administrator privilege checking
- Automated directory creation
- File installation to Program Files
- Start Menu and Desktop shortcuts
- Registry entries for uninstall
- Uninstall script generation

**Usage:**
```cmd
Right-click install-windows.bat → Run as Administrator
```

**Installation Location:**
```
C:\Program Files\DeepLearningProtocol\
```

---

#### `install-linux.sh` - Linux Installer
Bash script for automated installation on Linux systems.

**Features:**
- System-wide installation to `/opt/`
- Systemd service integration
- Service user creation
- Configuration management
- Automatic service enablement
- Comprehensive logging

**Prerequisites:**
- Root or sudo access
- .NET Runtime 10.0 (optional, script will warn if missing)

**Usage:**
```bash
sudo bash install-linux.sh
```

**Installation Location:**
```
/opt/deep-learning-protocol/
/etc/deep-learning-protocol/
/var/lib/deep-learning-protocol/
/var/log/deep-learning-protocol/
```

**Service Management:**
```bash
sudo systemctl start deep-learning-protocol
sudo systemctl status deep-learning-protocol
journalctl -u deep-learning-protocol -f
```

---

#### `install-macos.sh` - macOS Installer
Bash script for automated installation on macOS systems.

**Features:**
- Application installation to `/Applications/`
- Launch agent for auto-start
- User Library directory setup
- Command-line tool creation
- Proper permissions and ownership

**Prerequisites:**
- Administrator access
- .NET Runtime 10.0

**Usage:**
```bash
sudo bash install-macos.sh
```

**Installation Location:**
```
/Applications/DeepLearningProtocol/
~/Library/Application Support/DeepLearningProtocol/
/usr/local/bin/deep-learning-protocol
```

**Service Management:**
```bash
launchctl load /Library/LaunchAgents/com.quickattach0tech.deeplearningprotocol.plist
launchctl unload /Library/LaunchAgents/com.quickattach0tech.deeplearningprotocol.plist
```

---

## WiX Project Files

### `../DeepLearningProtocol.Installer/` Directory

Contains Windows Installer XML (WiX) files for creating MSI installers:

- `DeepLearningProtocol.Installer.wixproj` - WiX project file
- `Product.wxs` - WiX source file with installer configuration

**Features:**
- Automated MSI generation
- Code signing support
- Professional Windows installer look and feel
- Upgrade path support
- Uninstall support

**Build Requirements:**
- WiX Toolset 3.11+
- Visual Studio (optional)

**To Build:**
```bash
msbuild DeepLearningProtocol.Installer.wixproj /p:Configuration=Release
```

---

## Release Package Structure

When building for release, installers are packaged as:

### Installer Bundles
- `deeplearning-protocol-installers.tar.gz` - All installers (Unix format)
- `deeplearning-protocol-installers.zip` - All installers (Windows format)

### Individual Platform Binaries (Included in Release)
- `deeplearning-protocol-linux-x64.tar.gz`
- `deeplearning-protocol-win-x64.zip`
- `deeplearning-protocol-osx-x64.tar.gz`

---

## Customization

To customize installers for your deployment:

### Windows
1. Edit `../DeepLearningProtocol.Installer/Product.wxs`
2. Modify installation directories, shortcuts, or features
3. Rebuild the WiX project

### Linux
1. Edit `install-linux.sh`
2. Modify paths, service names, or user accounts
3. Test on target distribution

### macOS
1. Edit `install-macos.sh`
2. Modify paths, launch agent, or permissions
3. Test on target macOS version

---

## Support

For installation issues or customization help:
- See [INSTALLATION_GUIDE.md](../INSTALLATION_GUIDE.md)
- See [RELEASE_DISTRIBUTION_POLICY.md](../RELEASE_DISTRIBUTION_POLICY.md)
- GitHub Issues: https://github.com/quickattach0-tech/DeepLearningProtocol/issues

---

**Version:** 3.1  
**Last Updated:** January 25, 2026
