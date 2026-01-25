using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepLearningProtocol
{
    /// <summary>
    /// TranslationManager handles custom translation rules and text storage.
    /// Provides methods to manage translation rules and store translated texts in the database.
    /// </summary>
    public class TranslationManager
    {
        private readonly ProtocolDbContext _context;
        private readonly Translator _translator;

        public TranslationManager(ProtocolDbContext context)
        {
            _context = context ?? new ProtocolDbContext();
            _translator = new Translator();
        }

        /// <summary>
        /// Translates text from console and stores it in the database.
        /// Uses custom translation rules if they exist, otherwise falls back to translator.
        /// </summary>
        public TranslatedText TranslateAndStore(string sourceText, int executionDepth = 5)
        {
            try
            {
                // Check for custom rules first
                var spanish = GetTranslationForLanguage(sourceText, Translator.Language.Spanish);
                var arabic = GetTranslationForLanguage(sourceText, Translator.Language.Arabic);
                var french = GetTranslationForLanguage(sourceText, Translator.Language.French);

                var translatedText = new TranslatedText
                {
                    SourceText = sourceText,
                    SpanishTranslation = spanish,
                    ArabicTranslation = arabic,
                    FrenchTranslation = french,
                    ExecutionDepth = executionDepth,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    QualityScore = 75
                };

                _context.TranslatedTexts.Add(translatedText);
                _context.SaveChanges();

                return translatedText;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to translate and store text: {ex.Message}");
            }
        }

        /// <summary>Gets translation for a specific language, checking custom rules first</summary>
        private string GetTranslationForLanguage(string sourceText, Translator.Language language)
        {
            var lowerText = sourceText.ToLower();

            // Check custom rules first (highest priority)
            var rule = _context.TranslationRules
                .Where(r => r.IsActive)
                .OrderByDescending(r => r.Priority)
                .FirstOrDefault(r => r.SourceText.ToLower() == lowerText);

            if (rule != null)
            {
                rule.UsageCount++;
                _context.SaveChanges();

                return language switch
                {
                    Translator.Language.Spanish => rule.SpanishTranslation,
                    Translator.Language.Arabic => rule.ArabicTranslation,
                    Translator.Language.French => rule.FrenchTranslation,
                    _ => sourceText
                };
            }

            // Fall back to translator
            return Translator.Translate(sourceText, language);
        }

        /// <summary>Creates a new translation rule</summary>
        public bool CreateRule(string sourceText, string spanish, string arabic, string french, string category = "Custom", int priority = 5)
        {
            try
            {
                if (_context.TranslationRules.Any(r => r.SourceText.ToLower() == sourceText.ToLower()))
                    return false; // Rule already exists

                var rule = new TranslationRule
                {
                    SourceText = sourceText,
                    SpanishTranslation = spanish,
                    ArabicTranslation = arabic,
                    FrenchTranslation = french,
                    Category = category,
                    Priority = priority,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow
                };

                _context.TranslationRules.Add(rule);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Updates an existing translation rule</summary>
        public bool UpdateRule(string sourceText, string? spanish = null, string? arabic = null, string? french = null, int? priority = null)
        {
            try
            {
                var rule = _context.TranslationRules
                    .FirstOrDefault(r => r.SourceText.ToLower() == sourceText.ToLower());

                if (rule == null)
                    return false;

                if (!string.IsNullOrEmpty(spanish))
                    rule.SpanishTranslation = spanish;

                if (!string.IsNullOrEmpty(arabic))
                    rule.ArabicTranslation = arabic;

                if (!string.IsNullOrEmpty(french))
                    rule.FrenchTranslation = french;

                if (priority.HasValue && priority >= 1 && priority <= 10)
                    rule.Priority = priority.Value;

                rule.ModifiedAt = DateTime.UtcNow;
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Deletes a translation rule</summary>
        public bool DeleteRule(string sourceText)
        {
            try
            {
                var rule = _context.TranslationRules
                    .FirstOrDefault(r => r.SourceText.ToLower() == sourceText.ToLower());

                if (rule == null)
                    return false;

                _context.TranslationRules.Remove(rule);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Gets all translation rules</summary>
        public IEnumerable<TranslationRule> GetAllRules() =>
            _context.TranslationRules.OrderByDescending(r => r.Priority).ThenBy(r => r.SourceText).ToList();

        /// <summary>Gets all translated texts</summary>
        public IEnumerable<TranslatedText> GetAllTranslations() =>
            _context.TranslatedTexts.OrderByDescending(t => t.CreatedAt).ToList();

        /// <summary>Gets a translation record by ID</summary>
        public TranslatedText? GetTranslation(int id) =>
            _context.TranslatedTexts.FirstOrDefault(t => t.Id == id);

        /// <summary>Updates quality score of a translation</summary>
        public bool UpdateQualityScore(int translationId, int score)
        {
            try
            {
                if (score < 0 || score > 100)
                    return false;

                var translation = _context.TranslatedTexts.FirstOrDefault(t => t.Id == translationId);
                if (translation == null)
                    return false;

                translation.QualityScore = score;
                translation.ModifiedAt = DateTime.UtcNow;
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Marks a translation as manually verified</summary>
        public bool VerifyTranslation(int translationId)
        {
            try
            {
                var translation = _context.TranslatedTexts.FirstOrDefault(t => t.Id == translationId);
                if (translation == null)
                    return false;

                translation.IsManuallyVerified = true;
                translation.QualityScore = 100;
                translation.ModifiedAt = DateTime.UtcNow;
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Displays all translation rules in a formatted table</summary>
        public void DisplayAllRules()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║            Translation Rules Database                  ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

            var rules = GetAllRules().ToList();

            if (!rules.Any())
            {
                Console.WriteLine("No custom translation rules defined.\n");
                Console.Write("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Total Rules: {rules.Count}\n");
            Console.WriteLine($"{"Source",-25} {"Priority",-10} {"Category",-15} {"Usage",-8}");
            Console.WriteLine(new string('─', 60));

            foreach (var rule in rules.Take(15))
            {
                var source = rule.SourceText.Length > 24 ? rule.SourceText[..21] + "..." : rule.SourceText;
                Console.WriteLine($"{source,-25} {rule.Priority,-10} {rule.Category,-15} {rule.UsageCount,-8}");
            }

            if (rules.Count > 15)
                Console.WriteLine($"\n... and {rules.Count - 15} more rules");

            Console.Write("\nPress Enter to continue...");
            Console.ReadLine();
        }

        /// <summary>Displays all translated texts</summary>
        public void DisplayAllTranslations()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║          Stored Translations in Database              ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

            var translations = GetAllTranslations().ToList();

            if (!translations.Any())
            {
                Console.WriteLine("No translations stored yet.\n");
                Console.Write("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Total Translations: {translations.Count}\n");
            Console.WriteLine($"{"ID",-4} {"Source Text",-30} {"Quality",-8} {"Verified",-8}");
            Console.WriteLine(new string('─', 55));

            foreach (var trans in translations.Take(12))
            {
                var source = trans.SourceText.Length > 29 ? trans.SourceText[..26] + "..." : trans.SourceText;
                Console.WriteLine($"{trans.Id,-4} {source,-30} {trans.QualityScore,-8} {(trans.IsManuallyVerified ? "Yes" : "No"),-8}");
            }

            if (translations.Count > 12)
                Console.WriteLine($"\n... and {translations.Count - 12} more translations");

            Console.Write("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}
