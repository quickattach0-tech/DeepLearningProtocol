using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace DeepLearningProtocol
{
    /// <summary>
    /// Main entry point for the Deep Learning Protocol application.
    /// Delegates to MenuSystem for interactive menu display and protocol execution.
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "agent")
            {
                await RunAsAgent(args);
            }
            else if (args.Length > 0 && args[0] == "process-image")
            {
                ProcessInstructionImage();
            }
            else
            {
                MenuSystem.DisplayMainMenu();
            }
        }

        static void ProcessInstructionImage()
        {
            var ct = new CoreTranslation();
            var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Instructions", "Instruction.png");
            
            try
            {
                var result = ct.ProcessImage(imagePath);
                
                Console.WriteLine("=== Image Processing Results ===");
                Console.WriteLine($"Image: {result.ImagePath}");
                Console.WriteLine($"Dimensions: {result.Width}x{result.Height}");
                Console.WriteLine($"Contains Text: {result.ContainsText}");
                Console.WriteLine($"Extracted Text: {result.ExtractedText}");
                Console.WriteLine($"Translated Text: {result.TranslatedText}");
                Console.WriteLine($"Translation Quality: {result.TranslationQuality}");
                
                if (result.ColorAnalysis != null && result.ColorAnalysis.Any())
                {
                    Console.WriteLine("Top Colors:");
                    foreach (var color in result.ColorAnalysis.Take(5))
                    {
                        Console.WriteLine($"  {color.Key}: {color.Value:F1}%");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing image: {ex.Message}");
            }
        }
        {
            string agentName = args.Length > 1 ? args[1] : "Agent1";
            var protocol = new DeepLearningProtocol();
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chatHub")
                .Build();

            connection.On<string, string>("ReceiveMessage", async (user, message) =>
            {
                Console.WriteLine($"{user}: {message}");
                // Agent thinking: process the message through the protocol
                var result = protocol.ExecuteProtocol(message, "Analyze message", 1);
                Console.WriteLine($"{agentName} analysis: {result}");
                // Could send response back to chat
                await connection.InvokeAsync("SendMessage", agentName, $"Analysis: {result.Substring(0, Math.Min(100, result.Length))}");
            });

            connection.On<string, string>("ReceiveVote", (agent, vote) =>
            {
                Console.WriteLine($"{agent} voted: {vote}");
            });

            connection.On("ConferenceStarted", async () =>
            {
                Console.WriteLine("Conference started! Agent is thinking...");
                // Perform reasoning to decide vote
                var reasoning = protocol.ExecuteProtocol("Should I vote yes on this proposal?", "Make voting decision", 2);
                string vote = reasoning.Contains("yes") || reasoning.Contains("approve") ? "Yes" : "No";
                await connection.InvokeAsync("SendVote", agentName, vote);
            });

            await connection.StartAsync();
            Console.WriteLine($"{agentName} connected. Press any key to exit.");
            Console.ReadKey();
            await connection.StopAsync();
        }
    }
}


