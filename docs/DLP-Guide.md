# Data Loss Prevention (DLP) Guide

## What is DLP?

Data Loss Prevention is a protective layer that monitors state updates and blocks suspicious content to prevent accidental data loss.

When you use the Deep Learning Protocol, your state (progress) is precious. DLP ensures that accidental or malicious content doesn't corrupt it.

## Core Concept

**Before any state change → DLP checks → If suspicious, backup & block → Otherwise, allow**

```
UpdateState("new value")
    │
    ├─ DLP.IsSuspiciousContent("new value")?
    │
    ├─ YES → Backup current state & Block update
    │        State becomes "[DLP-BLOCKED]"
    │
    └─ NO → Allow update
             State becomes "new value"
```

## Detection Rules

The `DataLossPrevention.IsSuspiciousContent()` method detects:

### 1. Image File Extensions

Files with image extensions indicate meme or binary content:
- `.png`
- `.jpg`
- `.jpeg`

**Example:**
```
Detected: "cat_meme.png" → BLOCKED
Reason: .png extension suggests image/meme
```

### 2. Image Data URIs

Base64-encoded or inline image data:
- Contains `data:image/`
- Contains `base64,`

**Example:**
```
Detected: "data:image/png;base64,iVBORw0KGgo..." → BLOCKED
Reason: Base64 image data detected
```

### 3. Keywords

Specific keywords indicate meme content:
- Contains `"meme"` (case-insensitive)

**Example:**
```
Detected: "This is a funny meme lol" → BLOCKED
Reason: Word "meme" detected
```

### 4. Large Payloads

Single-line payloads exceeding threshold:
- Length > 200 characters
- No newline characters (single line)

**Example:**
```
Detected: "aaaaaaa...aaaaaaa" (250 chars, single line) → BLOCKED
Reason: Large single-line payload (likely binary/encoded)

Allowed: "aaaaaaa...aaaaaaa\naaaaaaa...aaaaaaa" (500+ chars, multiple lines)
Reason: Multi-line text is reasonable
```

## Backup Mechanism

### How Backups Work

When DLP blocks an update, it preserves the current state:

```csharp
public void BackupState(string currentState)
{
    // Create backups directory if needed
    Directory.CreateDirectory("./.dlp_backups");
    
    // Generate timestamp-based filename
    string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
    string filePath = $"./.dlp_backups/{timestamp}.txt";
    
    // Save current state
    File.WriteAllText(filePath, currentState);
}
```

### Backup Locations

Backups are saved in: `./.dlp_backups/`

**Filename Format:** `YYYY-MM-DD_HH-MM-SS-fff.txt`

**Example:**
```
./.dlp_backups/
├── 2025-12-18_14-30-45-123.txt
├── 2025-12-18_14-35-22-456.txt
└── 2025-12-18_14-40-10-789.txt
```

### Recovery

To restore a previous state:

```bash
# List all backups (newest first)
ls -lt ./.dlp_backups/

# View a specific backup
cat "./.dlp_backups/2025-12-18_14-30-45-123.txt"

# Restore manually (copy content back to your program state)
```

## Scenarios

### Scenario 1: Normal Update (Allowed)

```
Current State: "Aiming: Problem Solving"
New Input: "Code Quality Improvement"

DLP Check:
  ✓ No image extensions
  ✓ No data URIs
  ✓ No "meme" keyword
  ✓ Length < 200 OR has newlines
  
Result: UPDATE ALLOWED
New State: "Code Quality Improvement"
```

### Scenario 2: Meme Detection (Blocked)

```
Current State: "Processing..."
New Input: "funny_cat_meme.jpg"

DLP Check:
  ✗ Has .jpg extension (image file)
  
Result: UPDATE BLOCKED
Action: Backup "Processing..." to ./.dlp_backups/2025-12-18_14-30-45-123.txt
New State: "[DLP-BLOCKED]"
```

### Scenario 3: Payload Overflow (Blocked)

```
Current State: "Initial"
New Input: "Lorem ipsum dolor sit amet consectetur adipiscing elit... 
           [continues for 250+ chars on single line]"

DLP Check:
  ✗ Length > 200 characters AND single line (no \n)
  
Result: UPDATE BLOCKED
Action: Backup "Initial" to ./.dlp_backups/2025-12-18_14-30-45-124.txt
New State: "[DLP-BLOCKED]"
```

### Scenario 4: Base64 Image Data (Blocked)

```
Current State: "Processing..."
New Input: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUl..."

DLP Check:
  ✗ Contains "data:image/" prefix
  
Result: UPDATE BLOCKED
Action: Backup "Processing..." to ./.dlp_backups/2025-12-18_14-30-45-125.txt
New State: "[DLP-BLOCKED]"
```

### Scenario 5: Multi-line Text (Allowed)

```
Current State: "Initial"
New Input: "This is a very long message that explains something complex
           and continues across multiple lines because it's detailed
           and properly formatted like actual text should be."

DLP Check:
  ✓ Even if > 200 chars, HAS newlines
  ✓ No image extensions
  ✓ No "meme" keyword
  ✓ No data URIs
  
Result: UPDATE ALLOWED
New State: "This is a very long message that..."
```

## Code Implementation

### IsSuspiciousContent Method

```csharp
public bool IsSuspiciousContent(string content)
{
    // Rule 1: Check for image file extensions
    if (content.Contains(".png") || content.Contains(".jpg") || content.Contains(".jpeg"))
        return true;
    
    // Rule 2: Check for image data URIs
    if (content.Contains("data:image/") || content.Contains("base64,"))
        return true;
    
    // Rule 3: Check for "meme" keyword
    if (content.IndexOf("meme", StringComparison.OrdinalIgnoreCase) >= 0)
        return true;
    
    // Rule 4: Check for large single-line payloads
    if (content.Length > 200 && !content.Contains("\n"))
        return true;
    
    return false;
}
```

### UpdateState Method (Protected)

```csharp
public void UpdateState(string newState)
{
    // DLP check before update
    if (dlp.IsSuspiciousContent(newState))
    {
        dlp.BackupState(currentState); // Save before blocking
        currentState = "[DLP-BLOCKED]"; // Block update
        return;
    }
    
    // Update allowed
    currentState = newState;
}
```

## Using DLP in Your Code

### When DLP Activates

DLP protects state in two places within the protocol:

1. **SetAim() method** — Protects goal updates
   ```csharp
   SetAim("My goal here");  // ← Checked by DLP
   ```

2. **ExecuteProtocol() method** — Protects final state
   ```csharp
   ExecuteProtocol(input, goal, depth);  // ← UpdateState checked by DLP
   ```

### What Happens If DLP Blocks

If suspicious content is detected:

1. ✅ Current state is backed up
2. 🚫 Update is blocked
3. ⚠️ State becomes `[DLP-BLOCKED]`
4. 💾 User can recover from backups

### Recovery Process

```bash
# 1. Check if DLP blocked something
# (Program will display state as "[DLP-BLOCKED]")

# 2. Find the backup
ls ./.dlp_backups/

# 3. View the backup
cat ./.dlp_backups/2025-12-18_14-30-45-123.txt

# 4. Run program again with different input
# (DLP will no longer be in BLOCKED state)
```

## Configuration

### Adjusting Thresholds

To customize DLP behavior, edit the `IsSuspiciousContent()` method in [Program.cs](../DeepLearningProtocol/Program.cs#L195):

**Change payload size limit:**
```csharp
// Original: if (content.Length > 200 && ...)
// New: if (content.Length > 500 && ...)  // Higher threshold
```

**Add custom keyword:**
```csharp
// Add to IsSuspiciousContent():
if (content.IndexOf("yourKeyword", StringComparison.OrdinalIgnoreCase) >= 0)
    return true;
```

**Add new file extension:**
```csharp
// Add to IsSuspiciousContent():
if (content.Contains(".gif") || content.Contains(".bmp"))
    return true;
```

## Best Practices

### ✅ Do's

- ✅ Use multi-line inputs for long text
- ✅ Check `./.dlp_backups/` after seeing `[DLP-BLOCKED]`
- ✅ Use descriptive, natural language inputs
- ✅ Keep goals under 200 characters (single line)

### ❌ Don'ts

- ❌ Don't try to pass base64-encoded data directly
- ❌ Don't include file paths with image extensions as input
- ❌ Don't use `[DLP-BLOCKED]` as input (avoid recursive blocking)

## Testing DLP

### Test 1: Image File Detection

```bash
# Run program, choose Protocol
# Enter question: "cat_meme.png"
# Expected: [DLP-BLOCKED]
# Backup created: ./.dlp_backups/YYYY-MM-DD_HH-MM-SS-fff.txt
```

### Test 2: Payload Overflow

```bash
# Enter question: "aaa...aaa" (250+ chars, no newlines)
# Expected: [DLP-BLOCKED]
```

### Test 3: Normal Input (Should Pass)

```bash
# Enter question: "How can I optimize my code?"
# Expected: Normal processing, no blocking
```

### Test 4: Multi-line Input (Should Pass)

```bash
# Enter question: "How can I improve my code?
# This is a multi-line question that's long but properly formatted."
# Expected: Normal processing, even if >200 chars
```

## FAQ

**Q: Can I disable DLP?**
A: Yes, but not recommended. To disable: comment out DLP check in UpdateState(), though you lose protection.

**Q: Are backups permanent?**
A: No, they're in `./.dlp_backups/` and can be deleted manually. Consider archiving important backups elsewhere.

**Q: What if I accidentally delete a backup?**
A: Unfortunately, backups in `./.dlp_backups/` are gone if deleted. Keep `.git/` tracking for recovery.

**Q: Can DLP block legitimate content?**
A: Yes, if it matches detection rules (e.g., discussing memes). Workarounds:
- Spell as "m3me" or "mem3"
- Use multi-line format
- Add context text before/after

**Q: How do I modify detection rules?**
A: Edit `IsSuspiciousContent()` in [Program.cs](../DeepLearningProtocol/Program.cs#L195). See Configuration section.

**Q: Does DLP affect performance?**
A: Negligible. String checks are O(n) and only run on state updates.

---

**Next:** Learn about [Testing](Testing.md) to verify DLP behavior.

**Previous:** Read [Architecture Guide](Architecture.md) for system overview.
