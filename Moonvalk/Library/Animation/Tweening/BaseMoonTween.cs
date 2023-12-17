using System;
using System.Collections.Generic;
using Moonvalk.Accessory;
using Moonvalk.Utilities;

namespace Moonvalk.Animation {
	/// <summary>
	/// Container representing a singular Tween object.
	/// </summary>
	/// <typeparam name="Unit">The type of value that will be affected by Spring forces</typeparam>
	public abstract class BaseMoonTween<Unit> : IMoonTween<Unit> {
		#region Data Fields
		/// <summary>
		/// A reference to the property value(s) that will be modified.
		/// </summary>
		public Ref<float>[] Properties { get; protected set; }

		/// <summary>
		/// The target value that will be reached.
		/// </summary>
		public Unit[] TargetValues { get; protected set; }

		/// <summary>
		/// The starting value.
		/// </summary>
		public Unit[] StartValues { get; protected set; }

		/// <summary>
		/// A timer used to delay Tweens before playing.
		/// </summary>
		public MoonTimer DelayTimer { get; protected set; }

		/// <summary>
		/// A duration in seconds that it will take for this Tween to elapse.
		/// </summary>
		public float Duration { get; protected set; } = 1f;

		/// <summary>
		/// The percentage currently elapsed from 0f to 1f.
		/// </summary>
		public float Percentage { get; protected set; } = 0f;

		/// <summary>
		/// An EasingFunction to be applied to this Tween.
		/// </summary>
		public EasingFunction[] EasingFunctions { get; protected set; }

		/// <summary>
		/// The current state of this Tween object.
		/// </summary>
		public MoonTweenState CurrentState { get; protected set; } = MoonTweenState.Idle;

		/// <summary>
		/// Should this animation begin as soon as a Target value is assigned?
		/// </summary>
		public bool StartOnTargetAssigned { get; set; } = false;

		/// <summary>
		/// A map of Actions that will occur while this Tween is in an active state.
		/// </summary>
		public MoonActionMap<MoonTweenState> Events { get; protected set; } = new MoonActionMap<MoonTweenState>();

		/// <summary>
		/// Stores reference to custom tweens applied to user generated values.
		/// </summary>
		public static Dictionary<Ref<float>[], BaseMoonTween<Unit>> CustomTweens { get; protected set; }
		#endregion

		#region Constructor(s)
		/// <summary>
		/// Default constructor made without setting up references.
		/// </summary>
		protected BaseMoonTween() {
			// ...
		}

		/// <summary>
		/// Constructor for creating a new BaseTween.
		/// </summary>
		/// <param name="referenceValues_">Array of references to values.</param>
		protected BaseMoonTween(params Ref<float>[] referenceValues_) {
			this.SetReferences(referenceValues_);
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Sets all reference values that this Tween will manipulate.
		/// </summary>
		/// <param name="referenceValues_">Array of references to values.</param>
		/// <returns>Returns this Tween object.</returns>
		public BaseMoonTween<Unit> SetReferences(params Ref<float>[] referenceValues_) {
			// Store reference to properties.
			this.Properties = referenceValues_;

			// Create new arrays for storing property start, end, and easing functions.
			this.StartValues = new Unit[referenceValues_.Length];
			this.TargetValues = new Unit[referenceValues_.Length];
			this.EasingFunctions = new EasingFunction[referenceValues_.Length];
			for (var i = 0; i < this.EasingFunctions.Length; i++) {
				this.EasingFunctions[i] = Easing.Linear.None;
			}
			return this;
		}

		/// <summary>
		/// Starts this Tween with the current settings.
		/// </summary>
		/// <returns>Returns this Tween object.</returns>
		public BaseMoonTween<Unit> Start() {
			this.UpdateStartValues();
			this.Percentage = 0f;
			if (DelayTimer != null) {
				this.DelayTimer.Start();
				this.CurrentState = MoonTweenState.Idle;
			} else {
				this.CurrentState = MoonTweenState.Start;
			}
			this.Events.Run(this.CurrentState);
			(Global.GetSystem<MoonTweenSystem>() as MoonTweenSystem).Add(this);
			return this;
		}

		/// <summary>
		/// Stops this Tween.
		/// </summary>
		/// <returns>Returns this Tween object.</returns>
		public BaseMoonTween<Unit> Stop() {
			this.CurrentState = MoonTweenState.Stopped;
			return this;
		}

		/// <summary>
		/// Updates this Tween.
		/// </summary>
		/// <param name="deltaTime_">The duration of time between last and current game tick.</param>
		/// <returns>Returns true when this Tween is active and false when it is complete.</returns>
		public bool Update(float deltaTime_) {
			if (this.CurrentState == MoonTweenState.Complete) {
				return false;
			}
			if (this.CurrentState == MoonTweenState.Stopped) {
				return true;
			}
			if (this.CurrentState == MoonTweenState.Idle) {
				if (this.DelayTimer != null && this.DelayTimer.IsComplete()) {
					this.CurrentState = MoonTweenState.Start;
					this.Events.Run(this.CurrentState);
				} else {
					return true;
				}
			}
			
			var targetReached = this.Animate(deltaTime_);
			this.CurrentState = MoonTweenState.Update;
			this.Events.Run(this.CurrentState);
			if (targetReached) {
				this.CurrentState = MoonTweenState.Complete;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Sets the delay in seconds applied to this Tween.
		/// </summary>
		/// <param name="delaySeconds_">The delay duration in seconds.</param>
		/// <returns>Returns this Tween object.</returns>
		public BaseMoonTween<Unit> SetDelay(float delaySeconds_) {
			if (this.DelayTimer == null) {
				this.DelayTimer = new MoonTimer();
			}
			this.DelayTimer.SetDuration(delaySeconds_);
			return this;
		}
		
		/// <summary>
		/// Sets the duration in seconds that must elapse for this Tween to resolve.
		/// </summary>
		/// <param name="durationSeconds_">The duration of this Tween in seconds.</param>
		/// <returns>Returns this Tween object.</returns>
		public BaseMoonTween<Unit> SetDuration(float durationSeconds_) {
			this.Duration = durationSeconds_;
			return this;
		}

		/// <summary>
		/// Sets the target value(s) that this Tween will reach once complete.
		/// </summary>
		/// <param name="targetValues_">An array of target values for each property.</param>
		/// <returns>Returns this Tween object.</returns>
		public BaseMoonTween<Unit> To(params Unit[] targetValues_) {
			for (var i = 0; i < targetValues_.Length; i++) {
				this.TargetValues[i] = targetValues_[i];
			}
			if (this.StartOnTargetAssigned) {
				this.Start();
			}
			return this;
		}

		/// <summary>
		/// Sets an Easing Function for each available property.
		/// </summary>
		/// <param name="functions_">An array of Easing Functions per property.</param>
		/// <returns>Returns this Tween object.</returns>
		public BaseMoonTween<Unit> SetEase(params EasingFunction[] functions_) {
			for (var i = 0; i < EasingFunctions.Length; i++) {
				var nextFunc = (functions_.Length > i) ? functions_[i] : functions_[0];
				this.EasingFunctions[i] = nextFunc;
			}
			return this;
		}

		/// <summary>
		/// Called to set all parameters from a reference object.
		/// </summary>
		/// <param name="parameters_">All properties that will be assigned.</param>
		/// <returns>Returns this Tween object.</returns>
		public BaseMoonTween<Unit> SetParameters(MoonTweenParams parameters_) {
			this.SetDuration(parameters_.Duration).SetDelay(parameters_.Delay)
				.SetEase(parameters_.EasingType != Easing.Types.None ? Easing.Functions[parameters_.EasingType] : parameters_.EasingFunction);
			return this;
		}

		/// <summary>
		/// Removes this Tween object on the following game tick.
		/// </summary>
		/// <returns>Returns this Tween object.</returns>
		public BaseMoonTween<Unit> Delete() {
			this.Reset();
			this.CurrentState = MoonTweenState.Complete;
			return this;
		}

		/// <summary>
		/// Defines Actions that will occur when this Tween begins.
		/// </summary>
		/// <param name="tasksToAdd_">Array of Actions to add.</param>
		/// <returns>Returns this Tween object.</returns>
		public BaseMoonTween<Unit> OnStart(params Action[] tasksToAdd_) {
			this.Events.AddAction(MoonTweenState.Start, tasksToAdd_);
			return this;
		}

		/// <summary>
		/// Defines Actions that will occur when this Tween updates.
		/// </summary>
		/// <param name="tasksToAdd_">Array of Actions to add.</param>
		/// <returns>Returns this Tween object.</returns>
		public BaseMoonTween<Unit> OnUpdate(params Action[] tasksToAdd_) {
			this.Events.AddAction(MoonTweenState.Update, tasksToAdd_);
			return this;
		}

		/// <summary>
		/// Defines Actions that will occur once this Tween has completed.
		/// </summary>
		/// <param name="tasksToAdd_">Array of Actions to add.</param>
		/// <returns>Returns this Tween object.</returns>
		public BaseMoonTween<Unit> OnComplete(params Action[] tasksToAdd_) {
			this.Events.AddAction(MoonTweenState.Complete, tasksToAdd_);
			return this;
		}

		/// <summary>
		/// Defines Actions that will occur once this Tween has completed.
		/// </summary>
		/// <param name="tasksToAdd_">Array of Actions to add.</param>
		/// <returns>Returns this Tween object.</returns>
		public BaseMoonTween<Unit> Then(params Action[] tasksToAdd_) {
			return this.OnComplete(tasksToAdd_);
		}

		/// <summary>
		/// Clears all Actions that have been assigned to this Tween.
		/// </summary>
		/// <returns>Returns this Tween object.</returns>
		public BaseMoonTween<Unit> Reset() {
			this.Events.ClearAll();
			return this;
		}

		/// <summary>
		/// Clears all Actions that have been assigned to this Tween for the given state.
		/// </summary>
		/// <param name="state_">The state to reset actions for.</param>
		/// <returns>Returns this Tween object.</returns>
		public BaseMoonTween<Unit> Reset(MoonTweenState state_) {
			this.Events.Clear(state_);
			return this;
		}

		/// <summary>
		/// Adds a Tween that will begin following the original Tween's completion.
		/// </summary>
		/// <param name="triggeringTween_">The original Tween.</param>
		/// <param name="tweenToFollow_">The Tween to start on completion./param>
		/// <returns>Returns this Tween object.</returns>
		public static BaseMoonTween<Unit> operator +(BaseMoonTween<Unit> triggeringTween_, BaseMoonTween<Unit> tweenToFollow_) {
			return triggeringTween_.OnComplete(() => { tweenToFollow_.Start(); });
		}

		/// <summary>
		/// Adds an Action that will begin following this Tween's completion.
		/// </summary>
		/// <param name="triggeringTween_">The original MVTween.</param>
		/// <param name="taskForCompletion_">The Action to run on completion.</param>
		/// <returns>Returns this Tween object.</returns>
		public static BaseMoonTween<Unit> operator +(BaseMoonTween<Unit> triggeringTween_, Action taskForCompletion_) {
			return triggeringTween_.OnComplete(taskForCompletion_);
		}

		/// <summary>
		/// Called to force handle tasks for the current state.
		/// </summary>
		public void HandleTasks()  {
			this.Events.Run(this.CurrentState);
		}

		/// <summary>
		/// Gets the current state of this Tween.
		/// </summary>
		/// <returns>Returns the current state.</returns>
		public MoonTweenState GetCurrentState() {
			return this.CurrentState;
		}

		/// <summary>
		/// Initializes a custom tween based on a reference value as a property.
		/// </summary>
		/// <typeparam name="TweenType">The type of Tween that will be used.</typeparam>
		/// <param name="referenceValue_">The property to be animated.</param>
		/// <param name="target_">The target value.</param>
		/// <param name="parameters_">Properties that adjust how this animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should begin immediately.</param>
		/// <returns>Returns the new Tween object.</returns>
		public static BaseMoonTween<Unit> CustomTweenTo<TweenType>(
			Ref<float> referenceValue_,
			Unit target_,
			MoonTweenParams parameters_ = null,
			bool start_ = true
		) where TweenType : BaseMoonTween<Unit>, new() {
			var refs = new Ref<float>[1] { referenceValue_ };
			return BaseMoonTween<Unit>.CustomTweenTo<TweenType>(refs, target_, parameters_, start_);
		}

		/// <summary>
		/// Initializes a custom tween based on a reference value as a property.
		/// </summary>
		/// <typeparam name="TweenType">The type of Tween that will be used.</typeparam>
		/// <param name="referenceValue_">The property to be animated.</param>
		/// <param name="target_">The target value.</param>
		/// <param name="parameters_">Properties that adjust how this animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should begin immediately.</param>
		/// <returns>Returns the new Tween object.</returns>
		public static BaseMoonTween<Unit> CustomTweenTo<TweenType>(
			Ref<float>[] referenceValues_,
			Unit target_,
			MoonTweenParams parameters_ = null,
			bool start_ = true
		) where TweenType : BaseMoonTween<Unit>, new() {
			BaseMoonTween<Unit>.CustomTweens = BaseMoonTween<Unit>.CustomTweens ?? new Dictionary<Ref<float>[], BaseMoonTween<Unit>>();
			if (BaseMoonTween<Unit>.CustomTweens.ContainsKey(referenceValues_)) {
				BaseMoonTween<Unit>.CustomTweens[referenceValues_].Delete();
				BaseMoonTween<Unit>.CustomTweens.Remove(referenceValues_);
			}
			BaseMoonTween<Unit> tween = new TweenType() { StartOnTargetAssigned = start_ };
			tween.SetReferences(referenceValues_).SetParameters(parameters_ ?? new MoonTweenParams()).OnComplete(() => {
				BaseMoonTween<Unit>.CustomTweens.Remove(referenceValues_);
			}).To(target_);

			BaseMoonTween<Unit>.CustomTweens.Add(referenceValues_, tween);
			return tween;
		}

		/// <summary>
		/// Gets a custom Tween object for the provided reference value, if it exists.
		/// </summary>
		/// <typeparam name="Unit">The type of used for this reference value.</typeparam>
		/// <param name="referenceValue_">The reference value a Tween object is applied to.</param>
		/// <returns>Returns the requested Tween object if it exists or null if it cannot be found.</returns>
		public static BaseMoonTween<Unit> GetCustomTween(Ref<float> referenceValue_) {
			var refs = new Ref<float>[1] { referenceValue_ };
			return BaseMoonTween<Unit>.GetCustomTween(refs);
		}

		/// <summary>
		/// Gets a custom Tween object for the provided reference value, if it exists.
		/// </summary>
		/// <typeparam name="Unit">The type of used for this reference value.</typeparam>
		/// <param name="referenceValue_">The reference value a Tween object is applied to.</param>
		/// <returns>Returns the requested Tween object if it exists or null if it cannot be found.</returns>
		public static BaseMoonTween<Unit> GetCustomTween(Ref<float>[] referenceValues_) {
			if (BaseMoonTween<Unit>.CustomTweens.ContainsKey(referenceValues_)) {
				return BaseMoonTween<Unit>.CustomTweens[referenceValues_];
			}
			return null;
		}

		/// <summary>
		/// Returns true when this object is complete.
		/// </summary>
		/// <returns>True when state is complete.</returns>
		public bool IsComplete() {
			return this.CurrentState == MoonTweenState.Complete;
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Method used to update all properties available to this object.
		/// </summary>
		protected abstract void UpdateProperties();

		/// <summary>
		/// Animates this Tween from start to target.
		/// </summary>
		/// <param name="deltaTime_">The delta between last and current game tick.</param>
		/// <returns>Returns true when complete or false when actively animating.</returns>
		protected bool Animate(float deltaTime_) {
			var isComplete = false;

			// Complete delay before animating.
			if (this.DelayTimer != null && !this.DelayTimer.IsComplete()) {
				return isComplete;
			}

			// Begin animating by progressing percentage.
			var newPercentage = (this.Percentage + (deltaTime_ / this.Duration));
			if (newPercentage >= 1f) {
				this.Percentage = 1f;
				isComplete = true;
			} else {
				this.Percentage = newPercentage;
			}
			this.UpdateProperties();
			return isComplete;
		}

		/// <summary>
		/// Updates all starting values set the reference property values.
		/// </summary>
		protected abstract void UpdateStartValues();
		#endregion
	}
}
