using System.Collections.Generic;

namespace Moonvalk.Systems
{
    /// <summary>
    /// An abstract representation for a queue System that adds and removes updatable objects.
    /// </summary>
    /// <typeparam name="Type">The type of System.</typeparam>
    public abstract class MoonQueueSystem<Type> : MoonSystem<Type>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        protected MoonQueueSystem()
        {
            RemovalQueue = new List<IQueueItem>();
            Queue = new List<IQueueItem>();
            Initialize();
        }

        #region Data Fields
        /// <summary>
        /// A list of all current queued items.
        /// </summary>
        public List<IQueueItem> Queue { get; protected set; }

        /// <summary>
        /// A queue of all items that will be removed on the following frame.
        /// </summary>
        public List<IQueueItem> RemovalQueue { get; protected set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Runs this System during each game tick.
        /// </summary>
        /// <param name="delta_">The current delta between last and current frame.</param>
        public override void Execute(float delta_)
        {
            // Remove elements from the RemovalQueue.
            if (RemovalQueue.Count > 0)
            {
                for (var index = 0; index < RemovalQueue.Count; index++) Queue.Remove(RemovalQueue[index]);

                RemovalQueue.Clear();
            }

            // Cancel System execution when no objects exist to act upon.
            if (Queue.Count == 0) return;

            for (var index = 0; index < Queue.Count; index++)
            {
                var item = Queue[index];
                if (!item.Update(delta_))
                {
                    item.HandleTasks();
                    if (item.IsComplete()) RemovalQueue.Add(item);
                }
            }
        }

        /// <summary>
        /// Gets all current queue items.
        /// </summary>
        /// <returns>Returns the full list of IQueueItem items.</returns>
        public List<IQueueItem> GetAll()
        {
            return Queue;
        }

        /// <summary>
        /// Removes all current queued items.
        /// </summary>
        public void RemoveAll()
        {
            Queue.Clear();
        }

        /// <summary>
        /// Clears the queue applied to this system.
        /// </summary>
        public override void Clear()
        {
            RemoveAll();
        }

        /// <summary>
        /// Adds an updatable item to the queue.
        /// </summary>
        /// <param name="itemToAdd_">The item to add.</param>
        public void Add(IQueueItem itemToAdd_)
        {
            if (Queue.Contains(itemToAdd_)) return;

            Queue.Add(itemToAdd_);
        }

        /// <summary>
        /// Removes an update-able item from the queue.
        /// </summary>
        /// <param name="itemToRemove_">The item to remove.</param>
        public void Remove(IQueueItem itemToRemove_)
        {
            Queue.Remove(itemToRemove_);
        }
        #endregion
    }
}