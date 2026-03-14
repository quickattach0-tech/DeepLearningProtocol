using System;

namespace DeepLearningProtocol
{
    /// <summary>
    /// DeepLearningProtocol is the main orchestrator combining all interfaces and components.
    /// It implements a hierarchical reasoning system with state management, goal pursuit, 
    /// and depth-based recursive processing. QT (Quality Translation) protection is integrated throughout.
    /// </summary>
    public class DeepLearningProtocol : AbstractCore, IAimInterface, IDepthInterface, IStateInterface
    {
        /// <summary>Tracks the current operational state</summary>
        private string _currentState = "Initial";
        
        /// <summary>Tracks the current goal/aim</summary>
        private string _aim = "General Reasoning";
        
        /// <summary>CT instance for core translation and uptime tracking</summary>
        private readonly CoreTranslation _ct = new();

        /// <summary>
        /// Gets the current state of the protocol.
        /// </summary>
        /// <returns>The current state string</returns>
        public string GetCurrentState() => _currentState;

        /// <summary>
        /// Updates the protocol state with QT protection.
        /// Assesses quality and blocks low-quality content.
        /// </summary>
        /// <param name="newState">The new state to set</param>
        public void UpdateState(string newState)
        {
            // Assess quality before changing state
            int qualityScore = _ct.AssessQuality(newState);
            
            // If quality score is too low, block it
            if (qualityScore < 30)
            {
                Console.WriteLine("CT: Content quality score too low. State update blocked.");
                _ct.StoreQualityMetric(newState, qualityScore, CoreTranslation.Language.English, "");

                _currentState = $"[CT-BLOCKED]";
                return;
            }

            _currentState = newState;
            _ct.RecordUptimeEvent();
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
}
