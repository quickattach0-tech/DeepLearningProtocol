# Deep Learning Protocol

A conceptual C# implementation inspired by a hierarchical deep learning architecture with **Data Loss Prevention (DLP)** capabilities.

[![CI/CD Status](https://github.com/quickattach0-tech/DeepLearningProtocol/actions/workflows/dotnet.yml/badge.svg)](https://github.com/quickattach0-tech/DeepLearningProtocol/actions/workflows/dotnet.yml)

---

## 📖 Quick Navigation

- **[Getting Started](#quick-start)** — Build & run in 2 minutes
- **[Architecture](#architecture)** — System design overview
- **[Features](#features)** — Core capabilities
- **[FAQ](#faq)** — Common questions
- **[Wiki](https://github.com/quickattach0-tech/DeepLearningProtocol/wiki)** — Full documentation
- **[Contributing](CONTRIBUTING.md)** — How to contribute

---

## Architecture

The protocol implements a multi-layered learning system with four core interfaces:

- **AbstractCore**: The deepest reasoning layer (red/maroon node) — fundamental processing logic
- **IStateInterface**: Top-level state management and retrieval
- **IDepthInterface**: Hierarchical depth-based processing with recursive layers
- **IAimInterface**: Goal-directed processing with spiral-like exploration paths
- **DataLossPrevention (DLP)**: Meme and binary content detection with automatic state backups

## Project Structure

```
DeepLearningProtocol/
├── DeepLearningProtocol/              # Main executable project
│   ├── Program.cs                     # Core protocol + DLP + Menu system
│   └── DeepLearningProtocol.csproj
├── DeepLearningProtocol.Tests/        # XUnit test suite (7 tests)
│   └── DeepLearningProtocolTests.cs
├── DeepLearningProtocol.sln           # Solution file
├── .github/workflows/                 # CI/CD automation
│   └── dotnet.yml                     # GitHub Actions workflow
├── .vscode/
│   ├── tasks.json                     # Build/run/test tasks
│   └── launch.json                    # F5 debug configuration
├── docs/                              # Wiki documentation
├── .gitignore                         # Build artifacts
└── README.md                          # This file
```

## Quick Start

### Prerequisites
- .NET 10.0 SDK or higher
- Git (for cloning)
- Visual Studio Code (optional, for enhanced development)

### Build

```bash
dotnet build
```

### Run

**Option 1: VS Code (Recommended)**
```bash
# Press Ctrl+Shift+B to run the default task
```

**Option 2: CLI**
```bash
dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj
```

### Run Tests

```bash
dotnet test
```

All tests should pass ✅

## Features

### Interactive Menu System
- **Run Protocol**: Execute the deep learning protocol with custom inputs
- **View FAQ**: Browse 10 comprehensive answers about the system
- **ASCII UI**: Professional menu interface with clear navigation

### Data Loss Prevention (DLP)

The `DataLossPrevention` class monitors state updates and:

1. **Detects meme-like content** — checks for:
   - File extensions (`.png`, `.jpg`, `.jpeg`)
   - Image data URIs (`data:image/`, `base64,`)
   - Keywords ("meme")
   - Large single-line payloads (>200 chars, no newlines)

2. **Backs up state** — saves previous states to `./.dlp_backups/` with timestamps
3. **Blocks risky updates** — prevents accidental data loss by setting state to `[DLP-BLOCKED]`

### Example Output

```
Initial state: Initial
State updated: Aiming: Solve complex problem
State updated: Depth 5 processed

Result:
[Aim Pursuit] [Abstract Core] Deep abstract processing: [Depth 5] [Abstract Core] Deep abstract proce
ssing: ... Raw sensory data towards Solve complex problem

Final State: Depth 5 processed
```

## Development

### Add a Feature
1. Update [Program.cs](DeepLearningProtocol/Program.cs) with new methods or interfaces
2. Add corresponding tests in [DeepLearningProtocolTests.cs](DeepLearningProtocol.Tests/DeepLearningProtocolTests.cs)
3. Run `dotnet test` to verify

### Debug
Press **F5** in VS Code to launch the debugger with the configured [launch.json](.vscode/launch.json).

### Code Documentation
- All classes and methods are documented with XML comments
- Use IDE IntelliSense (Ctrl+Space) to view documentation
- Read inline comments in [Program.cs](DeepLearningProtocol/Program.cs) for implementation details

## CI/CD Pipeline

GitHub Actions automatically:
- ✅ Builds on push to `main`, `master`, `develop`
- ✅ Runs all unit tests
- ✅ Collects code coverage (optional Codecov integration)
- ✅ Publishes release binaries

See [.github/workflows/dotnet.yml](.github/workflows/dotnet.yml) for details.

## Future Enhancements

- Neural network layer integration
- Async processing with Task-based execution
- Persistent state serialization (JSON/database)
- Advanced DLP rules (ML-based content classification)
- REST API wrapper for protocol access
- Distributed processing support

## FAQ (Frequently Asked Questions)

**Q: What is the Deep Learning Protocol?**
A: A hierarchical reasoning system that processes information through multiple layers with built-in data protection. See [Architecture](#architecture) above.

**Q: How do I run the program?**
A: Three ways:
1. VS Code: Press Ctrl+Shift+B
2. CLI: `dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj`
3. Interactive: Run and follow the menu

**Q: What is Data Loss Prevention (DLP)?**
A: A protective layer that detects suspicious content (memes, binary payloads), blocks updates, and backs up state. See [Features](#features).

**Q: Can I ask custom questions?**
A: Yes! Run the program and select "Run Interactive Protocol" to enter custom questions, goals, and depth levels.

**Q: How do I extend the project?**
A: 
1. Update [Program.cs](DeepLearningProtocol/Program.cs)
2. Add tests to [DeepLearningProtocolTests.cs](DeepLearningProtocol.Tests/DeepLearningProtocolTests.cs)
3. Run `dotnet test` to verify

**For more FAQ**, see the [interactive menu](#run) or check the [Wiki](https://github.com/quickattach0-tech/DeepLearningProtocol/wiki).

## Documentation

- 📚 **[Wiki Home](https://github.com/quickattach0-tech/DeepLearningProtocol/wiki)** — Full documentation
- 🏗️ **[Architecture Guide](docs/Architecture.md)** — System design details
- 🧪 **[Testing Guide](docs/Testing.md)** — How to write and run tests
- 🛡️ **[DLP Guide](docs/DLP-Guide.md)** — Data Loss Prevention deep dive
- 🚀 **[Getting Started](docs/Getting-Started.md)** — Step-by-step tutorial
- 🔧 **[Contributing](CONTRIBUTING.md)** — Development guidelines

## Contributing

Contributions are welcome! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## License

See [LICENSE](LICENSE) file for details.

---

**Last Updated**: December 18, 2025  
**Maintained by**: [@quickattach0-tech](https://github.com/quickattach0-tech)
