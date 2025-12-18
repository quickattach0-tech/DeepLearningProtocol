# Deep Learning Protocol Wiki

Welcome to the Deep Learning Protocol Wiki! This is your comprehensive guide to understanding, using, and extending the system.

## 📚 Quick Navigation

### Getting Started
- **[Getting Started Guide](Getting-Started)** — 5-minute setup and first run
- **[Installation & Prerequisites](Getting-Started#prerequisites)** — What you need
- **[Your First Interaction](Getting-Started#first-interaction)** — Run and explore

### Understanding the System
- **[Architecture Overview](Architecture)** — System design and components
- **[Component Details](Architecture#components)** — Deep dive into each class
- **[Data Flow Diagrams](Architecture#data-flow)** — How data moves through the system
- **[Design Patterns](Architecture#key-design-patterns)** — Architectural patterns used

### Core Features
- **[Data Loss Prevention (DLP)](DLP-Guide)** — Content protection explained
- **[DLP Detection Rules](DLP-Guide#detection-rules)** — What gets blocked
- **[Backup Mechanism](DLP-Guide#backup-mechanism)** — State recovery
- **[DLP Scenarios](DLP-Guide#scenarios)** — Real-world examples

### Development
- **[Testing Guide](Testing)** — Test suite and writing tests
- **[Test Categories](Testing#test-categories)** — Different test types
- **[Running Tests](Testing#running-tests)** — Commands and options
- **[Contributing Guide](../CONTRIBUTING.md)** — How to contribute

---

## 🚀 Quick Start

### Build & Run (30 seconds)

```bash
# Clone & setup
git clone https://github.com/quickattach0-tech/DeepLearningProtocol.git
cd DeepLearningProtocol

# Build
dotnet build

# Run
dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj
```

### First Test (10 seconds)

```bash
dotnet test
```

Expected: All 7 tests pass ✅

---

## 📖 Documentation Map

### By Role

**Users (Just want to run the app)**
1. [Getting Started](Getting-Started) — Setup in 5 minutes
2. [FAQ](../README.md#faq) — Common questions
3. [Features](../README.md#features) — What it can do

**Developers (Want to understand internals)**
1. [Architecture](Architecture) — System design
2. [Code Structure](Architecture#components) — Class organization
3. [Testing Guide](Testing) — How tests work
4. [DLP Guide](DLP-Guide) — Protection mechanisms

**Contributors (Want to add features)**
1. [Contributing Guide](../CONTRIBUTING.md) — Development process
2. [Code Style Guide](../CONTRIBUTING.md#code-style-guide) — Formatting rules
3. [Testing Requirements](../CONTRIBUTING.md#testing-requirements) — What to test
4. [Workflow](../CONTRIBUTING.md#development-workflow) — PR process

---

## 🏗️ System Architecture (Visual)

```
┌─────────────────────────────────────────┐
│        Deep Learning Protocol           │
│   Multi-Layered Reasoning System        │
└──────────────────┬──────────────────────┘
                   │
        ┌──────────┴──────────┐
        │                     │
    ┌───▼────┐            ┌──▼──────┐
    │  User  │ ◄────────► │  FAQ    │
    │ Menu   │            │ Browser │
    │ System │            │         │
    └───┬────┘            └─────────┘
        │
    ┌───▼──────────────────────────────┐
    │   DeepLearningProtocol           │
    │   (Orchestrator)                 │
    │                                  │
    │  • SetAim()                      │
    │  • ProcessAtDepth()              │
    │  • PursueAim()                   │
    │  • ExecuteProtocol()             │
    └───┬──────────────────────────────┘
        │
    ┌───▼──────────────────────────────┐
    │  AbstractCore                    │
    │  (Processing Layer)              │
    │                                  │
    │  ProcessCoreReasoning()          │
    │  - Wraps in [Abstract Core]      │
    │  - Recursive depth application   │
    └───┬──────────────────────────────┘
        │
    ┌───▼──────────────────────────────┐
    │  DataLossPrevention (DLP)        │
    │  (Protection Layer)              │
    │                                  │
    │  • IsSuspiciousContent()         │
    │  • BackupState()                 │
    │  • Block risky updates           │
    └──────────────────────────────────┘
```

---

## 🔧 Commands Reference

### Build & Test

```bash
# Build
dotnet build

# Run
dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj

# Test (all)
dotnet test

# Test (specific)
dotnet test --filter "TestName"

# Test (with coverage)
dotnet test /p:CollectCoverage=true
```

### VS Code

```bash
# Launch in VS Code
code .

# In VS Code:
# Ctrl+Shift+B  — Run (default task)
# F5            — Debug with breakpoints
# Ctrl+Shift+T  — Run tests
```

---

## 📚 Documentation Structure

```
DeepLearningProtocol/
├── README.md                      # Project overview
├── CONTRIBUTING.md                # Contribution guide
├── docs/
│   ├── Getting-Started.md         # Installation & first run
│   ├── Architecture.md            # System design deep dive
│   ├── Testing.md                 # Test suite guide
│   └── DLP-Guide.md               # Data Loss Prevention details
├── .github/
│   └── workflows/
│       └── dotnet.yml             # CI/CD pipeline
└── DeepLearningProtocol/
    └── Program.cs                 # All implementation (573 lines)
```

---

## ❓ Common Questions

**Q: How do I get started?**  
A: Read the [Getting Started Guide](Getting-Started) — covers everything in 5 minutes.

**Q: How does the protocol work?**  
A: Check [Architecture Overview](Architecture) for a complete walkthrough.

**Q: What is DLP and how does it work?**  
A: See [DLP Guide](DLP-Guide) for comprehensive protection mechanism explanation.

**Q: How do I run tests?**  
A: See [Testing Guide](Testing) or run `dotnet test`.

**Q: How can I contribute?**  
A: Read [Contributing Guide](../CONTRIBUTING.md) for the full process.

**Q: Where's the main code?**  
A: Everything is in [Program.cs](../DeepLearningProtocol/Program.cs) (~573 lines, fully documented).

---

## 🎯 Use Cases

### Use Case 1: Learn About Deep Learning
- Read [Architecture Guide](Architecture)
- Run the interactive protocol
- Explore code in [Program.cs](../DeepLearningProtocol/Program.cs)

### Use Case 2: Understand Data Protection
- Read [DLP Guide](DLP-Guide)
- Review detection rules
- Test with different inputs

### Use Case 3: Write a Test
- Read [Testing Guide](Testing)
- Look at existing tests
- Follow [Code Style Guide](../CONTRIBUTING.md#code-style-guide)

### Use Case 4: Add a Feature
- Read [Contributing Guide](../CONTRIBUTING.md)
- Check [Architecture](Architecture) for extension points
- Follow development workflow

---

## 🔗 External Links

- **[GitHub Repository](https://github.com/quickattach0-tech/DeepLearningProtocol)**
- **[Issues & Bug Reports](https://github.com/quickattach0-tech/DeepLearningProtocol/issues)**
- **[Pull Requests](https://github.com/quickattach0-tech/DeepLearningProtocol/pulls)**
- **[Discussions](https://github.com/quickattach0-tech/DeepLearningProtocol/discussions)**

---

## 📋 Page Index

### Overview
- **[Home](Home)** — This page
- **[README](../README.md)** — Project overview
- **[FAQ](../README.md#faq)** — Common questions

### User Guides
- **[Getting Started](Getting-Started)** — Installation & first run
- **[Architecture](Architecture)** — How it all works
- **[DLP Guide](DLP-Guide)** — Data protection

### Developer Guides
- **[Testing Guide](Testing)** — Test suite
- **[Contributing Guide](../CONTRIBUTING.md)** — How to contribute

### Reference
- **[CLI Commands](#commands-reference)** — Common commands
- **[Architecture Diagram](#system-architecture-visual)** — Visual overview

---

## 🤝 Contributing

Have a question? Found a bug? Want to contribute?

- 🐛 **[Report Bug](https://github.com/quickattach0-tech/DeepLearningProtocol/issues)** — File an issue
- ✨ **[Request Feature](https://github.com/quickattach0-tech/DeepLearningProtocol/issues)** — Suggest enhancement
- 💬 **[Start Discussion](https://github.com/quickattach0-tech/DeepLearningProtocol/discussions)** — Ask question
- 🔧 **[Submit PR](https://github.com/quickattach0-tech/DeepLearningProtocol/pulls)** — Contribute code
- 📝 **[Improve Docs](../CONTRIBUTING.md)** — Update documentation

See [Contributing Guide](../CONTRIBUTING.md) for complete details.

---

## 📊 Project Status

- ✅ **Build:** Passing (2.1s)
- ✅ **Tests:** 7/7 passing
- ✅ **Documentation:** Complete
- ✅ **CI/CD:** GitHub Actions configured
- 🚀 **Status:** Production-ready

---

**Last Updated:** December 18, 2025  
**Maintained by:** [@quickattach0-tech](https://github.com/quickattach0-tech)

---

**Start exploring:** [Getting Started Guide](Getting-Started) →
