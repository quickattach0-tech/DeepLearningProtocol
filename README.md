# Deep Learning Protocol

> A hierarchical multi-interface reasoning system with Data Loss Prevention (DLP) capabilities.

[![CI/CD Status](https://github.com/quickattach0-tech/DeepLearningProtocol/actions/workflows/dotnet.yml/badge.svg)](https://github.com/quickattach0-tech/DeepLearningProtocol/actions/workflows/dotnet.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-blue)](https://dotnet.microsoft.com/)

---

## 🎯 Key Features

- **Hierarchical Architecture** — Multi-interface design with AbstractCore, State, Depth, and Aim layers
- **Data Loss Prevention** — Detects meme/binary content and backs up states automatically
- **Interactive Menu** — User-friendly CLI with 10-question FAQ system
- **Comprehensive Testing** — 8 XUnit tests covering all core functionality
- **Full Documentation** — Multiple guides for different user roles
- **CI/CD Automation** — Multi-platform releases via GitHub Actions

---

## 📚 Documentation

| Audience | Resource | Purpose |
|----------|----------|---------|
| **First-timers** | [Getting Started](docs/Getting-Started.md) | Build, run, and understand the basics |
| **Developers** | [Architecture Guide](docs/Architecture.md) | System design and components |
| **Security-minded** | [DLP Guide](docs/DLP-Guide.md) | Data protection deep dive |
| **Testers** | [Testing Guide](docs/Testing.md) | Writing and running tests |
| **Contributors** | [Contributing](CONTRIBUTING.md) | Development workflow & standards |
| **All** | [Full Wiki](https://github.com/quickattach0-tech/DeepLearningProtocol/wiki) | Complete reference |

---

## 🚀 Quick Start

### Prerequisites
- **.NET 10.0 SDK** or higher
- **Git** (for cloning)

### Build & Run

```bash
# Clone the repository
git clone https://github.com/quickattach0-tech/DeepLearningProtocol.git
cd DeepLearningProtocol

# Build
dotnet build

# Run
dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj

# Test (8 tests pass ✅)
dotnet test
```

**VS Code**: Press `Ctrl+Shift+B` to run, `F5` to debug.

---

## 🏗️ Architecture Overview

The protocol implements four core components:

| Component | Purpose | Responsibility |
|-----------|---------|-----------------|
| **AbstractCore** | Deepest reasoning layer | Fundamental processing logic |
| **IAimInterface** | Goal-directed processing | Strategic objectives & targets |
| **IDepthInterface** | Recursive hierarchical processing | N-level depth control |
| **IStateInterface** | State management | Current state tracking & updates |

**Plus**: `DataLossPrevention (DLP)` layer detects suspicious content and backs up states.

---

## 📦 Project Structure

```
DeepLearningProtocol/
├── DeepLearningProtocol/              Core protocol implementation
│   ├── Program.cs                     Main logic + DLP + Menu system
│   └── DeepLearningProtocol.csproj
├── DeepLearningProtocol.Tests/        Unit tests (8 tests, all passing)
├── docs/                              Comprehensive documentation
├── .github/workflows/dotnet.yml       CI/CD pipeline with multi-platform builds
├── .vscode/                           VS Code tasks & debug config
└── README.md                          This file
```

---

## 🧪 Features

### Interactive Protocol Execution
- Custom input questions
- Goal-directed processing
- Configurable depth levels (1-10)
- DLP-protected state management

### Data Loss Prevention (DLP)
Automatically detects and blocks:
- Image-like content (`.png`, `.jpg`, `base64`)
- Meme-related keywords
- Suspicious binary payloads
- State backups to `./.dlp_backups/`

### FAQ System
Browse pre-written answers about:
- How to use the protocol
- Architecture details
- DLP functionality
- Customization options

---

## 🛠️ Development

### Adding Features
```bash
# 1. Update Program.cs
# 2. Add tests to DeepLearningProtocol.Tests/DeepLearningProtocolTests.cs
# 3. Run tests
dotnet test
```

### Debugging
Press **F5** in VS Code for interactive debugging.

---

## 🔄 CI/CD Pipeline

GitHub Actions runs on every push:
- ✅ Multi-platform builds (Linux, Windows, macOS)
- ✅ Unit tests (8 tests)
- ✅ Code coverage collection
- ✅ Release artifact creation

See [`.github/workflows/dotnet.yml`](.github/workflows/dotnet.yml) for details.

---

## 📋 FAQ

**Q: What's the minimum to get started?**  
A: `git clone`, `dotnet build`, `dotnet run`. ~2 minutes total.

**Q: How do I run tests?**  
A: `dotnet test` — All tests passing ✅

**Q: Can I ask custom questions?**  
A: Yes! Select "Run Interactive Protocol" and enter your question, goal, and depth level.

**Q: What if I paste meme content?**  
A: The DLP layer detects it, backs up your state, and blocks the update.

**Q: How do I contribute?**  
A: See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines and workflow.

**For more FAQ**, see the [full wiki](https://github.com/quickattach0-tech/DeepLearningProtocol/wiki).

---

## 🤝 Contributing

We welcome contributions! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for:
- Code style guidelines
- Testing requirements
- Pull request workflow
- Commit message conventions

---

## 📄 License

This project is licensed under the MIT License — see [LICENSE](LICENSE) for details.

---

## 🔗 Links

- **[Full Wiki](https://github.com/quickattach0-tech/DeepLearningProtocol/wiki)** — Complete reference
- **[Issues](https://github.com/quickattach0-tech/DeepLearningProtocol/issues)** — Bug reports & feature requests
- **[Actions](https://github.com/quickattach0-tech/DeepLearningProtocol/actions)** — CI/CD pipeline status
- **[Releases](https://github.com/quickattach0-tech/DeepLearningProtocol/releases)** — Pre-built binaries

---

**Last Updated**: January 25, 2026 | **Status**: Production Ready | **Maintained by**: [@quickattach0-tech](https://github.com/quickattach0-tech)
