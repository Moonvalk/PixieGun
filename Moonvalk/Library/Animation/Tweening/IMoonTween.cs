using System;
using Moonvalk.Accessory;
using Moonvalk.Systems;

namespace Moonvalk.Animation {
	/// <summary>
	/// Contract that a Tween object must fulfill.
	/// </summary>
	public interface IMoonTween<Unit> : IQueueItem {
		#region Public Methods
		/// <summary>
		/// Sets all reference values that this Tween will manipulate.
		/// </summary>
		/// <param name="referenceValues_">Array of references to values.</param>
		/// <returns>Returns this Tween object.</returns>
		BaseMoonTween<Unit> SetReferences(params Ref<float>[] referenceValues_);

		/// <summary>
		/// Starts this Tween with the current settings.
		/// </summary>
		/// <returns>Returns this Tween object.</returns>
		BaseMoonTween<Unit> Start();

		/// <summary>
		/// Stops this Tween.
		/// </summary>
		/// <returns>Returns this Tween object.</returns>
		BaseMoonTween<Unit> Stop();

		/// <summary>
		/// Sets the delay in seconds applied to this Tween.
		/// </summary>
		/// <param name="delaySeconds_">The delay duration in seconds.</param>
		/// <returns>Returns this Tween object.</returns>
		BaseMoonTween<Unit> SetDelay(float delaySeconds_);
		
		/// <summary>
		/// Sets the duration in seconds that must elapse for this Tween to resolve.
		/// </summary>
		/// <param name="durationSeconds_">The duration of this Tween in seconds.</param>
		/// <returns>Returns this Tween object.</returns>
		BaseMoonTween<Unit> SetDuration(float durationSeconds_);

		/// <summary>
		/// Sets all parameters from a reference object.
		/// </summary>
		/// <param name="parameters_">All properties to be assigned for this Tween.</param>
		/// <returns>Returns this Tween object.</returns>
		BaseMoonTween<Unit> SetParameters(MoonTweenParams parameters_);

		/// <summary>
		/// Sets the target value(s) that this Tween will reach once complete.
		/// </summary>
		/// <param name="targetValues_">An array of target values for each property.</param>
		/// <returns>Returns this Tween object.</returns>
		BaseMoonTween<Unit> To(params Unit[] targetValues_);

		/// <summary>
		/// Sets an Easing Function for each available property.
		/// </summary>
		/// <param name="functions_">An array of Easing Functions per property.</param>
		/// <returns>Returns this Tween object.</returns>
		BaseMoonTween<Unit> SetEase(params EasingFunction[] functions_);

		/// <summary>
		/// Removes this Tween on the following game tick.
		/// </summary>
		/// <returns>Returns this Tween object.</returns>
		BaseMoonTween<Unit> Delete();

		/// <summary>
		/// Defines Actions that will occur when this Tween begins.
		/// </summary>
		/// <param name="tasksToAdd_">Array of Actions to add.</param>
		/// <returns>Returns this Tween object.</returns>
		BaseMoonTween<Unit> OnStart(params Action[] tasksToAdd_);

		/// <summary>
		/// Defines Actions that will occur when this Tween updates.
		/// </summary>
		/// <param name="tasksToAdd_">Array of Actions to add.</param>
		/// <returns>Returns this Tween object.</returns>
		BaseMoonTween<Unit> OnUpdate(params Action[] tasksToAdd_);

		/// <summary>
		/// Defines Actions that will occur once this Tween has completed.
		/// </summary>
		/// <param name="tasksToAdd_">Array of Actions to add.</param>
		/// <returns>Returns this Tween object.</returns>
		BaseMoonTween<Unit> OnComplete(params Action[] tasksToAdd_);

		/// <summary>
		/// Clears all Actions that have been assigned to this Tween.
		/// </summary>
		/// <returns>Returns this Tween object.</returns>
		BaseMoonTween<Unit> Reset();

		/// <summary>
		/// Clears all Actions that have been assigned to this Tween for the given state.
		/// </summary>
		/// <param name="state_">The state to reset actions for.</param>
		/// <returns>Returns this Tween object.</returns>
		BaseMoonTween<Unit> Reset(MoonTweenState state_);

		/// <summary>
		/// Gets the current state of this Tween.
		/// </summary>
		/// <returns>Returns the current state.</returns>
		MoonTweenState GetCurrentState();
		#endregion
	}
}