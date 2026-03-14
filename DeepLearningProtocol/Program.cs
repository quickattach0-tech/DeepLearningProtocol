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

        static async Task RunAsAgent(string[] args)
        {
            string agentName = args.Length > 1 ? args[1] : "DLP";
            var protocol = new DeepLearningProtocol();
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5033/chatHub")
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

            connection.On<string, string>("ReceiveRating", (agent, rating) =>
            {
                Console.WriteLine($"{agent} rated: {rating}/5 stars");
            });

            connection.On("ConferenceStarted", async () =>
            {
                Console.WriteLine("Conference started! Agent is thinking...");
                // Perform reasoning to decide rating
                var reasoning = protocol.ExecuteProtocol("Rate the quality of this discussion on a scale of 1-5", "Make rating decision", 2);
                // Extract rating from reasoning (simple heuristic)
                int rating = 3; // default
                if (reasoning.Contains("excellent") || reasoning.Contains("5")) rating = 5;
                else if (reasoning.Contains("good") || reasoning.Contains("4")) rating = 4;
                else if (reasoning.Contains("average") || reasoning.Contains("3")) rating = 3;
                else if (reasoning.Contains("poor") || reasoning.Contains("2")) rating = 2;
                else if (reasoning.Contains("terrible") || reasoning.Contains("1")) rating = 1;
                
                await connection.InvokeAsync("SendRating", agentName, rating.ToString());
                Console.WriteLine($"{agentName} rated: {rating}/5");
            });

            await connection.StartAsync();
            Console.WriteLine($"{agentName} connected. Press any key to exit.");
            Console.ReadKey();
            await connection.StopAsync();
        }
    }
}


