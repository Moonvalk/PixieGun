using System;
using System.Collections.Generic;
using Moonvalk.Accessory;
using Moonvalk.Utilities;

namespace Moonvalk.Animation
{
    /// <summary>
    /// Container representing a singular Wobble instance.
    /// </summary>
    /// <typeparam name="Unit">The type of value that will be affected by Spring forces</typeparam>
    public abstract class BaseMoonWobble<Unit> : IMoonWobble<Unit>
    {
        /// <summary>
        /// The maximum time allowed before a reset occurs.
        /// </summary>
        protected const float MaxTimeValue = 100000.0f;

        #region Data Fields
        /// <summary>
        /// A reference to the property value(s) that will be modified.
        /// </summary>
        public Ref<float>[] Properties { get; private set; }

        /// <summary>
        /// The starting value.
        /// </summary>
        public Unit[] StartValues { get; private set; }

        /// <summary>
        /// The overall strength of wobble applied to Properties. This is adjusted to
        /// add easing in and out of the animation.
        /// </summary>
        protected float _strength = 1f;

        /// <summary>
        /// The frequency of the sin wave applied to achieve animation.
        /// </summary>
        public float Frequency { get; private set; } = 5f;

        /// <summary>
        /// The amplitude of the sin wave applied to achieve animation.
        /// </summary>
        public float Amplitude { get; private set; } = 10f;

        /// <summary>
        /// The current time since the animation began.
        /// </summary>
        public float Time { get; private set; }

        /// <summary>
        /// The duration of the wobble animation. Setting this below zero will cause
        /// the animation to loop infinitely.
        /// </summary>
        public float Duration { get; private set; } = -1f;

        /// <summary>
        /// The percentage of the property that will be affected. This is useful for
        /// multi-axis values that need to be affected differently.
        /// </summary>
        public Unit Percentage { get; private set; }

        /// <summary>
        /// Reference to an optional tween used for easing into the animation.
        /// </summary>
        public MoonTween EaseInTween { get; private set; }

        /// <summary>
        /// Reference to an optional tween used for easing out of the animation.
        /// </summary>
        public MoonTween EaseOutTween { get; private set; }

        /// <summary>
        /// The current state of this Wobble object.
        /// </summary>
        public MoonWobbleState CurrentState { get; private set; } = MoonWobbleState.Idle;

        /// <summary>
        /// A map of Actions that will occur while this Wobble is in an active state.
        /// </summary>
        public MoonActionMap<MoonWobbleState> Events { get; protected set; } = new MoonActionMap<MoonWobbleState>();

        /// <summary>
        /// Stores reference to custom Wobbles applied to user generated values.
        /// </summary>
        public static Dictionary<Ref<float>[], BaseMoonWobble<Unit>> CustomWobbles { get; protected set; }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Default constructor made without setting up references.
        /// </summary>
        protected BaseMoonWobble()
        {
            // ...
        }

        /// <summary>
        /// Constructor for creating a new BaseWobble.
        /// </summary>
        /// <param name="referenceValues_">Array of references to values.</param>
        protected BaseMoonWobble(params Ref<float>[] referenceValues_)
        {
            SetReferences(referenceValues_);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets all reference values that this Wobble will manipulate.
        /// </summary>
        /// <param name="referenceValues_">Array of references to values.</param>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> SetReferences(params Ref<float>[] referenceValues_)
        {
            // Store reference to properties.
            Properties = referenceValues_;

            // Create new arrays for storing property start, end, and easing functions.
            StartValues = new Unit[referenceValues_.Length];
            return this;
        }

        /// <summary>
        /// Starts this Wobble with the current settings.
        /// </summary>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> Start()
        {
            UpdateStartValues();
            if (EaseInTween != null)
            {
                EaseInTween.Start();
                CurrentState = MoonWobbleState.Idle;
            }
            else
            {
                HandleDuration();
                CurrentState = MoonWobbleState.Start;
            }

            Events.Run(CurrentState);
            (Global.GetSystem<MoonWobbleSystem>() as MoonWobbleSystem)?.Add(this);
            return this;
        }

        /// <summary>
        /// Stops this Wobble.
        /// </summary>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> Stop()
        {
            if (EaseOutTween != null)
                EaseOutTween.Start();
            else
                CurrentState = MoonWobbleState.Stopped;

            return this;
        }

        /// <summary>
        /// Updates this Wobble each game tick.
        /// </summary>
        /// <param name="deltaTime_">The duration of time between last and current game tick.</param>
        /// <returns>Returns true when this object is active and false when it is complete.</returns>
        public bool Update(float deltaTime_)
        {
            Animate(deltaTime_);
            if (CurrentState == MoonWobbleState.Complete) return false;

            if (CurrentState == MoonWobbleState.Stopped || CurrentState == MoonWobbleState.Idle) return true;

            CurrentState = MoonWobbleState.Update;
            Events.Run(CurrentState);
            return true;
        }

        /// <summary>
        /// Called to add an ease in to the wobble animation.
        /// </summary>
        /// <param name="parameters_">Properties that adjust the ease in Tween.</param>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> EaseIn(MoonTweenParams parameters_ = null)
        {
            _strength = 0f;
            EaseInTween?.Delete();
            EaseInTween = null;
            EaseInTween = new MoonTween(() => ref _strength);
            EaseInTween.SetParameters(parameters_ ?? new MoonTweenParams()).To(1f);

            EaseInTween.OnStart(() =>
                {
                    CurrentState = MoonWobbleState.Start;
                    Events.Run(CurrentState);
                })
                .OnComplete(() => { HandleDuration(); });

            return this;
        }

        /// <summary>
        /// Called to add an ease out to the wobble animation.
        /// </summary>
        /// <param name="parameters_">Properties that adjust the ease in Tween.</param>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> EaseOut(MoonTweenParams parameters_ = null)
        {
            EaseOutTween?.Delete();
            EaseOutTween = null;
            EaseOutTween = new MoonTween(() => ref _strength);
            EaseOutTween.SetParameters(parameters_ ?? new MoonTweenParams()).To(0f);

            EaseOutTween.OnComplete(() => { CurrentState = MoonWobbleState.Complete; });
            return this;
        }

        /// <summary>
        /// Called to add an ease in and out to the wobble animation.
        /// </summary>
        /// <param name="parameters_">Properties that adjust the ease in Tween.</param>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> EaseInOut(MoonTweenParams parameters_ = null)
        {
            EaseIn(parameters_).EaseOut(parameters_);

            return this;
        }

        /// <summary>
        /// Sets the frequency of the sin wave used for animation.
        /// </summary>
        /// <param name="frequency_">The new frequency value.</param>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> SetFrequency(float frequency_)
        {
            Frequency = frequency_;
            return this;
        }

        /// <summary>
        /// Sets the amplitude of the sin wave used for animation.
        /// </summary>
        /// <param name="amplitude_">The new amplitude value.</param>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> SetAmplitude(float amplitude_)
        {
            Amplitude = amplitude_;
            return this;
        }

        /// <summary>
        /// Sets the duration of this animation when expected to run for a finite amount of time.
        /// </summary>
        /// <param name="duration_">The duration in seconds.</param>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> SetDuration(float duration_)
        {
            Duration = duration_;
            return this;
        }

        /// <summary>
        /// Sets the percentage of the property that will be affected. This is useful for
        /// multi-axis values that need to be affected differently.
        /// </summary>
        /// <param name="percentage_">The percentage value per axis, when applicable.</param>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> SetPercentage(Unit percentage_)
        {
            Percentage = percentage_;
            return this;
        }

        /// <summary>
        /// Called to set all parameters from a reference object.
        /// </summary>
        /// <param name="parameters_">All properties that will be assigned.</param>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> SetParameters(MoonWobbleParams parameters_)
        {
            SetFrequency(parameters_.Frequency).SetAmplitude(parameters_.Amplitude).SetDuration(parameters_.Duration);

            if (parameters_.EaseIn != null) EaseIn(parameters_.EaseIn);

            if (parameters_.EaseOut != null) EaseOut(parameters_.EaseOut);

            return this;
        }

        /// <summary>
        /// Removes this Wobble on the following game tick.
        /// </summary>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> Delete()
        {
            Reset();
            CurrentState = MoonWobbleState.Complete;
            return this;
        }

        /// <summary>
        /// Defines Actions that will occur when this Wobble begins.
        /// </summary>
        /// <param name="tasksToAdd_">Array of Actions to add.</param>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> OnStart(params Action[] tasksToAdd_)
        {
            Events.AddAction(MoonWobbleState.Start, tasksToAdd_);
            return this;
        }

        /// <summary>
        /// Defines Actions that will occur when this Wobble updates.
        /// </summary>
        /// <param name="tasksToAdd_">Array of Actions to add.</param>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> OnUpdate(params Action[] tasksToAdd_)
        {
            Events.AddAction(MoonWobbleState.Update, tasksToAdd_);
            return this;
        }

        /// <summary>
        /// Defines Actions that will occur once this Wobble has completed.
        /// </summary>
        /// <param name="tasksToAdd_">Array of Actions to add.</param>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> OnComplete(params Action[] tasksToAdd_)
        {
            Events.AddAction(MoonWobbleState.Complete, tasksToAdd_);
            return this;
        }

        /// <summary>
        /// Defines Actions that will occur once this Wobble has completed.
        /// </summary>
        /// <param name="tasksToAdd_">Array of Actions to add.</param>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> Then(params Action[] tasksToAdd_)
        {
            return OnComplete(tasksToAdd_);
        }

        /// <summary>
        /// Clears all Actions that have been assigned to this Wobble.
        /// </summary>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> Reset()
        {
            Events.ClearAll();
            return this;
        }

        /// <summary>
        /// Clears all Actions that have been assigned to this Wobble for the given state.
        /// </summary>
        /// <param name="state_">The state to reset actions for.</param>
        /// <returns>Returns this Wobble object.</returns>
        public BaseMoonWobble<Unit> Reset(MoonWobbleState state_)
        {
            Events.Clear(state_);
            return this;
        }

        /// <summary>
        /// Called to force handle tasks for the current state.
        /// </summary>
        public void HandleTasks()
        {
            Events.Run(CurrentState);
        }

        /// <summary>
        /// Gets the current state of this Wobble.
        /// </summary>
        /// <returns>Returns the current state.</returns>
        public MoonWobbleState GetCurrentState()
        {
            return CurrentState;
        }

        /// <summary>
        /// Initializes a custom Wobble based on a reference value as a property.
        /// </summary>
        /// <param name="referenceValue_">The property to be animated.</param>
        /// <param name="percentage_">
        /// the percentage of the property that will be affected. This is useful for
        /// multi-axis values that need to be affected differently.
        /// </param>
        /// <param name="parameters_">Properties that adjust how this animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should begin immediately.</param>
        /// <returns>Returns the new Wobble instance.</returns>
        public static BaseMoonWobble<Unit> CustomWobbleTo<WobbleType>(Ref<float> referenceValue_, Unit percentage_, MoonWobbleParams parameters_ = null,
            bool start_ = true)
            where WobbleType : BaseMoonWobble<Unit>, new()
        {
            var refs = new[] { referenceValue_ };
            return CustomWobbleTo<WobbleType>(refs, percentage_, parameters_, start_);
        }

        /// <summary>
        /// Initializes a custom Wobble based on a reference value as a property.
        /// </summary>
        /// <param name="referenceValues_">The property to be animated.</param>
        /// <param name="percentage_">
        /// the percentage of the property that will be affected. This is useful for
        /// multi-axis values that need to be affected differently.
        /// </param>
        /// <param name="parameters_">Properties that adjust how this animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should begin immediately.</param>
        /// <returns>Returns the new Wobble instance.</returns>
        public static BaseMoonWobble<Unit> CustomWobbleTo<WobbleType>(Ref<float>[] referenceValues_, Unit percentage_, MoonWobbleParams parameters_ = null,
            bool start_ = true)
            where WobbleType : BaseMoonWobble<Unit>, new()
        {
            CustomWobbles = CustomWobbles ?? new Dictionary<Ref<float>[], BaseMoonWobble<Unit>>();

            if (CustomWobbles.ContainsKey(referenceValues_))
            {
                CustomWobbles[referenceValues_].Delete();

                CustomWobbles.Remove(referenceValues_);
            }

            BaseMoonWobble<Unit> wobble = new WobbleType();
            wobble.SetReferences(referenceValues_)
                .SetParameters(parameters_ ?? new MoonWobbleParams())
                .SetPercentage(percentage_)
                .OnComplete(() => { CustomWobbles.Remove(referenceValues_); });

            if (start_) wobble.Start();

            CustomWobbles.Add(referenceValues_, wobble);
            return wobble;
        }

        /// <summary>
        /// Gets a custom Wobble object for the provided reference value, if it exists.
        /// </summary>
        /// <typeparam name="Unit">The type of used for this reference value.</typeparam>
        /// <param name="referenceValue_">The reference value a Wobble object is applied to.</param>
        /// <returns>Returns the requested Wobble object if it exists or null if it cannot be found.</returns>
        public static BaseMoonWobble<Unit> GetCustomWobble(Ref<float> referenceValue_)
        {
            var refs = new[] { referenceValue_ };
            return GetCustomWobble(refs);
        }

        /// <summary>
        /// Gets a custom Wobble object for the provided reference value, if it exists.
        /// </summary>
        /// <typeparam name="Unit">The type of used for this reference value.</typeparam>
        /// <param name="referenceValues_">The reference value a Wobble object is applied to.</param>
        /// <returns>Returns the requested Wobble object if it exists or null if it cannot be found.</returns>
        public static BaseMoonWobble<Unit> GetCustomWobble(Ref<float>[] referenceValues_)
        {
            if (CustomWobbles.TryGetValue(referenceValues_, out var wobble)) return wobble;

            return null;
        }

        /// <summary>
        /// Returns true when this object is complete.
        /// </summary>
        /// <returns>True when state is complete.</returns>
        public bool IsComplete()
        {
            return CurrentState == MoonWobbleState.Complete;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Method used to update all properties available to this object.
        /// </summary>
        protected abstract void UpdateProperties();

        /// <summary>
        /// Called to continue animating this wobble object.
        /// </summary>
        /// <param name="deltaTime_">Time elapsed between last and current frame.</param>
        protected void Animate(float deltaTime_)
        {
            Time = (Time + deltaTime_) % MaxTimeValue;
            UpdateProperties();
        }

        /// <summary>
        /// Updates all starting values set the reference property values.
        /// </summary>
        protected abstract void UpdateStartValues();

        /// <summary>
        /// Called to handle adding a timer for stopping this animation when a duration has been defined.
        /// </summary>
        protected void HandleDuration()
        {
            if (Duration > 0f) MoonTimer.Wait(Duration, () => { Stop(); });
        }
        #endregion
    }
}