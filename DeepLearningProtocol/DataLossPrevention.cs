using System;
using System.IO;

namespace DeepLearningProtocol
{
    /// <summary>
    /// DataLossPrevention (DLP) is a protective layer that detects and blocks suspicious content.
    /// It implements heuristics to identify meme-like or binary payloads and creates automatic backups.
    /// This prevents accidental data corruption from user input.
    /// </summary>
    public class DataLossPrevention
    {
        /// <summary>Directory path for storing state backups</summary>
        private readonly string _backupDir = "./.dlp_backups";

        /// <summary>
        /// Initializes the DLP system and creates backup directory if needed.
        /// Gracefully handles IO errors in restricted environments.
        /// </summary>
        public DataLossPrevention()
        {
            try
            {
                Directory.CreateDirectory(_backupDir);
            }
            catch
            {
                // ignore errors creating backup dir in restricted environments
            }
        }

        /// <summary>
        /// Detects if content appears to be meme-like or binary data.
        /// Uses multiple heuristics: file extensions, data URIs, keywords, and payload size.
        /// </summary>
        /// <param name="content">The content to analyze</param>
        /// <returns>True if content is suspicious; false otherwise</returns>
        public bool IsPotentialMeme(string content)
        {
            if (string.IsNullOrEmpty(content)) return false;
            var lower = content.ToLowerInvariant();
            
            // Check for image-related keywords and formats
            if (lower.Contains("meme") || lower.Contains(".png") || lower.Contains(".jpg") || 
                lower.Contains(".jpeg") || lower.Contains("data:image") || lower.Contains("base64,"))
                return true;
            
            // Large single-line payloads often indicate pasted binary data
            if (content.Length > 200 && !content.Contains("\n")) 
                return true;
            
            return false;
        }

        /// <summary>
        /// Backs up the current state to a timestamped file in the backup directory.
        /// Used before state updates to ensure previous states are preserved.
        /// </summary>
        /// <param name="state">The state to backup</param>
        public void BackupState(string state)
        {
            try
            {
                var file = Path.Combine(_backupDir, $"state_{DateTime.UtcNow:yyyyMMdd_HHmmss_fff}.txt");
                File.WriteAllText(file, state ?? string.Empty);
            }
            catch
            {
                // best-effort backup; swallow IO errors to avoid crashes
            }
        }
    }
}
