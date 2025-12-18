using System;

namespace DeepLearningProtocol
{
    // Abstract core - the deepest, most fundamental reasoning component (maroon/red node)
    public class AbstractCore
    {
        public virtual string ProcessCoreReasoning(string input)
        {
            return $"[Abstract Core] Deep abstract processing: {input}";
        }
    }

    // Aim Interface - goal-oriented processing layer
    public interface IAimInterface
    {
        string SetAim(string goal);
        string PursueAim(string currentState);
    }

    // Depth Interface - handles hierarchical depth in processing
    public interface IDepthInterface
    {
        string ProcessAtDepth(string input, int depthLevel);
    }

    // State Interface - manages and exposes current state
    public interface IStateInterface
    {
        string GetCurrentState();
        void UpdateState(string newState);
    }

    // Simple Data Loss Prevention helper
    // Implements basic rules to detect 'meme' or potentially large/binary content
    // and preserves backups of previous states to prevent loss.
    public class DataLossPrevention
    {
        private readonly string _backupDir = "./.dlp_backups";

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

        public bool IsPotentialMeme(string content)
        {
            if (string.IsNullOrEmpty(content)) return false;
            var lower = content.ToLowerInvariant();
            // simple heuristics: references to image files, base64 snippets, or word 'meme'
            if (lower.Contains("meme") || lower.Contains(".png") || lower.Contains(".jpg") || lower.Contains(".jpeg") || lower.Contains("data:image") || lower.Contains("base64,"))
                return true;
            // long single-line content may be a pasted binary blob
            if (content.Length > 200 && !content.Contains("\n")) return true;
            return false;
        }

        public void BackupState(string state)
        {
            try
            {
                var file = System.IO.Path.Combine(_backupDir, $"state_{DateTime.UtcNow:yyyyMMdd_HHmmss_fff}.txt");
                System.IO.File.WriteAllText(file, state ?? string.Empty);
            }
            catch
            {
                // best-effort backup; swallow IO errors
            }
        }
    }

    // Main Deep Learning Protocol class
    public class DeepLearningProtocol : AbstractCore, IAimInterface, IDepthInterface, IStateInterface
    {
        private string _currentState = "Initial";
        private string _aim = "General Reasoning";
        private readonly DataLossPrevention _dlp = new();

        public string GetCurrentState() => _currentState;

        public void UpdateState(string newState)
        {
            // backup current state before changing
            try { _dlp.BackupState(_currentState); } catch { }

            // If the incoming state looks like a meme or binary payload, prevent accidental loss
            if (_dlp.IsPotentialMeme(newState))
            {
                Console.WriteLine("DLP: Potential meme-like content detected. State update blocked and backed up.");
                // mark state as restricted but preserve previous
                _currentState = $"[DLP-BLOCKED]";
                return;
            }

            _currentState = newState;
            Console.WriteLine($"State updated: {_currentState}");
        }

        public string SetAim(string goal)
        {
            _aim = goal;
            UpdateState($"Aiming: {_aim}");
            return $"Aim set to: {_aim}";
        }

        public string PursueAim(string currentState)
        {
            var coreResult = ProcessCoreReasoning(currentState);
            return $"[Aim Pursuit] {coreResult} towards {_aim}";
        }

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

        public string ExecuteProtocol(string initialInput, string goal, int depth)
        {
            SetAim(goal);
            var depthOutput = ProcessAtDepth(initialInput, depth);
            var finalOutput = PursueAim(depthOutput);
            return finalOutput;
        }
    }

    class Program
    {
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
                        Console.WriteLine("Thank you for using the Deep Learning Protocol!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press Enter to try again...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        static void RunInteractiveProtocol()
        {
            var protocol = new DeepLearningProtocol();

            Console.Clear();
            Console.WriteLine("=== Deep Learning Protocol Interactive Console ===\n");
            Console.WriteLine($"Initial state: {protocol.GetCurrentState()}\n");

            // Prompt user for input
            Console.Write("Enter your question or input (or press Enter for default): ");
            var userInput = Console.ReadLine();
            var initialInput = string.IsNullOrWhiteSpace(userInput) ? "Raw sensory data" : userInput;

            Console.Write("Enter your goal (or press Enter for default): ");
            var userGoal = Console.ReadLine();
            var goal = string.IsNullOrWhiteSpace(userGoal) ? "Solve complex problem" : userGoal;

            Console.Write("Enter processing depth (1-10, or press Enter for default 5): ");
            var userDepthStr = Console.ReadLine();
            var depth = 5;
            if (!string.IsNullOrWhiteSpace(userDepthStr) && int.TryParse(userDepthStr, out var depthValue) && depthValue > 0 && depthValue <= 10)
            {
                depth = depthValue;
            }

            Console.WriteLine($"\n--- Processing: Input='{initialInput}', Goal='{goal}', Depth={depth} ---\n");

            var result = protocol.ExecuteProtocol(
                initialInput: initialInput,
                goal: goal,
                depth: depth
            );

            Console.WriteLine("\n--- Result ---");
            Console.WriteLine(result);
            Console.WriteLine($"\nFinal State: {protocol.GetCurrentState()}");

            Console.WriteLine("\nPress Enter to return to menu...");
            Console.ReadLine();
        }

        static void DisplayFAQ()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              FAQ - Frequently Asked Questions           ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

            var faqs = new[]
            {
                ("What is the Deep Learning Protocol?", "A hierarchical reasoning system that processes information through multiple layers: AbstractCore (deepest), Depth Interface (recursive processing), Aim Interface (goal-directed), and State Interface (state management). It includes Data Loss Prevention (DLP) to protect against accidental data corruption."),

                ("How do I run the program?", "Three ways:\n   - VS Code: Press Ctrl+Shift+B\n   - CLI: dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj\n   - Interactive: Run the program and answer console prompts for custom inputs, goals, and depth levels"),

                ("What is Data Loss Prevention (DLP)?", "A protective layer that:\n   - Detects meme/binary content (.png, .jpg, base64, \"meme\" keyword, large single-line payloads)\n   - Backs up states automatically to ./.dlp_backups/ with timestamps\n   - Blocks suspicious updates to prevent data loss"),

                ("What are the core components?", "- AbstractCore: Base reasoning layer\n   - IStateInterface: State get/update\n   - IAimInterface: Goal setting and pursuing\n   - IDepthInterface: Hierarchical processing at N levels\n   - DeepLearningProtocol: Main orchestrator combining all interfaces"),

                ("How does depth processing work?", "ProcessAtDepth(input, depth) recursively applies ProcessCoreReasoning() depth times. For example, depth=3 means the input gets wrapped in 3 layers of abstract processing."),

                ("Can I ask custom questions?", "Yes! The program is interactive. When you run it, you'll be prompted to enter:\n   - Your question/input\n   - Your goal\n   - Processing depth (1-10)\n   - Option to ask another question"),

                ("How do I run tests?", "Execute: dotnet test\n   This runs 7 XUnit tests covering all core methods and workflows."),

                ("What are the future enhancements?", "- Neural network layer integration\n   - Async processing with Tasks\n   - JSON/database persistence\n   - ML-based DLP rules\n   - REST API wrapper"),

                ("How do I extend the project?", "1. Update Program.cs with new methods/interfaces\n   2. Add tests to DeepLearningProtocol.Tests/DeepLearningProtocolTests.cs\n   3. Run dotnet test to verify"),

                ("What happens if I input meme-like content?", "The DLP layer detects it, backs up your current state, blocks the update, and sets state to [DLP-BLOCKED] to prevent accidental loss.")
            };

            for (int i = 0; i < faqs.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {faqs[i].Item1}");
                Console.WriteLine($"   → {faqs[i].Item2}\n");
            }

            Console.WriteLine("Press Enter to return to menu...");
            Console.ReadLine();
        }
    }
}
