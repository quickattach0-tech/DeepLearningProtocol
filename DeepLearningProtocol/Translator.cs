using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepLearningProtocol
{
    /// <summary>
    /// Translator class provides simple translation functionality across 4 languages:
    /// English, Spanish, Arabic, and French.
    /// Uses a phrase-based dictionary for common translations.
    /// </summary>
    public class Translator
    {
        /// <summary>Supported languages in the translation system</summary>
        public enum Language
        {
            English = 1,
            Spanish = 2,
            Arabic = 3,
            French = 4
        }

        /// <summary>
        /// Translation dictionary mapping English phrases to translations in other languages.
        /// Format: English phrase -> (Spanish, Arabic, French)
        /// </summary>
        private static readonly Dictionary<string, (string Spanish, string Arabic, string French)> TranslationDictionary = new()
        {
            // Greetings
            { "hello", ("hola", "مرحبا", "bonjour") },
            { "hi", ("hola", "مرحبا", "salut") },
            { "goodbye", ("adiós", "وداعا", "au revoir") },
            { "bye", ("adiós", "وداعا", "au revoir") },
            { "good morning", ("buenos días", "صباح الخير", "bonjour") },
            { "good evening", ("buenas noches", "مساء الخير", "bonsoir") },
            { "good night", ("buenas noches", "تصبح على خير", "bonne nuit") },
            { "thank you", ("gracias", "شكراً", "merci") },
            { "thanks", ("gracias", "شكراً", "merci") },
            { "please", ("por favor", "من فضلك", "s'il vous plaît") },
            { "you're welcome", ("de nada", "أهلا وسهلا", "de rien") },
            { "sorry", ("lo siento", "آسف", "désolé") },
            { "excuse me", ("perdón", "عذراً", "excusez-moi") },

            // Common phrases
            { "how are you", ("¿cómo estás?", "كيف حالك؟", "comment allez-vous?") },
            { "i'm fine", ("estoy bien", "أنا بخير", "je vais bien") },
            { "my name is", ("mi nombre es", "اسمي", "mon nom est") },
            { "what is your name", ("¿cuál es tu nombre?", "ما اسمك؟", "quel est votre nom?") },
            { "nice to meet you", ("mucho gusto", "يسعدني التعرف عليك", "ravi de vous rencontrer") },
            { "help", ("ayuda", "مساعدة", "aide") },
            { "water", ("agua", "ماء", "eau") },
            { "food", ("comida", "طعام", "nourriture") },
            { "yes", ("sí", "نعم", "oui") },
            { "no", ("no", "لا", "non") },

            // Numbers
            { "one", ("uno", "واحد", "un") },
            { "two", ("dos", "اثنين", "deux") },
            { "three", ("tres", "ثلاثة", "trois") },
            { "four", ("cuatro", "أربعة", "quatre") },
            { "five", ("cinco", "خمسة", "cinq") },
            { "ten", ("diez", "عشرة", "dix") },

            // Time
            { "hello", ("hola", "مرحبا", "bonjour") },
            { "morning", ("mañana", "صباح", "matin") },
            { "afternoon", ("tarde", "بعد الظهر", "après-midi") },
            { "evening", ("noche", "مساء", "soir") },
            { "today", ("hoy", "اليوم", "aujourd'hui") },
            { "tomorrow", ("mañana", "غداً", "demain") },
            { "yesterday", ("ayer", "أمس", "hier") },

            // Basic expressions
            { "do you speak english", ("¿hablas inglés?", "هل تتحدث الإنجليزية؟", "parlez-vous anglais?") },
            { "i don't understand", ("no entiendo", "لا أفهم", "je ne comprends pas") },
            { "can you help me", ("¿puedes ayudarme?", "هل يمكنك مساعدتي؟", "pouvez-vous m'aider?") },
            { "where is the bathroom", ("¿dónde está el baño?", "أين الحمام؟", "où sont les toilettes?") },
            { "how much", ("¿cuánto?", "كم؟", "combien?") },
            { "too expensive", ("muy caro", "مكلف جداً", "trop cher") },

            // Deep Learning Protocol related
            { "deep learning protocol", ("protocolo de aprendizaje profundo", "بروتوكول التعلم العميق", "protocole d'apprentissage profond") },
            { "abstract reasoning", ("razonamiento abstracto", "التفكير المجرد", "raisonnement abstrait") },
            { "data loss prevention", ("prevención de pérdida de datos", "منع فقدان البيانات", "prévention de la perte de données") },
            { "execute protocol", ("ejecutar protocolo", "تنفيذ البروتوكول", "exécuter le protocole") },
            { "processing depth", ("profundidad de procesamiento", "عمق المعالجة", "profondeur de traitement") },
            { "goal", ("objetivo", "الهدف", "objectif") },
            { "result", ("resultado", "النتيجة", "résultat") },
            { "state", ("estado", "الحالة", "état") },
        };

        /// <summary>
        /// Translates a phrase from English to the specified language.
        /// If exact match not found, performs word-by-word translation.
        /// </summary>
        /// <param name="englishPhrase">The English phrase to translate</param>
        /// <param name="targetLanguage">The target language for translation</param>
        /// <returns>Translated phrase or best attempt if not in dictionary</returns>
        public static string Translate(string englishPhrase, Language targetLanguage)
        {
            if (string.IsNullOrWhiteSpace(englishPhrase))
                return englishPhrase;

            var lowerPhrase = englishPhrase.ToLower();

            // Try exact match first
            if (TranslationDictionary.TryGetValue(lowerPhrase, out var translations))
            {
                return targetLanguage switch
                {
                    Language.Spanish => translations.Spanish,
                    Language.Arabic => translations.Arabic,
                    Language.French => translations.French,
                    _ => englishPhrase
                };
            }

            // Try word-by-word translation if no exact match
            return TranslateWordByWord(englishPhrase, targetLanguage);
        }

        /// <summary>
        /// Translates a phrase by translating individual words.
        /// Preserves capitalization and punctuation.
        /// </summary>
        private static string TranslateWordByWord(string phrase, Language targetLanguage)
        {
            var words = phrase.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var translatedWords = new List<string>();

            foreach (var word in words)
            {
                var cleanWord = word.ToLower();
                var foundTranslation = false;

                // Check if word is in dictionary
                if (TranslationDictionary.TryGetValue(cleanWord, out var translations))
                {
                    var translated = targetLanguage switch
                    {
                        Language.Spanish => translations.Spanish,
                        Language.Arabic => translations.Arabic,
                        Language.French => translations.French,
                        _ => word
                    };
                    translatedWords.Add(translated);
                    foundTranslation = true;
                }

                // If not found, keep original word
                if (!foundTranslation)
                {
                    translatedWords.Add(word);
                }
            }

            return string.Join(" ", translatedWords);
        }

        /// <summary>
        /// Gets the name of a language enum value.
        /// </summary>
        public static string GetLanguageName(Language language) => language switch
        {
            Language.English => "English",
            Language.Spanish => "Spanish",
            Language.Arabic => "Arabic",
            Language.French => "French",
            _ => "Unknown"
        };

        /// <summary>
        /// Provides language code for reference (ISO 639-1 codes).
        /// </summary>
        public static string GetLanguageCode(Language language) => language switch
        {
            Language.English => "en",
            Language.Spanish => "es",
            Language.Arabic => "ar",
            Language.French => "fr",
            _ => "xx"
        };

        /// <summary>
        /// Gets the dictionary entry count for statistics.
        /// </summary>
        public static int GetDictionarySize() => TranslationDictionary.Count;

        /// <summary>
        /// Checks if a phrase exists in the translation dictionary.
        /// </summary>
        public static bool IsPhraseAvailable(string englishPhrase) =>
            TranslationDictionary.ContainsKey(englishPhrase.ToLower());

        /// <summary>
        /// Gets all available phrases in the translation dictionary.
        /// </summary>
        public static IEnumerable<string> GetAvailablePhrases() =>
            TranslationDictionary.Keys.OrderBy(k => k);
    }
}
