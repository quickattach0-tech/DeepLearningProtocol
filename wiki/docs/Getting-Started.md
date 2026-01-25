# Getting Started with Deep Learning Protocol

Welcome! This guide will help you set up and run the Deep Learning Protocol in just 5 minutes.

## Prerequisites

You need:
- **[.NET 10.0 SDK](https://dotnet.microsoft.com/download)** or higher
- **Git** (for cloning the repository)
- **A terminal** (Command Prompt, PowerShell, or Bash)
- **VS Code** (optional, but recommended for enhanced experience)

### Check Your Installation

```bash
dotnet --version
```

Should output something like: `10.0.x` or higher

## Installation

### Step 1: Clone the Repository

```bash
git clone https://github.com/quickattach0-tech/DeepLearningProtocol.git
cd DeepLearningProtocol
```

### Step 2: Build the Project

```bash
dotnet build
```

Expected output:
```
...
Build succeeded. 0 Warning(s) ⏱️  2.14s
```

### Step 3: Run Tests (Optional)

```bash
dotnet test
```

All 7 tests should pass ✅

### Step 4: Run the Application

```bash
dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj
```

Or in VS Code, press **Ctrl+Shift+B**

## First Interaction

### The Main Menu

When you run the app, you'll see:

```
╔════════════════════════════════════════════════════════╗
║     Deep Learning Protocol - Interactive Menu          ║
╚════════════════════════════════════════════════════════╝

1. Run Interactive Protocol
2. View FAQ
3. Exit

Enter your choice (1-3): 
```

### Option 1: Run Interactive Protocol

```
Enter your question: How can I improve my code quality?
Enter your goal: Code excellence
Enter depth level (1-10): 3

[Processing...]

Result:
[Aim Pursuit] [Abstract Core] Deep abstract processing: [Depth 3]...
```

### Option 2: View FAQ

```
Deep Learning Protocol - FAQ

1. What is the Deep Learning Protocol?
2. How do I run the program?
3. What is Data Loss Prevention (DLP)?
...
10. What are the future enhancements?
11. Back to Main Menu

Select a question (1-11):
```

## VS Code Setup (Recommended)

The repository includes VS Code configuration for optimal development experience.

### Features

- **Ctrl+Shift+B** — Run the application
- **F5** — Debug with breakpoints
- **Terminal → Run Task** — Access build/test tasks

### Included Tasks

1. **dotnet run** (default) — Execute the protocol
2. **dotnet build** — Build the project
3. **dotnet test** — Run unit tests

## Common Commands

### Development Workflow

```bash
# Build only (no run)
dotnet build

# Run with specific project
dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj

# Run tests with verbose output
dotnet test --verbosity detailed

# Clean build artifacts
dotnet clean
```

### Testing

```bash
# Run all tests
dotnet test

# Run specific test
dotnet test --filter "TestMethodName"

# Generate coverage report
dotnet test /p:CollectCoverage=true
```

### Publishing

```bash
# Publish Release build
dotnet publish -c Release

# Output will be in: bin/Release/net10.0/publish/
```

## Project Structure

```
DeepLearningProtocol/
├── DeepLearningProtocol/               # Main executable
│   ├── Program.cs                      # All protocol logic (573 lines)
│   └── DeepLearningProtocol.csproj
├── DeepLearningProtocol.Tests/         # Unit tests
│   └── DeepLearningProtocolTests.cs
├── docs/                               # Documentation
│   ├── Architecture.md
│   ├── Getting-Started.md (this file)
│   ├── Testing.md
│   └── DLP-Guide.md
├── .vscode/
│   ├── tasks.json                      # VS Code tasks
│   └── launch.json                     # Debug config
└── .github/workflows/
    └── dotnet.yml                      # CI/CD pipeline
```

## Understanding the Code

The entire protocol is in [Program.cs](../DeepLearningProtocol/Program.cs), organized as:

### Classes

1. **AbstractCore** — Base reasoning layer
   - `ProcessCoreReasoning(input)` — Wraps in `[Abstract Core]`

2. **DataLossPrevention** — Content protection
   - `IsSuspiciousContent(text)` — Detects memes/binary data
   - `BackupState(currentState)` — Archives previous states

3. **DeepLearningProtocol** — Main orchestrator
   - `SetAim(goal)` — Store goal
   - `ProcessAtDepth(input, depth)` — Recursive processing
   - `PursueAim()` — Execute workflow
   - `ExecuteProtocol(input, goal, depth)` — Complete pipeline

4. **Program** — Interactive menu
   - `Main()` — Menu loop
   - `RunInteractiveProtocol()` — User input
   - `DisplayFAQ()` — FAQ browser

## FAQ for Beginners

**Q: What do I do if the build fails?**
A: 
1. Ensure .NET 10.0 is installed: `dotnet --version`
2. Clear cache: `dotnet clean && dotnet build`
3. Check [Issues](https://github.com/quickattach0-tech/DeepLearningProtocol/issues)

**Q: Can I run this on macOS/Linux/Windows?**
A: Yes! .NET is cross-platform. The app works on all three.

**Q: What's the difference between Program.cs classes?**
A: 
- **AbstractCore** — Does the thinking
- **DataLossPrevention** — Protects the data
- **DeepLearningProtocol** — Orchestrates thinking + protection
- **Program** — Manages user interaction

**Q: How do I debug?**
A: 
1. Set breakpoint (click line number in VS Code)
2. Press F5
3. App pauses at breakpoint, inspect variables

**Q: Where are state backups stored?**
A: In `./.dlp_backups/` directory with timestamp-based filenames

**Q: Can I modify the FAQ questions?**
A: Yes! Edit the `FAQs` dictionary in [Program.cs](../DeepLearningProtocol/Program.cs#L431)

## Next Steps

1. **Explore the Code** — Read through [Program.cs](../DeepLearningProtocol/Program.cs)
2. **Understand Architecture** — See [Architecture Guide](Architecture.md)
3. **Write a Test** — Follow [Testing Guide](Testing.md)
4. **Learn DLP** — Read [DLP Guide](DLP-Guide.md)
5. **Contribute** — Check [Contributing Guide](../CONTRIBUTING.md)

## Getting Help

- 📖 **Wiki** — [Full documentation](https://github.com/quickattach0-tech/DeepLearningProtocol/wiki)
- 🐛 **Issues** — [Report bugs](https://github.com/quickattach0-tech/DeepLearningProtocol/issues)
- 💬 **Discussions** — Ask questions
- 📧 **Email** — Contact maintainers

---

**Ready to explore?** Run the app now:
```bash
dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj
```
