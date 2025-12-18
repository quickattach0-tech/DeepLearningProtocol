# Quick Start Guide

Get the Deep Learning Protocol running in **2 minutes**.

## Prerequisites

- **.NET 10.0 SDK** or higher
- **Git**
- **Text editor** or **VS Code**

## Installation & Setup

### Step 1: Clone the Repository

```bash
git clone https://github.com/quickattach0-tech/DeepLearningProtocol.git
cd DeepLearningProtocol
```

### Step 2: Build the Project

```bash
dotnet build
```

**Expected Output**:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
Time Elapsed 00:00:05.02
```

### Step 3: Run the Program

Choose one option:

#### Option A: VS Code (Recommended)
1. Open the folder in VS Code
2. Press `Ctrl+Shift+B` to run the default build task

#### Option B: Command Line
```bash
dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj
```

## First Run

You'll see an interactive menu:

```
╔════════════════════════════════════════════════════════╗
║     Deep Learning Protocol - Interactive Menu          ║
╚════════════════════════════════════════════════════════╝

1. Run Interactive Protocol
2. View FAQ
3. Exit

Choose an option (1-3): 
```

### Try It Out

1. Select **1** to "Run Interactive Protocol"
2. Enter your question (or press Enter for default)
3. Enter your goal (or press Enter for default)
4. Enter depth level 1-10 (or press Enter for default 5)
5. See the result!
6. Choose to continue or exit

## Run Tests

Verify everything works with:

```bash
dotnet test
```

**Expected Output**:
```
Test summary: total: 8, failed: 0, succeeded: 8, skipped: 0
Build succeeded.
```

## Next Steps

- 📖 Read [Getting Started Guide](Getting-Started)
- 🏗️ Understand [Architecture Overview](Architecture-Overview)
- ❓ Browse [FAQ](FAQ)
- 🤝 See [Contributing](Contributing) to get involved

## Troubleshooting

| Issue | Solution |
|-------|----------|
| `.NET SDK not found` | Install [.NET 10.0](https://dotnet.microsoft.com/download) |
| `Build fails` | Run `dotnet clean` then `dotnet build` |
| `Tests fail` | Ensure .NET 10.0 is installed; run `dotnet --version` |

Need help? See [Troubleshooting](Troubleshooting) or [open an issue](https://github.com/quickattach0-tech/DeepLearningProtocol/issues).
