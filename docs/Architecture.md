# Architecture Guide

## System Overview

The Deep Learning Protocol is a hierarchical reasoning engine with built-in data protection. It processes information through multiple layers, each adding context and depth.

```
┌─────────────────────────────────────────────────────────┐
│              Program (Interactive Menu)                 │
│         • FAQ Display • Protocol Runner                 │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│            DeepLearningProtocol (Orchestrator)          │
│  SetAim() → ProcessAtDepth() → PursueAim()             │
└────────────────────┬────────────────────────────────────┘
                     │
    ┌────────────────┼────────────────┐
    │                │                │
┌───▼──┐      ┌─────▼────┐    ┌─────▼──┐
│ Aim  │      │ Depth    │    │ State  │
│Store │      │Process   │    │Manage  │
└──────┘      └──────────┘    └────────┘
    │                │                │
    └────────────────┼────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│            AbstractCore (Processing Layer)              │
│    ProcessCoreReasoning() — Wraps input in context     │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│         DataLossPrevention (Protection Layer)           │
│   IsSuspiciousContent() • BackupState()                │
│   Detects memes, binary data, large payloads           │
└─────────────────────────────────────────────────────────┘
```

## Components

### 1. Program Class (User Interface)

**Responsibility:** Manage user interaction and menu system

**Key Methods:**
- `Main()` — Menu loop with 3 options (Protocol, FAQ, Exit)
- `RunInteractiveProtocol()` — Collect user input (question, goal, depth)
- `DisplayFAQ()` — Browse 10 FAQ Q&A pairs

**Code Location:** [Program.cs, lines 1-160](../DeepLearningProtocol/Program.cs#L1)

### 2. AbstractCore Class (Base Processing)

**Responsibility:** Fundamental reasoning layer

**Interface:** None (standalone base class)

**Key Method:**
```csharp
public virtual string ProcessCoreReasoning(string input)
{
    return $"[Abstract Core] Deep abstract processing: {input}";
}
```

**Purpose:** Wrap input in context layer (used recursively for depth processing)

**Code Location:** [Program.cs, lines 162-172](../DeepLearningProtocol/Program.cs#L162)

### 3. IStateInterface

**Responsibility:** State management and retrieval

**Method Signatures:**
```csharp
string GetCurrentState();
void UpdateState(string newState);
```

**Used By:** DeepLearningProtocol to track processing state

**Code Location:** [Program.cs, lines 174-179](../DeepLearningProtocol/Program.cs#L174)

### 4. IAimInterface

**Responsibility:** Goal-directed processing

**Method Signatures:**
```csharp
void SetAim(string aim);
string PursueAim();
```

**Purpose:** 
- `SetAim()` — Store user's goal
- `PursueAim()` — Retrieve aim with formatting

**Code Location:** [Program.cs, lines 181-186](../DeepLearningProtocol/Program.cs#L181)

### 5. IDepthInterface

**Responsibility:** Hierarchical depth-based processing

**Method Signatures:**
```csharp
string ProcessAtDepth(string input, int depth);
```

**Purpose:** Recursively apply AbstractCore reasoning N times

**Example:**
```
Depth 1: [Abstract Core] input
Depth 2: [Abstract Core] [Abstract Core] input
Depth 3: [Abstract Core] [Abstract Core] [Abstract Core] input
```

**Code Location:** [Program.cs, lines 188-193](../DeepLearningProtocol/Program.cs#L188)

### 6. DataLossPrevention Class (DLP)

**Responsibility:** Content protection and state preservation

**Key Methods:**
```csharp
public bool IsSuspiciousContent(string content)
{
    // Detects:
    // - Image files (.png, .jpg, .jpeg)
    // - Base64 encoded data
    // - Keywords ("meme")
    // - Large single-line payloads (>200 chars, no newlines)
    
    return isSuspicious;
}

public void BackupState(string currentState)
{
    // Saves to: ./.dlp_backups/{timestamp}.txt
    // Preserves state history for recovery
}
```

**Protection Logic:**
1. On `UpdateState()` call, DLP checks content
2. If suspicious, backs up current state
3. Blocks update → state becomes `[DLP-BLOCKED]`
4. Otherwise, allows normal update

**Code Location:** [Program.cs, lines 195-240](../DeepLearningProtocol/Program.cs#L195)

### 7. DeepLearningProtocol Class (Orchestrator)

**Responsibility:** Combine all interfaces and coordinate workflow

**Inherits:** IStateInterface, IAimInterface, IDepthInterface, AbstractCore

**Key Methods:**

#### SetAim(string aim)
- Stores goal
- Updates state: `"Aiming: {aim}"`
- Protected by DLP

#### ProcessAtDepth(string input, int depth)
- Recursively calls `ProcessCoreReasoning()` depth times
- Each call adds `[Abstract Core]` layer

#### PursueAim()
- Returns formatted string: `"[Aim Pursuit] {currentProcessing}"`
- Shows how aim influenced processing

#### ExecuteProtocol(string input, string goal, int depth)
- **Complete workflow:**
  1. `SetAim(goal)` — Store goal
  2. `ProcessAtDepth(input, depth)` — Apply depth processing
  3. `PursueAim()` — Return formatted result
  4. `UpdateState()` — Log completion
- **Returns:** Formatted result string

**Code Location:** [Program.cs, lines 242-330](../DeepLearningProtocol/Program.cs#L242)

## Data Flow

### Scenario: User Input Processing

```
User Input:
Question: "How can I optimize my code?"
Goal: "Performance improvement"
Depth: 2

     ↓

Program.Main()
  └─ RunInteractiveProtocol() collects input
       └─ Calls: DeepLearningProtocol.ExecuteProtocol()

     ↓

DeepLearningProtocol.ExecuteProtocol()
  1. SetAim("Performance improvement")
     └─ UpdateState() → [DLP checks] → State updated
  
  2. ProcessAtDepth("How can I optimize my code?", 2)
     └─ Recursively call AbstractCore.ProcessCoreReasoning()
        Iteration 1: "[Abstract Core] How can I optimize my code?"
        Iteration 2: "[Abstract Core] [Abstract Core] How can I optimize my code?"
  
  3. PursueAim()
     └─ Returns: "[Aim Pursuit] {processed_result}"
  
  4. UpdateState() → [DLP checks] → Final state saved

     ↓

Program.Main()
  └─ Displays formatted result to user
     └─ Shows state tracking
```

## Interface Hierarchy

```
┌────────────────────────────────────────┐
│      DeepLearningProtocol              │
│  (Implements all interfaces below)     │
└────────────────────────────────────────┘
         │             │             │
         │             │             │
    ┌────▼─────┐  ┌───▼────┐  ┌────▼──────┐
    │ IAimInterface      │IStateInterface   │IDepthInterface │
    │ SetAim()  │  │GetCurrentState() │ProcessAtDepth()    │
    │ PursueAim()│  │UpdateState()    │                  │
    └───────────┘  └──────────┘  └─────────┘
         │             │             │
         └─────────────┼─────────────┘
                       │
                   Extends
                       │
         ┌─────────────▼──────────────┐
         │      AbstractCore          │
         │ ProcessCoreReasoning()     │
         └────────────────────────────┘
```

## Execution Flow Diagram

```
Start
  │
  ├─ Main Loop
  │  ├─ Display Menu
  │  └─ Get User Choice
  │
  ├─ [Choice 1] Protocol
  │  └─ RunInteractiveProtocol()
  │     ├─ Prompt: Question
  │     ├─ Prompt: Goal
  │     ├─ Prompt: Depth (1-10)
  │     └─ Call ExecuteProtocol()
  │        ├─ SetAim()
  │        ├─ ProcessAtDepth()
  │        ├─ PursueAim()
  │        └─ UpdateState()
  │           └─ [DLP Protection]
  │
  ├─ [Choice 2] FAQ
  │  └─ DisplayFAQ()
  │     ├─ Show Questions 1-10
  │     └─ Display Selected Answer
  │
  └─ [Choice 3] Exit → End Program
```

## Key Design Patterns

### 1. Layered Architecture
- **UI Layer** — Program.cs Main/menu methods
- **Business Layer** — DeepLearningProtocol orchestration
- **Core Layer** — AbstractCore reasoning
- **Protection Layer** — DataLossPrevention

### 2. Interface Segregation
- Each interface has single responsibility:
  - `IStateInterface` — State only
  - `IAimInterface` — Goal only
  - `IDepthInterface` — Depth processing only

### 3. Composition over Inheritance
- DeepLearningProtocol inherits AbstractCore
- Implements multiple interfaces
- Composes with DataLossPrevention

### 4. Protection by Default
- All state updates protected by DLP
- Backups created before blocking
- User can recover from `./.dlp_backups/`

## State Management

### State Transitions

```
Initial State
    │
    ├─ User runs protocol
    │
    └─ SetAim("goal")
       └─ State: "Aiming: goal"
          [DLP: Check] → Allowed or Blocked
       
       └─ ProcessAtDepth()
          └─ State: "Depth N processed"
             [DLP: Check] → Allowed or Blocked
       
       └─ Complete
          └─ Final State: "Depth N processed"
             or "[DLP-BLOCKED]" if suspicious
```

### Backup Mechanism

When DLP blocks an update:
```
Current State: "Aiming: goal"
    │
    ├─ New Update: suspicious_content
    │
    ├─ DLP.IsSuspiciousContent() → true
    │
    ├─ DLP.BackupState("Aiming: goal")
    │  └─ Saves to: ./.dlp_backups/2025-12-18_14-30-45-123.txt
    │
    └─ State → "[DLP-BLOCKED]"
```

## Extension Points

### Add New Processing Layer

```csharp
// In Program.cs, extend AbstractCore:
public class SemanticLayer : AbstractCore
{
    public override string ProcessCoreReasoning(string input)
    {
        var baseProcessing = base.ProcessCoreReasoning(input);
        return $"[Semantic] {baseProcessing}";
    }
}
```

### Add New Validation Rule to DLP

```csharp
// In DataLossPrevention.IsSuspiciousContent():
if (content.Contains("forbiddenKeyword"))
{
    return true; // Block this content
}
```

### Add Custom Interface

```csharp
public interface ICustomInterface
{
    void CustomMethod();
}

// Implement in DeepLearningProtocol class
```

## Performance Considerations

### Time Complexity

- `ProcessAtDepth(input, depth)` — O(depth) because each iteration adds one layer
- `ExecuteProtocol()` — O(depth) (dominated by ProcessAtDepth)
- `IsSuspiciousContent()` — O(n) where n = content length

### Space Complexity

- State storage — O(1) (single string)
- Backups — O(n × m) where n = number of backups, m = average backup size
- Depth processing — O(depth) call stack

### Optimization Tips

1. **Limit depth** — Keep depth ≤ 10 for reasonable performance
2. **DLP checks** — Backups only on blocked content, not every update
3. **FAQ caching** — Dictionary is static, no runtime overhead

## Testing Architecture

### Test Categories

1. **State Tests** — Verify state get/update
2. **Aim Tests** — Verify goal setting/pursuing
3. **Depth Tests** — Verify recursive layering (theory with depths 0, 1, 2)
4. **Integration Tests** — Full ExecuteProtocol workflow

See [Testing Guide](Testing.md) for details.

---

**Next:** Read [DLP Guide](DLP-Guide.md) to understand data protection in detail.
