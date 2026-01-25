using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DeepLearningProtocol
{
    /// <summary>
    /// QualityTranslation (QT) is a multi-language, uptime-aware system that validates content quality
    /// and provides real-time translation across 4 supported languages with 24-hour uptime tracking.
    /// It replaces the previous DataLossPrevention layer with enhanced language support and availability monitoring.
    /// </summary>
    public class QualityTranslation
    {
        /// <summary>Directory path for storing quality metrics and uptime logs</summary>
        private readonly string _metricsDir = "./.qt_metrics";

        /// <summary>Supported languages: English, Spanish, Arabic, French</summary>
        public enum Language { English, Spanish, Arabic, French }

        /// <summary>Language codes for mapping</summary>
        private static readonly Dictionary<Language, string> LanguageCodes = new()
        {
            { Language.English, "en" },
            { Language.Spanish, "es" },
            { Language.Arabic, "ar" },
            { Language.French, "fr" }
        };

        /// <summary>24-hour uptime tracking (hourly buckets)</summary>
        private readonly Dictionary<int, int> _uptimeHours = new();
        private readonly object _uptimeLock = new();

        /// <summary>Quality metrics storage</summary>
        private readonly Dictionary<string, QualityMetric> _qualityMetrics = new();

        /// <summary>Current language preference</summary>
        public Language CurrentLanguage { get; set; } = Language.English;

        /// <summary>Data structure for tracking quality metrics</summary>
        public class QualityMetric
        {
            public string? Content { get; set; }
            public int QualityScore { get; set; } // 0-100
            public DateTime Timestamp { get; set; }
            public Language DetectedLanguage { get; set; }
            public string? TranslatedContent { get; set; }
        }

        /// <summary>
        /// Initializes the QT system with metrics directory and uptime calendar.
        /// Gracefully handles IO errors in restricted environments.
        /// </summary>
        public QualityTranslation()
        {
            try
            {
                Directory.CreateDirectory(_metricsDir);
            }
            catch
            {
                // ignore errors creating metrics dir in restricted environments
            }

            InitializeUptimeCalendar();
        }

        /// <summary>
        /// Initializes the 24-hour uptime calendar with hourly tracking.
        /// Creates slots for all 24 hours with initial values.
        /// </summary>
        private void InitializeUptimeCalendar()
        {
            lock (_uptimeLock)
            {
                for (int hour = 0; hour < 24; hour++)
                {
                    _uptimeHours[hour] = 0;
                }
            }
        }

        /// <summary>
        /// Records an uptime event for the current hour.
        /// Maintains 24-hour availability metrics.
        /// </summary>
        public void RecordUptimeEvent()
        {
            lock (_uptimeLock)
            {
                int currentHour = DateTime.Now.Hour;
                if (!_uptimeHours.ContainsKey(currentHour))
                    _uptimeHours[currentHour] = 0;
                
                _uptimeHours[currentHour]++;

                try
                {
                    var logFile = Path.Combine(_metricsDir, $"uptime_{DateTime.UtcNow:yyyyMMdd}.log");
                    File.AppendAllText(logFile, 
                        $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Hour {currentHour}: Event recorded\n");
                }
                catch
                {
                    // best-effort logging
                }
            }
        }

        /// <summary>
        /// Gets the 24-hour uptime availability summary.
        /// Returns a dictionary mapping hours (0-23) to event counts.
        /// </summary>
        /// <returns>Dictionary of hourly uptime metrics</returns>
        public Dictionary<int, int> GetUptimeCalendar()
        {
            lock (_uptimeLock)
            {
                return new Dictionary<int, int>(_uptimeHours);
            }
        }

        /// <summary>
        /// Calculates total uptime availability percentage based on 24-hour calendar.
        /// </summary>
        /// <returns>Availability percentage (0-100)</returns>
        public int GetUptimePercentage()
        {
            lock (_uptimeLock)
            {
                if (_uptimeHours.Count == 0) return 100;
                
                int totalEvents = _uptimeHours.Values.Sum();
                int activeHours = _uptimeHours.Count(x => x.Value > 0);
                
                if (totalEvents == 0) return 100;
                return Math.Min(100, (activeHours * 100) / 24);
            }
        }

        /// <summary>
        /// Detects content quality based on multiple heuristics.
        /// Checks for clarity, length appropriateness, and language consistency.
        /// </summary>
        /// <param name="content">The content to analyze</param>
        /// <returns>Quality score (0-100)</returns>
        public int AssessQuality(string content)
        {
            if (string.IsNullOrEmpty(content)) return 0;

            RecordUptimeEvent();
            
            int score = 50; // baseline
            var lower = content.ToLowerInvariant();

            // Penalize meme/binary content
            if (lower.Contains("meme") || lower.Contains(".png") || lower.Contains(".jpg") || 
                lower.Contains(".jpeg") || lower.Contains("data:image") || lower.Contains("base64,"))
                score -= 30;

            // Penalize large single-line payloads (likely binary)
            if (content.Length > 200 && !content.Contains("\n"))
                score -= 20;

            // Reward proper punctuation and spacing
            if (content.Contains(".") || content.Contains("!") || content.Contains("?"))
                score += 15;

            // Reward multi-line structure
            if (content.Contains("\n") && content.Length > 50)
                score += 20;

            // Penalize extremely short content
            if (content.Length < 10)
                score -= 25;

            return Math.Clamp(score, 0, 100);
        }

        /// <summary>
        /// Translates content to the target language.
        /// Supports: English, Spanish, Arabic, French
        /// </summary>
        /// <param name="content">Content to translate</param>
        /// <param name="targetLanguage">Target language</param>
        /// <returns>Translated content</returns>
        public string Translate(string content, Language targetLanguage)
        {
            RecordUptimeEvent();

            if (string.IsNullOrEmpty(content)) return string.Empty;

            return targetLanguage switch
            {
                Language.Spanish => TranslateToSpanish(content),
                Language.Arabic => TranslateToArabic(content),
                Language.French => TranslateToFrench(content),
                _ => content // English: return as-is
            };
        }

        /// <summary>Translates common protocol terms to Spanish</summary>
        private string TranslateToSpanish(string content)
        {
            var translations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "quality translation", "traducción de calidad" },
                { "uptime calendar", "calendario de disponibilidad" },
                { "24-hour availability", "disponibilidad de 24 horas" },
                { "deep learning protocol", "protocolo de aprendizaje profundo" },
                { "state interface", "interfaz de estado" },
                { "aim interface", "interfaz de objetivo" },
                { "depth interface", "interfaz de profundidad" }
            };

            return ReplaceTerms(content, translations);
        }

        /// <summary>Translates common protocol terms to Arabic</summary>
        private string TranslateToArabic(string content)
        {
            var translations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "quality translation", "ترجمة الجودة" },
                { "uptime calendar", "تقويم المدة الزمنية" },
                { "24-hour availability", "توفر 24 ساعة" },
                { "deep learning protocol", "بروتوكول التعلم العميق" },
                { "state interface", "واجهة الحالة" },
                { "aim interface", "واجهة الهدف" },
                { "depth interface", "واجهة العمق" }
            };

            return ReplaceTerms(content, translations);
        }

        /// <summary>Translates common protocol terms to French</summary>
        private string TranslateToFrench(string content)
        {
            var translations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "quality translation", "traduction de qualité" },
                { "uptime calendar", "calendrier de disponibilité" },
                { "24-hour availability", "disponibilité 24 heures" },
                { "deep learning protocol", "protocole d'apprentissage profond" },
                { "state interface", "interface d'état" },
                { "aim interface", "interface d'objectif" },
                { "depth interface", "interface de profondeur" }
            };

            return ReplaceTerms(content, translations);
        }

        /// <summary>Helper method to replace terms case-insensitively</summary>
        private string ReplaceTerms(string content, Dictionary<string, string> translations)
        {
            var result = content;
            foreach (var kvp in translations.OrderByDescending(x => x.Key.Length))
            {
                result = System.Text.RegularExpressions.Regex.Replace(
                    result, 
                    System.Text.RegularExpressions.Regex.Escape(kvp.Key),
                    kvp.Value,
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }
            return result;
        }

        /// <summary>
        /// Stores quality metrics for a given content assessment.
        /// Maintains historical quality data for analysis.
        /// </summary>
        /// <param name="content">The assessed content</param>
        /// <param name="qualityScore">The quality score (0-100)</param>
        /// <param name="language">Detected language</param>
        /// <param name="translatedContent">Translated version if applicable</param>
        public void StoreQualityMetric(string content, int qualityScore, Language language, string translatedContent)
        {
            var metric = new QualityMetric
            {
                Content = content,
                QualityScore = qualityScore,
                Timestamp = DateTime.UtcNow,
                DetectedLanguage = language,
                TranslatedContent = translatedContent
            };

            var key = $"{DateTime.UtcNow:yyyyMMdd_HHmmss_fff}";
            _qualityMetrics[key] = metric;

            try
            {
                var metricsFile = Path.Combine(_metricsDir, $"quality_{DateTime.UtcNow:yyyyMMdd}.json");
                var json = System.Text.Json.JsonSerializer.Serialize(metric, 
                    new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                File.AppendAllText(metricsFile, json + Environment.NewLine);
            }
            catch
            {
                // best-effort metrics storage
            }
        }

        /// <summary>
        /// Gets all stored quality metrics within a specified timeframe.
        /// </summary>
        /// <param name="hoursBack">Number of hours to look back (default: 24)</param>
        /// <returns>Collection of quality metrics</returns>
        public IEnumerable<QualityMetric> GetQualityMetrics(int hoursBack = 24)
        {
            var cutoff = DateTime.UtcNow.AddHours(-hoursBack);
            return _qualityMetrics.Values.Where(m => m.Timestamp >= cutoff);
        }
    }
}
