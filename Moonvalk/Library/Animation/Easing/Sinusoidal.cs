using System;

namespace Moonvalk.Animation {
	/// <summary>
	/// Supplies Easing functions that affect how a value traverses from a start to an end point.
	/// </summary>
	public static partial class Easing {
		/// <summary>
		/// Static class containing Sinusoidal interpolation methods.
		/// </summary>
		public static class Sinusoidal {
			/// <summary>
			/// Applies Sinusoidal-In easing to a set of interpolation values.
			/// </summary>
			/// <param name="percentage_">The current percentage elapsed.</param>
			/// <param name="start_">The starting value.</param>
			/// <param name="end_">The ending value.</param>
			/// <returns>Returns a value between start and end with easing applied.</returns>
			public static float In(float percentage_, float start_, float end_) {
				float newPercentage = (float)(1f - Math.Cos((percentage_ * Math.PI) / 2f));
				return Linear.None(newPercentage, start_, end_);
			}

			/// <summary>
			/// Applies Sinusoidal-Out easing to a set of interpolation values.
			/// </summary>
			/// <param name="percentage_">The current percentage elapsed.</param>
			/// <param name="start_">The starting value.</param>
			/// <param name="end_">The ending value.</param>
			/// <returns>Returns a value between start and end with easing applied.</returns>
			public static float Out(float percentage_, float start_, float end_) {
				float newPercentage = (float)Math.Sin((percentage_ * Math.PI) / 2f);
				return Linear.None(newPercentage, start_, end_);
			}

			/// <summary>
			/// Applies Sinusoidal-InOut easing to a set of interpolation values.
			/// </summary>
			/// <param name="percentage_">The current percentage elapsed.</param>
			/// <param name="start_">The starting value.</param>
			/// <param name="end_">The ending value.</param>
			/// <returns>Returns a value between start and end with easing applied.</returns>
			public static float InOut(float percentage_, float start_, float end_) {
				float newPercentage = (float)(-(Math.Cos(Math.PI * percentage_) - 1f) / 2f);
				return Linear.None(newPercentage, start_, end_);
			}
		}
	}
}
