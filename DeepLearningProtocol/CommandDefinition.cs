using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DeepLearningProtocol
{
    /// <summary>
    /// CommandDefinition represents a string command stored in the database.
    /// Follows the Deep Learning Protocol pattern for extensible command execution.
    /// </summary>
    [Table("CommandDefinitions")]
    public class CommandDefinition
    {
        /// <summary>Unique identifier for the command</summary>
        [Key]
        public int Id { get; set; }

        /// <summary>Command name/trigger word</summary>
        [Required]
        [StringLength(100)]
        public string CommandName { get; set; } = string.Empty;

        /// <summary>Description of what the command does</summary>
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>The command execution pattern/logic</summary>
        [Required]
        [StringLength(2000)]
        public string CommandPattern { get; set; } = string.Empty;

        /// <summary>Category/type of command (e.g., "Protocol", "Data", "System")</summary>
        [StringLength(50)]
        public string Category { get; set; } = "Protocol";

        /// <summary>Whether the command is enabled</summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>Number of times this command has been executed</summary>
        public int ExecutionCount { get; set; } = 0;

        /// <summary>Timestamp when the command was created</summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Timestamp when the command was last modified</summary>
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Last execution timestamp</summary>
        public DateTime? LastExecutedAt { get; set; }

        /// <summary>Depth level for command execution (1-10, following protocol pattern)</summary>
        public int ExecutionDepth { get; set; } = 5;

        /// <summary>Whether to apply DLP protection during execution</summary>
        public bool ApplyDLPProtection { get; set; } = true;

        /// <summary>Command parameters in JSON format</summary>
        [StringLength(1000)]
        public string Parameters { get; set; } = "{}";

        /// <summary>Last execution result</summary>
        [StringLength(500)]
        public string LastExecutionResult { get; set; } = string.Empty;
    }
}
