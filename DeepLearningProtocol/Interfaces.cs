namespace DeepLearningProtocol
{
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
}
