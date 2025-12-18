# Architecture Overview

Complete guide to the Deep Learning Protocol architecture.

## System Design

The protocol implements a **hierarchical 4-layer reasoning system** with data protection:

```
┌────────────────────────────────────────────────────────┐
│              AbstractCore                              │
│         (Deepest Reasoning Layer)                      │
└────────────────────────────────────────────────────────┘
                     ▲
                     │
        ┌────────────┼────────────┐
        │            │            │
    ┌───▼───┐  ┌─────▼─────┐ ┌──▼──────┐
    │ Aim   │  │ Depth     │ │ State   │
    │Inter- │  │ Inter-    │ │ Inter-  │
    │face   │  │ face      │ │ face    │
    └───┬───┘  └─────┬─────┘ └──┬──────┘
        │            │          │
        └────────────┼──────────┘
                 ▼
        ┌─────────────────────┐
        │ Data Loss           │
        │ Prevention (DLP)    │
        └─────────────────────┘
```

## Core Components

### 1. AbstractCore
**The Foundation Layer**

- Provides fundamental reasoning logic
- Base class for DeepLearningProtocol
- Method: `ProcessCoreReasoning(string input)`
- Wraps input in abstract processing notation

**Responsibility**: 
- Define core operations
- Provide base functionality
- Enable inheritance for specialized processing

**Example**:
```csharp
public virtual string ProcessCoreReasoning(string input)
{
    return $"[Abstract Core] Deep abstract processing: {input}";
}
```

---

### 2. IAimInterface
**Goal-Directed Processing**

- Sets and pursues strategic objectives
- Drives exploration paths toward goals
- Methods:
  - `SetAim(string goal)` — Set a new goal
  - `PursueAim(string currentState)` — Pursue the current aim

**Responsibility**:
- Define strategic objectives
- Guide decision-making
- Establish success criteria

**Example**:
```csharp
public string SetAim(string goal)
{
    _aim = goal;
    UpdateState($"Aiming: {_aim}");
    return $"Aim set to: {_aim}";
}
```

---

### 3. IDepthInterface
**Hierarchical Processing**

- Recursive application of abstract reasoning
- Configurable processing depth (1-10 levels)
- Method: `ProcessAtDepth(string input, int depthLevel)`

**Responsibility**:
- Control processing complexity
- Handle recursive operations
- Enable N-level analysis

**Example**:
```csharp
public string ProcessAtDepth(string input, int depthLevel)
{
    var processed = input;
    for (int i = 0; i < depthLevel; i++)
    {
        processed = ProcessCoreReasoning(processed);
    }
    return $"[Depth {depthLevel}] {processed}";
}
```

---

### 4. IStateInterface
**State Management**

- Tracks operational state
- Protected updates with DLP integration
- Methods:
  - `GetCurrentState()` — Retrieve state
  - `UpdateState(string newState)` — Update with protection

**Responsibility**:
- Track system state
- Manage state transitions
- Ensure consistency

**Example**:
```csharp
public string GetCurrentState() => _currentState;

public void UpdateState(string newState)
{
    _dlp.BackupState(_currentState);
    if (_dlp.IsPotentialMeme(newState))
    {
        _currentState = "[DLP-BLOCKED]";
        return;
    }
    _currentState = newState;
}
```

---

## Data Loss Prevention (DLP)

**Protective Layer** for state updates.

### What It Detects

✅ Meme-like content:
- File extensions: `.png`, `.jpg`, `.jpeg`
- Image data: `data:image/`, `base64,`
- Keywords: `"meme"`

✅ Binary payloads:
- Large single-line content (>200 chars, no newlines)

### What It Does

1. **Backs up** — Saves previous state with timestamp
2. **Blocks** — Prevents suspicious updates
3. **Logs** — Sets state to `[DLP-BLOCKED]` for visibility

### Backup Structure

```
.dlp_backups/
├── state_20231218_150530_123.txt
├── state_20231218_150540_456.txt
└── state_20231218_150550_789.txt
```

---

## Data Flow

### Normal Execution

```
User Input
    ▼
SetAim (IAimInterface)
    ▼
ProcessAtDepth (IDepthInterface)
    ▼
AbstractCore.ProcessCoreReasoning()
    ▼
PursueAim (IAimInterface)
    ▼
UpdateState with DLP Protection (IStateInterface)
    ▼
Result to User
```

### Protected State Update

```
User Updates State
    ▼
DLP.IsPotentialMeme()?
    ├─ YES → Block & Set [DLP-BLOCKED]
    └─ NO  → DLP.BackupState() → UpdateState()
    ▼
State Changed
```

---

## Integration Points

### With Interactive Menu
- StateInterface tracks menu selections
- AimInterface sets menu goals
- DepthInterface manages menu navigation depth

### With FAQ System
- StateInterface tracks current FAQ
- AimInterface provides answers
- DepthInterface navigates question hierarchy

### With Testing
- Each interface has dedicated unit tests
- Edge cases covered (depth limits, meme detection, etc.)
- 8 comprehensive tests (all passing ✅)

---

## Class Diagram

```
┌─────────────────────────────────┐
│    AbstractCore (Abstract)      │
│  + ProcessCoreReasoning()       │
└──────────────┬──────────────────┘
               │
               │ inherits & implements
               ▼
┌──────────────────────────────────────────┐
│    DeepLearningProtocol                  │
├──────────────────────────────────────────┤
│ IAimInterface:                           │
│  + SetAim(goal)                          │
│  + PursueAim(state)                      │
│                                          │
│ IDepthInterface:                         │
│  + ProcessAtDepth(input, depth)          │
│                                          │
│ IStateInterface:                         │
│  + GetCurrentState()                     │
│  + UpdateState(newState)                 │
│                                          │
│ - _dlp: DataLossPrevention              │
│ - ExecuteProtocol()                      │
└──────────────────────────────────────────┘

┌──────────────────────────────────┐
│  DataLossPrevention (Standalone) │
├──────────────────────────────────┤
│  + IsPotentialMeme(content)      │
│  + BackupState(state)            │
└──────────────────────────────────┘
```

---

## Execution Flow Example

**User Input**: "How to solve AI?"

**Execution Steps**:

1. **SetAim("Solve AI Problems")**
   - Updates aim to "Solve AI Problems"
   - Updates state to "Aiming: Solve AI Problems"

2. **ProcessAtDepth("How to solve AI?", 3)**
   - Layer 1: `[Abstract Core] ... How to solve AI?`
   - Layer 2: `[Abstract Core] ... [Abstract Core] ... How to solve AI?`
   - Layer 3: `[Abstract Core] ... [Abstract Core] ... [Abstract Core] ... How to solve AI?`

3. **PursueAim(depthOutput)**
   - Combines depth result with aim
   - Returns: `[Aim Pursuit] ... towards Solve AI Problems`

4. **UpdateState (DLP Protected)**
   - If content is suspicious → **blocked**
   - If safe → **backed up** and **updated**

---

## Next Steps

- 📖 Read [Getting Started](Getting-Started)
- 🧪 See [Testing Guide](Testing-Guide)
- 🤝 Contribute via [Contributing](Contributing)
