using System;
using System.Collections.Generic;

namespace DeepLearningProtocol
{
    /// <summary>
    /// MenuSystem handles the interactive user interface for the Deep Learning Protocol.
    /// Provides menu navigation, FAQ display, and protocol execution workflows.
    /// </summary>
    public class MenuSystem
    {
        /// <summary>Static dictionary storing all FAQ questions and answers for easy reference</summary>
        private static readonly Dictionary<int, (string Question, string Answer)> FAQs = new()
        {
            { 1, (
                "What is the Deep Learning Protocol?",
                "A hierarchical reasoning system that processes information through multiple layers:\n" +
                "  • AbstractCore (deepest layer)\n" +
                "  • Depth Interface (recursive processing)\n" +
                "  • Aim Interface (goal-directed exploration)\n" +
                "  • State Interface (state management)\n" +
                "  • Data Loss Prevention (DLP) for content protection"
            )},
            { 2, (
                "How do I run the program?",
                "Three ways:\n" +
                "  1. VS Code: Press Ctrl+Shift+B (default task)\n" +
                "  2. CLI: dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj\n" +
                "  3. Interactive: Run it and follow the menu prompts"
            )},
            { 3, (
                "What is Data Loss Prevention (DLP)?",
                "A protective layer that:\n" +
                "  • Detects meme/binary content (.png, .jpg, base64, 'meme' keyword)\n" +
                "  • Blocks suspicious updates to prevent data loss\n" +
                "  • Backs up states to ./.dlp_backups/ with timestamps"
            )},
            { 4, (
                "What are the core components?",
                "  • AbstractCore: Base reasoning layer\n" +
                "  • IStateInterface: State get/update\n" +
                "  • IAimInterface: Goal setting and pursuing\n" +
                "  • IDepthInterface: Hierarchical processing at N levels\n" +
                "  • DeepLearningProtocol: Main orchestrator"
            )},
            { 5, (
                "How does depth processing work?",
                "ProcessAtDepth(input, depth) recursively applies ProcessCoreReasoning() depth times.\n" +
                "Example: depth=3 wraps input in 3 layers of abstract processing"
            )},
            { 6, (
                "Can I ask custom questions?",
                "Yes! When you run the protocol, you'll be prompted to enter:\n" +
                "  • Your question/input\n" +
                "  • Your goal\n" +
                "  • Processing depth (1-10)\n" +
                "  • Option to ask another question"
            )},
            { 7, (
                "How do I run tests?",
                "Run: dotnet test\n" +
                "This executes 8 XUnit tests covering all core methods and edge cases"
            )},
            { 8, (
                "What happens if I input meme-like content?",
                "The DLP layer:\n" +
                "  • Detects the suspicious content\n" +
                "  • Backs up your current state\n" +
                "  • Blocks the update\n" +
                "  • Sets state to [DLP-BLOCKED] to prevent accidental loss"
            )},
            { 9, (
                "How do I extend the project?",
                "1. Add new .cs files for new classes\n" +
                "2. Add tests to DeepLearningProtocol.Tests/DeepLearningProtocolTests.cs\n" +
                "3. Run dotnet test to verify"
            )},
            { 10, (
                "What are future enhancements?",
                "  • Neural network layer integration\n" +
                "  • Async processing with Tasks\n" +
                "  • JSON/database persistence\n" +
                "  • ML-based DLP rules\n" +
                "  • REST API wrapper"
            )}
        };

        /// <summary>
        /// Displays the main menu and handles user choices.
        /// </summary>
        public static void DisplayMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║     Deep Learning Protocol - Interactive Menu          ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
                Console.WriteLine("1. Run Interactive Protocol");
                Console.WriteLine("2. View FAQ");
                Console.WriteLine("3. Translate Text");
                Console.WriteLine("4. View System Data Map");
                Console.WriteLine("5. Translate & Store Text");
                Console.WriteLine("6. Manage Translation Rules");
                Console.WriteLine("7. Exit\n");
                Console.Write("Choose an option (1-7): ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        RunInteractiveProtocol();
                        break;
                    case "2":
                        DisplayFAQ();
                        break;
                    case "3":
                        RunTranslator();
                        break;
                    case "4":
                        DisplaySystemDataMap();
                        break;
                    case "5":
                        TranslateAndStoreText();
                        break;
                    case "6":
                        ManageTranslationRules();
                        break;
                    case "7":
                        Console.WriteLine("\nThank you for using Deep Learning Protocol!");
                        return;
                    default:
                        Console.WriteLine("\nInvalid choice. Press Enter to continue...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        /// <summary>
        /// Displays the FAQ menu and allows users to view answers to specific questions.
        /// </summary>
        private static void DisplayFAQ()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                    FAQ - Frequently Asked Questions     ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

                // Display all FAQ questions with numbers
                foreach (var faq in FAQs)
                {
                    Console.WriteLine($"{faq.Key}. {faq.Value.Question}");
                }

                Console.WriteLine($"{FAQs.Count + 1}. Back to Main Menu\n");
                Console.Write("Choose a question (1-11): ");

                if (int.TryParse(Console.ReadLine(), out int faqChoice))
                {
                    if (faqChoice == FAQs.Count + 1)
                    {
                        break; // Return to main menu
                    }

                    if (FAQs.TryGetValue(faqChoice, out var faq))
                    {
                        Console.Clear();
                        Console.WriteLine($"╔════════════════════════════════════════════════════════╗");
                        Console.WriteLine($"║  Q: {faq.Question}");
                        Console.WriteLine($"╚════════════════════════════════════════════════════════╝\n");
                        Console.WriteLine($"A: {faq.Answer}\n");
                        Console.Write("Press Enter to continue...");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid selection. Press Enter to continue...");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Press Enter to continue...");
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// Runs the interactive protocol workflow.
        /// Prompts user for input, goal, and depth level, then executes the protocol.
        /// Includes DLP protection for suspicious content.
        /// </summary>
        private static void RunTranslator()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║          Multi-Language Translator (4 Languages)      ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

                Console.WriteLine($"Dictionary size: {Translator.GetDictionarySize()} phrases\n");

                Console.WriteLine("Available Languages:");
                Console.WriteLine("1. Spanish");
                Console.WriteLine("2. Arabic");
                Console.WriteLine("3. French");
                Console.WriteLine("4. View Available Phrases");
                Console.WriteLine("5. Back to Main Menu\n");

                Console.Write("Choose a language or option (1-5): ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                    case "2":
                    case "3":
                        TranslateText(choice);
                        break;
                    case "4":
                        DisplayAvailablePhrases();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("\nInvalid choice. Press Enter to continue...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        /// <summary>
        /// Translates user input text to the selected language.
        /// </summary>
        private static void TranslateText(string languageChoice)
        {
            var targetLanguage = languageChoice switch
            {
                "1" => Translator.Language.Spanish,
                "2" => Translator.Language.Arabic,
                "3" => Translator.Language.French,
                _ => Translator.Language.English
            };

            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║           Translator to {Translator.GetLanguageName(targetLanguage),10}");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

            Console.WriteLine($"Translating to: {Translator.GetLanguageName(targetLanguage)} (Code: {Translator.GetLanguageCode(targetLanguage)})\n");

            Console.Write("Enter text in English to translate: ");
            var englishText = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(englishText))
            {
                Console.WriteLine("\nNo text entered. Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            var translation = Translator.Translate(englishText, targetLanguage);

            Console.WriteLine("\n--- Translation Result ---");
            Console.WriteLine($"Original (English): {englishText}");
            Console.WriteLine($"Translated ({Translator.GetLanguageName(targetLanguage)}): {translation}\n");

            if (Translator.IsPhraseAvailable(englishText.ToLower()))
            {
                Console.WriteLine("✓ This phrase is in the translation dictionary!");
            }
            else
            {
                Console.WriteLine("ℹ This phrase was translated using available vocabulary.");
            }

            Console.Write("\nPress Enter to continue...");
            Console.ReadLine();
        }

        /// <summary>
        /// Displays all available phrases in the translation dictionary.
        /// </summary>
        private static void DisplayAvailablePhrases()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║          Available Phrases for Translation             ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

            var phrases = Translator.GetAvailablePhrases().ToList();
            Console.WriteLine($"Total phrases available: {phrases.Count}\n");

            // Display phrases in columns
            var columnsPerPage = 2;
            var rowsPerPage = 15;
            var pageCount = (int)Math.Ceiling((double)phrases.Count / (columnsPerPage * rowsPerPage));

            for (int page = 0; page < pageCount; page++)
            {
                if (page > 0)
                {
                    Console.Write("\nPress Enter to see more phrases...");
                    Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                    Console.WriteLine("║          Available Phrases for Translation             ║");
                    Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
                }

                var startIdx = page * columnsPerPage * rowsPerPage;
                var endIdx = Math.Min(startIdx + columnsPerPage * rowsPerPage, phrases.Count);

                for (int i = startIdx; i < endIdx; i += columnsPerPage)
                {
                    var phrase1 = i < phrases.Count ? phrases[i] : "";
                    var phrase2 = i + 1 < phrases.Count ? phrases[i + 1] : "";
                    Console.WriteLine($"  • {phrase1,-30} • {phrase2,-30}");
                }
            }

            Console.Write("\nPress Enter to return to translator menu...");
            Console.ReadLine();
        }

        /// <summary>
        /// Displays the system data translation map for all core data.
        /// Shows states, interfaces, and operations in selected language.
        /// </summary>
        private static void DisplaySystemDataMap()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║            System Data Translation Map                ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

                Console.WriteLine($"Total system data entries: {CoreData.GetTotalDataSize()}\n");

                Console.WriteLine("Select language to view translations:");
                Console.WriteLine("1. Spanish");
                Console.WriteLine("2. Arabic");
                Console.WriteLine("3. French");
                Console.WriteLine("4. Back to Main Menu\n");

                Console.Write("Choose language (1-4): ");
                var choice = Console.ReadLine();

                var language = choice switch
                {
                    "1" => Translator.Language.Spanish,
                    "2" => Translator.Language.Arabic,
                    "3" => Translator.Language.French,
                    "4" => null,
                    _ => (Translator.Language?)null
                };

                if (choice == "4")
                    return;

                if (language.HasValue)
                {
                    CoreData.DisplayDataMap(language.Value);
                }
                else
                {
                    Console.WriteLine("\nInvalid choice. Press Enter to continue...");
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// Runs the interactive protocol workflow.
        /// Prompts user for input, goal, and depth level, then executes the protocol.
        /// Includes DLP protection for suspicious content.
        /// </summary>
        private static void RunInteractiveProtocol()
        {
            // Initialize the protocol instance
            var protocol = new DeepLearningProtocol();

            // Outer loop allows multiple protocol executions in one session
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║        Deep Learning Protocol - Execution             ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
                Console.WriteLine($"Current State: {protocol.GetCurrentState()}\n");

                // Prompt for user question/input
                Console.Write("Enter your question or input (or press Enter for default 'Raw sensory data'): ");
                var userInput = Console.ReadLine();
                var initialInput = string.IsNullOrWhiteSpace(userInput) ? "Raw sensory data" : userInput;

                // Prompt for goal
                Console.Write("Enter your goal (or press Enter for default 'Solve complex problem'): ");
                var userGoal = Console.ReadLine();
                var goal = string.IsNullOrWhiteSpace(userGoal) ? "Solve complex problem" : userGoal;

                // Prompt for depth level with validation
                Console.Write("Enter processing depth (1-10, or press Enter for default 5): ");
                var userDepthStr = Console.ReadLine();
                var depth = 5;
                if (!string.IsNullOrWhiteSpace(userDepthStr) && int.TryParse(userDepthStr, out var depthValue))
                {
                    if (depthValue > 0 && depthValue <= 10)
                    {
                        depth = depthValue;
                    }
                    else
                    {
                        Console.WriteLine("Depth out of range. Using default depth of 5.");
                        System.Threading.Thread.Sleep(1500);
                    }
                }

                // Display processing summary
                Console.WriteLine($"\n--- Processing: Input='{initialInput}', Goal='{goal}', Depth={depth} ---\n");
                System.Threading.Thread.Sleep(1000);

                // Execute the protocol
                var result = protocol.ExecuteProtocol(
                    initialInput: initialInput,
                    goal: goal,
                    depth: depth
                );

                // Display results
                Console.WriteLine("\n--- Protocol Result ---");
                Console.WriteLine(result);
                Console.WriteLine($"\nFinal State: {protocol.GetCurrentState()}");

                // Ask if user wants to continue
                Console.WriteLine("\n--- Continue? (y/n) ---");
                var again = Console.ReadLine();
                if (again?.ToLower() != "y")
                {
                    break; // Exit interactive protocol loop
                }
            }
        }

        /// <summary>
        /// Translates text from console input and stores it in the database.
        /// </summary>
        private static void TranslateAndStoreText()
        {
            using (var context = new ProtocolDbContext())
            {
                var manager = new TranslationManager(context);

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                    Console.WriteLine("║         Translate & Store Text to Database             ║");
                    Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

                    Console.Write("Enter text to translate (or 'back' to return): ");
                    var text = Console.ReadLine();

                    if (text?.ToLower() == "back")
                        break;

                    if (string.IsNullOrWhiteSpace(text))
                    {
                        Console.WriteLine("\nNo text entered. Press Enter to continue...");
                        Console.ReadLine();
                        continue;
                    }

                    try
                    {
                        var translated = manager.TranslateAndStore(text);

                        Console.WriteLine("\n--- Translation Stored Successfully ---");
                        Console.WriteLine($"ID: {translated.Id}");
                        Console.WriteLine($"Spanish: {translated.SpanishTranslation}");
                        Console.WriteLine($"Arabic: {translated.ArabicTranslation}");
                        Console.WriteLine($"French: {translated.FrenchTranslation}");
                        Console.WriteLine($"\nQuality Score: {translated.QualityScore}/100");

                        Console.Write("\nPress Enter to continue...");
                        Console.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\n[ERROR] {ex.Message}");
                        Console.Write("Press Enter to continue...");
                        Console.ReadLine();
                    }
                }
            }
        }

        /// <summary>
        /// Manages translation rules - create, update, delete, and view.
        /// </summary>
        private static void ManageTranslationRules()
        {
            using (var context = new ProtocolDbContext())
            {
                var manager = new TranslationManager(context);

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                    Console.WriteLine("║          Manage Translation Rules                     ║");
                    Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

                    Console.WriteLine("1. View All Rules");
                    Console.WriteLine("2. Create New Rule");
                    Console.WriteLine("3. Update Rule");
                    Console.WriteLine("4. Delete Rule");
                    Console.WriteLine("5. View Translation History");
                    Console.WriteLine("6. Back to Main Menu\n");

                    Console.Write("Choose an option (1-6): ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            manager.DisplayAllRules();
                            break;

                        case "2":
                            Console.Clear();
                            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                            Console.WriteLine("║              Create Translation Rule                  ║");
                            Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

                            Console.Write("Enter source text (English): ");
                            var source = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(source))
                            {
                                Console.WriteLine("\nSource text cannot be empty. Press Enter...");
                                Console.ReadLine();
                                break;
                            }

                            Console.Write("Enter Spanish translation: ");
                            var spanish = Console.ReadLine() ?? string.Empty;

                            Console.Write("Enter Arabic translation: ");
                            var arabic = Console.ReadLine() ?? string.Empty;

                            Console.Write("Enter French translation: ");
                            var french = Console.ReadLine() ?? string.Empty;

                            Console.Write("Enter category (default: Custom): ");
                            var category = Console.ReadLine() ?? "Custom";

                            Console.Write("Enter priority 1-10 (default: 5): ");
                            int priority = int.TryParse(Console.ReadLine(), out var p) ? p : 5;

                            if (manager.CreateRule(source, spanish, arabic, french, category, priority))
                                Console.WriteLine("\n✓ Rule created successfully!");
                            else
                                Console.WriteLine("\n✗ Rule already exists or error occurred.");

                            Console.Write("Press Enter to continue...");
                            Console.ReadLine();
                            break;

                        case "3":
                            Console.Clear();
                            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                            Console.WriteLine("║              Update Translation Rule                  ║");
                            Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

                            Console.Write("Enter source text to update: ");
                            var sourceToUpdate = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(sourceToUpdate))
                            {
                                Console.WriteLine("\nSource text cannot be empty. Press Enter...");
                                Console.ReadLine();
                                break;
                            }

                            Console.Write("Enter new Spanish translation (leave blank to skip): ");
                            var newSpanish = Console.ReadLine();

                            Console.Write("Enter new Arabic translation (leave blank to skip): ");
                            var newArabic = Console.ReadLine();

                            Console.Write("Enter new French translation (leave blank to skip): ");
                            var newFrench = Console.ReadLine();

                            if (manager.UpdateRule(sourceToUpdate, newSpanish, newArabic, newFrench))
                                Console.WriteLine("\n✓ Rule updated successfully!");
                            else
                                Console.WriteLine("\n✗ Rule not found or error occurred.");

                            Console.Write("Press Enter to continue...");
                            Console.ReadLine();
                            break;

                        case "4":
                            Console.Clear();
                            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                            Console.WriteLine("║              Delete Translation Rule                  ║");
                            Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

                            Console.Write("Enter source text to delete: ");
                            var sourceToDelete = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(sourceToDelete))
                            {
                                Console.WriteLine("\nSource text cannot be empty. Press Enter...");
                                Console.ReadLine();
                                break;
                            }

                            Console.Write("Are you sure? (y/n): ");
                            if (Console.ReadLine()?.ToLower() == "y")
                            {
                                if (manager.DeleteRule(sourceToDelete))
                                    Console.WriteLine("\n✓ Rule deleted successfully!");
                                else
                                    Console.WriteLine("\n✗ Rule not found.");
                            }

                            Console.Write("Press Enter to continue...");
                            Console.ReadLine();
                            break;

                        case "5":
                            manager.DisplayAllTranslations();
                            break;

                        case "6":
                            return;

                        default:
                            Console.WriteLine("\nInvalid choice. Press Enter to continue...");
                            Console.ReadLine();
                            break;
                    }
                }
            }
        }
    }
}
