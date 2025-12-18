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
            var protocol = new DeepLearningProtocol();

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

            Console.WriteLine("\n--- Ask Another Question? (y/n) ---");
            var again = Console.ReadLine();
            if (again?.ToLower() == "y")
            {
                // Recursively call Main to allow multiple questions
                Main(args);
            }
            else
            {
                Console.WriteLine("Thank you for using the Deep Learning Protocol!");
            }
        }
    }
}
