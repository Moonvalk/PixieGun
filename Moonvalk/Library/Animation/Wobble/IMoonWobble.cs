using System;
using Moonvalk.Accessory;
using Moonvalk.Systems;

namespace Moonvalk.Animation {
	/// <summary>
	/// Contract that a Wobble object must fulfill.
	/// </summary>
	public interface IMoonWobble<Unit> : IQueueItem {
		#region Public Methods
		/// <summary>
		/// Sets all reference values that this Wobble will manipulate.
		/// </summary>
		/// <param name="referenceValues_">Array of references to values.</param>
		/// <returns>Returns this Wobble object.</returns>
		BaseMoonWobble<Unit> SetReferences(params Ref<float>[] referenceValues_);

		/// <summary>
		/// Starts this Wobble with the current settings.
		/// </summary>
		/// <returns>Returns this Wobble object.</returns>
		BaseMoonWobble<Unit> Start();

		/// <summary>
		/// Stops this Wobble.
		/// </summary>
		/// <returns>Returns this Wobble object.</returns>
		BaseMoonWobble<Unit> Stop();

		/// <summary>
		/// Removes this Wobble on the following game tick.
		/// </summary>
		/// <returns>Returns this Wobble object.</returns>
		BaseMoonWobble<Unit> Delete();

		/// <summary>
		/// Called to add an ease in to the wobble animation.
		/// </summary>
		/// <param name="parameters_">Properties that adjust the ease in Tween.</param>
		/// <returns>Returns this Wobble object.</returns>
		BaseMoonWobble<Unit> EaseIn(MoonTweenParams parameters_ = null);

		/// <summary>
		/// Called to add an ease out to the wobble animation.
		/// </summary>
		/// <param name="parameters_">Properties that adjust the ease in Tween.</param>
		/// <returns>Returns this Wobble object.</returns>
		BaseMoonWobble<Unit> EaseOut(MoonTweenParams parameters_ = null);
		
		/// <summary>
		/// Called to add an ease in and out to the wobble animation.
		/// </summary>
		/// <param name="parameters_">Properties that adjust the ease in Tween.</param>
		/// <returns>Returns this Wobble object.</returns>
		BaseMoonWobble<Unit> EaseInOut(MoonTweenParams parameters_ = null);

		/// <summary>
		/// Sets the frequency of the sin wave used for animation.
		/// </summary>
		/// <param name="frequency_">The new frequency value.</param>
		/// <returns>Returns this Wobble object.</returns>
		BaseMoonWobble<Unit> SetFrequency(float frequency_);
		
		/// <summary>
		/// Sets the amplitude of the sin wave used for animation.
		/// </summary>
		/// <param name="amplitude_">The new amplitude value.</param>
		/// <returns>Returns this Wobble object.</returns>
		BaseMoonWobble<Unit> SetAmplitude(float amplitude_);

		/// <summary>
		/// Sets the duration of this animation when expected to run for a finite amount of time.
		/// </summary>
		/// <param name="duration_">The duration in seconds.</param>
		/// <returns>Returns this Wobble object.</returns>
		BaseMoonWobble<Unit> SetDuration(float duration_);

		/// <summary>
		/// Sets the percentage of the property that will be affected. This is useful for
		/// multi-axis values that need to be affected differently.
		/// </summary>
		/// <param name="percentage_">The percentage value per axis, when applicable.</param>
		/// <returns>Returns this Wobble object.</returns>
		BaseMoonWobble<Unit> SetPercentage(Unit percentage_);

		/// <summary>
		/// Sets all parameters from a reference object.
		/// </summary>
		/// <param name="parameters_">All properties to be assigned for this Wobble.</param>
		/// <returns>Returns this Wobble object.</returns>
		BaseMoonWobble<Unit> SetParameters(MoonWobbleParams parameters_);

		/// <summary>
		/// Defines Actions that will occur when this Wobble begins.
		/// </summary>
		/// <param name="tasksToAdd_">Array of Actions to add.</param>
		/// <returns>Returns this Wobble object.</returns>
		BaseMoonWobble<Unit> OnStart(params Action[] tasksToAdd_);

		/// <summary>
		/// Defines Actions that will occur when this Wobble updates.
		/// </summary>
		/// <param name="tasksToAdd_">Array of Actions to add.</param>
		/// <returns>Returns this Wobble object.</returns>
		BaseMoonWobble<Unit> OnUpdate(params Action[] tasksToAdd_);

		/// <summary>
		/// Defines Actions that will occur once this Wobble has completed.
		/// </summary>
		/// <param name="tasksToAdd_">Array of Actions to add.</param>
		/// <returns>Returns this Wobble object.</returns>
		BaseMoonWobble<Unit> OnComplete(params Action[] tasksToAdd_);

		/// <summary>
		/// Clears all Actions that have been assigned to this Wobble.
		/// </summary>
		/// <returns>Returns this Wobble object.</returns>
		BaseMoonWobble<Unit> Reset();

		/// <summary>
		/// Clears all Actions that have been assigned to this Wobble for the given state.
		/// </summary>
		/// <param name="state_">The state to reset actions for.</param>
		/// <returns>Returns this Wobble object.</returns>
		BaseMoonWobble<Unit> Reset(MoonWobbleState state_);

		/// <summary>
		/// Gets the current state of this Wobble.
		/// </summary>
		/// <returns>Returns the current state.</returns>
		MoonWobbleState GetCurrentState();
		#endregion
	}
}