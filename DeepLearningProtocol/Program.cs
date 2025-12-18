using System;
using System.Collections.Generic;

namespace DeepLearningProtocol
{
    /// <summary>
    /// AbstractCore represents the deepest reasoning layer (maroon/red node in the architecture).
    /// It provides fundamental processing logic that wraps input with standardized abstract processing notation.
    /// This is the base class inherited by DeepLearningProtocol.
    /// </summary>
    public class AbstractCore
    {
        /// <summary>
        /// Processes input through the core reasoning engine.
        /// </summary>
        /// <param name="input">The input string to process</param>
        /// <returns>A formatted string indicating abstract core processing has occurred</returns>
        public virtual string ProcessCoreReasoning(string input)
        {
            return $"[Abstract Core] Deep abstract processing: {input}";
        }
    }

    /// <summary>
    /// IAimInterface defines goal-oriented processing operations.
    /// Goals drive the protocol's reasoning direction through spiral-like exploration paths.
    /// </summary>
    public interface IAimInterface
    {
        /// <summary>Sets a new goal/aim for the protocol</summary>
        string SetAim(string goal);

        /// <summary>Pursues the current aim given a state</summary>
        string PursueAim(string currentState);
    }

    /// <summary>
    /// IDepthInterface defines hierarchical depth-based processing.
    /// Allows recursive application of abstract reasoning at configurable depth levels.
    /// </summary>
    public interface IDepthInterface
    {
        /// <summary>Processes input at a specified hierarchical depth level</summary>
        string ProcessAtDepth(string input, int depthLevel);
    }

    /// <summary>
    /// IStateInterface defines state management operations.
    /// Tracks and updates the protocol's current operational state.
    /// </summary>
    public interface IStateInterface
    {
        /// <summary>Retrieves the current state</summary>
        string GetCurrentState();

        /// <summary>Updates the state (with DLP protection)</summary>
        void UpdateState(string newState);
    }

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
                System.IO.Directory.CreateDirectory(_backupDir);
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
                var file = System.IO.Path.Combine(_backupDir, $"state_{DateTime.UtcNow:yyyyMMdd_HHmmss_fff}.txt");
                System.IO.File.WriteAllText(file, state ?? string.Empty);
            }
            catch
            {
                // best-effort backup; swallow IO errors to avoid crashes
            }
        }
    }

    /// <summary>
    /// DeepLearningProtocol is the main orchestrator combining all interfaces and components.
    /// It implements a hierarchical reasoning system with state management, goal pursuit, 
    /// and depth-based recursive processing. DLP protection is integrated throughout.
    /// </summary>
    public class DeepLearningProtocol : AbstractCore, IAimInterface, IDepthInterface, IStateInterface
    {
        /// <summary>Tracks the current operational state</summary>
        private string _currentState = "Initial";
        
        /// <summary>Tracks the current goal/aim</summary>
        private string _aim = "General Reasoning";
        
        /// <summary>DLP instance for content protection</summary>
        private readonly DataLossPrevention _dlp = new();

        /// <summary>
        /// Gets the current state of the protocol.
        /// </summary>
        /// <returns>The current state string</returns>
        public string GetCurrentState() => _currentState;

        /// <summary>
        /// Updates the protocol state with DLP protection.
        /// Backs up previous state and blocks suspicious content.
        /// </summary>
        /// <param name="newState">The new state to set</param>
        public void UpdateState(string newState)
        {
            // Backup current state before changing
            try { _dlp.BackupState(_currentState); } catch { }

            // If the incoming state looks like meme or binary payload, block it
            if (_dlp.IsPotentialMeme(newState))
            {
                Console.WriteLine("DLP: Potential meme-like content detected. State update blocked and backed up.");
                _currentState = $"[DLP-BLOCKED]";
                return;
            }

            _currentState = newState;
            Console.WriteLine($"State updated: {_currentState}");
        }

        /// <summary>
        /// Sets a new goal/aim and updates the state accordingly.
        /// </summary>
        /// <param name="goal">The goal to pursue</param>
        /// <returns>Confirmation message</returns>
        public string SetAim(string goal)
        {
            _aim = goal;
            UpdateState($"Aiming: {_aim}");
            return $"Aim set to: {_aim}";
        }

        /// <summary>
        /// Pursues the current aim given a state.
        /// Applies core reasoning and associates result with the aim.
        /// </summary>
        /// <param name="currentState">The current state to pursue from</param>
        /// <returns>Result of aim pursuit</returns>
        public string PursueAim(string currentState)
        {
            var coreResult = ProcessCoreReasoning(currentState);
            return $"[Aim Pursuit] {coreResult} towards {_aim}";
        }

        /// <summary>
        /// Processes input at a specified hierarchical depth.
        /// Recursively applies abstract reasoning 'depthLevel' times.
        /// </summary>
        /// <param name="input">The input to process</param>
        /// <param name="depthLevel">How many layers of processing to apply</param>
        /// <returns>Result of depth processing</returns>
        public string ProcessAtDepth(string input, int depthLevel)
        {
            var processed = input;
            for (int i = 0; i < depthLevel; i++)
            {
                processed = ProcessCoreReasoning(processed);
            }
            UpdateState($"Depth {depthLevel} processed");
            return $"[Depth {depthLevel}] {processed}";
        }

        /// <summary>
        /// Executes the complete protocol workflow:
        /// 1. Sets the aim
        /// 2. Processes input at specified depth
        /// 3. Pursues the aim with the processed result
        /// </summary>
        /// <param name="initialInput">The initial input/question</param>
        /// <param name="goal">The goal to pursue</param>
        /// <param name="depth">How many levels of hierarchical processing</param>
        /// <returns>Final protocol result</returns>
        public string ExecuteProtocol(string initialInput, string goal, int depth)
        {
            SetAim(goal);
            var depthOutput = ProcessAtDepth(initialInput, depth);
            var finalOutput = PursueAim(depthOutput);
            return finalOutput;
        }
    }


    /// <summary>
    /// Main entry point for the Deep Learning Protocol application.
    /// Provides an interactive menu for users to run the protocol, view FAQs, or exit.
    /// </summary>
    class Program
    {
        // Static dictionary storing all FAQ questions and answers for easy reference
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
                "This executes 7 XUnit tests covering all core methods and edge cases"
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
                "1. Update Program.cs with new methods/interfaces\n" +
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
        /// Main entry point - displays the main menu and handles user choices.
        /// </summary>
        static void Main(string[] args)
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
        static void DisplayFAQ()
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
        static void RunInteractiveProtocol()
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

