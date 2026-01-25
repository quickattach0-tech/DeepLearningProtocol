#!/bin/bash

# Deep Learning Protocol Linux Installer Script
# Version: 3.1
# This script installs Deep Learning Protocol on Linux systems

set -e

VERSION="3.2"
INSTALL_DIR="/opt/deep-learning-protocol"
BIN_DIR="$INSTALL_DIR/bin"
DATA_DIR="/var/lib/deep-learning-protocol"
CONFIG_DIR="/etc/deep-learning-protocol"
LOG_DIR="/var/log/deep-learning-protocol"
SYSTEMD_SERVICE="/etc/systemd/system/deep-learning-protocol.service"
SHORTCUT_DIR="/usr/share/applications"

# Color codes for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Check if running as root
if [ "$EUID" -ne 0 ]; then 
  echo -e "${RED}This installer must be run as root (use sudo)${NC}"
  exit 1
fi

# Check system requirements
check_requirements() {
  echo "Checking system requirements..."
  
  if ! command -v dotnet &> /dev/null; then
    echo -e "${YELLOW}Warning: .NET Runtime 10.0 is not installed.${NC}"
    echo "Please install .NET Runtime 10.0 from https://dotnet.microsoft.com/download"
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
  mkdir -p "$DATA_DIR"
  mkdir -p "$CONFIG_DIR"
  mkdir -p "$LOG_DIR"
  echo -e "${GREEN}✓ Directories created${NC}"
}

# Install application files
install_files() {
  echo "Installing application files..."
  
  # Copy binary and dependencies
  cp -r bin/Release/net10.0/linux-x64/publish/* "$BIN_DIR/"
  
  # Set executable permissions
  chmod +x "$BIN_DIR/DeepLearningProtocol"
  
  # Copy configuration file if exists
  if [ -f "appsettings.json" ]; then
    cp appsettings.json "$CONFIG_DIR/appsettings.json"
    chmod 640 "$CONFIG_DIR/appsettings.json"
  fi
  
  # Create symlink in /usr/local/bin
  ln -sf "$BIN_DIR/DeepLearningProtocol" /usr/local/bin/deep-learning-protocol
  
  echo -e "${GREEN}✓ Application files installed${NC}"
}

# Create systemd service file
create_systemd_service() {
  echo "Creating systemd service..."
  
  cat > "$SYSTEMD_SERVICE" << 'EOF'
[Unit]
Description=Deep Learning Protocol Service
After=network.target

[Service]
Type=simple
User=deeplearning
Group=deeplearning
WorkingDirectory=/var/lib/deep-learning-protocol
ExecStart=/opt/deep-learning-protocol/bin/DeepLearningProtocol
Restart=on-failure
RestartSec=10

Environment="DOTNET_EnableDiagnostics=0"
StandardOutput=journal
StandardError=journal

[Install]
WantedBy=multi-user.target
EOF
  
  chmod 644 "$SYSTEMD_SERVICE"
  echo -e "${GREEN}✓ Systemd service created${NC}"
}

# Create application launcher
create_launcher() {
  echo "Creating application launcher..."
  
  cat > "$SHORTCUT_DIR/deep-learning-protocol.desktop" << 'EOF'
[Desktop Entry]
Version=1.0
Type=Application
Name=Deep Learning Protocol
Comment=A hierarchical multi-interface reasoning system
Exec=deep-learning-protocol
Icon=application-x-executable
Terminal=true
Categories=Development;Utility;
EOF
  
  chmod 644 "$SHORTCUT_DIR/deep-learning-protocol.desktop"
  echo -e "${GREEN}✓ Application launcher created${NC}"
}

# Create user and group
create_user() {
  echo "Creating service user..."
  
  if ! id "deeplearning" &>/dev/null; then
    useradd -r -s /bin/false -d "$DATA_DIR" deeplearning || true
    echo -e "${GREEN}✓ Service user created${NC}"
  else
    echo -e "${YELLOW}Service user 'deeplearning' already exists${NC}"
  fi
  
  # Set proper permissions
  chown -R deeplearning:deeplearning "$DATA_DIR"
  chown -R deeplearning:deeplearning "$LOG_DIR"
  chown -R deeplearning:deeplearning "$CONFIG_DIR"
  chmod 750 "$DATA_DIR"
  chmod 750 "$LOG_DIR"
  chmod 750 "$CONFIG_DIR"
}

# Register and enable service
register_service() {
  echo "Registering systemd service..."
  
  systemctl daemon-reload
  systemctl enable deep-learning-protocol.service
  
  echo -e "${GREEN}✓ Service registered and enabled${NC}"
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
  echo "Service Management:"
  echo "  Start service:   sudo systemctl start deep-learning-protocol"
  echo "  Stop service:    sudo systemctl stop deep-learning-protocol"
  echo "  Restart service: sudo systemctl restart deep-learning-protocol"
  echo "  View logs:       journalctl -u deep-learning-protocol -f"
  echo ""
  echo "Command Line:"
  echo "  Run: deep-learning-protocol"
  echo ""
  echo "Next Steps:"
  echo "  1. Configure the application in $CONFIG_DIR/appsettings.json"
  echo "  2. Start the service: sudo systemctl start deep-learning-protocol"
  echo ""
}

# Main installation flow
main() {
  echo "Deep Learning Protocol Linux Installer v$VERSION"
  echo "=================================================="
  echo ""
  
  check_requirements
  create_directories
  install_files
  create_user
  create_systemd_service
  register_service
  create_launcher
  
  show_summary
}

# Run installation
main
