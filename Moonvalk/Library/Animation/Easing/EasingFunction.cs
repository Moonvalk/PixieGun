
namespace Moonvalk.Animation {
	/// <summary>
	/// Reference type for an applicable Easing function.
	/// </summary>
	/// <param name="percentage_">The current percentage to apply easing for.</param>
	/// <param name="start_">The start value.</param>
	/// <param name="end_">The end value.</param>
	/// <returns>Returns a new value with easing applied.</returns>
	public delegate float EasingFunction(float percentage_, float start_, float end_);
}
