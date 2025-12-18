# FAQ - Frequently Asked Questions

## General Questions

### Q: What is the Deep Learning Protocol?
**A:** A hierarchical multi-interface reasoning system implemented in C# that demonstrates:
- Layered architecture (AbstractCore + 3 interfaces)
- Goal-directed processing (AimInterface)
- Recursive depth processing (DepthInterface)
- State management (StateInterface)
- Data Loss Prevention (DLP) for content protection

It's both a **learning tool** and a **production-ready example** of good C# architecture.

### Q: Who should use this project?
**A:**
- **Students** learning about architecture patterns
- **Developers** wanting to understand multi-interface design
- **Teams** looking for a DLP reference implementation
- **Contributors** wanting to learn open-source workflows

### Q: Is this a machine learning project?
**A:** No. The name refers to the *conceptual* hierarchy (inspired by deep learning's layers), not actual neural networks. It demonstrates layered reasoning without ML complexity.

---

## Getting Started Questions

### Q: How do I install it?
**A:**
```bash
git clone https://github.com/quickattach0-tech/DeepLearningProtocol.git
cd DeepLearningProtocol
dotnet build
dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj
```

### Q: What are the system requirements?
**A:**
- **.NET 10.0 SDK** (or compatible)
- **OS**: Linux, Windows, or macOS
- **RAM**: 256MB minimum
- **Disk**: ~500MB (includes build artifacts)

### Q: Can I run it in VS Code?
**A:** Yes! Press `Ctrl+Shift+B` to build and run. Press `F5` to debug.

### Q: How do I run tests?
**A:** `dotnet test` — runs 8 unit tests (all passing ✅)

---

## Usage Questions

### Q: What's the interactive protocol?
**A:** A menu-driven system that:
1. Takes your question/input
2. Accepts a goal for processing
3. Processes at your chosen depth (1-10)
4. Returns the result with state tracking

### Q: Can I use custom inputs?
**A:** Yes! Run "Option 1: Run Interactive Protocol" and enter any text.

### Q: What happens if I input meme content?
**A:** The DLP layer detects it and blocks the update:
- Your previous state is backed up
- New state is set to `[DLP-BLOCKED]`
- You're notified of the detection

### Q: How deep should my processing be?
**A:**
- **1-3**: Quick analysis
- **4-7**: Standard reasoning
- **8-10**: Deep analysis (slower)

---

## Architecture Questions

### Q: What are the 4 core interfaces?
**A:**
1. **AbstractCore** — Base reasoning layer
2. **IAimInterface** — Goal-directed processing
3. **IDepthInterface** — Recursive depth control
4. **IStateInterface** — State management

[Learn more](Architecture-Overview)

### Q: How does DLP work?
**A:** It detects:
- Image-like content (`.png`, `.jpg`, `base64`)
- Meme keywords
- Suspicious binary payloads

Then it:
- Backs up your current state
- Blocks the update
- Sets state to `[DLP-BLOCKED]`

[Detailed guide](DLP)

### Q: Why use interfaces?
**A:** Because they:
- **Separate concerns** — Each interface handles one thing
- **Enable testing** — Mock implementations for unit tests
- **Allow flexibility** — Easy to swap implementations
- **Improve readability** — Clear what each part does

---

## Development Questions

### Q: How do I add a feature?
**A:**
1. Update `Program.cs` with your feature
2. Add unit tests to `DeepLearningProtocol.Tests/`
3. Run `dotnet test` to verify
4. Submit a pull request

[Contributing guide](Contributing)

### Q: How do I run tests?
**A:**
```bash
dotnet test
dotnet test --verbosity detailed
dotnet test -- --filter TestMethodName
```

### Q: Can I debug?
**A:** Yes! Press `F5` in VS Code to debug with breakpoints.

### Q: How are tests organized?
**A:**
- **Unit tests** for each interface (IAim, IDepth, IState)
- **Integration tests** for AbstractCore
- **Edge case tests** for DLP detection
- **8 tests total, all passing**

[Testing guide](Testing-Guide)

---

## Deployment Questions

### Q: Can I use this in production?
**A:** The protocol itself is production-ready, but consider:
- **Single-threaded** — Add async/await for concurrency
- **In-memory** — Add persistence (DB/JSON) if needed
- **Interactive** — Wrap as API if needed

### Q: How do I release binaries?
**A:** GitHub Actions automatically creates multi-platform releases:
- Linux x64 (self-contained)
- Windows x64 (self-contained)
- macOS x64 (self-contained)
- Framework-dependent (portable)

Triggered on push to `main` branch.

[CI/CD guide](CI-CD-Pipeline)

### Q: Can I use it commercially?
**A:** Yes! It's MIT licensed. See [Distribution Policy](Distribution-Policy).

---

## Troubleshooting Questions

### Q: Build fails with "entry point" error
**A:** Make sure you have only one `Main()` method. The protocol defines `Program.Main()` — don't add another.

### Q: Tests timeout
**A:** Increase timeout in VS Code's `settings.json`:
```json
"dotnet.test.arguments": ["--timeout", "60000"]
```

### Q: DLP blocks legitimate content
**A:** DLP is conservative to prevent data loss. If content is blocked:
- Check if it contains image keywords
- Check if it's a very long single line
- Consider the detection logic in [DLP guide](DLP)

### Q: Can't find .NET SDK
**A:** Install from [dotnet.microsoft.com](https://dotnet.microsoft.com/download)

```bash
dotnet --version
```

---

## Contributing Questions

### Q: How do I contribute?
**A:**
1. Fork the repository
2. Create a feature branch
3. Make changes + add tests
4. Submit a pull request

[Full guide](Contributing)

### Q: What's the code style?
**A:**
- C# conventions (PascalCase for public)
- XML comments on public methods
- Unit tests for all features
- Pass `dotnet test`

### Q: Can I suggest features?
**A:** Yes! [Open an issue](https://github.com/quickattach0-tech/DeepLearningProtocol/issues) to discuss.

---

## Still Have Questions?

- 📖 Read the full [Architecture Guide](Architecture-Overview)
- 🤝 Open an [issue](https://github.com/quickattach0-tech/DeepLearningProtocol/issues)
- 📧 Check the [Contributing guide](Contributing) for contact info
