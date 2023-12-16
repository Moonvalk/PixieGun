using System;

namespace Moonvalk.Animation {
	/// <summary>
	/// Supplies Easing functions that affect how a value traverses from a start to an end point.
	/// </summary>
	public static partial class Easing {
		/// <summary>
		/// Static class containing Back interpolation methods.
		/// </summary>
		public static class Back {
			/// <summary>
			/// Applies Back-In easing to a set of interpolation values.
			/// </summary>
			/// <param name="percentage_">The current percentage elapsed.</param>
			/// <param name="start_">The starting value.</param>
			/// <param name="end_">The ending value.</param>
			/// <returns>Returns a value between start and end with easing applied.</returns>
			public static float In(float percentage_, float start_, float end_) {
				const float c1 = 1.70158f;
				const float c3 = (c1 + 1f);

				float newPercentage = (c3 * percentage_ * percentage_ * percentage_ - c1 * percentage_ * percentage_);
				return Linear.None(newPercentage, start_, end_);
			}

			/// <summary>
			/// Applies Back-Out easing to a set of interpolation values.
			/// </summary>
			/// <param name="percentage_">The current percentage elapsed.</param>
			/// <param name="start_">The starting value.</param>
			/// <param name="end_">The ending value.</param>
			/// <returns>Returns a value between start and end with easing applied.</returns>
			public static float Out(float percentage_, float start_, float end_) {
				const float c1 = 1.70158f;
				const float c3 = (c1 + 1f);

				float newPercentage = (float)(1f + c3 * Math.Pow(percentage_ - 1f, 3f) + c1 * Math.Pow(percentage_ - 1f, 2f));
				return Linear.None(newPercentage, start_, end_);
			}

			/// <summary>
			/// Applies Back-InOut easing to a set of interpolation values.
			/// </summary>
			/// <param name="percentage_">The current percentage elapsed.</param>
			/// <param name="start_">The starting value.</param>
			/// <param name="end_">The ending value.</param>
			/// <returns>Returns a value between start and end with easing applied.</returns>
			public static float InOut(float percentage_, float start_, float end_) {
				const float c1 = 1.70158f;
				const float c2 = (c1 * 1.525f);

				float newPercentage = (percentage_ < 0.5f)
					? (float)(Math.Pow(2f * percentage_, 2f) * ((c2 + 1f) * 2f * percentage_ - c2)) / 2f
					: (float)(Math.Pow(2f * percentage_ - 2f, 2f) * ((c2 + 1f) * (percentage_ * 2f - 2f) + c2) + 2f) / 2f;
				return Linear.None(newPercentage, start_, end_);
			}
		}
	}
}
