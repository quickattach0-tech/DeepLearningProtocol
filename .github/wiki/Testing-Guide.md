# Testing Guide

Complete guide to testing the Deep Learning Protocol.

## Overview

The project uses **XUnit** framework with **8 passing tests** covering:
- AbstractCore functionality
- IAimInterface operations
- IDepthInterface recursion
- IStateInterface management
- DataLossPrevention detection
- Integration scenarios

## Running Tests

### Quick Start
```bash
dotnet test
```

**Output**:
```
Test summary: total: 8, failed: 0, succeeded: 8, skipped: 0
Build succeeded.
```

### With Details
```bash
dotnet test --verbosity detailed
```

### Specific Test
```bash
dotnet test --filter TestMethodName
```

### With Code Coverage
```bash
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

---

## Test Structure

### File Location
```
DeepLearningProtocol.Tests/
└── DeepLearningProtocolTests.cs
```

### Test Organization

```csharp
public class DeepLearningProtocolTests
{
    // Category 1: Initialization Tests
    [Fact]
    public void Constructor_InitializesDefaultState() { }

    // Category 2: AbstractCore Tests
    [Fact]
    public void ProcessCoreReasoning_WithInput_ReturnsFormattedOutput() { }

    // Category 3: State Interface Tests
    [Fact]
    public void GetCurrentState_ReturnsInitialState() { }
    
    [Fact]
    public void UpdateState_WithValidInput_UpdatesState() { }

    // Category 4: Aim Interface Tests
    [Fact]
    public void SetAim_WithGoal_UpdatesAimAndState() { }

    // Category 5: Depth Interface Tests
    [Fact]
    public void ProcessAtDepth_WithDepth5_ProcessesRecursively() { }

    // Category 6: DLP Protection Tests
    [Fact]
    public void UpdateState_WithMemeContent_BlocksUpdate() { }

    // Category 7: Integration Tests
    [Fact]
    public void ExecuteProtocol_WithValidInputs_ReturnsResult() { }
}
```

---

## Writing Tests

### Test Template

```csharp
[Fact]
public void MethodName_WithCondition_ExpectedBehavior()
{
    // Arrange: Set up test data
    var protocol = new DeepLearningProtocol();
    var input = "test input";

    // Act: Execute the method
    var result = protocol.ProcessCoreReasoning(input);

    // Assert: Verify the result
    Assert.NotEmpty(result);
    Assert.Contains(input, result);
}
```

### Naming Convention

Use the pattern: `[MethodName]_[Condition]_[ExpectedResult]`

**Examples**:
- ✅ `SetAim_WithValidGoal_UpdatesAimAndState`
- ✅ `ProcessAtDepth_WithDepth10_ReturnsNestedOutput`
- ✅ `UpdateState_WithMemeContent_BlocksAndSetsState`

---

## XUnit Assertions

### Common Assertions

```csharp
// String assertions
Assert.Equal(expected, actual);
Assert.NotEmpty(text);
Assert.Contains("substring", text);
Assert.StartsWith("prefix", text);

// Numeric assertions
Assert.True(condition);
Assert.False(condition);
Assert.Equal(expected, actual);
Assert.InRange(value, min, max);

// Collection assertions
Assert.Empty(collection);
Assert.Single(collection);
Assert.Contains(item, collection);
```

### Example
```csharp
[Fact]
public void SetAim_WithValidGoal_UpdatesState()
{
    var protocol = new DeepLearningProtocol();
    var result = protocol.SetAim("Test Goal");

    // Assert the return value
    Assert.NotNull(result);
    Assert.Contains("Test Goal", result);

    // Assert the state was updated
    Assert.Equal("Aiming: Test Goal", protocol.GetCurrentState());
}
```

---

## Test Categories

### 1. Unit Tests

Test individual methods in isolation.

```csharp
[Fact]
public void ProcessCoreReasoning_WithInput_ReturnsWrappedOutput()
{
    var protocol = new DeepLearningProtocol();
    var result = protocol.ProcessCoreReasoning("test");
    
    Assert.Contains("[Abstract Core]", result);
    Assert.Contains("test", result);
}
```

### 2. Integration Tests

Test multiple components working together.

```csharp
[Fact]
public void ExecuteProtocol_WithAllInputs_ReturnsCompleteResult()
{
    var protocol = new DeepLearningProtocol();
    var result = protocol.ExecuteProtocol(
        initialInput: "question",
        goal: "test goal",
        depth: 2
    );
    
    Assert.NotNull(result);
    Assert.Contains("Aim Pursuit", result);
    Assert.Contains("test goal", result);
}
```

### 3. Edge Case Tests

Test boundary conditions.

```csharp
[Fact]
public void ProcessAtDepth_WithDepth1_ProcessesOnce()
{
    var protocol = new DeepLearningProtocol();
    var result = protocol.ProcessAtDepth("input", 1);
    
    Assert.Contains("[Abstract Core]", result);
    Assert.Contains("[Depth 1]", result);
}

[Fact]
public void ProcessAtDepth_WithDepth10_ProcessesTenTimes()
{
    var protocol = new DeepLearningProtocol();
    var result = protocol.ProcessAtDepth("input", 10);
    
    Assert.Contains("[Depth 10]", result);
    // Verify nested processing occurred
    Assert.DoesNotContain("[Depth 11]", result);
}
```

### 4. DLP Protection Tests

Test Data Loss Prevention functionality.

```csharp
[Fact]
public void UpdateState_WithMemeKeyword_BlocksUpdate()
{
    var protocol = new DeepLearningProtocol();
    protocol.UpdateState("Starting state");
    
    // Try to update with meme content
    protocol.UpdateState("meme content");
    
    // Verify it was blocked
    Assert.Equal("[DLP-BLOCKED]", protocol.GetCurrentState());
}

[Fact]
public void UpdateState_WithImageExtension_BlocksUpdate()
{
    var protocol = new DeepLearningProtocol();
    protocol.UpdateState("image.png");
    
    Assert.Equal("[DLP-BLOCKED]", protocol.GetCurrentState());
}
```

---

## Coverage Goals

### Coverage Targets

| Component | Target | Status |
|-----------|--------|--------|
| AbstractCore | 100% | ✅ |
| IAimInterface | 100% | ✅ |
| IDepthInterface | 100% | ✅ |
| IStateInterface | 100% | ✅ |
| DataLossPrevention | 90% | ✅ |
| Integration | 80% | ✅ |
| **Overall** | **>80%** | ✅ |

### What's Tested

✅ All public methods  
✅ All code paths  
✅ Error conditions  
✅ Boundary values  
✅ State transitions  
✅ DLP detection  

### What's NOT Tested

❌ Private methods (covered by public tests)  
❌ External dependencies (file I/O if added)  
❌ Performance benchmarks  

---

## Continuous Integration

### GitHub Actions

On every push:
```yaml
1. Run: dotnet build
2. Run: dotnet test
3. Collect: Code coverage
4. Report: Results to PR
```

### Local Verification

Before pushing, run:
```bash
dotnet clean
dotnet build
dotnet test --verbosity detailed
```

---

## Troubleshooting

### Tests Timeout

**Problem**: Test takes too long

**Solution**:
- Check for infinite loops in depth processing
- Use `--verbosity detailed` to see which test hangs
- Add timeout to test:

```csharp
[Fact(Timeout = 5000)]  // 5 second timeout
public void LongRunningTest() { }
```

### Test Fails Intermittently

**Problem**: Race conditions or state issues

**Solution**:
- Ensure each test creates its own `DeepLearningProtocol` instance
- Don't share state between tests
- Use `[Fact]` not `[Theory]` for isolated tests

### DLP Tests Failing

**Problem**: DLP detection seems wrong

**Solution**:
- Check detection logic in `DataLossPrevention.IsPotentialMeme()`
- Verify test input matches detection rules
- Review the [DLP Guide](DLP) for detection patterns

---

## Adding New Tests

### Checklist

- [ ] Test name follows convention
- [ ] Test is in correct category
- [ ] All assertions are clear
- [ ] Test passes locally
- [ ] Test has comments if complex
- [ ] Run `dotnet test` to verify
- [ ] All 8 tests still pass

### Example: Adding a New Method Test

1. **Add method to `Program.cs`**:
```csharp
public string NewMethod(string input)
{
    return ProcessCoreReasoning(input);
}
```

2. **Add test to `DeepLearningProtocolTests.cs`**:
```csharp
[Fact]
public void NewMethod_WithInput_ReturnsOutput()
{
    var protocol = new DeepLearningProtocol();
    var result = protocol.NewMethod("test");
    
    Assert.NotNull(result);
    Assert.Contains("test", result);
}
```

3. **Run tests**:
```bash
dotnet test
```

4. **Verify all tests pass**:
```
Test summary: total: 9, failed: 0, succeeded: 9
```

---

## Best Practices

✅ **DO**:
- Write one test per behavior
- Use descriptive names
- Keep tests simple and focused
- Test one thing at a time
- Use `Assert` methods appropriately

❌ **DON'T**:
- Test private methods directly
- Create complex test data
- Test multiple behaviors in one test
- Skip assertions
- Ignore test failures

---

## Resources

- [XUnit Documentation](https://xunit.net/)
- [Testing Best Practices](https://docs.microsoft.com/en-us/dotnet/core/testing/best-practices-for-writing-unit-tests)
- [Architecture Guide](Architecture-Overview)
- [Contributing Guide](Contributing)

---

**Happy Testing!** 🧪✅
