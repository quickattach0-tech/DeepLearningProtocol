using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeepLearningProtocol
{
    /// <summary>
    /// Represents a code file stored in the code repository database
    /// </summary>
    [Table("CodeFiles")]
    public class CodeFile
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// File name including extension (e.g., "MenuSystem.cs")
        /// </summary>
        [Required]
        [StringLength(256)]
        public string FileName { get; set; }

        /// <summary>
        /// Relative path from project root (e.g., "DeepLearningProtocol/MenuSystem.cs")
        /// </summary>
        [Required]
        [StringLength(512)]
        public string FilePath { get; set; }

        /// <summary>
        /// Full source code content
        /// </summary>
        [Required]
        public string CodeContent { get; set; }

        /// <summary>
        /// Programming language (C#, JSON, XML, Markdown, etc.)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Language { get; set; }

        /// <summary>
        /// File size in bytes
        /// </summary>
        public int FileSizeBytes { get; set; }

        /// <summary>
        /// Number of lines in file
        /// </summary>
        public int LineCount { get; set; }

        /// <summary>
        /// Last modified time of source file
        /// </summary>
        public DateTime? SourceModifiedAt { get; set; }

        /// <summary>
        /// When file was stored in database
        /// </summary>
        [Required]
        public DateTime StoredAt { get; set; }

        /// <summary>
        /// When file was last reviewed/updated in database
        /// </summary>
        public DateTime? LastReviewedAt { get; set; }

        /// <summary>
        /// Purpose or description of the file
        /// </summary>
        [StringLength(500)]
        public string Purpose { get; set; }

        /// <summary>
        /// Review status (New, Reviewed, Approved, Deprecated)
        /// </summary>
        [StringLength(50)]
        public string ReviewStatus { get; set; } = "New";

        /// <summary>
        /// Notes from review process
        /// </summary>
        [StringLength(1000)]
        public string ReviewNotes { get; set; }

        /// <summary>
        /// Suggested improvements or updates
        /// </summary>
        [StringLength(1000)]
        public string SuggestedUpdates { get; set; }

        /// <summary>
        /// Number of times code has been reviewed from console
        /// </summary>
        public int ReviewCount { get; set; }

        /// <summary>
        /// Whether this is the active version
        /// </summary>
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// Represents a review session for code files
    /// </summary>
    [Table("CodeReviews")]
    public class CodeReview
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Reference to CodeFile being reviewed
        /// </summary>
        [Required]
        public int CodeFileId { get; set; }

        /// <summary>
        /// Review type (Code, Documentation, Quality, Security, etc.)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ReviewType { get; set; }

        /// <summary>
        /// Reviewer feedback
        /// </summary>
        [StringLength(1000)]
        public string Feedback { get; set; }

        /// <summary>
        /// Issues found during review
        /// </summary>
        [StringLength(1000)]
        public string IssuesFound { get; set; }

        /// <summary>
        /// Recommended changes
        /// </summary>
        [StringLength(1000)]
        public string RecommendedChanges { get; set; }

        /// <summary>
        /// Quality score (0-100)
        /// </summary>
        public int QualityScore { get; set; }

        /// <summary>
        /// Review date and time
        /// </summary>
        [Required]
        public DateTime ReviewedAt { get; set; }

        /// <summary>
        /// Whether issues have been resolved
        /// </summary>
        public bool IssuesResolved { get; set; }

        /// <summary>
        /// Priority level for fixes (1-10, higher = more urgent)
        /// </summary>
        public int Priority { get; set; } = 5;
    }
}
