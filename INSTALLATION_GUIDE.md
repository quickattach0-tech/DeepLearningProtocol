# Deep Learning Protocol - Installation Guide

**Version:** 3.2  
**Last Updated:** January 25, 2026

**Highlights:** This release includes a built-in SignalR server endpoint for real-time notifications and Docker runtime fixes for reliable containerization.

## SignalR Endpoint
The application exposes a SignalR Hub at `/hub/notifications` for real-time client notifications. Clients can connect via WebSocket or Server-Sent Events and call `SendNotification` to broadcast messages to all connected clients.

---

## Overview

The Deep Learning Protocol is now distributed with platform-specific installers instead of raw source code. This guide covers installation procedures for Windows, Linux, and macOS.

## System Requirements

### Windows
- Windows 10 or later
- .NET Runtime 10.0 (automatically bundled in the installer)
- 500 MB free disk space
- Administrator privileges for installation

### Linux
- Ubuntu 20.04 LTS or later, or equivalent distribution
- .NET Runtime 10.0 (can be installed separately or guided by installer)
- 500 MB free disk space
- sudo/root privileges for installation

### macOS
- macOS 12.0 (Monterey) or later
- .NET Runtime 10.0
- 500 MB free disk space
- Administrator privileges for installation

---

## Installation Methods

### Method 1: Quick Installers (Recommended)

#### Windows (Batch Script)
1. Download `deeplearning-protocol-installers.zip`
2. Extract the contents
3. Right-click `install-windows.bat` → "Run as Administrator"
4. Follow the on-screen prompts
5. The application will be installed to `C:\Program Files\DeepLearningProtocol`

#### Linux (Bash Script)
```bash
# Download and extract
wget https://github.com/quickattach0-tech/DeepLearningProtocol/releases/download/v3.2/deeplearning-protocol-installers.tar.gz
tar -xzf deeplearning-protocol-installers.tar.gz

# Run installer with sudo
sudo bash install-linux.sh

# Start the service
sudo systemctl start deep-learning-protocol
sudo systemctl status deep-learning-protocol
```

#### macOS (Bash Script)
```bash
# Download and extract
wget https://github.com/quickattach0-tech/DeepLearningProtocol/releases/download/v3.2/deeplearning-protocol-installers.tar.gz
tar -xzf deeplearning-protocol-installers.tar.gz

# Run installer
sudo bash install-macos.sh

# Load the launch agent
launchctl load /Library/LaunchAgents/com.quickattach0tech.deeplearningprotocol.plist
```

### Method 2: Self-Contained Binaries

If you prefer manual installation or have custom requirements:

1. Download the platform-specific binary archive:
   - Linux: `deeplearning-protocol-linux-x64.tar.gz`
   - Windows: `deeplearning-protocol-win-x64.zip`
   - macOS: `deeplearning-protocol-osx-x64.tar.gz`

2. Extract to your desired location

3. Run the executable:
   ```bash
   ./DeepLearningProtocol  # Linux/macOS
   DeepLearningProtocol.exe  # Windows
   ```

---

## Post-Installation Configuration

### Configuration File Location

| Platform | Location |
|----------|----------|
| Windows | `%APPDATA%\DeepLearningProtocol\config\appsettings.json` |
| Linux | `/etc/deep-learning-protocol/appsettings.json` |
| macOS | `~/Library/Application Support/DeepLearningProtocol/config/appsettings.json` |

### Data Directories

| Platform | Location |
|----------|----------|
| Windows | `%APPDATA%\DeepLearningProtocol\` |
| Linux | `/var/lib/deep-learning-protocol/` |
| macOS | `~/Library/Application Support/DeepLearningProtocol/` |

### Log Directories

| Platform | Location |
|----------|----------|
| Windows | `%APPDATA%\DeepLearningProtocol\logs\` |
| Linux | `/var/log/deep-learning-protocol/` |
| macOS | `~/Library/Application Support/DeepLearningProtocol/logs/` |

---

## Service Management

### Windows
- The application runs as a standard Windows application
- Create scheduled tasks for automated execution if needed
- Access Start Menu → Deep Learning Protocol to launch

### Linux
```bash
# Start service
sudo systemctl start deep-learning-protocol

# Stop service
sudo systemctl stop deep-learning-protocol

# Restart service
sudo systemctl restart deep-learning-protocol

# Check status
sudo systemctl status deep-learning-protocol

# View logs
journalctl -u deep-learning-protocol -f

# Enable on boot
sudo systemctl enable deep-learning-protocol
```

### macOS
```bash
# Load launch agent
launchctl load /Library/LaunchAgents/com.quickattach0tech.deeplearningprotocol.plist

# Unload launch agent
launchctl unload /Library/LaunchAgents/com.quickattach0tech.deeplearningprotocol.plist

# Check status
launchctl list | grep deeplearningprotocol

# View logs
tail -f ~/Library/Application\ Support/DeepLearningProtocol/logs/output.log
```

---

## Uninstallation

### Windows
1. Open Control Panel → Programs and Features
2. Find "Deep Learning Protocol"
3. Click "Uninstall" and follow the prompts
4. Or run: `C:\Program Files\DeepLearningProtocol\uninstall.bat`

### Linux
```bash
# Using systemctl
sudo systemctl stop deep-learning-protocol
sudo systemctl disable deep-learning-protocol
sudo systemctl daemon-reload

# Remove installation
sudo rm -rf /opt/deep-learning-protocol
sudo rm -rf /etc/deep-learning-protocol
sudo rm -rf /var/lib/deep-learning-protocol
sudo rm -rf /var/log/deep-learning-protocol
sudo rm /usr/local/bin/deep-learning-protocol
```

### macOS
```bash
# Unload launch agent
launchctl unload /Library/LaunchAgents/com.quickattach0tech.deeplearningprotocol.plist

# Remove installation
sudo rm -rf /Applications/DeepLearningProtocol
rm -rf ~/Library/Application\ Support/DeepLearningProtocol
sudo rm /usr/local/bin/deep-learning-protocol
sudo rm /Library/LaunchAgents/com.quickattach0tech.deeplearningprotocol.plist
```

---

## Troubleshooting

### .NET Runtime Not Found

**Error:** "dotnet: command not found" or "'dotnet' is not recognized"

**Solution:**
1. Install .NET Runtime 10.0 from https://dotnet.microsoft.com/download
2. Verify installation: `dotnet --version`
3. Re-run the installer

### Permission Denied (Linux/macOS)

**Error:** "Permission denied" when running installer

**Solution:**
```bash
# Ensure executable permissions
chmod +x install-linux.sh
chmod +x install-macos.sh

# Run with sudo
sudo bash install-linux.sh
```

### Service Fails to Start

**Linux:**
```bash
# Check logs
journalctl -u deep-learning-protocol -n 50

# Check service status
systemctl status deep-learning-protocol
```

**macOS:**
```bash
# Check logs
tail -f ~/Library/Application\ Support/DeepLearningProtocol/logs/error.log

# List launch agents
launchctl list | grep deeplearning
```

### Configuration Issues

1. Check the configuration file in the appropriate location
2. Ensure JSON syntax is valid
3. Verify file permissions (Linux/macOS):
   ```bash
   chmod 640 /etc/deep-learning-protocol/appsettings.json
   ```

---

## Updating

To update to a newer version:

1. **Windows:** Uninstall the current version, then run the new installer
2. **Linux:** Run the new installer with `sudo` (it will update in-place)
3. **macOS:** Run the new installer with `sudo` (it will update in-place)

Data and configuration files are preserved during updates.

---

## Support

For issues or questions:
- GitHub Issues: https://github.com/quickattach0-tech/DeepLearningProtocol/issues
- Documentation: https://github.com/quickattach0-tech/DeepLearningProtocol/docs
- Email: support@quickattach0-tech.dev

---

## License

Deep Learning Protocol is licensed under the MIT License. See LICENSE file for details.
