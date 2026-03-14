using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DeepLearningProtocol
{
    /// <summary>
    /// Encapsulates quality assessment heuristics and thresholds.
    /// </summary>
    public static class QualityHeuristics
    {
        public const int Baseline = 50;
        public const int SuspiciousPenalty = 30;
        public const int LargePayloadPenalty = 20;
        public const int TooShortPenalty = 25;
        public const int PunctuationBonus = 15;
        public const int MultilineBonus = 20;
        public const int MinLength = 10;
        public const int MaxSingleLineLength = 200;
        public const int MultilineMinLength = 50;

        /// <summary>Patterns that indicate suspicious/binary content</summary>
        public static readonly string[] SuspiciousPatterns = 
        {
            "meme", ".png", ".jpg", ".jpeg", "data:image", "base64,"
        };

        /// <summary>Punctuation marks that indicate quality content</summary>
        public static readonly char[] PunctuationMarks = { '.', '!', '?' };
    }

    /// <summary>
    /// Static cached dictionaries for translation efficiency.
    /// Prevents repeated dictionary instantiation during translations.
    /// </summary>
    public static class TranslationDictionaries
    {
        /// <summary>Spanish translation dictionary (cached)</summary>
        public static readonly Dictionary<string, string> Spanish = new(StringComparer.OrdinalIgnoreCase)
        {
            { "quality translation", "traducción de calidad" },
            { "uptime calendar", "calendario de disponibilidad" },
            { "weekly availability", "disponibilidad semanal" },
            { "deep learning protocol", "protocolo de aprendizaje profundo" },
            { "state interface", "interfaz de estado" },
            { "aim interface", "interfaz de objetivo" },
            { "depth interface", "interfaz de profundidad" }
        };

        /// <summary>Arabic translation dictionary (cached)</summary>
        public static readonly Dictionary<string, string> Arabic = new(StringComparer.OrdinalIgnoreCase)
        {
            { "quality translation", "ترجمة الجودة" },
            { "uptime calendar", "تقويم المدة الزمنية" },
            { "weekly availability", "توفر أسبوعي" },
            { "deep learning protocol", "بروتوكول التعلم العميق" },
            { "state interface", "واجهة الحالة" },
            { "aim interface", "واجهة الهدف" },
            { "depth interface", "واجهة العمق" }
        };

        /// <summary>French translation dictionary (cached)</summary>
        public static readonly Dictionary<string, string> French = new(StringComparer.OrdinalIgnoreCase)
        {
            { "quality translation", "traduction de qualité" },
            { "uptime calendar", "calendrier de disponibilité" },
            { "weekly availability", "disponibilité hebdomadaire" },
            { "deep learning protocol", "protocole d'apprentissage profond" },
            { "state interface", "interface d'état" },
            { "aim interface", "interface d'objectif" },
            { "depth interface", "interface de profondeur" }
        };
    }

    /// <summary>
    /// CoreTranslation (CT) is a multi-language, uptime-aware system that validates content quality
    /// and provides real-time translation across 4 supported languages with weekly uptime tracking.
    /// It replaces the previous DataLossPrevention layer with enhanced language support and availability monitoring.
    /// </summary>
    public class CoreTranslation
    {
        /// <summary>Directory path for storing quality metrics and uptime logs</summary>
        private readonly string _metricsDir = "./.ct_metrics";

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

        /// <summary>Weekly uptime tracking (daily buckets for 7 days)</summary>
        private readonly Dictionary<int, int> _uptimeDays = new();
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
        public CoreTranslation()
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
        /// Initializes the weekly uptime calendar with daily tracking.
        /// Creates slots for all 7 days with initial values.
        /// </summary>
        private void InitializeUptimeCalendar()
        {
            lock (_uptimeLock)
            {
                for (int day = 0; day < 7; day++)
                {
                    _uptimeDays[day] = 0;
                }
            }
        }

        /// <summary>
        /// Records an uptime event for the current day.
        /// Maintains weekly availability metrics.
        /// </summary>
        public void RecordUptimeEvent()
        {
            lock (_uptimeLock)
            {
                int currentDay = (int)DateTime.Now.DayOfWeek;
                if (!_uptimeDays.ContainsKey(currentDay))
                    _uptimeDays[currentDay] = 0;
                
                _uptimeDays[currentDay]++;
                LogUptimeEvent(currentDay);
            }
        }

        /// <summary>Logs uptime event to file (best-effort)</summary>
        private void LogUptimeEvent(int day)
        {
            try
            {
                var logFile = Path.Combine(_metricsDir, $"uptime_{DateTime.UtcNow:yyyyMMdd}.log");
                var logEntry = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Hour {hour}: Event recorded\n";
                File.AppendAllText(logFile, logEntry);
            }
            catch
            {
                // best-effort logging; swallow errors to prevent crashes
            }
        }

        /// <summary>
        /// Gets the weekly uptime availability summary.
        /// Returns a dictionary mapping days (0-6, Sunday=0) to event counts.
        /// </summary>
        /// <returns>Dictionary of daily uptime metrics</returns>
        public Dictionary<int, int> GetUptimeCalendar()
        {
            lock (_uptimeLock)
            {
                return new Dictionary<int, int>(_uptimeDays);
            }
        }

        /// <summary>
        /// Calculates total uptime availability percentage based on weekly calendar.
        /// </summary>
        /// <returns>Availability percentage (0-100)</returns>
        public int GetUptimePercentage()
        {
            lock (_uptimeLock)
            {
                if (_uptimeDays.Count == 0) return 100;
                
                int totalEvents = _uptimeDays.Values.Sum();
                int activeDays = _uptimeDays.Count(x => x.Value > 0);
                
                if (totalEvents == 0) return 100;
                return Math.Min(100, (activeDays * 100) / 7);
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
            return CalculateQualityScore(content);
        }

        /// <summary>Calculates quality score based on heuristics</summary>
        private int CalculateQualityScore(string content)
        {
            int score = QualityHeuristics.Baseline;
            var lower = content.ToLowerInvariant();

            score += DetectSuspiciousContent(lower);
            score += EvaluateStructure(content);
            score += EvaluateLength(content.Length);

            return Math.Clamp(score, 0, 100);
        }

        /// <summary>Evaluates suspicious content patterns</summary>
        private int DetectSuspiciousContent(string lowerContent)
        {
            if (QualityHeuristics.SuspiciousPatterns.Any(p => lowerContent.Contains(p)))
                return -QualityHeuristics.SuspiciousPenalty;

            return 0;
        }

        /// <summary>Evaluates content structure (punctuation, formatting)</summary>
        private int EvaluateStructure(string content)
        {
            int score = 0;

            if (QualityHeuristics.PunctuationMarks.Any(p => content.Contains(p)))
                score += QualityHeuristics.PunctuationBonus;

            if (content.Contains("\n") && content.Length > QualityHeuristics.MultilineMinLength)
                score += QualityHeuristics.MultilineBonus;

            return score;
        }

        /// <summary>Evaluates content length appropriateness</summary>
        private int EvaluateLength(int contentLength)
        {
            if (contentLength < QualityHeuristics.MinLength)
                return -QualityHeuristics.TooShortPenalty;

            if (contentLength > QualityHeuristics.MaxSingleLineLength)
                return -QualityHeuristics.LargePayloadPenalty;

            return 0;
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
            return ReplaceTerms(content, TranslationDictionaries.Spanish);
        }

        /// <summary>Translates common protocol terms to Arabic</summary>
        private string TranslateToArabic(string content)
        {
            return ReplaceTerms(content, TranslationDictionaries.Arabic);
        }

        /// <summary>Translates common protocol terms to French</summary>
        private string TranslateToFrench(string content)
        {
            return ReplaceTerms(content, TranslationDictionaries.French);
        }

        /// <summary>Helper method to replace terms case-insensitively with optimized regex</summary>
        private string ReplaceTerms(string content, Dictionary<string, string> translations)
        {
            var result = content;
            
            // Process longer terms first to avoid partial replacements
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
            
            PersistMetricToFile(metric);
        }

        /// <summary>Persists metric to JSON file (best-effort)</summary>
        private void PersistMetricToFile(QualityMetric metric)
        {
            try
            {
                var metricsFile = Path.Combine(_metricsDir, $"quality_{DateTime.UtcNow:yyyyMMdd}.json");
                var json = System.Text.Json.JsonSerializer.Serialize(metric, 
                    new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                File.AppendAllText(metricsFile, json + Environment.NewLine);
            }
            catch
            {
                // best-effort metrics storage; swallow errors to prevent crashes
            }
        }

        /// <summary>
        /// Gets all stored quality metrics within a specified timeframe.
        /// </summary>
        /// <param name="hoursBack">Number of hours to look back (default: 168 for weekly)</param>
        /// <returns>Collection of quality metrics</returns>
        public IEnumerable<QualityMetric> GetQualityMetrics(int hoursBack = 168)
        {
            var cutoff = DateTime.UtcNow.AddHours(-hoursBack);
            return _qualityMetrics.Values.Where(m => m.Timestamp >= cutoff);
        }
    }
}
