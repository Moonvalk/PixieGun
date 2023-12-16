
namespace Moonvalk.Animation {
	/// <summary>
	/// Supplies Easing functions that affect how a value traverses from a start to an end point.
	/// </summary>
	public static partial class Easing {
		/// <summary>
		/// Static class containing Bounce interpolation methods.
		/// </summary>
		public static class Bounce {
			/// <summary>
			/// Applies Bounce-In easing to a set of interpolation values.
			/// </summary>
			/// <param name="percentage_">The current percentage elapsed.</param>
			/// <param name="start_">The starting value.</param>
			/// <param name="end_">The ending value.</param>
			/// <returns>Returns a value between start and end with easing applied.</returns>
			public static float In(float percentage_, float start_, float end_) {
				float newPercentage = 1f - getOutPercentage(1f - percentage_);
				return Linear.None(newPercentage, start_, end_);
			}

			/// <summary>
			/// Applies Bounce-Out easing to a set of interpolation values.
			/// </summary>
			/// <param name="percentage_">The current percentage elapsed.</param>
			/// <param name="start_">The starting value.</param>
			/// <param name="end_">The ending value.</param>
			/// <returns>Returns a value between start and end with easing applied.</returns>
			public static float Out(float percentage_, float start_, float end_) {
				return Linear.None(getOutPercentage(percentage_), start_, end_);
			}

			/// <summary>
			/// Applies Bounce-InOut easing to a set of interpolation values.
			/// </summary>
			/// <param name="percentage_">The current percentage elapsed.</param>
			/// <param name="start_">The starting value.</param>
			/// <param name="end_">The ending value.</param>
			/// <returns>Returns a value between start and end with easing applied.</returns>
			public static float InOut(float percentage_, float start_, float end_) {
				float newPercentage = (percentage_ < 0.5f)
					? (1f - getOutPercentage(1f - 2f * percentage_)) / 2f
					: (1f + getOutPercentage(2f * percentage_ - 1f)) / 2f;
				return Linear.None(newPercentage, start_, end_);
			}

			/// <summary>
			/// Gets an Easing Bounce-Out percentage via the input percentage.
			/// </summary>
			/// <param name="percentage_">The current percentage elapsed.</param>
			/// <returns>Returns an Easing Bounce-Out percentage elapsed.</returns>
			private static float getOutPercentage(float percentage_) {
				const float n1 = 7.5625f;
				const float d1 = 2.75f;

				if (percentage_ < 1 / d1)
				{
					percentage_ = n1 * percentage_ * percentage_;
				}
				else if (percentage_ < 2 / d1)
				{
					percentage_ = n1 * (percentage_ -= 1.5f / d1) * percentage_ + 0.75f;
				}
				else if (percentage_ < 2.5 / d1)
				{
					percentage_ = n1 * (percentage_ -= 2.25f / d1) * percentage_ + 0.9375f;
				}
				else
				{
					percentage_ = n1 * (percentage_ -= 2.625f / d1) * percentage_ + 0.984375f;
				}
				return percentage_;
			}
		}
	}
}