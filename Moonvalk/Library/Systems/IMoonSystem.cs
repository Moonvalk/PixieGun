namespace Moonvalk.Systems
{
    /// <summary>
    /// Contract for any MoonSystem that will be run within the MoonSystemManager.
    /// </summary>
    public interface IMoonSystem
    {
        /// <summary>
        /// Execution method for each System.
        /// </summary>
        /// <param name="deltaTime_">The duration in time between last and current frame.</param>
        void Execute(float deltaTime_);

        /// <summary>
        /// Used to clear the current queue applied to a system.
        /// </summary>
        void Clear();
    }
}