# Core Translation (CT) System Guide

## Overview

**Core Translation (CT)** is a replacement for the previous Data Loss Prevention (DLP) system, offering enhanced capabilities through multi-language support, quality scoring, and 24-hour uptime tracking. It provides a comprehensive approach to content validation with global language awareness.

Version: **3.1+**

---

## 🌍 Core Features

### 1. **Multi-Language Support**

CT supports 4 languages with full translation capabilities:

| Language | Code | Primary Use | Example |
|----------|------|-------------|---------|
| **English** | `en` | Default/Control | Original content |
| **Spanish** | `es` | European Markets | "protocolo de aprendizaje profundo" |
| **Arabic** | `ar` | MENA Region | "بروتوكول التعلم العميق" |
| **French** | `fr` | European Markets | "protocole d'apprentissage profond" |

#### Language Enum
```csharp
public enum Language { 
    English, 
    Spanish, 
    Arabic, 
    French 
}
```

---

### 2. **Quality Scoring System**

Content is assessed on a **0-100 scale** with multiple criteria:

#### Scoring Heuristics

| Criterion | Impact | Logic |
|-----------|--------|-------|
| **Meme/Binary Detection** | -30 | Penalizes `.png`, `.jpg`, base64, "meme" keyword |
| **Large Single-Line Payloads** | -20 | Detects binary data patterns (>200 chars, no newlines) |
| **Punctuation Quality** | +15 | Rewards `.`, `!`, `?` presence |
| **Multi-line Structure** | +20 | Rewards proper formatting (50+ chars, contains `\n`) |
| **Length Validation** | -25 | Penalizes extremely short content (<10 chars) |

#### Score Interpretation

```
90-100: Excellent     → Accepted without restriction
70-89:  Good          → Accepted, tracked
50-69:  Fair          → Accepted with monitoring
30-49:  Poor          → Tracked, optional warning
0-29:   Reject        → Content blocked
```

#### Example Quality Assessment

```csharp
var qt = new CoreTranslation();

// Assess content quality
int score = qt.AssessQuality("This is well-formatted content with proper punctuation.");
// Returns: ~85 (good quality)

// Block low-quality content
int poorScore = qt.AssessQuality("test");
// Returns: ~25 (rejected)
```

---

### 3. **24-Hour Uptime Calendar**

Real-time tracking of system availability across 24 hourly buckets:

#### Uptime Calendar Structure

```csharp
Dictionary<int, int> uptimeCalendar = qt.GetUptimeCalendar();
// Output: { 0: 15, 1: 23, 2: 18, ..., 23: 12 }
// Key: Hour of day (0-23)
// Value: Number of events recorded in that hour
```

#### Recording Events

```csharp
// Record uptime event (called during quality checks)
qt.RecordUptimeEvent();

// Get calendar
var calendar = qt.GetUptimeCalendar();
// Shows which hours had activity

// Get availability percentage
int availability = qt.GetUptimePercentage();
// Returns: 75% (18 active hours out of 24)
```

#### Availability Calculation

```
Availability % = (Active Hours / 24) × 100
Example: 18 hours with events ÷ 24 = 75%
```

---

### 4. **Quality Metrics Storage**

All translations and assessments are stored in persistent metrics:

#### QualityMetric Structure

```csharp
public class QualityMetric
{
    public string? Content { get; set; }           // Original content
    public int QualityScore { get; set; }          // 0-100 score
    public DateTime Timestamp { get; set; }         // When assessed
    public Language DetectedLanguage { get; set; } // Source language
    public string? TranslatedContent { get; set; } // Translated version
}
```

#### Storing Metrics

```csharp
qt.StoreQualityMetric(
    content: "Original text",
    qualityScore: 85,
    language: CoreTranslation.Language.English,
    translatedContent: "Texto original"
);

// Metrics saved to: ./.ct_metrics/quality_YYYYMMDD.json
```

#### Retrieving Metrics

```csharp
// Get all metrics from last 24 hours
var metrics24h = qt.GetQualityMetrics(hoursBack: 24);

// Get all metrics from last week
var metrics7d = qt.GetQualityMetrics(hoursBack: 168);
```

---

## 🌐 Translation Methods

### Translation to Spanish

```csharp
string spanish = qt.Translate("deep learning protocol", CoreTranslation.Language.Spanish);
// Returns: "protocolo de aprendizaje profundo"
```

**Supported Translations:**
- `quality translation` → `traducción de calidad`
- `uptime calendar` → `calendario de disponibilidad`
- `24-hour availability` → `disponibilidad de 24 horas`
- `deep learning protocol` → `protocolo de aprendizaje profundo`
- `state interface` → `interfaz de estado`
- `aim interface` → `interfaz de objetivo`
- `depth interface` → `interfaz de profundidad`

### Translation to Arabic

```csharp
string arabic = qt.Translate("quality translation", CoreTranslation.Language.Arabic);
// Returns: "ترجمة الجودة"
```

**Supported Translations:**
- `quality translation` → `ترجمة الجودة`
- `uptime calendar` → `تقويم المدة الزمنية`
- `24-hour availability` → `توفر 24 ساعة`
- And more...

### Translation to French

```csharp
string french = qt.Translate("24-hour availability", CoreTranslation.Language.French);
// Returns: "disponibilité 24 heures"
```

**Supported Translations:**
- `quality translation` → `traduction de qualité`
- `uptime calendar` → `calendrier de disponibilité`
- `24-hour availability` → `disponibilité 24 heures`
- And more...

---

## 🔧 Integration with Deep Learning Protocol

### Default Usage in DeepLearningProtocol.cs

```csharp
public class DeepLearningProtocol : AbstractCore, IAimInterface, IDepthInterface, IStateInterface
{
    private readonly CoreTranslation _qt = new();

    public void UpdateState(string newState)
    {
        // Assess quality before changing state
        int qualityScore = _qt.AssessQuality(newState);
        
        // Block if quality score too low
        if (qualityScore < 30)
        {
            Console.WriteLine("CT: Content quality score too low. State update blocked.");
            _qt.StoreQualityMetric(newState, qualityScore, CoreTranslation.Language.English, "");
            _currentState = "[CT-BLOCKED]";
            return;
        }

        // Record uptime event
        _qt.RecordUptimeEvent();
        _currentState = newState;
    }
}
```

### Usage in StringCommandExecutor.cs

```csharp
public class StringCommandExecutor
{
    private readonly CoreTranslation _qt;

    public StringCommandExecutor(ProtocolDbContext context, DeepLearningProtocol protocol, CoreTranslation qt)
    {
        _qt = qt ?? new CoreTranslation();
    }

    public string ExecuteCommand(string commandName, string? input = null)
    {
        // Check Quality Translation protection
        if (command.ApplyDLPProtection)
        {
            var qualityScore = _qt.AssessQuality(command.CommandPattern);
            if (qualityScore < 30)
            {
                _qt.StoreQualityMetric(command.CommandPattern, qualityScore, 
                    CoreTranslation.Language.English, "");
                return "[CT-BLOCKED] Command pattern quality score too low.";
            }
            // Record uptime event
            _qt.RecordUptimeEvent();
        }
        // ... rest of execution
    }
}
```

---

## 📊 Data Storage

### Directory Structure

```
./.ct_metrics/
├── quality_20260125.json    # Daily quality metrics (JSON)
├── quality_20260126.json    # Daily quality metrics
└── uptime_20260125.log      # Hourly uptime log
```

### Sample Quality Metrics JSON

```json
{
  "Content": "This is well-formatted protocol documentation.",
  "QualityScore": 87,
  "Timestamp": "2026-01-25T14:30:45.123Z",
  "DetectedLanguage": "English",
  "TranslatedContent": "Esta es documentación de protocolo bien formateada."
}
```

### Sample Uptime Log

```
[2026-01-25 14:30:45] Hour 14: Event recorded
[2026-01-25 15:02:18] Hour 15: Event recorded
[2026-01-25 15:45:33] Hour 15: Event recorded
```

---

## 🎯 Best Practices

### 1. **Quality Threshold Configuration**

Adjust the quality threshold based on your use case:

```csharp
// Current default: score < 30 blocks content
// For stricter validation: score < 50 blocks content
// For looser validation: score < 10 blocks content

if (qualityScore < 30)  // Adjust this threshold
{
    // Block content
}
```

### 2. **Multi-Language Content Handling**

When processing international content:

```csharp
// Assess quality
int score = qt.AssessQuality(content);

// Determine language and translate
string spanish = qt.Translate(content, CoreTranslation.Language.Spanish);

// Store metrics
qt.StoreQualityMetric(content, score, CoreTranslation.Language.English, spanish);

// Track uptime
qt.RecordUptimeEvent();
```

### 3. **Uptime Monitoring**

Monitor system availability:

```csharp
// Get current availability
var calendar = qt.GetUptimeCalendar();
int availability = qt.GetUptimePercentage();

if (availability < 75)
{
    Console.WriteLine("Warning: System availability below 75%");
}
```

### 4. **Quality-Based Routing**

Route content based on quality scores:

```csharp
int score = qt.AssessQuality(content);

if (score >= 90)
{
    ProcessAsHighPriority(content);
}
else if (score >= 70)
{
    ProcessAsNormal(content);
}
else if (score >= 50)
{
    LogAndReview(content);
}
else
{
    BlockAndAlert(content);
}
```

---

## 🚀 Migration from DLP to CT

### Breaking Changes

| Aspect | DLP (Old) | CT (New) |
|--------|-----------|---------|
| **Method** | `IsPotentialMeme()` | `AssessQuality()` |
| **Return Value** | Boolean | Integer (0-100) |
| **Backup Method** | `BackupState()` | Metrics storage |
| **Storage** | `./.dlp_backups/` | `./.ct_metrics/` |
| **Languages** | None | 4 languages |

### Migration Code

```csharp
// OLD (DLP)
var dlp = new DataLossPrevention();
if (dlp.IsPotentialMeme(content)) {
    dlp.BackupState(content);
    // Block
}

// NEW (CT)
var qt = new CoreTranslation();
int score = qt.AssessQuality(content);
if (score < 30) {
    qt.StoreQualityMetric(content, score, CoreTranslation.Language.English, "");
    // Block
}
```

---

## 📈 Monitoring & Analytics

### Quality Score Trends

```csharp
var recentMetrics = qt.GetQualityMetrics(hoursBack: 24);

int avgScore = (int)recentMetrics.Average(m => m.QualityScore);
int minScore = recentMetrics.Min(m => m.QualityScore);
int maxScore = recentMetrics.Max(m => m.QualityScore);

Console.WriteLine($"24h Average Quality: {avgScore}/100");
Console.WriteLine($"Range: {minScore}-{maxScore}");
```

### Language Distribution

```csharp
var metrics = qt.GetQualityMetrics(hoursBack: 168);
var byLanguage = metrics
    .GroupBy(m => m.DetectedLanguage)
    .Select(g => new { Language = g.Key, Count = g.Count() });

foreach (var group in byLanguage)
{
    Console.WriteLine($"{group.Language}: {group.Count} assessments");
}
```

### Uptime SLA Tracking

```csharp
// Get 7-day availability
var calendar = qt.GetUptimeCalendar();
int activeHours = calendar.Count(x => x.Value > 0);
double availability = (activeHours / 24.0) * 100;

if (availability >= 99.5)
    Console.WriteLine("✓ Exceeds 99.5% SLA");
else
    Console.WriteLine($"⚠ Currently at {availability:F1}%");
```

---

## ❓ FAQ

**Q: What's the difference between DLP and CT?**

A: DLP was a binary (safe/unsafe) filter. CT is a nuanced 0-100 quality scoring system with multi-language support and uptime tracking, enabling fine-grained content validation.

**Q: Can I configure the quality threshold?**

A: Yes, currently hardcoded to 30 in the code. You can modify the threshold check to any value (0-100) based on your requirements.

**Q: How is quality score calculated?**

A: Multiple heuristics: penalizes meme/binary patterns and short content, rewards proper punctuation and structure. See "Scoring Heuristics" section above.

**Q: Can I add custom translation phrases?**

A: Currently, translations are static in the `Translate()` method. Extend the method to include your custom phrases in the translation dictionaries.

**Q: What happens to old DLP backups?**

A: DLP backups in `./.dlp_backups/` are not automatically migrated. Archive them manually if needed; CT uses `./.ct_metrics/` instead.

**Q: Does CT affect performance?**

A: Minimal impact. Quality assessment is O(n) in content length. Uptime tracking is O(1). Metrics storage is asynchronous (best-effort).

---

## 📚 Related Documentation

- [Architecture Guide](Architecture.md) — System design
- [Getting Started](Getting-Started.md) — Quick start guide
- [Testing Guide](Testing.md) — Writing tests for CT
- [README](../README.md) — Main project overview

---

## Version History

| Version | Date | Change |
|---------|------|--------|
| **3.1** | 2026-01-25 | Initial CT implementation |
| **3.0** | 2025-12-XX | Last DLP version |

---

**Need help?** Check the [GitHub Wiki](https://github.com/quickattach0-tech/DeepLearningProtocol/wiki) or open an issue!
