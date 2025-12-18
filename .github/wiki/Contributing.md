# Contributing Guide

Thank you for your interest in contributing to the Deep Learning Protocol project!

## 🤝 How to Contribute

### Reporting Issues
1. Check [existing issues](https://github.com/quickattach0-tech/DeepLearningProtocol/issues) first
2. Use a clear, descriptive title
3. Include steps to reproduce
4. Provide system information (OS, .NET version)

### Suggesting Enhancements
1. Open an [issue](https://github.com/quickattach0-tech/DeepLearningProtocol/issues) with `[Enhancement]` prefix
2. Explain the use case
3. Propose implementation approach
4. Discuss before coding

### Code Contributions

#### Step 1: Fork and Clone
```bash
git clone https://github.com/YOUR_USERNAME/DeepLearningProtocol.git
cd DeepLearningProtocol
git remote add upstream https://github.com/quickattach0-tech/DeepLearningProtocol.git
```

#### Step 2: Create a Feature Branch
```bash
git checkout -b feature/your-feature-name
```

#### Step 3: Make Changes
- Keep commits atomic (one feature per commit)
- Write clear commit messages
- Follow code style guidelines (see below)

#### Step 4: Add Tests
- Every feature needs a test
- Place tests in `DeepLearningProtocol.Tests/`
- Run `dotnet test` to verify

#### Step 5: Push and Create PR
```bash
git push origin feature/your-feature-name
```

Then create a Pull Request with:
- Clear title and description
- Reference to related issues
- List of changes

---

## 📋 Code Style Guide

### C# Conventions
```csharp
// Public classes and methods: PascalCase
public class DeepLearningProtocol { }
public string SetAim(string goal) { }

// Private fields: _camelCase
private string _currentState;

// Local variables: camelCase
var result = ProcessCoreReasoning(input);

// Constants: UPPER_CASE
private const int MAX_DEPTH = 10;
```

### XML Documentation
All public types and members must have XML documentation:

```csharp
/// <summary>
/// Processes input through the core reasoning engine.
/// </summary>
/// <param name="input">The input string to process</param>
/// <returns>Processed result string</returns>
public string ProcessCoreReasoning(string input)
{
    return $"[Abstract Core] {input}";
}
```

### Naming Conventions
- Classes: `PascalCase` (e.g., `DataLossPrevention`)
- Methods: `PascalCase` (e.g., `ProcessAtDepth`)
- Parameters: `camelCase` (e.g., `depthLevel`)
- Private fields: `_camelCase` (e.g., `_currentState`)

---

## 🧪 Testing Requirements

### Writing Tests
1. Use XUnit framework
2. One test per behavior
3. Clear test names: `[MethodName]_[Condition]_[Result]`

```csharp
[Fact]
public void SetAim_WithValidGoal_UpdatesAimAndState()
{
    var protocol = new DeepLearningProtocol();
    var result = protocol.SetAim("Test Goal");
    
    Assert.Contains("Test Goal", result);
    Assert.Equal("Aiming: Test Goal", protocol.GetCurrentState());
}
```

### Coverage Requirements
- Minimum 80% code coverage
- All public methods tested
- Edge cases covered
- Error paths tested

### Running Tests
```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true

# Run specific test
dotnet test --filter TestMethodName
```

---

## 🔍 Code Review Process

### What Reviewers Check
- ✅ Code follows style guide
- ✅ Tests are included and passing
- ✅ Documentation is complete
- ✅ No breaking changes
- ✅ Performance is reasonable

### What to Expect
- Reviewers may request changes
- Be open to feedback
- Iterate until approved
- Squash commits if requested

---

## 📦 Commit Message Guidelines

### Format
```
<type>(<scope>): <subject>

<body>

<footer>
```

### Examples
```
feat(DLP): add pattern detection for sensitive content
fix(state): handle concurrent state updates correctly
docs(README): update quick start instructions
test(depth): add edge case for max depth level
refactor(core): simplify AbstractCore reasoning
```

### Types
- `feat` — New feature
- `fix` — Bug fix
- `docs` — Documentation
- `style` — Code style
- `refactor` — Code refactoring
- `test` — Test additions
- `chore` — Build/config changes

---

## 🚀 Release Process

1. **Merge** to `main` branch
2. **GitHub Actions** automatically:
   - Runs tests
   - Builds multi-platform binaries
   - Creates release artifacts
3. **Manual** (maintainer):
   - Create GitHub release
   - Add release notes
   - Publish to NuGet (if applicable)

---

## 📚 Project Structure

```
DeepLearningProtocol/
├── DeepLearningProtocol/              # Main implementation
│   ├── Program.cs                     # Core logic (478 lines)
│   └── DeepLearningProtocol.csproj
├── DeepLearningProtocol.Tests/        # Unit tests (8 tests)
│   └── DeepLearningProtocolTests.cs
├── docs/                              # User documentation
├── .github/workflows/                 # CI/CD automation
├── .vscode/                           # VS Code config
├── README.md                          # Project overview
├── CONTRIBUTING.md                    # This file
└── LICENSE                            # MIT License
```

---

## 💡 Development Tips

### Useful Commands
```bash
# Build only
dotnet build

# Build and run
dotnet run --project DeepLearningProtocol/

# Run tests with output
dotnet test --verbosity detailed

# Clean build artifacts
dotnet clean

# Format code
dotnet format

# Check for issues
dotnet build /p:EnforceCodeStyleInBuild=true
```

### Debug in VS Code
1. Open project in VS Code
2. Press `F5` to start debugging
3. Set breakpoints (click line number)
4. Step through code
5. Use Debug Console for REPL

### Architecture Files
- **Core**: `DeepLearningProtocol/Program.cs`
- **Tests**: `DeepLearningProtocol.Tests/DeepLearningProtocolTests.cs`
- **Tasks**: `.vscode/tasks.json`
- **Debug**: `.vscode/launch.json`

---

## ❓ Questions?

- Check [FAQ](FAQ)
- Read [Architecture Overview](Architecture-Overview)
- Open an [issue](https://github.com/quickattach0-tech/DeepLearningProtocol/issues)

---

## 🎉 Thank You!

Your contributions make this project better for everyone. We appreciate:
- **Bug reports** — Help us fix issues
- **Feature ideas** — Drive innovation
- **Code contributions** — Improve quality
- **Documentation** — Help others learn
- **Testing** — Ensure reliability

**Welcome to the team!** 🚀
