using System;
using Moonvalk.Systems;

namespace Moonvalk.Utilities
{
    /// <summary>
    /// Contract for a Timer to implement in order to be added to the TimerSystem.
    /// </summary>
    public interface IMoonTimer : IQueueItem
    {
        /// <summary>
        /// Sets the duration of this Timer.
        /// </summary>
        /// <param name="duration_">The duration in seconds that need to elapse while this Timer runs.</param>
        /// <returns>Returns this Timer object.</returns>
        BaseMoonTimer SetDuration(float duration_);

        /// <summary>
        /// Adds Actions that will be run when this Timer is complete.
        /// </summary>
        /// <param name="tasksToAdd_">Array of Actions to add for this state.</param>
        /// <returns>Returns this Timer object.</returns>
        BaseMoonTimer OnComplete(params Action[] tasksToAdd_);

        /// <summary>
        /// Adds Actions that will be run when this Timer is started.
        /// </summary>
        /// <param name="tasksToAdd_">Array of Actions to add for this state.</param>
        /// <returns>Returns this Timer object.</returns>
        BaseMoonTimer OnStart(params Action[] tasksToAdd_);

        /// <summary>
        /// Adds Actions that will be run when this Timer is updated.
        /// </summary>
        /// <param name="tasksToAdd_">Array of Actions to add for this state.</param>
        /// <returns>Returns this Timer object.</returns>
        BaseMoonTimer OnUpdate(params Action[] tasksToAdd_);

        /// <summary>
        /// Starts this Timer with the latest configured settings.
        /// </summary>
        /// <returns>This Timer object.</returns>
        BaseMoonTimer Start();

        /// <summary>
        /// Starts this timer with a new duration in seconds.
        /// </summary>
        /// <param name="duration_">The duration in seconds for this timer to run.</param>
        /// <returns>Returns reference to this timer object.</returns>
        BaseMoonTimer Start(float duration_);

        /// <summary>
        /// Stops this Timer.
        /// </summary>
        /// <returns>Returns reference to this timer object.</returns>
        BaseMoonTimer Stop();

        /// <summary>
        /// Pauses this Timer.
        /// </summary>
        /// <returns>Returns reference to this timer object.</returns>
        BaseMoonTimer Pause();

        /// <summary>
        /// Resumes this Timer from wherever last left off.
        /// </summary>
        /// <returns>Returns reference to this timer object.</returns>
        BaseMoonTimer Resume();

        /// <summary>
        /// Applies a new time scale to this timer. Time scale is used to slow or speed up how quickly a timer should run for.
        /// </summary>
        /// <param name="timeScale_">The new time scale value.</param>
        /// <returns>Returns reference to this timer object.</returns>
        BaseMoonTimer SetTimeScale(float timeScale_);
    }
}