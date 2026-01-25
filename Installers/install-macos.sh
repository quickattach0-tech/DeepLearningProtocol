#!/bin/bash

# Deep Learning Protocol macOS Installer Script
# Version: 3.1
# This script installs Deep Learning Protocol on macOS systems

set -e

VERSION="3.2"
INSTALL_DIR="/Applications/DeepLearningProtocol"
BIN_DIR="$INSTALL_DIR/bin"
DATA_DIR="$HOME/Library/Application Support/DeepLearningProtocol"
CONFIG_DIR="$DATA_DIR/config"
LOG_DIR="$DATA_DIR/logs"
LAUNCH_AGENT="/Library/LaunchAgents/com.quickattach0tech.deeplearningprotocol.plist"

# Color codes for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Check system requirements
check_requirements() {
  echo "Checking system requirements..."
  
  if ! command -v dotnet &> /dev/null; then
    echo -e "${YELLOW}Warning: .NET Runtime is not installed.${NC}"
    echo "Please install .NET Runtime from https://dotnet.microsoft.com/download/dotnet/latest/runtime"
    read -p "Continue anyway? (y/n) " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
      exit 1
    fi
  else
    DOTNET_VERSION=$(dotnet --version)
    echo -e "${GREEN}✓ .NET Runtime found: $DOTNET_VERSION${NC}"
  fi
}

# Create necessary directories
create_directories() {
  echo "Creating directories..."
  mkdir -p "$BIN_DIR"
  mkdir -p "$CONFIG_DIR"
  mkdir -p "$LOG_DIR"
  echo -e "${GREEN}✓ Directories created${NC}"
}

# Install application files
install_files() {
  echo "Installing application files..."
  
  # Copy binary and dependencies
  cp -r bin/Release/net10.0/osx-x64/publish/* "$BIN_DIR/"
  
  # Set executable permissions
  chmod +x "$BIN_DIR/DeepLearningProtocol"
  
  # Copy configuration file if exists
  if [ -f "appsettings.json" ]; then
    cp appsettings.json "$CONFIG_DIR/appsettings.json"
    chmod 640 "$CONFIG_DIR/appsettings.json"
  fi
  
  # Create symlink in /usr/local/bin
  sudo ln -sf "$BIN_DIR/DeepLearningProtocol" /usr/local/bin/deep-learning-protocol
  
  echo -e "${GREEN}✓ Application files installed${NC}"
}

# Create launch agent plist
create_launch_agent() {
  echo "Creating launch agent..."
  
  sudo tee "$LAUNCH_AGENT" > /dev/null << EOF
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>Label</key>
    <string>com.quickattach0tech.deeplearningprotocol</string>
    <key>Program</key>
    <string>$BIN_DIR/DeepLearningProtocol</string>
    <key>RunAtLoad</key>
    <true/>
    <key>KeepAlive</key>
    <true/>
    <key>StandardOutPath</key>
    <string>$LOG_DIR/output.log</string>
    <key>StandardErrorPath</key>
    <string>$LOG_DIR/error.log</string>
    <key>WorkingDirectory</key>
    <string>$DATA_DIR</string>
</dict>
</plist>
EOF
  
  sudo chmod 644 "$LAUNCH_AGENT"
  echo -e "${GREEN}✓ Launch agent created${NC}"
}

# Create application icon
create_icon() {
  echo "Creating application icon directory..."

  ICONS_DIR="$INSTALL_DIR/Icon.iconset"
  mkdir -p "$ICONS_DIR"

  # Add README with guidance on placing icon assets
  cat > "$ICONS_DIR/README.md" << 'EOF'
# Icon installation
Place a valid `Icon.icns` file here to provide an application icon.
You can create an `.icns` from PNG assets on macOS using:

  mkdir -p Icon.iconset
  sips -z 16 16     icon-16.png --out Icon.iconset/icon_16x16.png
  sips -z 32 32     icon-16@2x.png --out Icon.iconset/icon_16x16@2x.png
  sips -z 32 32     icon-32.png --out Icon.iconset/icon_32x32.png
  sips -z 64 64     icon-32@2x.png --out Icon.iconset/icon_32x32@2x.png
  sips -z 128 128   icon-128.png --out Icon.iconset/icon_128x128.png
  sips -z 256 256   icon-128@2x.png --out Icon.iconset/icon_128x128@2x.png
  iconutil -c icns Icon.iconset

Copy the resulting `Icon.icns` to this directory.
EOF

  echo -e "${GREEN}✓ Application icon directory created (place Icon.icns in $ICONS_DIR)${NC}"
}

# Display installation summary
show_summary() {
  echo ""
  echo -e "${GREEN}================================${NC}"
  echo -e "${GREEN}Deep Learning Protocol v$VERSION${NC}"
  echo -e "${GREEN}Installation Complete!${NC}"
  echo -e "${GREEN}================================${NC}"
  echo ""
  echo "Installation Details:"
  echo "  Install Directory: $INSTALL_DIR"
  echo "  Binary Location: $BIN_DIR/DeepLearningProtocol"
  echo "  Config Directory: $CONFIG_DIR"
  echo "  Data Directory: $DATA_DIR"
  echo "  Log Directory: $LOG_DIR"
  echo ""
  echo "Launch Agent:"
  echo "  Location: $LAUNCH_AGENT"
  echo "  Start: launchctl load $LAUNCH_AGENT"
  echo "  Stop: launchctl unload $LAUNCH_AGENT"
  echo "  Status: launchctl list | grep deeplearningprotocol"
  echo ""
  echo "Command Line:"
  echo "  Run: deep-learning-protocol"
  echo ""
  echo "Next Steps:"
  echo "  1. Configure the application in $CONFIG_DIR/appsettings.json"
  echo "  2. Load the launch agent: launchctl load $LAUNCH_AGENT"
  echo ""
}

# Main installation flow
main() {
  echo "Deep Learning Protocol macOS Installer v$VERSION"
  echo "=================================================="
  echo ""
  
  check_requirements
  create_directories
  install_files
  create_launch_agent
  create_icon
  
  show_summary
}

# Run installation
main
