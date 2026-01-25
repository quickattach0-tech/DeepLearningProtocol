# Code Repository & Review System - v1.2.0

## Overview

The Code Repository & Review System is a comprehensive tool for storing, managing, and reviewing your application's source code directly within the database. It provides a complete workflow for code assessment, quality tracking, and version history.

## Features

### 1. **Code Storage**
- Automatically store project source files to database
- Supports: C#, XML, JSON, Markdown, Bash, YAML files
- Tracks file metadata: size, line count, modification timestamps
- Filters out build artifacts (bin/, obj/ directories)

### 2. **Code Review Workflow**
Structured review lifecycle for quality management:

```
NEW → IN_REVIEW → NEEDS_UPDATES/APPROVED → DEPRECATED
                        ↓
                  PRODUCTION READY
```

**Status Definitions:**
- **NEW**: Initial state when code is stored (default)
- **IN_REVIEW**: Code is being examined, tested, and evaluated
- **NEEDS_UPDATES**: Review identified issues requiring changes
- **APPROVED**: Code passed review and is production-ready
- **DEPRECATED**: Code is no longer in use or superseded

### 3. **Quality Scoring**
Quality scores (0-100) with interpretation:
- **0-40**: Critical issues - immediate fixes required
- **40-70**: Minor issues - improvements recommended
- **70-85**: Good - minor enhancements suggested
- **85-95**: Excellent - meets all standards
- **95-100**: Outstanding - exemplary code

### 4. **Review Records**
Each code review includes:
- Review type: Code, Documentation, Quality, Security, Architecture, Testing
- Quality score (0-100 scale)
- Feedback: General reviewer comments
- Issues Found: Specific problems identified
- Recommended Changes: Suggested improvements
- Priority level: 1-10 (higher = more urgent)
- Resolution status: Whether issues are fixed
- Timestamps: Complete audit trail

### 5. **Database Schema**

#### CodeFiles Table
```
- Id (int): Primary key
- FileName (string): File name with extension
- FilePath (string): Relative path from project root
- CodeContent (text): Full source code
- Language (string): Programming language (C#, JSON, XML, etc.)
- FileSizeBytes (int): Size in bytes
- LineCount (int): Number of lines
- SourceModifiedAt (datetime): Last modified time of source
- StoredAt (datetime): When stored in database
- LastReviewedAt (datetime): Most recent review date
- Purpose (string): File description/purpose
- ReviewStatus (string): Current workflow status
- ReviewNotes (string): Comments from reviews
- SuggestedUpdates (string): Recommended changes
- ReviewCount (int): Number of reviews conducted
- IsActive (bool): Whether file is currently active

Indexes: FileName, ReviewStatus, IsActive
```

#### CodeReviews Table
```
- Id (int): Primary key
- CodeFileId (int): Reference to CodeFile
- ReviewType (string): Type of review conducted
- Feedback (string): General reviewer feedback
- IssuesFound (string): Specific issues identified
- RecommendedChanges (string): Suggested improvements
- QualityScore (int): Score 0-100
- ReviewedAt (datetime): When review was performed
- IssuesResolved (bool): Whether issues are fixed
- Priority (int): Urgency level 1-10

Indexes: CodeFileId, Priority
```

## Menu Interface

### Main Menu Option 7: Code Repository & Review

```
1. Store Project Source Files
   - Scan project directory
   - Auto-identify language by extension
   - Skip binary and build directories
   - Store metadata and full code content

2. View Code Files Index
   - Display all stored files in table format
   - Show: ID, filename, language, status, lines, last reviewed
   - Quick reference for available files

3. Review Code File
   - Display full code with line numbers
   - Show file metadata and review history
   - Summary mode: First 30 + last 10 lines (for large files)
   - Full mode: Complete source code
   - Display review notes and suggested updates

4. Add Code Review Record
   - Select file to review
   - Enter review type (Code/Documentation/Quality/Security/etc.)
   - Assign quality score (0-100)
   - Add feedback, issues, and recommendations
   - Auto-calculates priority based on score

5. View Review Workflow
   - Display complete review process documentation
   - Explain status transitions
   - Show quality score interpretation
   - Best practices for reviews

6. Update Review Status
   - Change file status (New → In_Review → etc.)
   - Add or update review notes
   - Track suggested updates
   - Update LastReviewedAt timestamp

7. Get Files by Status
   - Filter files by current review status
   - View all files needing updates
   - Identify approved files
   - List deprecated code

8. Back to Main Menu
```

## Usage Examples

### Example 1: Store Project Source Code
```
Option 7 → 1: Store Project Source Files
Enter project path: /workspaces/DeepLearningProtocol/DeepLearningProtocol
→ Scans directory recursively
→ Stores MenuSystem.cs, Program.cs, CodeManager.cs, etc.
→ Output: "Stored 15 source files to code repository."
```

### Example 2: Review a Code File
```
Option 7 → 3: Review Code File
View files index (automatically displayed)
Enter file ID: 5
Show summary only? (y/n): y
→ Displays MenuSystem.cs with line numbers
→ Shows purpose, size, status, review history
→ Displays review notes and suggested updates
```

### Example 3: Add Code Review
```
Option 7 → 4: Add Code Review Record
Enter file ID: 5
Review type: Quality
Quality score: 82
Feedback: Well-structured menu system with good error handling
Issues found: Missing input validation in some menu options
Recommended changes: Add try-catch blocks for all user input parsing
→ Creates CodeReview record
→ Auto-sets priority to 5 (score 82 = mid priority)
→ Output: "✓ Code review added with quality score: 82"
```

### Example 4: Update Review Status
```
Option 7 → 6: Update Review Status
Enter file ID: 5
Status options: New, In_Review, Needs_Updates, Approved, Deprecated
New status: Approved
Review notes: Passed all quality checks, ready for production
Suggested updates: Consider input validation in v1.3.0
→ Updates CodeFile record
→ Sets LastReviewedAt timestamp
→ Output: "✓ Status updated to: Approved"
```

## Workflow Best Practices

### 1. **Initial Code Submission**
- Store project files using option 1
- All files start in "NEW" status
- Metadata (size, lines) auto-captured

### 2. **Code Review Process**
- Change status to "IN_REVIEW" (option 6)
- Thoroughly examine code (option 3)
- Add detailed review records (option 4)
- Score based on quality criteria
- Document all findings

### 3. **Issue Resolution**
- If issues found: Update status to "NEEDS_UPDATES"
- Include specific recommendations
- Set priority based on severity
- Re-review after fixes applied

### 4. **Approval & Archive**
- After fixes: Update status to "APPROVED"
- Approved code is production-ready
- Mark as "DEPRECATED" when superseded
- Keep full history for audit trail

### 5. **Quality Tracking**
- Review quality scores establish baseline
- Track improvements over time
- Identify problem areas
- Monitor review frequency

## Database Configuration

### Default Connection
```
Server=(localdb)\mssqllocaldb
Database=DeepLearningProtocol
Trusted_Connection=true
```

### Custom Connection
Set environment variable:
```bash
export DLP_CONNECTION_STRING="Server=myserver;Database=mydb;User Id=sa;Password=pwd;"
```

## Performance Considerations

### Indexes
- **CodeFiles.ReviewStatus**: Filter by status (NEW, APPROVED, etc.)
- **CodeFiles.FileName**: Quick file lookup
- **CodeFiles.IsActive**: Exclude deprecated files
- **CodeReviews.CodeFileId**: Find reviews for a file
- **CodeReviews.Priority**: Sort by urgency

### Large Files
- Use summary mode for files > 10,000 lines
- Review quality scores instead of full source for quick assessment
- Consider splitting very large files

### Query Optimization
- GetCodeFilesByReviewStatus() uses indexed filtering
- GetCodeReviews() orders by date (most recent first)
- Display methods pre-load minimal necessary data

## Advanced Features

### Programmatic API

```csharp
using (var context = new ProtocolDbContext())
{
    var manager = new CodeManager(context);
    
    // Store a file
    int fileId = manager.StoreCodeFile(
        "/path/to/file.cs", 
        "Core menu system"
    );
    
    // Get file for review
    var codeFile = manager.GetCodeFile(fileId);
    
    // Add review
    int reviewId = manager.AddCodeReview(
        fileId: 5,
        reviewType: "Quality",
        feedback: "Good structure",
        qualityScore: 85
    );
    
    // Update status
    manager.UpdateReviewStatus(
        fileId: 5,
        status: "Approved",
        notes: "Passed review"
    );
}
```

### File Language Support

| Extension | Language |
|-----------|----------|
| .cs | C# |
| .csproj | XML |
| .json | JSON |
| .xml | XML |
| .md | Markdown |
| .txt | Text |
| .sh | Bash |
| .yml | YAML |

## Troubleshooting

### Issue: "Code file not found" when reviewing
**Solution**: Verify file ID exists using option 2 (View Index)

### Issue: Large files slow to display
**Solution**: Use summary mode (option 3) for files > 5,000 lines

### Issue: Quality score seems low
**Solution**: Review "Issues Found" field and "Recommended Changes" for specific feedback

### Issue: Files not appearing in index
**Solution**: Verify they're marked as IsActive=true, not deprecated

## Version History

### v1.2.0 (Current)
- Code Repository & Review System
- CodeFile and CodeReview entities
- 8-option menu interface
- Quality scoring and workflow management
- Complete review documentation system

### v1.1.0
- Translation Database with SQL Integration
- Custom translation rules with priority matching
- Console text-to-database storage

### v1.0.0
- Interactive Protocol System
- Multi-language translator
- Core system architecture

## Future Enhancements

### Planned for v1.3.0
- Automated code quality analysis integration
- Comparison view for file versions
- Code metrics (complexity, test coverage, duplication)
- Email notifications for review assignments
- Batch review operations
- Export reviews to PDF/HTML reports
- Search full-text code content
- Tags and categorization system

### Planned for v1.4.0
- Integration with GitHub API
- Sync reviews with pull requests
- Multi-file review sessions
- Code clone detection
- Performance profiling data storage
- Security scan results tracking

## Related Documentation

- [TRANSLATION_DATABASE.md](TRANSLATION_DATABASE.md) - Translation rule management
- [Architecture.md](Architecture.md) - System architecture overview
- [Getting-Started.md](Getting-Started.md) - Installation and first run
- [Testing.md](Testing.md) - Test suite documentation

## Support & Issues

For issues, questions, or suggestions regarding the Code Repository system:
1. Check the troubleshooting section above
2. Review workflow documentation (option 5 in menu)
3. Check recent reviews and feedback (option 3)
4. Consult Architecture.md for database design details
