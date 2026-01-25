# Testing Guide

## Overview

The Deep Learning Protocol includes a comprehensive test suite with 7 XUnit tests covering all core functionality.

**Test Framework:** XUnit 2.9.2  
**Test Project:** `DeepLearningProtocol.Tests`  
**Test File:** [DeepLearningProtocolTests.cs](../DeepLearningProtocol.Tests/DeepLearningProtocolTests.cs)

## Running Tests

### Quick Start

```bash
# Run all tests
dotnet test

# Expected output:
# ✓ All tests pass (exit code 0)
# Test run time: ~1-2 seconds
```

### Run Specific Test

```bash
# Run single test by name
dotnet test --filter "GetCurrentState_ReturnsInitialState"

# Run tests matching pattern
dotnet test --filter "State"

# Run with verbose output
dotnet test --verbosity detailed
```

### Run with Coverage

```bash
# Generate code coverage report
dotnet test /p:CollectCoverage=true

# With detailed output
dotnet test --verbosity detailed /p:CollectCoverage=true
```

## Test Suite

### Test 1: GetCurrentState_ReturnsInitialState

**What it tests:** State interface retrieval

**Code:**
```csharp
[Fact]
public void GetCurrentState_ReturnsInitialState()
{
    var protocol = new DeepLearningProtocol();
    var state = protocol.GetCurrentState();
    
    Assert.Equal("Initial", state);
}
```

**Purpose:** Verify initial state is "Initial"  
**Category:** State Management  
**Expected:** PASS ✓

---

### Test 2: UpdateState_ChangesCurrentState

**What it tests:** State interface update

**Code:**
```csharp
[Fact]
public void UpdateState_ChangesCurrentState()
{
    var protocol = new DeepLearningProtocol();
    protocol.UpdateState("New State");
    var state = protocol.GetCurrentState();
    
    Assert.Equal("New State", state);
}
```

**Purpose:** Verify state can be updated  
**Category:** State Management  
**Expected:** PASS ✓

---

### Test 3: SetAim_UpdatesAimAndState

**What it tests:** Aim interface goal setting

**Code:**
```csharp
[Fact]
public void SetAim_UpdatesAimAndState()
{
    var protocol = new DeepLearningProtocol();
    protocol.SetAim("Test Goal");
    var state = protocol.GetCurrentState();
    
    Assert.Contains("Test Goal", state);
}
```

**Purpose:** Verify SetAim updates state with goal  
**Category:** Aim Management  
**Expected:** PASS ✓

---

### Test 4: PursueAim_ReturnsCoreResultWithAim

**What it tests:** Aim interface pursuit

**Code:**
```csharp
[Fact]
public void PursueAim_ReturnsCoreResultWithAim()
{
    var protocol = new DeepLearningProtocol();
    protocol.SetAim("Goal");
    var result = protocol.PursueAim();
    
    Assert.Contains("[Aim Pursuit]", result);
}
```

**Purpose:** Verify PursueAim returns formatted result  
**Category:** Aim Management  
**Expected:** PASS ✓

---

### Test 5: ProcessAtDepth_AppliesCorrectDepth_Depth0

**What it tests:** Depth processing with depth=0

**Code:**
```csharp
[Theory]
[InlineData(0)]
[InlineData(1)]
[InlineData(2)]
public void ProcessAtDepth_AppliesCorrectDepth(int depth)
{
    var protocol = new DeepLearningProtocol();
    var result = protocol.ProcessAtDepth("input", depth);
    
    // Verify number of [Abstract Core] layers equals depth
    int layerCount = result.Split(new[] { "[Abstract Core]" }, StringSplitOptions.None).Length - 1;
    Assert.Equal(depth, layerCount);
}
```

**Purpose:** Verify depth processing creates correct number of layers  
**Depth Values Tested:** 0, 1, 2  
**Category:** Depth Processing  
**Expected:** PASS ✓

---

### Test 6: ExecuteProtocol_FullFlow

**What it tests:** Complete protocol execution

**Code:**
```csharp
[Fact]
public void ExecuteProtocol_FullFlow()
{
    var protocol = new DeepLearningProtocol();
    var result = protocol.ExecuteProtocol("Question", "Goal", 2);
    
    Assert.Contains("[Aim Pursuit]", result);
    Assert.Contains("[Abstract Core]", result);
    Assert.Contains("Question", result);
}
```

**Purpose:** Verify complete workflow executes correctly  
**Category:** Integration  
**Expected:** PASS ✓

---

### Test 7: (Additional Tests May Be Present)

Check [DeepLearningProtocolTests.cs](../DeepLearningProtocol.Tests/DeepLearningProtocolTests.cs) for complete list.

## Test Categories

### State Management Tests
- ✓ GetCurrentState_ReturnsInitialState
- ✓ UpdateState_ChangesCurrentState

**Purpose:** Verify IStateInterface implementation  
**Coverage:** State getter and setter

### Aim Management Tests
- ✓ SetAim_UpdatesAimAndState
- ✓ PursueAim_ReturnsCoreResultWithAim

**Purpose:** Verify IAimInterface implementation  
**Coverage:** Goal setting and pursuing

### Depth Processing Tests
- ✓ ProcessAtDepth_AppliesCorrectDepth (Theory with 0, 1, 2)

**Purpose:** Verify IDepthInterface recursive layering  
**Coverage:** Depth levels 0-2

### Integration Tests
- ✓ ExecuteProtocol_FullFlow

**Purpose:** Verify complete workflow  
**Coverage:** Full protocol execution with depth, aim, state

## Writing New Tests

### Test Template

```csharp
[Fact]
public void MethodName_Scenario_ExpectedBehavior()
{
    // Arrange
    var protocol = new DeepLearningProtocol();
    var input = "test input";
    
    // Act
    var result = protocol.SomeMethod(input);
    
    // Assert
    Assert.NotNull(result);
    Assert.Contains("expected", result);
}
```

### Theory Test (Multiple Data Points)

```csharp
[Theory]
[InlineData("value1", "expected1")]
[InlineData("value2", "expected2")]
public void MethodName_Scenario_ExpectedBehavior(string input, string expected)
{
    // Arrange
    var protocol = new DeepLearningProtocol();
    
    // Act
    var result = protocol.SomeMethod(input);
    
    // Assert
    Assert.Contains(expected, result);
}
```

### Adding to Test Suite

1. Open [DeepLearningProtocolTests.cs](../DeepLearningProtocol.Tests/DeepLearningProtocolTests.cs)
2. Add method to `DeepLearningProtocolTests` class
3. Use `[Fact]` or `[Theory]` attribute
4. Follow AAA pattern (Arrange, Act, Assert)
5. Run `dotnet test` to verify

**Example:**
```csharp
[Fact]
public void MyNewTest_Scenario_ExpectedResult()
{
    // Arrange
    var protocol = new DeepLearningProtocol();
    
    // Act
    var result = protocol.ExecuteProtocol("test", "goal", 1);
    
    // Assert
    Assert.NotNull(result);
    Assert.Contains("[Aim Pursuit]", result);
}
```

## Test Best Practices

### ✅ Do's

- ✅ Use descriptive test names: `Method_Scenario_Expected`
- ✅ Follow AAA pattern: Arrange, Act, Assert
- ✅ Use `[Theory]` for multiple similar test cases
- ✅ Test happy path AND edge cases
- ✅ Keep tests focused (one assertion per scenario)
- ✅ Use meaningful assertion messages

### ❌ Don'ts

- ❌ Don't hardcode magic numbers (use variables)
- ❌ Don't mix multiple scenarios in one test
- ❌ Don't skip failed tests (fix them instead)
- ❌ Don't test internal implementation details
- ❌ Don't depend on test execution order

## XUnit Assertions

### Common Assertions

```csharp
// Equality
Assert.Equal(expected, actual);
Assert.NotEqual(notExpected, actual);

// String operations
Assert.Contains("substring", "string");
Assert.StartsWith("prefix", "string");
Assert.EndsWith("suffix", "string");

// Null checks
Assert.Null(obj);
Assert.NotNull(obj);

// Boolean
Assert.True(condition);
Assert.False(condition);

// Exceptions
Assert.Throws<ExceptionType>(() => method());
var ex = Assert.Throws<ExceptionType>(() => method());
Assert.Equal("message", ex.Message);

// Collections
Assert.Single(collection);
Assert.Empty(collection);
Assert.Contains(item, collection);
Assert.DoesNotContain(item, collection);
```

## Debugging Tests

### Run with Breakpoints

1. Open [DeepLearningProtocolTests.cs](../DeepLearningProtocol.Tests/DeepLearningProtocolTests.cs)
2. Set breakpoint by clicking line number
3. Run test: `dotnet test --filter "TestName"` (pauses at breakpoint)
4. Or use VS Code: F5 → select test file

### Verbose Output

```bash
dotnet test --verbosity detailed
```

Output shows each test step, useful for debugging failures.

## CI/CD Integration

Tests run automatically via GitHub Actions:

```yaml
# In .github/workflows/dotnet.yml
- name: Run tests
  run: dotnet test
```

**When tests run:**
- On push to `main`, `master`, `develop`
- On pull request
- Manually via GitHub Actions

**Success criteria:**
- All tests pass (exit code 0)
- Coverage not below threshold (if configured)

## Test Coverage

### Current Coverage

All public methods of `DeepLearningProtocol` are covered:
- ✓ GetCurrentState() — Test 1
- ✓ UpdateState() — Test 2
- ✓ SetAim() — Test 3
- ✓ PursueAim() — Test 4
- ✓ ProcessAtDepth() — Test 5
- ✓ ExecuteProtocol() — Test 6

### Increasing Coverage

To improve coverage:

1. **Test edge cases:**
   ```csharp
   [Theory]
   [InlineData(0)]
   [InlineData(-1)]  // Edge case: negative depth
   [InlineData(100)] // Edge case: very large depth
   public void ProcessAtDepth_EdgeCases(int depth) { }
   ```

2. **Test error conditions:**
   ```csharp
   [Fact]
   public void ExecuteProtocol_NullInput_HandlesGracefully()
   {
       var protocol = new DeepLearningProtocol();
       // Should handle null or throw specific exception
   }
   ```

3. **Test DLP integration:**
   ```csharp
   [Fact]
   public void UpdateState_SuspiciousContent_Blocked()
   {
       var protocol = new DeepLearningProtocol();
       protocol.UpdateState("meme_content.png");
       Assert.Equal("[DLP-BLOCKED]", protocol.GetCurrentState());
   }
   ```

## Troubleshooting

### Tests Fail to Run

**Problem:** `dotnet test` returns error  
**Solution:**
```bash
dotnet clean
dotnet build
dotnet test
```

### Build Fails

**Problem:** CS compilation errors  
**Solution:**
```bash
# Check syntax
dotnet build -v detailed

# Fix .csproj references if needed
```

### Timeout Errors

**Problem:** Test hangs indefinitely  
**Solution:**
- Add timeout to test: `[Fact(Timeout = 5000)]`
- Check for infinite loops in code
- Use `async` if test is I/O bound

### Assertion Failures

**Problem:** Assert.Equal fails with "expected X, got Y"  
**Solution:**
- Verify logic in code under test
- Add Debug.WriteLine() for logging
- Run with `--verbosity detailed` for more info

## Continuous Integration

### GitHub Actions Configuration

Tests run on:
- **Trigger:** Push to main/master/develop, Pull Requests
- **Platform:** ubuntu-latest
- **Runtime:** .NET 10.0
- **Steps:**
  1. Checkout code
  2. Setup .NET
  3. Restore dependencies
  4. Build project
  5. Run tests
  6. (Optional) Collect coverage

### Local Pre-commit Testing

Before pushing, always run:

```bash
# Full validation
dotnet clean && dotnet build && dotnet test
```

If all pass, safe to push.

## Performance Testing

### Measure Test Execution Time

```bash
# Time all tests
time dotnet test

# With detailed timing
dotnet test --logger "console;verbosity=detailed"
```

### Optimize Slow Tests

1. **Profile the code:**
   ```bash
   dotnet test --logger trx --collect:"XPlat Code Coverage"
   ```

2. **Parallelize test execution:**
   ```bash
   dotnet test -p:ParallelizeAssembly=true -p:ParallelizeTestCollections=true
   ```

3. **Cache expensive operations:**
   ```csharp
   private static readonly DeepLearningProtocol _cachedProtocol 
       = new DeepLearningProtocol();
   ```

---

**Next:** Learn about [DLP Protection](DLP-Guide.md) to test protection mechanisms.

**Previous:** Read [Architecture](Architecture.md) to understand what you're testing.
