using System;

namespace Moonvalk.Animation
{
	/// <summary>
	/// Supplies Easing functions that affect how a value traverses from a start to an end point.
	/// </summary>
	public static partial class Easing
	{
		/// <summary>
		/// Static class containing Quintic interpolation methods.
		/// </summary>
		public static class Quintic
		{
			/// <summary>
			/// Applies Quintic-In easing to a set of interpolation values.
			/// </summary>
			/// <param name="percentage_">The current percentage elapsed.</param>
			/// <param name="start_">The starting value.</param>
			/// <param name="end_">The ending value.</param>
			/// <returns>Returns a value between start and end with easing applied.</returns>
			public static float In(float percentage_, float start_, float end_)
			{
				var newPercentage = (percentage_ * percentage_ * percentage_ * percentage_ * percentage_);
				return Linear.None(newPercentage, start_, end_);
			}

			/// <summary>
			/// Applies Quintic-Out easing to a set of interpolation values.
			/// </summary>
			/// <param name="percentage_">The current percentage elapsed.</param>
			/// <param name="start_">The starting value.</param>
			/// <param name="end_">The ending value.</param>
			/// <returns>Returns a value between start and end with easing applied.</returns>
			public static float Out(float percentage_, float start_, float end_) 
			{
				var newPercentage = (float)(1f - Math.Pow(1f - percentage_, 5f));
				return Linear.None(newPercentage, start_, end_);
			}

			/// <summary>
			/// Applies Quintic-InOut easing to a set of interpolation values.
			/// </summary>
			/// <param name="percentage_">The current percentage elapsed.</param>
			/// <param name="start_">The starting value.</param>
			/// <param name="end_">The ending value.</param>
			/// <returns>Returns a value between start and end with easing applied.</returns>
			public static float InOut(float percentage_, float start_, float end_)
			{
				var newPercentage = (percentage_ < 0.5f)
					? (16f * percentage_ * percentage_ * percentage_ * percentage_ * percentage_)
					: (float)(1f - Math.Pow(-2f * percentage_ + 2f, 5f) / 2f);
				return Linear.None(newPercentage, start_, end_);
			}
		}
	}
}
