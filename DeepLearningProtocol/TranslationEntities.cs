using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeepLearningProtocol
{
    /// <summary>
    /// TranslationRule defines custom translation mappings stored in the database.
    /// Allows users to extend and customize the translator with domain-specific terms.
    /// </summary>
    [Table("TranslationRules")]
    public class TranslationRule
    {
        /// <summary>Unique identifier for the rule</summary>
        [Key]
        public int Id { get; set; }

        /// <summary>Source text to translate (typically in English)</summary>
        [Required]
        [StringLength(500)]
        public string SourceText { get; set; } = string.Empty;

        /// <summary>Spanish translation</summary>
        [Required]
        [StringLength(500)]
        public string SpanishTranslation { get; set; } = string.Empty;

        /// <summary>Arabic translation</summary>
        [Required]
        [StringLength(500)]
        public string ArabicTranslation { get; set; } = string.Empty;

        /// <summary>French translation</summary>
        [Required]
        [StringLength(500)]
        public string FrenchTranslation { get; set; } = string.Empty;

        /// <summary>Category/domain for the rule (e.g., "Medical", "Technical", "Protocol")</summary>
        [StringLength(50)]
        public string Category { get; set; } = "Custom";

        /// <summary>Whether this rule is active</summary>
        public bool IsActive { get; set; } = true;

        /// <summary>Priority for rule matching (higher = checked first)</summary>
        public int Priority { get; set; } = 5;

        /// <summary>Timestamp when the rule was created</summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Timestamp when the rule was last modified</summary>
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Number of times this rule has been used</summary>
        public int UsageCount { get; set; } = 0;
    }

    /// <summary>
    /// TranslatedText represents a text entry translated and stored in the database.
    /// Tracks source text, all translations, and metadata for translation history.
    /// </summary>
    [Table("TranslatedTexts")]
    public class TranslatedText
    {
        /// <summary>Unique identifier for the translation record</summary>
        [Key]
        public int Id { get; set; }

        /// <summary>Original source text in English</summary>
        [Required]
        [StringLength(2000)]
        public string SourceText { get; set; } = string.Empty;

        /// <summary>Spanish translation</summary>
        [StringLength(2000)]
        public string SpanishTranslation { get; set; } = string.Empty;

        /// <summary>Arabic translation</summary>
        [StringLength(2000)]
        public string ArabicTranslation { get; set; } = string.Empty;

        /// <summary>French translation</summary>
        [StringLength(2000)]
        public string FrenchTranslation { get; set; } = string.Empty;

        /// <summary>Whether the translation was manually corrected</summary>
        public bool IsManuallyVerified { get; set; } = false;

        /// <summary>Quality score of the translation (0-100)</summary>
        public int QualityScore { get; set; } = 75;

        /// <summary>Number of times this translation has been viewed</summary>
        public int ViewCount { get; set; } = 0;

        /// <summary>Notes or comments about the translation</summary>
        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;

        /// <summary>Timestamp when the translation was created</summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Timestamp when the translation was last modified</summary>
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Depth level used for protocol execution (1-10)</summary>
        public int ExecutionDepth { get; set; } = 5;
    }
}
