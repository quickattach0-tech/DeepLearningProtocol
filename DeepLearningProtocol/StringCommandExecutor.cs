using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace DeepLearningProtocol
{
    /// <summary>
    /// StringCommandExecutor handles execution of string commands stored in the database.
    /// Follows the Deep Learning Protocol pattern with hierarchical processing and DLP protection.
    /// </summary>
    public class StringCommandExecutor
    {
        private readonly ProtocolDbContext _context;
        private readonly DeepLearningProtocol _protocol;
        private readonly CoreTranslation _ct;

        /// <summary>Initializes the command executor with database context and protocol</summary>
        public StringCommandExecutor(ProtocolDbContext context, DeepLearningProtocol protocol, CoreTranslation ct)
        {
            _context = context ?? new ProtocolDbContext();
            _protocol = protocol ?? new DeepLearningProtocol();
            _ct = ct ?? new CoreTranslation();
        }

        /// <summary>
        /// Executes a command by name with protocol-level processing.
        /// Applies DLP protection if configured.
        /// </summary>
        public string ExecuteCommand(string commandName, string? input = null)
        {
            try
            {
                var command = _context.CommandDefinitions
                    .FirstOrDefault(c => c.CommandName.ToLower() == commandName.ToLower() && c.IsEnabled);

                if (command == null)
                    return $"[ERROR] Command '{commandName}' not found or disabled.";

                // Check Core Translation protection
                if (command.ApplyDLPProtection)
                {
                    var qualityScore = _ct.AssessQuality(command.CommandPattern);
                    if (qualityScore < 30)
                    {
                        _ct.StoreQualityMetric(command.CommandPattern, qualityScore, CoreTranslation.Language.English, "");
                        return "[CT-BLOCKED] Command pattern quality score too low.";
                    }
                    // Record uptime event
                    _ct.RecordUptimeEvent();
                }

                // Execute through protocol with configured depth
                var goal = $"Execute command: {command.CommandName}";
                var executionInput = input ?? command.CommandPattern;
                
                var result = _protocol.ExecuteProtocol(
                    initialInput: executionInput,
                    goal: goal,
                    depth: command.ExecutionDepth
                );

                // Update command statistics
                command.ExecutionCount++;
                command.LastExecutedAt = DateTime.UtcNow;
                command.LastExecutionResult = result[..Math.Min(500, result.Length)];
                command.ModifiedAt = DateTime.UtcNow;

                _context.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                return $"[ERROR] Command execution failed: {ex.Message}";
            }
        }

        /// <summary>Creates and stores a new command definition in the database</summary>
        public bool CreateCommand(string commandName, string pattern, string? description = null, string category = "Protocol")
        {
            try
            {
                if (_context.CommandDefinitions.Any(c => c.CommandName.ToLower() == commandName.ToLower()))
                    return false; // Command already exists

                var command = new CommandDefinition
                {
                    CommandName = commandName,
                    CommandPattern = pattern,
                    Description = description ?? $"Command: {commandName}",
                    Category = category,
                    IsEnabled = true,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow
                };

                _context.CommandDefinitions.Add(command);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Updates an existing command definition</summary>
        public bool UpdateCommand(string commandName, string? pattern = null, string? description = null, int? depth = null)
        {
            try
            {
                var command = _context.CommandDefinitions
                    .FirstOrDefault(c => c.CommandName.ToLower() == commandName.ToLower());

                if (command == null)
                    return false;

                if (!string.IsNullOrEmpty(pattern))
                    command.CommandPattern = pattern;

                if (!string.IsNullOrEmpty(description))
                    command.Description = description;

                if (depth.HasValue && depth >= 1 && depth <= 10)
                    command.ExecutionDepth = depth.Value;

                command.ModifiedAt = DateTime.UtcNow;
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Disables a command without deleting it</summary>
        public bool DisableCommand(string commandName)
        {
            try
            {
                var command = _context.CommandDefinitions
                    .FirstOrDefault(c => c.CommandName.ToLower() == commandName.ToLower());

                if (command == null)
                    return false;

                command.IsEnabled = false;
                command.ModifiedAt = DateTime.UtcNow;
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Enables a previously disabled command</summary>
        public bool EnableCommand(string commandName)
        {
            try
            {
                var command = _context.CommandDefinitions
                    .FirstOrDefault(c => c.CommandName.ToLower() == commandName.ToLower());

                if (command == null)
                    return false;

                command.IsEnabled = true;
                command.ModifiedAt = DateTime.UtcNow;
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Gets all available commands</summary>
        public IEnumerable<CommandDefinition> GetAllCommands() =>
            _context.CommandDefinitions.OrderBy(c => c.CommandName).ToList();

        /// <summary>Gets commands by category</summary>
        public IEnumerable<CommandDefinition> GetCommandsByCategory(string category) =>
            _context.CommandDefinitions
                .Where(c => c.Category == category && c.IsEnabled)
                .OrderBy(c => c.CommandName)
                .ToList();

        /// <summary>Gets command by name</summary>
        public CommandDefinition? GetCommand(string commandName) =>
            _context.CommandDefinitions
                .FirstOrDefault(c => c.CommandName.ToLower() == commandName.ToLower());

        /// <summary>Deletes a command from the database</summary>
        public bool DeleteCommand(string commandName)
        {
            try
            {
                var command = _context.CommandDefinitions
                    .FirstOrDefault(c => c.CommandName.ToLower() == commandName.ToLower());

                if (command == null)
                    return false;

                _context.CommandDefinitions.Remove(command);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Gets command statistics</summary>
        public void DisplayCommandStats(string commandName)
        {
            var command = GetCommand(commandName);
            if (command == null)
            {
                Console.WriteLine($"[ERROR] Command '{commandName}' not found.");
                return;
            }

            Console.WriteLine($"\n╔════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║          Command Statistics: {commandName,-25}║");
            Console.WriteLine($"╚════════════════════════════════════════════════════════╝\n");

            Console.WriteLine($"Name:                {command.CommandName}");
            Console.WriteLine($"Description:        {command.Description}");
            Console.WriteLine($"Category:           {command.Category}");
            Console.WriteLine($"Status:             {(command.IsEnabled ? "Enabled" : "Disabled")}");
            Console.WriteLine($"Execution Count:    {command.ExecutionCount}");
            Console.WriteLine($"Execution Depth:    {command.ExecutionDepth}");
            Console.WriteLine($"DLP Protection:     {(command.ApplyDLPProtection ? "Yes" : "No")}");
            Console.WriteLine($"Created:            {command.CreatedAt:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Modified:           {command.ModifiedAt:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Last Executed:      {(command.LastExecutedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Never")}");
            Console.WriteLine($"Last Result:        {command.LastExecutionResult[..Math.Min(60, command.LastExecutionResult.Length)]}...");

            Console.Write("\nPress Enter to continue...");
            Console.ReadLine();
        }

        /// <summary>Displays all available commands in a formatted table</summary>
        public void DisplayAllCommands()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              Available String Commands                 ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

            var commands = GetAllCommands().ToList();

            if (!commands.Any())
            {
                Console.WriteLine("No commands defined yet.\n");
                Console.Write("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"{"Name",-20} {"Category",-12} {"Enabled",-8} {"Executions",-12}");
            Console.WriteLine(new string('─', 55));

            foreach (var cmd in commands)
            {
                Console.WriteLine($"{cmd.CommandName,-20} {cmd.Category,-12} {(cmd.IsEnabled ? "Yes" : "No"),-8} {cmd.ExecutionCount,-12}");
            }

            Console.WriteLine();
            Console.Write("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
