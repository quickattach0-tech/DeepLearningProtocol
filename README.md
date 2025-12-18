# Deep Learning Protocol

A conceptual C# implementation inspired by a hierarchical deep learning architecture with **Data Loss Prevention (DLP)** capabilities.

[![CI/CD Status](https://github.com/quickattach0-tech/DeepLearningProtocol/actions/workflows/dotnet.yml/badge.svg)](https://github.com/quickattach0-tech/DeepLearningProtocol/actions/workflows/dotnet.yml)

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
│   ├── Program.cs                     # Core protocol + DLP implementation
│   └── DeepLearningProtocol.csproj
├── DeepLearningProtocol.Tests/        # XUnit test suite
│   └── DeepLearningProtocolTests.cs
├── DeepLearningProtocol.sln           # Solution file with startup project configured
├── .vscode/
│   ├── tasks.json                     # VS Code build/run/test tasks
│   └── launch.json                    # F5 debug configuration
├── .gitignore                         # .NET build artifacts
└── README.md                          # This file
```

## Quick Start

### Prerequisites
- .NET 10.0 SDK or higher
- Visual Studio Code (optional, for enhanced tasks/debugging)

### Build

```bash
dotnet build
```

### Run

**Option 1: VS Code**
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

## Features

### Data Loss Prevention (DLP)

The `DataLossPrevention` class monitors state updates and:

1. **Detects meme-like content** — checks for:
   - File extensions (`.png`, `.jpg`, `.jpeg`)
   - Image data URIs (`data:image/`, `base64,`)
   - Keywords ("meme")
   - Large single-line payloads (>200 chars, no newlines)

2. **Backs up state** — saves previous states to `./.dlp_backups/` with timestamps
3. **Blocks risky updates** — prevents accidental data loss by setting state to `[DLP-BLOCKED]` when suspicious content is detected

### Example Output

```
Initial state: Initial
State updated: Aiming: Solve complex problem
State updated: Depth 5 processed

Result:
[Aim Pursuit] [Abstract Core] Deep abstract processing: [Depth 5] [Abstract Core] Deep abstract processing: ... Raw sensory data towards Solve complex problem

Final State: Depth 5 processed
```

## Development

### Add a Feature
1. Update `Program.cs` with new methods or interfaces
2. Add corresponding tests in `DeepLearningProtocol.Tests/DeepLearningProtocolTests.cs`
3. Run `dotnet test` to verify

### Debug
Press **F5** in VS Code to launch the debugger with the configured `launch.json`.

## Future Enhancements

- Neural network layer integration
- Async processing with Task-based execution
- Persistent state serialization (JSON/database)
- Advanced DLP rules (ML-based content classification)
- REST API wrapper for protocol access

## License

See [LICENSE](LICENSE) file for details.
