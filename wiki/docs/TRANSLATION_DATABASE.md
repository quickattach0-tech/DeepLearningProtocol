# Translation Database Management Guide

## Overview

The Deep Learning Protocol v1.1.0 includes a comprehensive translation database system using Entity Framework and SQL Server. This guide explains how to use the translation storage and rule management features.

## Architecture

### Database Components

**TranslationRule Entity** - Custom translation rule definitions
- Source text (English)
- Translations (Spanish, Arabic, French)
- Category classification
- Priority (1-10) for rule matching order
- Usage tracking
- Active/Inactive toggle

**TranslatedText Entity** - Stored translation records
- Source text
- All language translations
- Quality score (0-100)
- Manual verification flag
- View count
- Notes/comments
- Execution depth

**ProtocolDbContext** - Entity Framework DbContext
- Manages database connections
- Provides DbSet for both entities
- Configurable via environment variables
- Default: LocalDB

## Database Configuration

### Default Configuration

```
Server: (localdb)\mssqllocaldb
Database: DeepLearningProtocol
Authentication: Integrated (Windows)
```

### Custom Configuration

Set the `DLP_CONNECTION_STRING` environment variable:

```bash
# Windows
set DLP_CONNECTION_STRING=Server=your-server;Database=YourDB;User Id=sa;Password=YourPassword

# Linux/Mac
export DLP_CONNECTION_STRING=Server=your-server;Database=YourDB;User Id=sa;Password=YourPassword
```

## Using the Translation Database

### Menu Access

From the main menu, select:
- **Option 5: Translate & Store Text** - Store translations in database
- **Option 6: Manage Translation Rules** - Create/update/delete rules

### Option 5: Translate & Store Text

1. Enter text in English
2. System translates using rules + fallback translator
3. All translations stored with metadata:
   - Quality score (75 default)
   - Execution depth (5 default)
   - Timestamps
   - Manual verification flag

Example workflow:
```
Enter text to translate: "good morning"
↓
Spanish: buenos días (from translator)
Arabic: صباح الخير (from translator)
French: bonjour (from translator)
↓
Stored with ID and timestamps
```

### Option 6: Manage Translation Rules

#### 1. View All Rules
- Lists all custom translation rules
- Shows: Source text, Priority, Category, Usage count
- Sorted by priority (highest first)

#### 2. Create New Rule
Creates a custom translation rule:
```
Source text: "protocol analysis"
Spanish: "análisis de protocolo"
Arabic: "تحليل البروتوكول"
French: "analyse du protocole"
Category: Protocol (or Custom)
Priority: 1-10 (default: 5)
```

Rules with higher priority are checked first!

#### 3. Update Rule
Modify existing rule:
- Change translations
- Adjust priority
- Update category
- Keep source text (unique identifier)

#### 4. Delete Rule
Remove a rule completely from the database.

#### 5. View Translation History
Browse all stored translations:
- Source text
- Quality score
- Manual verification status
- View count

## Rule Priority System

Priority values (1-10):
- **1-3**: Low priority, checked last
- **4-7**: Medium priority (default: 5)
- **8-10**: High priority, checked first

When translating, system checks:
1. Custom rules (by priority, highest first)
2. Built-in translator dictionary
3. Word-by-word fallback

### Example Priority Ordering

```
Priority 9 "hello" → "hola especial"
Priority 7 "greeting" → "saludo"
Priority 5 "thank you" → "gracias"
Priority 1 "basic" → "básico"
```

When translating "hello", Priority 9 rule is used.

## Translation Quality Management

### Quality Score
- Range: 0-100
- Default: 75
- 100: Manually verified by user

### Verification Workflow
1. Translate & store text (quality: 75)
2. Review translation in database
3. Update quality score if needed (0-100)
4. Mark as "Manually Verified" (auto-sets to 100)

### Browsing Stored Translations
Access history:
- Source text and all translations
- Quality score
- Verification status
- View count
- Timestamps

## Programmatic Usage

Using TranslationManager in code:

```csharp
using var context = new ProtocolDbContext();
var manager = new TranslationManager(context);

// Translate and store
var translation = manager.TranslateAndStore("hello world");
Console.WriteLine($"ID: {translation.Id}");
Console.WriteLine($"Spanish: {translation.SpanishTranslation}");

// Create custom rule
manager.CreateRule(
    sourceText: "special greeting",
    spanish: "saludo especial",
    arabic: "تحية خاصة",
    french: "salutation spéciale",
    category: "Custom",
    priority: 8
);

// Update rule
manager.UpdateRule("special greeting", 
    spanish: "saludo muy especial",
    priority: 9);

// Get all rules
var rules = manager.GetAllRules();

// Verify translation
manager.VerifyTranslation(translationId);

// Update quality score
manager.UpdateQualityScore(translationId, 90);
```

## Database Indexes

**TranslationRules:**
- Unique index on SourceText (fast lookup)
- Index on IsActive (filter enabled rules)
- Index on Priority (rule ordering)

**TranslatedTexts:**
- Index on IsManuallyVerified (find verified translations)
- Index on QualityScore (find high/low quality)

## Features by Version

### v1.0.0
- Phrase translator (60+ phrases)
- Multi-language support
- System data translation bridge

### v1.1.0 (New)
- Translation database storage
- Custom rule management
- Priority-based rule matching
- Quality tracking
- Console text input to database
- Rule CRUD operations
- Translation history browser

## Best Practices

1. **Organize Rules by Priority**
   - Domain-specific: Priority 8-10
   - Common: Priority 5-7
   - Rare: Priority 1-3

2. **Use Categories**
   - Protocol: System-related terms
   - Medical: Health terminology
   - Technical: IT/Programming terms
   - Custom: Domain-specific

3. **Regular Quality Reviews**
   - Monitor quality scores
   - Mark verified translations
   - Remove low-quality rules

4. **Version Custom Rules**
   - Document when rules change
   - Track rule modifications
   - Keep notes for context

## Troubleshooting

### Database Connection Issues
- Verify LocalDB is installed: `sqllocaldb info`
- Check connection string
- Ensure database exists

### Rule Not Applied
- Check IsActive flag (must be true)
- Verify Priority is high enough
- Confirm exact text match (case-insensitive internally)

### Quality Scores Not Updating
- Use UpdateQualityScore() or VerifyTranslation()
- Remember: Manual verification sets score to 100

## Future Enhancements

- Translation rule versioning
- Bulk import/export of rules
- Rule statistics and analytics
- Machine learning-based quality scoring
- Rule suggestion based on usage patterns
- Multi-language source support
- Translation memory integration

## Related Documentation

- [Translator Feature Guide](TRANSLATOR_FEATURE.md)
- [Architecture Guide](Architecture.md)
- [Getting Started](Getting-Started.md)
