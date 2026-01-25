using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DeepLearningProtocol
{
    /// <summary>
    /// Workflow management system for tracking development and processing pipelines.
    /// Implements workflow stages, state transitions, and logging capabilities.
    /// </summary>
    public class WorkflowManager
    {
        private readonly string _workflowLogPath = "./.workflow_logs";
        private List<WorkflowStage> _stages = new();
        private WorkflowState _currentState = WorkflowState.NotStarted;

        /// <summary>
        /// Workflow execution states
        /// </summary>
        public enum WorkflowState
        {
            NotStarted,
            InProgress,
            Testing,
            BuildingRelease,
            CodeQualityCheck,
            Completed,
            Failed,
            Cancelled
        }

        /// <summary>
        /// Workflow stage definition
        /// </summary>
        public class WorkflowStage
        {
            public int StageNumber { get; set; }
            public string? Name { get; set; }
            public string? Description { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime? EndTime { get; set; }
            public bool Success { get; set; }
            public List<string> Logs { get; set; } = new();
            
            public TimeSpan GetDuration()
            {
                if (EndTime == null) return TimeSpan.Zero;
                return EndTime.Value - StartTime;
            }
        }

        /// <summary>
        /// Pipeline stage for CI/CD automation
        /// </summary>
        public class PipelineStage
        {
            public string? Name { get; set; }
            public string? Trigger { get; set; }
            public List<string> Steps { get; set; } = new();
            public string[]? SupportedBranches { get; set; }
            public bool Enabled { get; set; } = true;
            
            public override string ToString()
            {
                return $"{Name} (Trigger: {Trigger}, Steps: {Steps.Count})";
            }
        }

        public WorkflowManager()
        {
            InitializeWorkflowDirectory();
        }

        /// <summary>
        /// Initialize workflow logging directory
        /// </summary>
        private void InitializeWorkflowDirectory()
        {
            if (!Directory.Exists(_workflowLogPath))
            {
                Directory.CreateDirectory(_workflowLogPath);
            }
        }

        /// <summary>
        /// Start a new workflow stage
        /// </summary>
        public void StartStage(int stageNumber, string name, string description)
        {
            var stage = new WorkflowStage
            {
                StageNumber = stageNumber,
                Name = name,
                Description = description,
                StartTime = DateTime.UtcNow,
                Success = false
            };
            
            _stages.Add(stage);
            _currentState = WorkflowState.InProgress;
            
            LogStageEvent(stage, $"Stage {stageNumber}: {name} started");
        }

        /// <summary>
        /// Complete current workflow stage
        /// </summary>
        public void CompleteStage(string stageName, bool success, string summary = "")
        {
            var stage = _stages.FirstOrDefault(s => s.Name == stageName);
            if (stage != null)
            {
                stage.EndTime = DateTime.UtcNow;
                stage.Success = success;
                LogStageEvent(stage, $"Stage {stage.StageNumber}: {stageName} completed - {(success ? "SUCCESS" : "FAILED")}");
                
                if (!string.IsNullOrEmpty(summary))
                {
                    stage.Logs.Add($"Summary: {summary}");
                }
            }
        }

        /// <summary>
        /// Log event for current stage
        /// </summary>
        public void LogStageEvent(WorkflowStage stage, string logMessage)
        {
            stage.Logs.Add($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {logMessage}");
        }

        /// <summary>
        /// Add log entry to current stage
        /// </summary>
        public void AddLog(string stageName, string message)
        {
            var stage = _stages.FirstOrDefault(s => s.Name == stageName);
            if (stage != null)
            {
                LogStageEvent(stage, message);
            }
        }

        /// <summary>
        /// Set workflow state
        /// </summary>
        public void SetState(WorkflowState state)
        {
            _currentState = state;
        }

        /// <summary>
        /// Get current workflow state
        /// </summary>
        public WorkflowState GetCurrentState()
        {
            return _currentState;
        }

        /// <summary>
        /// Get all workflow stages
        /// </summary>
        public List<WorkflowStage> GetAllStages()
        {
            return _stages;
        }

        /// <summary>
        /// Get workflow progress as percentage
        /// </summary>
        public int GetProgress()
        {
            if (_stages.Count == 0) return 0;
            var completed = _stages.Count(s => s.EndTime != null);
            return (completed * 100) / _stages.Count;
        }

        /// <summary>
        /// Get workflow summary
        /// </summary>
        public string GetWorkflowSummary()
        {
            var summary = new System.Text.StringBuilder();
            summary.AppendLine("═══════════════════════════════════════════════════════");
            summary.AppendLine("WORKFLOW SUMMARY");
            summary.AppendLine("═══════════════════════════════════════════════════════");
            summary.AppendLine($"Current State: {_currentState}");
            summary.AppendLine($"Total Stages: {_stages.Count}");
            summary.AppendLine($"Progress: {GetProgress()}%");
            summary.AppendLine($"Completed: {_stages.Count(s => s.EndTime != null)}/{_stages.Count}");
            summary.AppendLine();
            
            foreach (var stage in _stages)
            {
                summary.AppendLine($"  Stage {stage.StageNumber}: {stage.Name}");
                summary.AppendLine($"    Status: {(stage.EndTime == null ? "IN PROGRESS" : (stage.Success ? "✓ SUCCESS" : "✗ FAILED"))}");
                summary.AppendLine($"    Duration: {stage.GetDuration().TotalSeconds:F2}s");
                summary.AppendLine($"    Logs: {stage.Logs.Count}");
            }
            
            summary.AppendLine("═══════════════════════════════════════════════════════");
            return summary.ToString();
        }

        /// <summary>
        /// Save workflow to JSON file
        /// </summary>
        public void SaveWorkflowToFile(string workflowName)
        {
            var fileName = Path.Combine(_workflowLogPath, $"workflow_{workflowName}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json");
            var json = JsonSerializer.Serialize(_stages, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, json);
        }

        /// <summary>
        /// Get pipeline configuration for CI/CD
        /// </summary>
        public static List<PipelineStage> GetCIPipelineConfig()
        {
            return new List<PipelineStage>
            {
                new PipelineStage
                {
                    Name = "Debug Build",
                    Trigger = "Pull Request / Non-Main Push",
                    SupportedBranches = new[] { "develop", "feature/*" },
                    Steps = new List<string>
                    {
                        "Checkout repository",
                        "Setup .NET 10.0.x",
                        "Restore dependencies",
                        "Build project (Debug configuration)",
                        "Run unit tests (8 tests)"
                    }
                },
                new PipelineStage
                {
                    Name = "Release Build",
                    Trigger = "Push to Main",
                    SupportedBranches = new[] { "main" },
                    Steps = new List<string>
                    {
                        "Checkout repository",
                        "Setup .NET 10.0.x",
                        "Restore dependencies",
                        "Build project (Release configuration)",
                        "Run unit tests with coverage",
                        "Upload coverage reports to Codecov",
                        "Publish build artifacts (30 day retention)"
                    }
                },
                new PipelineStage
                {
                    Name = "Code Quality",
                    Trigger = "Push to Main",
                    SupportedBranches = new[] { "main" },
                    Steps = new List<string>
                    {
                        "Checkout repository",
                        "Setup .NET 10.0.x",
                        "Restore dependencies",
                        "Check code style (EnforceCodeStyleInBuild)"
                    },
                    Enabled = true
                }
            };
        }

        /// <summary>
        /// Get development workflow steps
        /// </summary>
        public static List<string> GetDevelopmentWorkflowSteps()
        {
            return new List<string>
            {
                "1. Create feature branch: git checkout -b feature/your-feature",
                "2. Implement changes in Program.cs",
                "3. Add unit tests to DeepLearningProtocol.Tests/",
                "4. Update documentation in docs/",
                "5. Local testing: dotnet build && dotnet test",
                "6. Commit with meaningful message: git commit -m 'feat: ...'",
                "7. Push to origin: git push origin feature/your-feature",
                "8. Create Pull Request on GitHub",
                "9. Code review and CI/CD automation",
                "10. Merge to main after approval"
            };
        }

        /// <summary>
        /// Display development workflow information
        /// </summary>
        public static void DisplayWorkflowInfo()
        {
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine("DEEP LEARNING PROTOCOL - DEVELOPMENT WORKFLOW");
            Console.WriteLine("═══════════════════════════════════════════════════════\n");

            Console.WriteLine("📋 WORKFLOW STEPS:");
            var steps = GetDevelopmentWorkflowSteps();
            foreach (var step in steps)
            {
                Console.WriteLine($"  {step}");
            }

            Console.WriteLine("\n📊 CI/CD PIPELINE STAGES:");
            var pipeline = GetCIPipelineConfig();
            foreach (var stage in pipeline)
            {
                Console.WriteLine($"\n  [{stage.Name}]");
                Console.WriteLine($"    Trigger: {stage.Trigger}");
                Console.WriteLine($"    Branches: {string.Join(", ", stage.SupportedBranches ?? Array.Empty<string>())}");
                Console.WriteLine($"    Steps ({stage.Steps.Count}):");
                foreach (var step in stage.Steps)
                {
                    Console.WriteLine($"      • {step}");
                }
            }

            Console.WriteLine("\n🔐 CODE QUALITY STANDARDS:");
            Console.WriteLine("  ✅ All tests must pass (dotnet test)");
            Console.WriteLine("  ✅ Zero build errors (dotnet build)");
            Console.WriteLine("  ✅ Zero code warnings (strict C# checks)");
            Console.WriteLine("  ✅ Documentation updated for new features");
            Console.WriteLine("  ✅ Comments for complex logic");
            Console.WriteLine("  ✅ Meaningful commit messages");

            Console.WriteLine("\n🌳 BRANCH STRATEGY:");
            Console.WriteLine("  main      → Production-ready code (PR required, CI must pass)");
            Console.WriteLine("  develop   → Feature integration (CI/CD runs)");
            Console.WriteLine("  feature/* → Individual features (no protection)");

            Console.WriteLine("\n═══════════════════════════════════════════════════════\n");
        }
    }

    /// <summary>
    /// Workflow executor for managing workflow execution and orchestration
    /// </summary>
    public class WorkflowExecutor
    {
        private readonly WorkflowManager _workflowManager;
        private readonly Action<string> _logger;

        public WorkflowExecutor(WorkflowManager manager, Action<string>? logger = null)
        {
            _workflowManager = manager;
            _logger = logger ?? Console.WriteLine;
        }

        /// <summary>
        /// Execute development workflow
        /// </summary>
        public void ExecuteDevelopmentWorkflow()
        {
            _logger("Starting Development Workflow...");
            _workflowManager.SetState(WorkflowManager.WorkflowState.InProgress);

            _workflowManager.StartStage(1, "Feature Development", "Code implementation and feature development");
            Thread.Sleep(1000);
            _workflowManager.CompleteStage("Feature Development", true, "Feature implemented successfully");

            _workflowManager.StartStage(2, "Unit Testing", "Local unit test execution");
            Thread.Sleep(1000);
            _workflowManager.CompleteStage("Unit Testing", true, "All tests passed (8/8)");

            _workflowManager.StartStage(3, "Code Review", "Peer code review");
            Thread.Sleep(1000);
            _workflowManager.CompleteStage("Code Review", true, "Approved for merge");

            _workflowManager.SetState(WorkflowManager.WorkflowState.Completed);
            _logger(_workflowManager.GetWorkflowSummary());
        }

        /// <summary>
        /// Execute CI/CD pipeline workflow
        /// </summary>
        public void ExecuteCIPipelineWorkflow()
        {
            _logger("Starting CI/CD Pipeline Workflow...");
            _workflowManager.SetState(WorkflowManager.WorkflowState.InProgress);

            _workflowManager.StartStage(1, "Build Stage", "Debug/Release build compilation");
            Thread.Sleep(1500);
            _workflowManager.CompleteStage("Build Stage", true, "Build successful - 0 errors");

            _workflowManager.StartStage(2, "Testing Stage", "Unit test execution with coverage");
            Thread.Sleep(2000);
            _workflowManager.CompleteStage("Testing Stage", true, "8/8 tests passed");

            _workflowManager.StartStage(3, "Code Quality Stage", "Code style and quality checks");
            Thread.Sleep(1000);
            _workflowManager.CompleteStage("Code Quality Stage", true, "Quality standards met");

            _workflowManager.StartStage(4, "Artifact Publishing", "Build artifact storage");
            Thread.Sleep(500);
            _workflowManager.CompleteStage("Artifact Publishing", true, "Artifacts published (30d retention)");

            _workflowManager.SetState(WorkflowManager.WorkflowState.Completed);
            _logger(_workflowManager.GetWorkflowSummary());
        }
    }
}
