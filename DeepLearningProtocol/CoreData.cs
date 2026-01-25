using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepLearningProtocol
{
    /// <summary>
    /// CoreData manages system data and serves as a bridge between core protocol operations
    /// and the translation system. Provides unified data access across the application.
    /// </summary>
    public class CoreData
    {
        /// <summary>System data types that can be translated</summary>
        public enum DataType
        {
            State,
            Interface,
            Operation,
            Message,
            Error,
            Status
        }

        /// <summary>Core system states with translations</summary>
        private static readonly Dictionary<string, (string Spanish, string Arabic, string French)> SystemStates = new()
        {
            { "initialized", ("inicializado", "تم التهيئة", "initialisé") },
            { "processing", ("procesando", "جاري المعالجة", "traitement en cours") },
            { "completed", ("completado", "مكتمل", "terminé") },
            { "error", ("error", "خطأ", "erreur") },
            { "waiting", ("esperando", "في انتظار", "en attente") },
            { "ready", ("listo", "جاهز", "prêt") },
            { "dlp-blocked", ("bloqueado por dlp", "محظور بواسطة dlp", "bloqué par dlp") },
            { "backup-created", ("copia de seguridad creada", "تم إنشاء نسخة احتياطية", "sauvegarde créée") },
        };

        /// <summary>Core interface names with translations</summary>
        private static readonly Dictionary<string, (string Spanish, string Arabic, string French)> InterfaceNames = new()
        {
            { "state interface", ("interfaz de estado", "واجهة الحالة", "interface d'état") },
            { "depth interface", ("interfaz de profundidad", "واجهة العمق", "interface de profondeur") },
            { "aim interface", ("interfaz de objetivo", "واجهة الهدف", "interface d'objectif") },
            { "abstract core", ("núcleo abstracto", "النواة المجردة", "noyau abstrait") },
            { "data loss prevention", ("prevención de pérdida de datos", "منع فقدان البيانات", "prévention de la perte de données") },
            { "menu system", ("sistema de menú", "نظام القائمة", "système de menu") },
            { "translator", ("traductor", "المترجم", "traducteur") },
        };

        /// <summary>Core operation names with translations</summary>
        private static readonly Dictionary<string, (string Spanish, string Arabic, string French)> Operations = new()
        {
            { "execute protocol", ("ejecutar protocolo", "تنفيذ البروتوكول", "exécuter le protocole") },
            { "process reasoning", ("procesar razonamiento", "معالجة التفكير", "traiter le raisonnement") },
            { "update state", ("actualizar estado", "تحديث الحالة", "mettre à jour l'état") },
            { "check backup", ("verificar copia de seguridad", "التحقق من النسخة الاحتياطية", "vérifier la sauvegarde") },
            { "translate text", ("traducir texto", "ترجمة النص", "traduire le texte") },
        };

        /// <summary>Gets translated system data by type and key</summary>
        public static string GetTranslatedData(string key, DataType dataType, Translator.Language targetLanguage)
        {
            var lowerKey = key.ToLower();
            var dictionary = dataType switch
            {
                DataType.State => SystemStates,
                DataType.Interface => InterfaceNames,
                DataType.Operation => Operations,
                _ => new Dictionary<string, (string Spanish, string Arabic, string French)>()
            };

            if (!dictionary.TryGetValue(lowerKey, out var translations))
            {
                // Fallback to translator for unmapped data
                return Translator.Translate(key, targetLanguage);
            }

            return targetLanguage switch
            {
                Translator.Language.Spanish => translations.Spanish,
                Translator.Language.Arabic => translations.Arabic,
                Translator.Language.French => translations.French,
                _ => key
            };
        }

        /// <summary>Checks if system data exists for a given type and key</summary>
        public static bool HasData(string key, DataType dataType)
        {
            var lowerKey = key.ToLower();
            return dataType switch
            {
                DataType.State => SystemStates.ContainsKey(lowerKey),
                DataType.Interface => InterfaceNames.ContainsKey(lowerKey),
                DataType.Operation => Operations.ContainsKey(lowerKey),
                _ => false
            };
        }

        /// <summary>Data bridge function that translates system messages and status</summary>
        public static string DataBridge(string systemMessage, Translator.Language targetLanguage = Translator.Language.English)
        {
            if (string.IsNullOrWhiteSpace(systemMessage))
                return systemMessage;

            // Try exact match first
            if (HasData(systemMessage, DataType.State))
                return GetTranslatedData(systemMessage, DataType.State, targetLanguage);

            if (HasData(systemMessage, DataType.Interface))
                return GetTranslatedData(systemMessage, DataType.Interface, targetLanguage);

            if (HasData(systemMessage, DataType.Operation))
                return GetTranslatedData(systemMessage, DataType.Operation, targetLanguage);

            // Fallback to translator
            return Translator.Translate(systemMessage, targetLanguage);
        }

        /// <summary>Gets all available system states</summary>
        public static IEnumerable<string> GetAvailableStates() => SystemStates.Keys.OrderBy(k => k);

        /// <summary>Gets all available interface names</summary>
        public static IEnumerable<string> GetAvailableInterfaces() => InterfaceNames.Keys.OrderBy(k => k);

        /// <summary>Gets all available operations</summary>
        public static IEnumerable<string> GetAvailableOperations() => Operations.Keys.OrderBy(k => k);

        /// <summary>Gets count of all core data entries</summary>
        public static int GetTotalDataSize() => SystemStates.Count + InterfaceNames.Count + Operations.Count;

        /// <summary>Displays system data map for debugging/reference</summary>
        public static void DisplayDataMap(Translator.Language language)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              System Data Translation Map               ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

            Console.WriteLine($"Target Language: {Translator.GetLanguageName(language)}\n");

            // Display states
            Console.WriteLine("--- System States ---");
            foreach (var state in GetAvailableStates())
            {
                var translated = GetTranslatedData(state, DataType.State, language);
                Console.WriteLine($"  {state,-20} → {translated}");
            }

            Console.WriteLine("\n--- Interface Names ---");
            foreach (var iface in GetAvailableInterfaces())
            {
                var translated = GetTranslatedData(iface, DataType.Interface, language);
                Console.WriteLine($"  {iface,-20} → {translated}");
            }

            Console.WriteLine("\n--- Operations ---");
            foreach (var op in GetAvailableOperations())
            {
                var translated = GetTranslatedData(op, DataType.Operation, language);
                Console.WriteLine($"  {op,-20} → {translated}");
            }

            Console.WriteLine($"\n--- Summary ---");
            Console.WriteLine($"Total system data entries: {GetTotalDataSize()}");
            Console.WriteLine($"  States: {SystemStates.Count}");
            Console.WriteLine($"  Interfaces: {InterfaceNames.Count}");
            Console.WriteLine($"  Operations: {Operations.Count}");

            Console.Write("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}
