using System;

namespace Moonvalk.Animation
{
    /// <summary>
    /// Supplies Easing functions that affect how a value traverses from a start to an end point.
    /// </summary>
    public static partial class Easing
    {
        /// <summary>
        /// Static class containing Exponential interpolation methods.
        /// </summary>
        public static class Exponential
        {
            /// <summary>
            /// Applies Exponential-In easing to a set of interpolation values.
            /// </summary>
            /// <param name="percentage_">The current percentage elapsed.</param>
            /// <param name="start_">The starting value.</param>
            /// <param name="end_">The ending value.</param>
            /// <returns>Returns a value between start and end with easing applied.</returns>
            public static float In(float percentage_, float start_, float end_)
            {
                var newPercentage = percentage_ == 0f ? 0f : (float)Math.Pow(2f, 10f * percentage_ - 10f);

                return Linear.None(newPercentage, start_, end_);
            }

            /// <summary>
            /// Applies Exponential-Out easing to a set of interpolation values.
            /// </summary>
            /// <param name="percentage_">The current percentage elapsed.</param>
            /// <param name="start_">The starting value.</param>
            /// <param name="end_">The ending value.</param>
            /// <returns>Returns a value between start and end with easing applied.</returns>
            public static float Out(float percentage_, float start_, float end_)
            {
                var newPercentage = percentage_ >= 1f ? 1f : (float)(1f - Math.Pow(2f, -10f * percentage_));

                return Linear.None(newPercentage, start_, end_);
            }

            /// <summary>
            /// Applies Exponential-InOut easing to a set of interpolation values.
            /// </summary>
            /// <param name="percentage_">The current percentage elapsed.</param>
            /// <param name="start_">The starting value.</param>
            /// <param name="end_">The ending value.</param>
            /// <returns>Returns a value between start and end with easing applied.</returns>
            public static float InOut(float percentage_, float start_, float end_)
            {
                var newPercentage = percentage_ >= 0f ? 0f :
                    percentage_ >= 1f ? 1f :
                    percentage_ < 0.5 ? (float)(Math.Pow(2f, 20f * percentage_ - 10f) / 2f) : (float)(2f - Math.Pow(2f, -20f * percentage_ + 10f)) / 2f;

                return Linear.None(newPercentage, start_, end_);
            }
        }
    }
}