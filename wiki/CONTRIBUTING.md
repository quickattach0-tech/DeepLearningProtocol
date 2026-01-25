# Contributing to Deep Learning Protocol

Thank you for your interest in contributing! This guide will help you get started.

## Code of Conduct

Be respectful, inclusive, and professional. We welcome contributors of all backgrounds and experience levels.

## Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) or higher
- Git
- VS Code (recommended)

### Development Setup

```bash
# Clone repository
git clone https://github.com/quickattach0-tech/DeepLearningProtocol.git
cd DeepLearningProtocol

# Verify setup
dotnet build
dotnet test
```

If all tests pass, you're ready to contribute!

## Types of Contributions

### 1. Bug Reports 🐛

Found a bug? Help us fix it!

**Before opening an issue, check:**
- Is this a .NET version issue? (We require .NET 10.0+)
- Does it happen in other environments?
- Can you reproduce it consistently?

**How to report:**
1. Go to [Issues](https://github.com/quickattach0-tech/DeepLearningProtocol/issues)
2. Click "New Issue"
3. Use this template:

```markdown
## Description
Brief description of the bug.

## Steps to Reproduce
1. Run: `dotnet run --project ...`
2. Enter: ...
3. Observe: ...

## Expected Behavior
What should happen?

## Actual Behavior
What actually happened?

## Environment
- OS: [Windows/macOS/Linux]
- .NET Version: [10.0.x]
- VS Code Version: [if applicable]
```

### 2. Feature Requests ✨

Have an idea? We'd love to hear it!

**Feature request template:**

```markdown
## Description
What would this feature do?

## Use Case
Why do you need this? What problem does it solve?

## Proposed Solution
How should it work? Provide examples if possible.

## Alternatives Considered
What other approaches could work?
```

### 3. Code Contributions 💻

### 4. Documentation 📚

### 5. Testing 🧪

## Development Workflow

### Step 1: Create a Fork

```bash
# Go to GitHub repository
# Click "Fork" button
# Clone your fork
git clone https://github.com/YOUR_USERNAME/DeepLearningProtocol.git
cd DeepLearningProtocol
```

### Step 2: Create a Feature Branch

```bash
# Create branch with descriptive name
git checkout -b feature/description-of-feature
# or
git checkout -b fix/description-of-bug
```

**Branch naming:**
- Features: `feature/descriptive-name`
- Fixes: `fix/descriptive-name`
- Documentation: `docs/descriptive-name`
- Tests: `test/descriptive-name`

### Step 3: Make Changes

**File to modify:** [DeepLearningProtocol/Program.cs](DeepLearningProtocol/Program.cs)

**Guidelines:**
- Keep changes focused (one feature per branch)
- Add XML documentation for public methods
- Follow C# naming conventions (PascalCase for classes/methods)
- Write clean, readable code

**Example change:**

```csharp
/// <summary>
/// Processes input with enhanced semantic understanding.
/// </summary>
/// <param name="input">The user's input text</param>
/// <returns>Enhanced processing result</returns>
public string ProcessWithSemantic(string input)
{
    // Implementation here
    return result;
}
```

### Step 4: Add Tests

For every new feature, add corresponding tests in [DeepLearningProtocolTests.cs](DeepLearningProtocol.Tests/DeepLearningProtocolTests.cs):

```csharp
[Fact]
public void YourFeature_Scenario_ExpectedResult()
{
    // Arrange
    var protocol = new DeepLearningProtocol();
    
    // Act
    var result = protocol.YourNewMethod("input");
    
    // Assert
    Assert.NotNull(result);
    Assert.Contains("expected", result);
}
```

### Step 5: Verify Changes

```bash
# Build project
dotnet build

# Run all tests
dotnet test

# Run specific test
dotnet test --filter "YourFeature"

# Check code formatting
# (Currently no formatter enforced, but aim for consistency)
```

### Step 6: Commit Changes

```bash
# Stage changes
git add .

# Commit with descriptive message
git commit -m "Add feature: semantic processing layer"
# or
git commit -m "Fix: DLP detection for base64 images"
```

**Commit message guidelines:**
- Start with verb: Add, Fix, Update, Remove, Refactor, Improve
- Keep first line under 50 characters
- Add details in body if needed

**Good examples:**
- "Add semantic layer to depth processing"
- "Fix DLP detection for PNG files"
- "Update README with API documentation"
- "Refactor ExecuteProtocol method"

### Step 7: Push to Your Fork

```bash
git push origin feature/descriptive-name
```

### Step 8: Create Pull Request

1. Go to [Pull Requests](https://github.com/quickattach0-tech/DeepLearningProtocol/pulls)
2. Click "New Pull Request"
3. Select `main` as base branch, your branch as compare
4. Fill in PR template:

```markdown
## Description
What changes does this PR introduce?

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Documentation update
- [ ] Test addition

## Related Issue
Closes #(issue number) [if applicable]

## How to Test
Steps to verify the changes work:
1. Build: `dotnet build`
2. Test: `dotnet test`
3. Run: `dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj`

## Testing Details
- Tests added: Yes/No
- All tests passing: Yes/No
- Coverage maintained: Yes/No

## Checklist
- [ ] Code follows style guidelines
- [ ] Tests added/updated
- [ ] Documentation updated
- [ ] No new warnings generated
```

### Step 9: Respond to Review

Maintainers will review your PR. They may:
- ✓ Approve (ready to merge)
- 💬 Comment (need discussion)
- 🔄 Request changes (need modifications)

**If changes requested:**
```bash
# Make changes to your files
# Stage and commit
git add .
git commit -m "Address review feedback"

# Push (no need to recreate PR)
git push origin feature/descriptive-name
```

### Step 10: Merge

Once approved, maintainers will merge your PR to `main`. Congratulations! 🎉

## Code Style Guide

### Naming Conventions

```csharp
// Classes - PascalCase
public class DataLossPrevention { }

// Methods - PascalCase
public void ExecuteProtocol() { }

// Properties - PascalCase
public string CurrentState { get; set; }

// Private fields - _camelCase
private string _internalState;

// Constants - UPPER_CASE
private const int MAX_DEPTH = 10;

// Local variables - camelCase
var currentState = GetCurrentState();
```

### Documentation

All public classes and methods should have XML documentation:

```csharp
/// <summary>
/// Brief description of what this does.
/// </summary>
/// <param name="paramName">Description of parameter</param>
/// <returns>Description of return value</returns>
/// <remarks>
/// Additional context or examples if needed.
/// </remarks>
public string MyMethod(string paramName)
{
    // Implementation
}
```

### Structure

```csharp
public class MyClass
{
    // 1. Fields (private first, then internal)
    private string _field;
    
    // 2. Constructors
    public MyClass() { }
    
    // 3. Public methods
    public void PublicMethod() { }
    
    // 4. Private methods
    private void PrivateMethod() { }
}
```

## Testing Requirements

### Minimum Testing Standards

- ✓ New features must include tests
- ✓ Bug fixes should include test demonstrating the bug
- ✓ All tests must pass before PR merge
- ✓ No decrease in code coverage

### Running Tests

```bash
# All tests
dotnet test

# Specific test
dotnet test --filter "MethodName"

# With coverage
dotnet test /p:CollectCoverage=true

# Verbose output
dotnet test --verbosity detailed
```

See [Testing Guide](docs/Testing.md) for more details.

## Documentation Requirements

### For New Features

1. **Update README.md** if it's a user-facing feature
2. **Add to FAQ** if it's a common question
3. **Create docs page** in `/docs` if it's complex
4. **Add XML comments** in code

### For Bug Fixes

No documentation needed unless it affects user behavior.

## Security

### Reporting Security Issues

**Do NOT open public issues for security vulnerabilities.**

Email security concerns to: [Add security contact if available]

Include:
- Description of vulnerability
- Steps to reproduce
- Potential impact

## Support

### Getting Help

- 📖 **[Wiki](https://github.com/quickattach0-tech/DeepLearningProtocol/wiki)** — Full documentation
- 💬 **[Discussions](https://github.com/quickattach0-tech/DeepLearningProtocol/discussions)** — Ask questions
- 🐛 **[Issues](https://github.com/quickattach0-tech/DeepLearningProtocol/issues)** — Report bugs

## Roadmap

Check [Future Enhancements](README.md#future-enhancements) for planned features:

- Neural network layer integration
- Async processing with Tasks
- Persistent state (JSON/database)
- Advanced DLP rules (ML-based)
- REST API wrapper

Interested in any of these? Start a discussion or open an issue!

## License

By contributing, you agree that your contributions will be licensed under the same license as the project. See [LICENSE](LICENSE).

## Recognition

Contributors will be recognized in:
- GitHub contributors page
- Project README acknowledgments
- Release notes

---

**Thank you for contributing!** Every contribution, no matter how small, helps make this project better. 🚀

## Quick Reference

**Common Commands:**
```bash
# Setup
git clone <your-fork>
cd DeepLearningProtocol
git checkout -b feature/my-feature

# Development
dotnet build          # Build
dotnet test           # Test
dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj  # Run

# Commit
git add .
git commit -m "Add feature: description"
git push origin feature/my-feature

# Then create PR on GitHub
```

**Need Help?** Open an issue with tag `help-wanted` or `question`!
