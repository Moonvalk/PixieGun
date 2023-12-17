using System;

namespace Moonvalk.Utilities
{
    /// <summary>
    /// A simple use timer object with basic functionality.
    /// </summary>
    public class MoonTimer : BaseMoonTimer
    {
        #region Constructors
        /// <summary>
        /// Default constructor taking no additional properties.
        /// </summary>
        public MoonTimer()
        {
            // ...
        }

        /// <summary>
        /// Constructor that allows the user to set a duration.
        /// </summary>
        /// <param name="duration_">The duration in seconds that this timer will run for.</param>
        public MoonTimer(float duration_) : base(duration_)
        {
            // ...
        }

        /// <summary>
        /// Constructor that allows the user to set completion tasks.
        /// </summary>
        /// <param name="onCompleteTasks_">Tasks to run on completion.</param>
        public MoonTimer(params Action[] onCompleteTasks_) : base(onCompleteTasks_)
        {
            // ...
        }

        /// <summary>
        /// Constructor that allows the user to set a duration and completion tasks.
        /// </summary>
        /// <param name="duration_">The duration in seconds that this timer will run for.</param>
        /// <param name="onCompleteTasks_">Tasks to run on completion.</param>
        public MoonTimer(float duration_, params Action[] onCompleteTasks_) : base(duration_, onCompleteTasks_)
        {
            // ...
        }

        /// <summary>
        /// Called to generate a new unmanaged Timer object that simply waits before executing provided tasks.
        /// </summary>
        /// <param name="duration_">The duration in seconds before executing.</param>
        /// <param name="onComplete_">Tasks to be run on completion.</param>
        /// <returns>Returns the new Timer object.</returns>
        public static MoonTimer Wait(float duration_, params Action[] onComplete_)
        {
            var newTimer = new MoonTimer(duration_, onComplete_);
            newTimer.Start();
            return newTimer;
        }
        #endregion
    }
}