using System;

namespace Moonvalk.Utilities
{
    /// <summary>
    /// A configurable timer object with additional functionality for referencing tasks.
    /// </summary>
    public abstract class BaseMoonTimer : IMoonTimer
    {
        #region Public Getters/Setters
        /// <summary>
        /// Returns true when this Timer is actively running.
        /// </summary>
        /// <value>Returns a boolean value representing whether this Timer is running.</value>
        public bool IsRunning =>
            CurrentState == BaseMoonTimerState.Start || CurrentState == BaseMoonTimerState.Update;
        #endregion

        #region Private Methods
        /// <summary>
        /// Runs this Timer object.
        /// </summary>
        /// <param name="deltaTime_">Duration of time between last and current game tick.</param>
        /// <returns>Returns true when complete or false when actively running.</returns>
        protected bool RunTimer(float deltaTime_)
        {
            TimeRemaining -= deltaTime_ * TimeScale;
            if (TimeRemaining <= 0f) return true;

            return false;
        }
        #endregion

        #region Data Fields
        /// <summary>
        /// The duration in seconds this timer takes to complete.
        /// </summary>
        public float Duration { get; private set; }

        /// <summary>
        /// The time remaining in seconds.
        /// </summary>
        public float TimeRemaining { get; private set; }

        /// <summary>
        /// A multiplier to be applied to delta time.
        /// </summary>
        public float TimeScale { get; private set; } = 1f;

        /// <summary>
        /// The current timer state.
        /// </summary>
        public BaseMoonTimerState CurrentState { get; private set; } = BaseMoonTimerState.Idle;

        /// <summary>
        /// A map of all tasks that need to be completed by each available timer state.
        /// </summary>
        public MoonActionMap<BaseMoonTimerState> Events { get; protected set; } = new MoonActionMap<BaseMoonTimerState>();
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor taking no additional properties.
        /// </summary>
        protected BaseMoonTimer()
        {
            // ...
        }

        /// <summary>
        /// Constructor that allows the user to set a duration.
        /// </summary>
        /// <param name="duration_">The duration in seconds that this timer will run for.</param>
        protected BaseMoonTimer(float duration_)
        {
            SetDuration(duration_);
        }

        /// <summary>
        /// Constructor that allows the user to set completion tasks.
        /// </summary>
        /// <param name="onCompleteTasks_">Tasks to run on completion.</param>
        protected BaseMoonTimer(params Action[] onCompleteTasks_)
        {
            OnComplete(onCompleteTasks_);
        }

        /// <summary>
        /// Constructor that allows the user to set a duration and completion tasks.
        /// </summary>
        /// <param name="duration_">The duration in seconds that this timer will run for.</param>
        /// <param name="onCompleteTasks_">Tasks to run on completion.</param>
        protected BaseMoonTimer(float duration_, params Action[] onCompleteTasks_)
        {
            SetDuration(duration_);
            OnComplete(onCompleteTasks_);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the duration of this Timer.
        /// </summary>
        /// <param name="duration_">The duration in seconds that need to elapse while this Timer runs.</param>
        /// <returns>Returns this MoonTimer object.</returns>
        public BaseMoonTimer SetDuration(float duration_)
        {
            Duration = duration_;
            return this;
        }

        /// <summary>
        /// Adds Actions that will be run when this MoonTimer is complete.
        /// </summary>
        /// <param name="tasksToAdd_">Array of Actions to add for this state.</param>
        /// <returns>Returns this MoonTimer object.</returns>
        public BaseMoonTimer OnComplete(params Action[] tasksToAdd_)
        {
            Events.AddAction(BaseMoonTimerState.Complete, tasksToAdd_);
            return this;
        }

        /// <summary>
        /// Adds Actions that will be run when this MoonTimer is complete at the specified order index.
        /// </summary>
        /// <param name="index_">
        /// The index within the actions map where these tasks will be placed. Use
        /// -1 to add to the end of the list. Use 0 to add before any other tasks.
        /// </param>
        /// <param name="tasksToAdd_">The tasks to add.</param>
        /// <returns>Returns this MoonTimer object.</returns>
        public BaseMoonTimer OnComplete(int index_, params Action[] tasksToAdd_)
        {
            Events.AddAction(BaseMoonTimerState.Complete, index_, tasksToAdd_);
            return this;
        }

        /// <summary>
        /// Adds Actions that will be run when this MoonTimer is started.
        /// </summary>
        /// <param name="tasksToAdd_">Array of Actions to add for this state.</param>
        /// <returns>Returns this MoonTimer object.</returns>
        public BaseMoonTimer OnStart(params Action[] tasksToAdd_)
        {
            Events.AddAction(BaseMoonTimerState.Start, tasksToAdd_);
            return this;
        }

        /// <summary>
        /// Adds Actions that will be run when this MoonTimer is updated.
        /// </summary>
        /// <param name="tasksToAdd_">Array of Actions to add for this state.</param>
        /// <returns>Returns this MoonTimer object.</returns>
        public BaseMoonTimer OnUpdate(params Action[] tasksToAdd_)
        {
            Events.AddAction(BaseMoonTimerState.Update, tasksToAdd_);
            return this;
        }

        /// <summary>
        /// Starts this Timer with the latest configured settings.
        /// </summary>
        /// <returns>This MoonTimer object.</returns>
        public BaseMoonTimer Start()
        {
            TimeRemaining = Duration;
            CurrentState = BaseMoonTimerState.Start;
            Events.Run(CurrentState);

            (Global.GetSystem<MoonTimerSystem>() as MoonTimerSystem)?.Add(this);
            return this;
        }

        /// <summary>
        /// Starts this timer with a new duration in seconds.
        /// </summary>
        /// <param name="duration_">The duration in seconds for this timer to run.</param>
        /// <returns>Returns reference to this timer object.</returns>
        public BaseMoonTimer Start(float duration_)
        {
            SetDuration(duration_);
            return Start();
        }

        /// <summary>
        /// Stops this Timer.
        /// </summary>
        /// <returns>Returns reference to this timer object.</returns>
        public BaseMoonTimer Stop()
        {
            CurrentState = BaseMoonTimerState.Stopped;
            return this;
        }

        /// <summary>
        /// Pauses this Timer.
        /// </summary>
        /// <returns>Returns reference to this timer object.</returns>
        public BaseMoonTimer Pause()
        {
            CurrentState = BaseMoonTimerState.Idle;
            return this;
        }

        /// <summary>
        /// Resumes this Timer from wherever last left off.
        /// </summary>
        /// <returns>Returns reference to this timer object.</returns>
        public BaseMoonTimer Resume()
        {
            CurrentState = BaseMoonTimerState.Update;
            return this;
        }

        /// <summary>
        /// Applies a new time scale to this timer. Time scale is used to slow or speed up how quickly a timer should run for.
        /// </summary>
        /// <param name="timeScale_">The new time scale value.</param>
        /// <returns>Returns reference to this timer object.</returns>
        public BaseMoonTimer SetTimeScale(float timeScale_)
        {
            if (timeScale_ < 0f) return this;

            TimeScale = timeScale_;
            return this;
        }

        /// <summary>
        /// Updates this Timer.
        /// </summary>
        /// <param name="deltaTime_">Duration of time taken since last and current game tick.</param>
        /// <returns>Returns true when actively running and false once complete.</returns>
        public bool Update(float deltaTime_)
        {
            if (CurrentState == BaseMoonTimerState.Complete || CurrentState == BaseMoonTimerState.Stopped) return false;

            if (CurrentState == BaseMoonTimerState.Idle) return true;

            CurrentState = BaseMoonTimerState.Update;
            Events.Run(CurrentState);
            var complete = RunTimer(deltaTime_);
            if (complete)
            {
                CurrentState = BaseMoonTimerState.Complete;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Handles tasks for the current state.
        /// </summary>
        public void HandleTasks()
        {
            Events.Run(CurrentState);
        }

        /// <summary>
        /// Returns true when this Timer is complete.
        /// </summary>
        /// <returns>True when state is complete.</returns>
        public bool IsComplete()
        {
            return CurrentState == BaseMoonTimerState.Complete;
        }

        /// <summary>
        /// Clears all Actions that have been assigned to this timer.
        /// </summary>
        /// <returns>Returns this timer object.</returns>
        public BaseMoonTimer Reset()
        {
            Events.ClearAll();
            return this;
        }

        /// <summary>
        /// Clears all Actions that have been assigned to this timer for the given state.
        /// </summary>
        /// <param name="state_">The state to reset actions for.</param>
        /// <returns>Returns this timer object.</returns>
        public BaseMoonTimer Reset(BaseMoonTimerState state_)
        {
            Events.Clear(state_);
            return this;
        }
        #endregion
    }
}