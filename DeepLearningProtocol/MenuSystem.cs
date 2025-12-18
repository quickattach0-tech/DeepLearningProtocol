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
                Console.WriteLine("3. Exit\n");
                Console.Write("Choose an option (1-3): ");

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
    }
}
