using System;
using System.Collections.Generic;
using Moonvalk.Accessory;
using Moonvalk.Utilities;

namespace Moonvalk.Animation {
	/// <summary>
	/// Container representing a singular Wobble instance.
	/// </summary>
	/// <typeparam name="Unit">The type of value that will be affected by Spring forces</typeparam>
	public abstract class BaseMoonWobble<Unit> : IMoonWobble<Unit> {
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
		public float Time { get; private set; } = 0f;

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

		/// <summary>
		/// The maximum time allowed before a reset occurs.
		/// </summary>
		protected const float MAX_TIME_VALUE = 100000.0f;

		#region Constructor(s)
		/// <summary>
		/// Default constructor made without setting up references.
		/// </summary>
		protected BaseMoonWobble() {
			// ...
		}

		/// <summary>
		/// Constructor for creating a new BaseWobble.
		/// </summary>
		/// <param name="referenceValues_">Array of references to values.</param>
		protected BaseMoonWobble(params Ref<float>[] referenceValues_) {
			this.SetReferences(referenceValues_);
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Sets all reference values that this Wobble will manipulate.
		/// </summary>
		/// <param name="referenceValues_">Array of references to values.</param>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> SetReferences(params Ref<float>[] referenceValues_) {
			// Store reference to properties.
			this.Properties = referenceValues_;

			// Create new arrays for storing property start, end, and easing functions.
			this.StartValues = new Unit[referenceValues_.Length];
			return this;
		}

		/// <summary>
		/// Starts this Wobble with the current settings.
		/// </summary>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> Start() {
			this.updateStartValues();
			if (this.EaseInTween != null) {
				this.EaseInTween.Start();
				this.CurrentState = MoonWobbleState.Idle;
			} else {
				this.handleDuration();
				this.CurrentState = MoonWobbleState.Start;
			}
			this.Events.Run(this.CurrentState);
			(Global.GetSystem<MoonWobbleSystem>() as MoonWobbleSystem).Add(this);
			return this;
		}

		/// <summary>
		/// Stops this Wobble.
		/// </summary>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> Stop() {
			if (this.EaseOutTween != null) {
				this.EaseOutTween.Start();
			} else {
				this.CurrentState = MoonWobbleState.Stopped;
			}
			return this;
		}

		/// <summary>
		/// Updates this Wobble each game tick.
		/// </summary>
		/// <param name="deltaTime_">The duration of time between last and current game tick.</param>
		/// <returns>Returns true when this object is active and false when it is complete.</returns>
		public bool Update(float deltaTime_) {
			this.animate(deltaTime_);
			if (this.CurrentState == MoonWobbleState.Complete) {
				return false;
			}
			if (this.CurrentState == MoonWobbleState.Stopped || this.CurrentState == MoonWobbleState.Idle) {
				return true;
			}
			this.CurrentState = MoonWobbleState.Update;
			this.Events.Run(this.CurrentState);
			return true;
		}

		/// <summary>
		/// Called to add an ease in to the wobble animation.
		/// </summary>
		/// <param name="parameters_">Properties that adjust the ease in Tween.</param>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> EaseIn(MoonTweenParams parameters_ = null) {
			this._strength = 0f;
			this.EaseInTween?.Delete();
			this.EaseInTween = null;
			this.EaseInTween = new MoonTween(() => ref this._strength);
			this.EaseInTween.SetParameters(parameters_ ?? new MoonTweenParams()).To(1f);
			this.EaseInTween.OnStart(() => {
				this.CurrentState = MoonWobbleState.Start;
				this.Events.Run(this.CurrentState);
			}).OnComplete(() => {
				this.handleDuration();
			});
			return this;
		}

		/// <summary>
		/// Called to add an ease out to the wobble animation.
		/// </summary>
		/// <param name="parameters_">Properties that adjust the ease in Tween.</param>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> EaseOut(MoonTweenParams parameters_ = null) {
			this.EaseOutTween?.Delete();
			this.EaseOutTween = null;
			this.EaseOutTween = new MoonTween(() => ref this._strength);
			this.EaseOutTween.SetParameters(parameters_ ?? new MoonTweenParams()).To(0f);
			this.EaseOutTween.OnComplete(() => {
				this.CurrentState = MoonWobbleState.Complete;
			});
			return this;
		}
		
		/// <summary>
		/// Called to add an ease in and out to the wobble animation.
		/// </summary>
		/// <param name="parameters_">Properties that adjust the ease in Tween.</param>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> EaseInOut(MoonTweenParams parameters_ = null) {
			this.EaseIn(parameters_).EaseOut(parameters_);
			return this;
		}

		/// <summary>
		/// Sets the frequency of the sin wave used for animation.
		/// </summary>
		/// <param name="frequency_">The new frequency value.</param>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> SetFrequency(float frequency_) {
			this.Frequency = frequency_;
			return this;
		}
		
		/// <summary>
		/// Sets the amplitude of the sin wave used for animation.
		/// </summary>
		/// <param name="amplitude_">The new amplitude value.</param>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> SetAmplitude(float amplitude_) {
			this.Amplitude = amplitude_;
			return this;
		}

		/// <summary>
		/// Sets the duration of this animation when expected to run for a finite amount of time.
		/// </summary>
		/// <param name="duration_">The duration in seconds.</param>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> SetDuration(float duration_) {
			this.Duration = duration_;
			return this;
		}

		/// <summary>
		/// Sets the percentage of the property that will be affected. This is useful for
		/// multi-axis values that need to be affected differently.
		/// </summary>
		/// <param name="percentage_">The percentage value per axis, when applicable.</param>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> SetPercentage(Unit percentage_) {
			this.Percentage = percentage_;
			return this;
		}

		/// <summary>
		/// Called to set all parameters from a reference object.
		/// </summary>
		/// <param name="parameters_">All properties that will be assigned.</param>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> SetParameters(MoonWobbleParams parameters_) {
			this.SetFrequency(parameters_.Frequency).SetAmplitude(parameters_.Amplitude).SetDuration(parameters_.Duration);
			if (parameters_.EaseIn != null) {
				this.EaseIn(parameters_.EaseIn);
			}
			if (parameters_.EaseOut != null) {
				this.EaseOut(parameters_.EaseOut);
			}
			return this;
		}

		/// <summary>
		/// Removes this Wobble on the following game tick.
		/// </summary>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> Delete() {
			this.Reset();
			this.CurrentState = MoonWobbleState.Complete;
			return this;
		}

		/// <summary>
		/// Defines Actions that will occur when this Wobble begins.
		/// </summary>
		/// <param name="tasksToAdd_">Array of Actions to add.</param>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> OnStart(params Action[] tasksToAdd_) {
			this.Events.AddAction(MoonWobbleState.Start, tasksToAdd_);
			return this;
		}

		/// <summary>
		/// Defines Actions that will occur when this Wobble updates.
		/// </summary>
		/// <param name="tasksToAdd_">Array of Actions to add.</param>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> OnUpdate(params Action[] tasksToAdd_) {
			this.Events.AddAction(MoonWobbleState.Update, tasksToAdd_);
			return this;
		}

		/// <summary>
		/// Defines Actions that will occur once this Wobble has completed.
		/// </summary>
		/// <param name="tasksToAdd_">Array of Actions to add.</param>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> OnComplete(params Action[] tasksToAdd_) {
			this.Events.AddAction(MoonWobbleState.Complete, tasksToAdd_);
			return this;
		}

		/// <summary>
		/// Defines Actions that will occur once this Wobble has completed.
		/// </summary>
		/// <param name="tasksToAdd_">Array of Actions to add.</param>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> Then(params Action[] tasksToAdd_) {
			return this.OnComplete(tasksToAdd_);
		}

		/// <summary>
		/// Clears all Actions that have been assigned to this Wobble.
		/// </summary>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> Reset() {
			this.Events.ClearAll();
			return this;
		}

		/// <summary>
		/// Clears all Actions that have been assigned to this Wobble for the given state.
		/// </summary>
		/// <param name="state_">The state to reset actions for.</param>
		/// <returns>Returns this Wobble object.</returns>
		public BaseMoonWobble<Unit> Reset(MoonWobbleState state_) {
			this.Events.Clear(state_);
			return this;
		}

		/// <summary>
		/// Called to force handle tasks for the current state.
		/// </summary>
		public void HandleTasks()  {
			this.Events.Run(this.CurrentState);
		}

		/// <summary>
		/// Gets the current state of this Wobble.
		/// </summary>
		/// <returns>Returns the current state.</returns>
		public MoonWobbleState GetCurrentState() {
			return this.CurrentState;
		}

		/// <summary>
		/// Initializes a custom Wobble based on a reference value as a property.
		/// </summary>
		/// <param name="referenceValue_">The property to be animated.</param>
		/// <param name="percentage_">the percentage of the property that will be affected. This is useful for
		/// multi-axis values that need to be affected differently.</param>
		/// <param name="parameters_">Properties that adjust how this animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should begin immediately.</param>
		/// <returns>Returns the new Wobble instance.</returns>
		public static BaseMoonWobble<Unit> CustomWobbleTo<WobbleType>(
			Ref<float> referenceValue_,
			Unit percentage_,
			MoonWobbleParams parameters_ = null,
			bool start_ = true
		) where WobbleType : BaseMoonWobble<Unit>, new() {
			Ref<float>[] refs = new Ref<float>[1] { referenceValue_ };
			return BaseMoonWobble<Unit>.CustomWobbleTo<WobbleType>(refs, percentage_, parameters_, start_);
		}

		/// <summary>
		/// Initializes a custom Wobble based on a reference value as a property.
		/// </summary>
		/// <param name="referenceValues_">The property to be animated.</param>
		/// <param name="percentage_">the percentage of the property that will be affected. This is useful for
		/// multi-axis values that need to be affected differently.</param>
		/// <param name="parameters_">Properties that adjust how this animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should begin immediately.</param>
		/// <returns>Returns the new Wobble instance.</returns>
		public static BaseMoonWobble<Unit> CustomWobbleTo<WobbleType>(
			Ref<float>[] referenceValues_,
			Unit percentage_,
			MoonWobbleParams parameters_ = null,
			bool start_ = true
		) where WobbleType : BaseMoonWobble<Unit>, new() {
			BaseMoonWobble<Unit>.CustomWobbles = BaseMoonWobble<Unit>.CustomWobbles ?? new Dictionary<Ref<float>[], BaseMoonWobble<Unit>>();
			if (BaseMoonWobble<Unit>.CustomWobbles.ContainsKey(referenceValues_)) {
				BaseMoonWobble<Unit>.CustomWobbles[referenceValues_].Delete();
				BaseMoonWobble<Unit>.CustomWobbles.Remove(referenceValues_);
			}
			BaseMoonWobble<Unit> wobble = new WobbleType();
			wobble.SetReferences(referenceValues_).SetParameters(parameters_ ?? new MoonWobbleParams())
				.SetPercentage(percentage_).OnComplete(() => {
					BaseMoonWobble<Unit>.CustomWobbles.Remove(referenceValues_);
			});
			if (start_) {
				wobble.Start();
			}

			BaseMoonWobble<Unit>.CustomWobbles.Add(referenceValues_, wobble);
			return wobble;
		}

		/// <summary>
		/// Gets a custom Wobble object for the provided reference value, if it exists.
		/// </summary>
		/// <typeparam name="Unit">The type of used for this reference value.</typeparam>
		/// <param name="referenceValue_">The reference value a Wobble object is applied to.</param>
		/// <returns>Returns the requested Wobble object if it exists or null if it cannot be found.</returns>
		public static BaseMoonWobble<Unit> GetCustomWobble(Ref<float> referenceValue_) {
			Ref<float>[] refs = new Ref<float>[1] { referenceValue_ };
			return BaseMoonWobble<Unit>.GetCustomWobble(refs);
		}

		/// <summary>
		/// Gets a custom Wobble object for the provided reference value, if it exists.
		/// </summary>
		/// <typeparam name="Unit">The type of used for this reference value.</typeparam>
		/// <param name="referenceValues_">The reference value a Wobble object is applied to.</param>
		/// <returns>Returns the requested Wobble object if it exists or null if it cannot be found.</returns>
		public static BaseMoonWobble<Unit> GetCustomWobble(Ref<float>[] referenceValues_) {
			if (BaseMoonWobble<Unit>.CustomWobbles.ContainsKey(referenceValues_)) {
				return BaseMoonWobble<Unit>.CustomWobbles[referenceValues_];
			}
			return null;
		}

		/// <summary>
		/// Returns true when this object is complete.
		/// </summary>
		/// <returns>True when state is complete.</returns>
		public bool IsComplete() {
			return this.CurrentState == MoonWobbleState.Complete;
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Method used to update all properties available to this object.
		/// </summary>
		protected abstract void updateProperties();

		/// <summary>
		/// Called to continue animating this wobble object.
		/// </summary>
		/// <param name="deltaTime_">Time elapsed between last and current frame.</param>
		protected void animate(float deltaTime_) {
			this.Time = (this.Time + deltaTime_) % BaseMoonWobble<Unit>.MAX_TIME_VALUE;
			this.updateProperties();
		}

		/// <summary>
		/// Updates all starting values set the reference property values.
		/// </summary>
		protected abstract void updateStartValues();

		/// <summary>
		/// Called to handle adding a timer for stopping this animation when a duration has been defined.
		/// </summary>
		protected void handleDuration() {
			if (this.Duration > 0f) {
				MoonTimer.Wait(this.Duration, () => {
					this.Stop();
				});
			}
		}
		#endregion
	}
}
