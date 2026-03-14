using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Tesseract;

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

        /// <summary>German translation dictionary (cached)</summary>
        public static readonly Dictionary<string, string> German = new(StringComparer.OrdinalIgnoreCase)
        {
            { "quality translation", "Qualitätsübersetzung" },
            { "uptime calendar", "Verfügbarkeitskalender" },
            { "weekly availability", "wöchentliche Verfügbarkeit" },
            { "deep learning protocol", "Deep Learning Protokoll" },
            { "state interface", "Statusschnittstelle" },
            { "aim interface", "Zielschnittstelle" },
            { "depth interface", "Tiefenschnittstelle" }
        };

        /// <summary>Italian translation dictionary (cached)</summary>
        public static readonly Dictionary<string, string> Italian = new(StringComparer.OrdinalIgnoreCase)
        {
            { "quality translation", "traduzione di qualità" },
            { "uptime calendar", "calendario disponibilità" },
            { "weekly availability", "disponibilità settimanale" },
            { "deep learning protocol", "protocollo di apprendimento profondo" },
            { "state interface", "interfaccia di stato" },
            { "aim interface", "interfaccia obiettivo" },
            { "depth interface", "interfaccia profondità" }
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

        /// <summary>Supported languages: English, Spanish, Arabic, French, German, Italian</summary>
        public enum Language { English, Spanish, Arabic, French, German, Italian }

        /// <summary>Language codes for mapping</summary>
        private static readonly Dictionary<Language, string> LanguageCodes = new()
        {
            { Language.English, "en" },
            { Language.Spanish, "es" },
            { Language.Arabic, "ar" },
            { Language.French, "fr" },
            { Language.German, "de" },
            { Language.Italian, "it" }
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
                var logEntry = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Hour {DateTime.UtcNow.Hour}: Event recorded\n";
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
                Language.German => TranslateToGerman(content),
                Language.Italian => TranslateToItalian(content),
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

        /// <summary>Translates common protocol terms to German</summary>
        private string TranslateToGerman(string content)
        {
            return ReplaceTerms(content, TranslationDictionaries.German);
        }

        /// <summary>Translates common protocol terms to Italian</summary>
        private string TranslateToItalian(string content)
        {
            return ReplaceTerms(content, TranslationDictionaries.Italian);
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
        /// Single testing system for translations: translates core text multiple times and validates consistency.
        /// </summary>
        /// <param name="coreText">The core text to translate and test</param>
        /// <param name="targetLanguage">Target language for translation</param>
        /// <param name="iterations">Number of translation iterations to perform</param>
        /// <returns>Test results with consistency score and quality metrics</returns>
        public TranslationTestResult TestTranslation(string coreText, Language targetLanguage, int iterations = 5)
        {
            var results = new List<string>();
            var qualityScores = new List<int>();
            var timestamps = new List<DateTime>();

            for (int i = 0; i < iterations; i++)
            {
                var translated = Translate(coreText, targetLanguage);
                results.Add(translated);
                
                var quality = AssessQuality(translated);
                qualityScores.Add(quality);
                
                timestamps.Add(DateTime.UtcNow);
                
                // Store metric
                StoreQualityMetric(translated, quality, targetLanguage, coreText);
                
                // Small delay to simulate processing
                System.Threading.Thread.Sleep(10);
            }

            // Calculate consistency (how many unique results)
            var uniqueResults = results.Distinct().Count();
            var consistencyScore = (iterations - uniqueResults + 1) * 100 / iterations;

            // Average quality
            var avgQuality = qualityScores.Average();

            return new TranslationTestResult
            {
                CoreText = coreText,
                TargetLanguage = targetLanguage,
                Iterations = iterations,
                Results = results,
                QualityScores = qualityScores,
                Timestamps = timestamps,
                ConsistencyScore = consistencyScore,
                AverageQuality = avgQuality,
                IsConsistent = uniqueResults == 1,
                TestTimestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Result structure for translation testing
        /// </summary>
        public class TranslationTestResult
        {
            public string? CoreText { get; set; }
            public Language TargetLanguage { get; set; }
            public int Iterations { get; set; }
            public List<string>? Results { get; set; }
            public List<int>? QualityScores { get; set; }
            public List<DateTime>? Timestamps { get; set; }
            public int ConsistencyScore { get; set; } // 0-100
            public double AverageQuality { get; set; }
            public bool IsConsistent { get; set; }
            public DateTime TestTimestamp { get; set; }
        }

        /// <summary>
        /// Processes an image using deep learning protocol analysis.
        /// Extracts features, analyzes content, and applies translation if text is detected.
        /// </summary>
        /// <param name="imagePath">Path to the image file</param>
        /// <returns>Image processing result with analysis and features</returns>
        public ImageProcessingResult ProcessImage(string imagePath)
        {
            if (!File.Exists(imagePath))
                throw new FileNotFoundException("Image file not found", imagePath);

            var result = new ImageProcessingResult
            {
                ImagePath = imagePath,
                ProcessingTimestamp = DateTime.UtcNow
            };

            try
            {
                using (var image = Image.Load<Rgba32>(imagePath))
                {
                    result.Width = image.Width;
                    result.Height = image.Height;
                    result.PixelCount = image.Width * image.Height;

                    // Analyze color distribution
                    result.ColorAnalysis = AnalyzeColorDistribution(image);

                    // Detect if image contains text (simple heuristic)
                    result.ContainsText = DetectTextContent(image);

                    // Extract features for deep learning analysis
                    result.Features = ExtractImageFeatures(image);

                    // If text detected, attempt OCR and translation
                    if (result.ContainsText)
                    {
                        result.ExtractedText = ExtractTextWithOCR(imagePath);
                        if (!string.IsNullOrEmpty(result.ExtractedText))
                        {
                            result.TranslatedText = Translate(result.ExtractedText, CurrentLanguage);
                            result.TranslationQuality = AssessQuality(result.TranslatedText);
                        }
                    }

                    // Store quality metric for the processing
                    StoreQualityMetric($"Image processed: {Path.GetFileName(imagePath)}", 
                                     result.ContainsText ? 80 : 60, 
                                     CurrentLanguage, 
                                     $"Image analysis result: {result.Width}x{result.Height}");

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.ProcessingTimestamp = DateTime.UtcNow;
            }

            return result;
        }

        /// <summary>
        /// Analyzes color distribution in the image
        /// </summary>
        private Dictionary<string, double> AnalyzeColorDistribution(Image<Rgba32> image)
        {
            var colors = new Dictionary<string, int>();
            long totalPixels = 0;

            image.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    var pixelRow = accessor.GetRowSpan(y);
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        var pixel = pixelRow[x];
                        var colorKey = $"{pixel.R},{pixel.G},{pixel.B}";
                        if (!colors.ContainsKey(colorKey))
                            colors[colorKey] = 0;
                        colors[colorKey]++;
                        totalPixels++;
                    }
                }
            });

            // Calculate percentages for top colors
            var topColors = colors.OrderByDescending(c => c.Value)
                                 .Take(10)
                                 .ToDictionary(c => c.Key, c => (double)c.Value / totalPixels * 100);

            return topColors;
        }

        /// <summary>
        /// Simple heuristic to detect if image contains text
        /// </summary>
        private bool DetectTextContent(Image<Rgba32> image)
        {
            // Simple text detection based on contrast and patterns
            // This is a placeholder - real OCR would be needed for accurate detection
            int highContrastPixels = 0;
            int totalPixels = 0;

            image.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    var pixelRow = accessor.GetRowSpan(y);
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        var pixel = pixelRow[x];
                        // Check for high contrast (potential text edges)
                        if (Math.Abs(pixel.R - pixel.G) > 50 || Math.Abs(pixel.G - pixel.B) > 50 || Math.Abs(pixel.R - pixel.B) > 50)
                            highContrastPixels++;
                        totalPixels++;
                    }
                }
            });

            return (double)highContrastPixels / totalPixels > 0.1; // 10% high contrast suggests text
        }

        /// <summary>
        /// Extracts text from image using OCR
        /// </summary>
        private string ExtractTextWithOCR(string imagePath)
        {
            try
            {
                using (var engine = new TesseractEngine("./tessdata", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(imagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            return page.GetText().Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Fallback if OCR fails
                return $"OCR failed: {ex.Message}";
            }
        }

        /// <summary>
        /// Extracts features from image for analysis
        /// </summary>
        private double[] ExtractImageFeatures(Image<Rgba32> image)
        {
            var features = new List<double>();

            // Basic features
            features.Add(image.Width);
            features.Add(image.Height);
            features.Add((double)image.Width / image.Height); // Aspect ratio

            // Color statistics
            double avgR = 0, avgG = 0, avgB = 0;
            int count = 0;

            image.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    var pixelRow = accessor.GetRowSpan(y);
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        var pixel = pixelRow[x];
                        avgR += pixel.R;
                        avgG += pixel.G;
                        avgB += pixel.B;
                        count++;
                    }
                }
            });

            features.Add(avgR / count); // Average red
            features.Add(avgG / count); // Average green
            features.Add(avgB / count); // Average blue

            return features.ToArray();
        }

        /// <summary>
        /// Result structure for image processing
        /// </summary>
        public class ImageProcessingResult
        {
            public string? ImagePath { get; set; }
            public bool Success { get; set; }
            public string? ErrorMessage { get; set; }
            public DateTime ProcessingTimestamp { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int PixelCount { get; set; }
            public Dictionary<string, double>? ColorAnalysis { get; set; }
            public bool ContainsText { get; set; }
            public string? ExtractedText { get; set; }
            public string? TranslatedText { get; set; }
            public int TranslationQuality { get; set; }
            public double[]? Features { get; set; }
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
