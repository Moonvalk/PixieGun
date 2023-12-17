namespace Moonvalk.Systems
{
    /// <summary>
    /// Contract for an update-able MoonQueueSystem object.
    /// </summary>
    public interface IQueueItem
    {
        /// <summary>
        /// Updates this object.
        /// </summary>
        /// <param name="deltaTime_">The duration of time between last and current game tick.</param>
        /// <returns>Returns true when this object is active and false when it is complete.</returns>
        bool Update(float deltaTime_);

        /// <summary>
        /// Handles all tasks for the current state of this queued item.
        /// </summary>
        void HandleTasks();

        /// <summary>
        /// Determines if this item is complete.
        /// </summary>
        /// <returns>True when the queue item is complete.</returns>
        bool IsComplete();
    }
}