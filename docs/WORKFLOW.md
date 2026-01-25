# Development & CI/CD Workflow Guide

## Overview

The Deep Learning Protocol uses a structured development workflow combined with automated CI/CD pipelines to ensure code quality, consistency, and reliability. This guide covers the complete workflow from development to production.

---

## 🔄 Development Workflow

### Workflow Stages

```
Feature Development
    ↓ (code implementation)
Local Testing
    ↓ (build & run tests)
Code Review Preparation
    ↓ (commit & push)
Pull Request Review
    ↓ (peer review on GitHub)
CI/CD Pipeline Execution
    ↓ (automated tests & build)
Merge to Main
    ↓ (after approval)
Release Publication
```

### Step-by-Step Development Process

#### 1. **Create Feature Branch**
```bash
# Start new feature development
git checkout -b feature/my-feature

# Or for bug fixes
git checkout -b fix/my-fix

# Or for documentation
git checkout -b docs/my-docs
```

**Guidelines:**
- Use descriptive branch names
- Prefix with `feature/`, `fix/`, `docs/`, `refactor/`, etc.
- Use lowercase with hyphens for spaces
- Example: `feature/quality-translation`, `fix/uptime-tracking`

#### 2. **Implement Changes**

Update relevant files in your feature branch:

**For Core Features:**
- Modify `/DeepLearningProtocol/Program.cs`
- Update `/DeepLearningProtocol/DeepLearningProtocol.cs`
- Add/modify interface implementations as needed

**For Testing:**
- Add tests to `/DeepLearningProtocol.Tests/DeepLearningProtocolTests.cs`
- Ensure test coverage for new code
- Follow XUnit testing patterns

**For Documentation:**
- Update relevant `.md` files in `/docs/`
- Keep README.md in sync
- Update DOCS_INDEX.md with new sections

**Code Quality Checklist:**
- [ ] Code follows C# conventions
- [ ] Meaningful variable/method names
- [ ] Comments for complex logic
- [ ] No unnecessary code duplication
- [ ] Proper error handling
- [ ] Nullable reference annotations where applicable

#### 3. **Test Locally**

Run complete local validation before pushing:

```bash
# Clean and rebuild
dotnet clean
dotnet build --configuration Debug

# Run all unit tests
dotnet test

# Manual testing (interactive)
dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj

# Build in Release mode
dotnet build --configuration Release
```

**Test Output Should Show:**
- ✅ 0 compilation errors
- ✅ 0 code warnings
- ✅ 8/8 unit tests passing
- ✅ No runtime errors

#### 4. **Commit Changes**

Use clear, meaningful commit messages:

```bash
# Feature commits
git commit -m "feat: add quality translation system"

# Bug fix commits
git commit -m "fix: resolve uptime calendar tracking"

# Documentation commits
git commit -m "docs: update workflow guide"

# Refactoring commits
git commit -m "refactor: improve code organization"

# Style/formatting commits
git commit -m "style: fix code formatting"
```

**Commit Message Format:**
```
<type>: <subject>

<body>

<footer>
```

Types: `feat`, `fix`, `docs`, `refactor`, `style`, `test`, `chore`

#### 5. **Push to Remote**

```bash
# Push feature branch to origin
git push origin feature/my-feature

# Track remote branch
git push -u origin feature/my-feature
```

#### 6. **Create Pull Request**

On GitHub:
1. Navigate to your forked repository
2. Click "New Pull Request"
3. Select base: `main`, compare: `feature/my-feature`
4. Fill in PR title and description
5. Reference related issues: `Closes #123`
6. Submit PR

**PR Description Template:**
```markdown
## Description
Brief description of changes.

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Documentation update
- [ ] Refactoring

## Testing
Describe testing performed:
- Local build: ✅ 0 errors
- Unit tests: ✅ 8/8 passing
- Manual testing: ✅ Verified

## Checklist
- [ ] Code follows style guidelines
- [ ] Comments added for complex logic
- [ ] Documentation updated
- [ ] Tests added/updated
- [ ] All tests passing
```

#### 7. **Address Review Feedback**

When reviewers request changes:
1. Make requested changes locally
2. Commit with descriptive message
3. Push to the same feature branch
4. PR automatically updates
5. Comment when changes complete

#### 8. **Merge to Main**

After approval:
1. Ensure all CI/CD checks pass (green checkmarks)
2. Resolve any merge conflicts
3. Click "Squash and merge" or "Create a merge commit"
4. Delete remote feature branch

---

## 🚀 CI/CD Pipeline Workflow

### Automated Pipeline Overview

The CI/CD pipeline runs automatically on GitHub Actions and consists of three parallel/sequential stages:

```
┌─────────────────────────────────────────────────────┐
│        GitHub Push Event (code committed)           │
└──────────────────┬──────────────────────────────────┘
                   │
         ┌─────────┴──────────┐
         │                    │
    [IF: Pull Request]   [IF: Push to Main/Develop]
         │                    │
    [Debug Build]      [Release Build]
         │                    │
    ✓ Checkout          ✓ Checkout
    ✓ Setup .NET        ✓ Setup .NET
    ✓ Restore deps      ✓ Restore deps
    ✓ Build (Debug)     ✓ Build (Release)
    ✓ Run tests         ✓ Run tests + coverage
         │                ✓ Upload coverage
         │                ✓ Upload artifacts
         │                    │
         └─────────┬──────────┘
                   │
         [Code Quality Check]
         (main only - optional)
                   │
            ✓ Style Check
```

### Pipeline Stages Explained

#### **Stage 1: Debug Build** (Pull Requests & Feature Branches)
- **Trigger:** Any PR or push to non-main branches
- **Configuration:** Debug mode (faster compilation)
- **Purpose:** Quick validation of code changes
- **Duration:** ~1-2 minutes
- **Actions:**
  - Checkout latest code
  - Setup .NET 10.0.x
  - Restore NuGet dependencies
  - Build in Debug configuration
  - Run full test suite (8 tests)

#### **Stage 2: Release Build** (Main Branch)
- **Trigger:** Push to main branch
- **Configuration:** Release mode (optimized)
- **Purpose:** Production-quality build validation
- **Duration:** ~2-3 minutes
- **Actions:**
  - Checkout latest code
  - Setup .NET 10.0.x
  - Restore NuGet dependencies
  - Build in Release configuration
  - Run test suite with code coverage collection
  - Upload coverage reports to Codecov
  - Store build artifacts (30-day retention)

#### **Stage 3: Code Quality** (Main Branch)
- **Trigger:** Push to main branch
- **Configuration:** Style enforcement
- **Purpose:** Maintain code quality standards
- **Duration:** ~1 minute
- **Actions:**
  - Checkout code
  - Setup .NET environment
  - Enforce code style in build (optional, non-blocking)
  - Report style violations

### Pipeline Configuration

Pipeline defined in: `.github/workflows/dotnet.yml`

**Environment Variables:**
```yaml
DOTNET_VERSION: '10.0.x'
BUILD_CONFIGURATION: Release
ARTIFACT_RETENTION_DAYS: 30
RELEASE_ARTIFACT_RETENTION_DAYS: 90
```

**Supported Branches:**
- `main` - Production branch (full pipeline)
- `master` - Legacy production branch
- `develop` - Development branch (partial pipeline)

### Monitoring Pipeline Status

**View Pipeline Results:**
1. Go to **Actions** tab on GitHub
2. Select workflow: "CI/CD for Deep Learning Protocol"
3. View latest run status
4. Click run to see detailed logs

**Check PR Status:**
- PR shows pipeline status checks
- Green checkmarks = all stages passed
- Red X = one or more stages failed

**Status Badge:**
```markdown
[![CI/CD Status](https://github.com/quickattach0-tech/DeepLearningProtocol/actions/workflows/dotnet.yml/badge.svg)](https://github.com/quickattach0-tech/DeepLearningProtocol/actions/workflows/dotnet.yml)
```

---

## 🌳 Branch Strategy

### Main Branches

| Branch | Purpose | Stability | CI/CD |
|--------|---------|-----------|-------|
| **main** | Production-ready code | Stable | Full pipeline |
| **develop** | Integration branch | Semi-stable | Partial pipeline |
| **master** | Legacy/fallback | Stable | Full pipeline |

### Feature Branches

```
Feature:    feature/my-feature
Bugfix:     fix/my-fix
Docs:       docs/my-docs
Refactor:   refactor/my-refactor
Hotfix:     hotfix/critical-issue
```

**Naming Convention:**
- Use lowercase
- Use hyphens to separate words
- Be descriptive: `feature/quality-scoring` ✅ vs `feature/new` ❌
- Prefix with type: `feature/`, `fix/`, etc.

### Branch Lifecycle

```
feature/my-feature (created)
    ↓
 Local commits
    ↓
git push origin feature/my-feature
    ↓
Create Pull Request on GitHub
    ↓
CI/CD Pipeline Validation
    ↓
Code Review
    ↓
Changes requested? → Yes: Commit & push updates
    ↓ (No)
Approved ✅
    ↓
Merge to main
    ↓
Delete feature branch
    ↓
Pipeline runs on main (Release build + artifacts)
```

---

## 📋 Code Quality Standards

All contributions must meet these standards:

### Build Quality
- ✅ **Zero Errors:** `dotnet build` produces no compilation errors
- ✅ **Zero Warnings:** No code warnings (strict nullable reference checks)
- ✅ **Clean Output:** Build output is clean with no warnings

### Test Quality
- ✅ **100% Pass Rate:** All tests pass: `dotnet test`
- ✅ **Coverage:** New code has unit test coverage
- ✅ **Test Framework:** Use XUnit 2.9.2
- ✅ **Test Naming:** Clear, descriptive test names

### Code Quality
- ✅ **Style:** Follow C# coding conventions
- ✅ **Naming:** Meaningful variable/method names
- ✅ **Comments:** Document complex logic
- ✅ **DRY:** Avoid code duplication
- ✅ **SOLID:** Apply SOLID principles where applicable
- ✅ **Null Safety:** Use nullable reference annotations

### Documentation Quality
- ✅ **Updated:** Update docs for new features
- ✅ **Complete:** Include examples and explanations
- ✅ **Clear:** Use simple, understandable language
- ✅ **Accurate:** Ensure docs match implementation

### Commit Quality
- ✅ **Meaningful:** Clear, descriptive commit messages
- ✅ **Atomic:** Logical grouping of changes
- ✅ **Proper Format:** Follow commit message conventions
- ✅ **Frequency:** Regular commits, not massive single commits

---

## 🔧 Workflow Management Code

The `Workflow.cs` class provides programmatic access to workflow management:

### WorkflowManager Class

```csharp
// Create workflow manager
var workflowManager = new WorkflowManager();

// Start a stage
workflowManager.StartStage(1, "Build", "Compilation stage");

// Add log entry
workflowManager.AddLog("Build", "Compiling project...");

// Complete stage
workflowManager.CompleteStage("Build", success: true, 
    summary: "Build successful - 0 errors");

// Get workflow summary
string summary = workflowManager.GetWorkflowSummary();
Console.WriteLine(summary);

// Save to file
workflowManager.SaveWorkflowToFile("dev-session");

// Get pipeline config
var pipeline = WorkflowManager.GetCIPipelineConfig();
```

### WorkflowExecutor Class

```csharp
// Create executor
var executor = new WorkflowExecutor(workflowManager);

// Execute development workflow
executor.ExecuteDevelopmentWorkflow();

// Execute CI/CD pipeline workflow
executor.ExecuteCIPipelineWorkflow();
```

### Display Workflow Info

```csharp
// Display development workflow information
WorkflowManager.DisplayWorkflowInfo();
```

---

## 📊 Workflow Examples

### Example 1: Feature Development & Release

```bash
# 1. Create feature branch
git checkout -b feature/uptime-tracking

# 2. Implement feature
# ... modify Program.cs, add tests, update docs ...

# 3. Test locally
dotnet build
dotnet test
dotnet run

# 4. Commit changes
git add .
git commit -m "feat: add 24-hour uptime tracking system"

# 5. Push to remote
git push -u origin feature/uptime-tracking

# 6. On GitHub: Create PR, wait for CI/CD (1-2 min)
# ... reviewer approves ...

# 7. Merge to main
# PR shows all checks passing, merge with squash

# 8. CI/CD runs again on main
# Release build + coverage + artifacts uploaded
```

### Example 2: Bug Fix & Patch Release

```bash
# 1. Create bugfix branch
git checkout -b fix/translation-cache-issue

# 2. Fix the bug
# ... modify Workflow.cs, update test ...

# 3. Verify fix
dotnet test    # Should pass
dotnet run     # Manual test

# 4. Commit
git commit -m "fix: resolve translation cache duplicate entries"

# 5. Push and create PR
git push -u origin fix/translation-cache-issue

# 6. After CI/CD passes and review approval → merge

# 7. Release as patch version (v3.1.2)
```

### Example 3: Documentation Update

```bash
# 1. Create docs branch
git checkout -b docs/workflow-guide

# 2. Update documentation
# ... modify WORKFLOW.md, README.md ...

# 3. No code changes, so light testing
git add docs/
git commit -m "docs: add comprehensive workflow guide"

# 4. Push and create PR
git push -u origin docs/workflow-guide

# 5. Debug build validates docs syntax, merge
```

---

## 🆘 Troubleshooting

### Build Fails Locally

```bash
# Clean and rebuild
dotnet clean
dotnet build

# If still failing:
# 1. Check .NET version: dotnet --version
# 2. Restore packages: dotnet restore
# 3. Check for syntax errors in your changes
```

### Tests Fail

```bash
# Run tests with verbose output
dotnet test --verbosity detailed

# Run specific test
dotnet test --filter "TestClassName"

# Common issues:
# - Missing using statements
# - Test data not initialized
# - Async/await issues
```

### CI/CD Pipeline Fails

**Check GitHub Actions logs:**
1. Go to Actions tab
2. Click failed workflow
3. Expand failed job
4. Read error message and stack trace

**Common causes:**
- Code changed since last local test
- Race condition in tests
- Missing dependency
- File encoding issue
- Line ending differences (CRLF vs LF)

**Fix line endings (if needed):**
```bash
# Windows → Unix line endings
git config core.autocrlf input
```

### Merge Conflicts

```bash
# If main has changed while you developed:
git fetch origin
git rebase origin/main

# Or merge main into your branch:
git merge origin/main

# Resolve conflicts in editor, then:
git add .
git commit -m "merge: resolve conflicts with main"
git push origin feature/my-feature
```

---

## 📚 Related Documentation

- [Getting Started](Getting-Started.md) - Quick start guide
- [Architecture Guide](Architecture.md) - System design
- [Testing Guide](Testing.md) - Writing tests
- [Contributing Guidelines](../CONTRIBUTING.md) - Contribution standards
- [CI/CD Configuration](.github/workflows/dotnet.yml) - Pipeline details

---

## 🤝 Getting Help

- **Questions?** Open an issue on GitHub
- **Found a bug?** Create a bug report
- **Want to contribute?** See CONTRIBUTING.md
- **Need guidance?** Check the Wiki

---

**Last Updated:** January 25, 2026  
**Status:** Production Ready  
**Maintained by:** [@quickattach0-tech](https://github.com/quickattach0-tech)
