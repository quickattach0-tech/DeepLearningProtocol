using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DeepLearningProtocol
{
    /// <summary>
    /// Manages code file storage, retrieval, and review workflows
    /// </summary>
    public class CodeManager
    {
        private readonly ProtocolDbContext _context;

        public CodeManager(ProtocolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Store a code file in the database
        /// </summary>
        public int StoreCodeFile(string filePath, string purpose = "")
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"File not found: {filePath}");

                var fileInfo = new FileInfo(filePath);
                var codeContent = File.ReadAllText(filePath);
                var lineCount = codeContent.Split('\n').Length;
                var language = GetLanguageFromExtension(fileInfo.Extension);
                var relativePath = GetRelativePath(filePath);

                var codeFile = new CodeFile
                {
                    FileName = fileInfo.Name,
                    FilePath = relativePath,
                    CodeContent = codeContent,
                    Language = language,
                    FileSizeBytes = (int)fileInfo.Length,
                    LineCount = lineCount,
                    SourceModifiedAt = fileInfo.LastWriteTime,
                    StoredAt = DateTime.UtcNow,
                    Purpose = purpose,
                    ReviewStatus = "New",
                    IsActive = true
                };

                _context.CodeFiles.Add(codeFile);
                _context.SaveChanges();

                return codeFile.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error storing code file: {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Retrieve a code file for review
        /// </summary>
        public CodeFile GetCodeFile(int id)
        {
            var file = _context.CodeFiles.FirstOrDefault(f => f.Id == id && f.IsActive);
            if (file != null)
            {
                file.ReviewCount++;
                file.LastReviewedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }
            return file;
        }

        /// <summary>
        /// Get code file by file name
        /// </summary>
        public CodeFile GetCodeFileByName(string fileName)
        {
            return _context.CodeFiles.FirstOrDefault(f => f.FileName == fileName && f.IsActive);
        }

        /// <summary>
        /// Get all code files
        /// </summary>
        public List<CodeFile> GetAllCodeFiles()
        {
            return _context.CodeFiles.Where(f => f.IsActive).OrderBy(f => f.FileName).ToList();
        }

        /// <summary>
        /// Get code files by language
        /// </summary>
        public List<CodeFile> GetCodeFilesByLanguage(string language)
        {
            return _context.CodeFiles
                .Where(f => f.Language == language && f.IsActive)
                .OrderBy(f => f.FileName)
                .ToList();
        }

        /// <summary>
        /// Get code files by review status
        /// </summary>
        public List<CodeFile> GetCodeFilesByReviewStatus(string status)
        {
            return _context.CodeFiles
                .Where(f => f.ReviewStatus == status && f.IsActive)
                .OrderByDescending(f => f.StoredAt)
                .ToList();
        }

        /// <summary>
        /// Update review status and notes
        /// </summary>
        public void UpdateReviewStatus(int fileId, string status, string notes = "", string suggestedUpdates = "")
        {
            var file = _context.CodeFiles.FirstOrDefault(f => f.Id == fileId);
            if (file != null)
            {
                file.ReviewStatus = status;
                file.ReviewNotes = notes;
                file.SuggestedUpdates = suggestedUpdates;
                file.LastReviewedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Add a code review record
        /// </summary>
        public int AddCodeReview(int fileId, string reviewType, string feedback = "", 
            string issuesFound = "", string recommendedChanges = "", int qualityScore = 75)
        {
            var review = new CodeReview
            {
                CodeFileId = fileId,
                ReviewType = reviewType,
                Feedback = feedback,
                IssuesFound = issuesFound,
                RecommendedChanges = recommendedChanges,
                QualityScore = qualityScore,
                ReviewedAt = DateTime.UtcNow,
                Priority = qualityScore < 60 ? 8 : qualityScore < 80 ? 5 : 2
            };

            _context.CodeReviews.Add(review);
            _context.SaveChanges();

            return review.Id;
        }

        /// <summary>
        /// Get reviews for a code file
        /// </summary>
        public List<CodeReview> GetCodeReviews(int fileId)
        {
            return _context.CodeReviews
                .Where(r => r.CodeFileId == fileId)
                .OrderByDescending(r => r.ReviewedAt)
                .ToList();
        }

        /// <summary>
        /// Display code file with line numbers for console review
        /// </summary>
        public void DisplayCodeForReview(int fileId, bool showSummaryOnly = false)
        {
            var file = GetCodeFile(fileId);
            if (file == null)
            {
                Console.WriteLine("Code file not found.");
                return;
            }

            Console.WriteLine("\n" + new string('=', 80));
            Console.WriteLine($"FILE: {file.FilePath}");
            Console.WriteLine($"LANGUAGE: {file.Language} | SIZE: {file.FileSizeBytes} bytes | LINES: {file.LineCount}");
            Console.WriteLine($"STATUS: {file.ReviewStatus} | REVIEWS: {file.ReviewCount} | LAST REVIEWED: {file.LastReviewedAt:yyyy-MM-dd HH:mm}");
            
            if (!string.IsNullOrEmpty(file.Purpose))
                Console.WriteLine($"PURPOSE: {file.Purpose}");
            
            Console.WriteLine(new string('=', 80));

            if (showSummaryOnly)
            {
                Console.WriteLine("\n[SUMMARY MODE - First 30 and last 10 lines shown]");
                DisplayCodeLines(file, true);
            }
            else
            {
                Console.WriteLine("\n[FULL CODE]");
                DisplayCodeLines(file, false);
            }

            if (!string.IsNullOrEmpty(file.ReviewNotes))
            {
                Console.WriteLine("\n" + new string('-', 80));
                Console.WriteLine($"REVIEW NOTES: {file.ReviewNotes}");
            }

            if (!string.IsNullOrEmpty(file.SuggestedUpdates))
            {
                Console.WriteLine($"SUGGESTED UPDATES: {file.SuggestedUpdates}");
            }

            Console.WriteLine(new string('=', 80) + "\n");
        }

        private void DisplayCodeLines(CodeFile file, bool summarize)
        {
            var lines = file.CodeContent.Split('\n');
            
            if (summarize && lines.Length > 40)
            {
                for (int i = 0; i < 30; i++)
                    Console.WriteLine($"{i + 1,4}: {lines[i]}");
                
                Console.WriteLine($"\n... ({lines.Length - 40} lines omitted) ...\n");
                
                for (int i = lines.Length - 10; i < lines.Length; i++)
                    Console.WriteLine($"{i + 1,4}: {lines[i]}");
            }
            else
            {
                for (int i = 0; i < lines.Length; i++)
                    Console.WriteLine($"{i + 1,4}: {lines[i]}");
            }
        }

        /// <summary>
        /// Display all code files index
        /// </summary>
        public void DisplayCodeFilesIndex()
        {
            var files = GetAllCodeFiles();
            if (!files.Any())
            {
                Console.WriteLine("No code files stored in repository.");
                return;
            }

            Console.WriteLine("\n" + new string('=', 100));
            Console.WriteLine("CODE REPOSITORY INDEX");
            Console.WriteLine(new string('=', 100));
            Console.WriteLine("{0,-5} {1,-30} {2,-10} {3,-12} {4,-8} {5,-20}", "ID", "File Name", "Language", "Status", "Lines", "Last Reviewed");
            Console.WriteLine(new string('-', 100));

            foreach (var file in files)
            {
                var lastReviewed = file.LastReviewedAt?.ToString("yyyy-MM-dd HH:mm") ?? "Never";
                Console.WriteLine($"{file.Id,-5} {file.FileName,-30} {file.Language,-10} {file.ReviewStatus,-12} {file.LineCount,-8} {lastReviewed,-20}");
            }

            Console.WriteLine(new string('=', 100) + "\n");
        }

        /// <summary>
        /// Auto-store all project source files
        /// </summary>
        public int StoreProjectSourceFiles(string projectPath)
        {
            int count = 0;
            try
            {
                var projectDir = new DirectoryInfo(projectPath);
                if (!projectDir.Exists)
                {
                    Console.WriteLine($"Project path not found: {projectPath}");
                    return 0;
                }

                var extensions = new[] { ".cs", ".csproj", ".json", ".xml", ".md" };
                var files = projectDir.GetFiles("*", SearchOption.AllDirectories)
                    .Where(f => extensions.Contains(f.Extension) && !f.FullName.Contains("bin") && !f.FullName.Contains("obj"))
                    .ToList();

                foreach (var file in files)
                {
                    if (GetCodeFileByName(file.Name) == null)
                    {
                        if (StoreCodeFile(file.FullName) > 0)
                            count++;
                    }
                }

                Console.WriteLine($"Stored {count} source files to code repository.");
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error storing project files: {ex.Message}");
                return count;
            }
        }

        /// <summary>
        /// Display code review workflow menu
        /// </summary>
        public void DisplayCodeReviewWorkflow()
        {
            Console.WriteLine("\n" + new string('=', 80));
            Console.WriteLine("CODE REVIEW WORKFLOW");
            Console.WriteLine(new string('=', 80));
            Console.WriteLine(@"
1. NEW - Initial state when code is stored
2. IN_REVIEW - Code is being reviewed (examine, test, validate)
3. NEEDS_UPDATES - Review found issues requiring changes
4. APPROVED - Code passed review and is production-ready
5. DEPRECATED - Code is no longer in use

Quality Score Guide (0-100):
  0-40:   Critical issues - immediate fixes required
  40-70:  Minor issues - improvements recommended
  70-85:  Good - minor enhancements suggested
  85-95:  Excellent - code meets standards
  95-100: Outstanding - exemplary code

Review Workflow:
  1. Display code and existing reviews
  2. Identify issues/improvements
  3. Assign quality score
  4. Update status (New → In_Review → Needs_Updates/Approved → Deprecated)
  5. Add detailed feedback and recommendations
  6. Track progress with priority levels (1=low, 10=critical)
");
            Console.WriteLine(new string('=', 80) + "\n");
        }

        private string GetLanguageFromExtension(string extension)
        {
            return extension.ToLower() switch
            {
                ".cs" => "C#",
                ".csproj" => "XML",
                ".json" => "JSON",
                ".xml" => "XML",
                ".md" => "Markdown",
                ".txt" => "Text",
                ".sh" => "Bash",
                ".yml" => "YAML",
                _ => "Unknown"
            };
        }

        private string GetRelativePath(string fullPath)
        {
            var projectRoot = Directory.GetCurrentDirectory();
            if (fullPath.StartsWith(projectRoot))
                return fullPath.Substring(projectRoot.Length + 1).Replace("\\", "/");
            return fullPath;
        }
    }
}
