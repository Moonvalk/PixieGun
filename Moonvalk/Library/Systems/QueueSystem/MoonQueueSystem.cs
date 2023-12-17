using System.Collections.Generic;

namespace Moonvalk.Systems
{
    /// <summary>
    /// An abstract representation for a queue System that adds and removes updatable objects.
    /// </summary>
    /// <typeparam name="Type">The type of System.</typeparam>
    public abstract class MoonQueueSystem<Type> : MoonSystem<Type>
    {
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

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected MoonQueueSystem()
        {
            this.RemovalQueue = new List<IQueueItem>();
            this.Queue = new List<IQueueItem>();
            base.Initialize();
        }

        #region Public Methods

        /// <summary>
        /// Runs this System during each game tick.
        /// </summary>
        /// <param name="delta_">The current delta between last and current frame.</param>
        public override void Execute(float delta_)
        {
            // Remove elements from the RemovalQueue.
            if (this.RemovalQueue.Count > 0)
            {
                for (var index = 0; index < this.RemovalQueue.Count; index++)
                {
                    this.Queue.Remove(this.RemovalQueue[index]);
                }

                this.RemovalQueue.Clear();
            }

            // Cancel System execution when no objects exist to act upon.
            if (this.Queue.Count == 0)
            {
                return;
            }

            for (var index = 0; index < this.Queue.Count; index++)
            {
                var item = this.Queue[index];
                if (!item.Update(delta_))
                {
                    item.HandleTasks();
                    if (item.IsComplete())
                    {
                        this.RemovalQueue.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Gets all current queue items.
        /// </summary>
        /// <returns>Returns the full list of IQueueItem items.</returns>
        public List<IQueueItem> GetAll()
        {
            return this.Queue;
        }

        /// <summary>
        /// Removes all current queued items.
        /// </summary>
        public void RemoveAll()
        {
            this.Queue.Clear();
        }

        /// <summary>
        /// Clears the queue applied to this system.
        /// </summary>
        public override void Clear()
        {
            this.RemoveAll();
        }

        /// <summary>
        /// Adds an updatable item to the queue.
        /// </summary>
        /// <param name="itemToAdd_">The item to add.</param>
        public void Add(IQueueItem itemToAdd_)
        {
            if (this.Queue.Contains(itemToAdd_))
            {
                return;
            }

            this.Queue.Add(itemToAdd_);
        }

        /// <summary>
        /// Removes an update-able item from the queue.
        /// </summary>
        /// <param name="itemToRemove_">The item to remove.</param>
        public void Remove(IQueueItem itemToRemove_)
        {
            this.Queue.Remove(itemToRemove_);
        }

        #endregion
    }
}