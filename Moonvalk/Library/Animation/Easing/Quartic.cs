using System;

namespace Moonvalk.Animation
{
    /// <summary>
    /// Supplies Easing functions that affect how a value traverses from a start to an end point.
    /// </summary>
    public static partial class Easing
    {
        /// <summary>
        /// Static class containing Sinusoidal interpolation methods.
        /// </summary>
        public static class Quartic
        {
            /// <summary>
            /// Applies Quartic-In easing to a set of interpolation values.
            /// </summary>
            /// <param name="percentage_">The current percentage elapsed.</param>
            /// <param name="start_">The starting value.</param>
            /// <param name="end_">The ending value.</param>
            /// <returns>Returns a value between start and end with easing applied.</returns>
            public static float In(float percentage_, float start_, float end_)
            {
                var newPercentage = percentage_ * percentage_ * percentage_ * percentage_;
                return Linear.None(newPercentage, start_, end_);
            }

            /// <summary>
            /// Applies Quartic-Out easing to a set of interpolation values.
            /// </summary>
            /// <param name="percentage_">The current percentage elapsed.</param>
            /// <param name="start_">The starting value.</param>
            /// <param name="end_">The ending value.</param>
            /// <returns>Returns a value between start and end with easing applied.</returns>
            public static float Out(float percentage_, float start_, float end_)
            {
                var newPercentage = (float)(1f - Math.Pow(1f - percentage_, 4f));
                return Linear.None(newPercentage, start_, end_);
            }

            /// <summary>
            /// Applies Quartic-InOut easing to a set of interpolation values.
            /// </summary>
            /// <param name="percentage_">The current percentage elapsed.</param>
            /// <param name="start_">The starting value.</param>
            /// <param name="end_">The ending value.</param>
            /// <returns>Returns a value between start and end with easing applied.</returns>
            public static float InOut(float percentage_, float start_, float end_)
            {
                var newPercentage = percentage_ < 0.5f
                    ? 8f * percentage_ * percentage_ * percentage_ * percentage_
                    : (float)(1f - Math.Pow(-2f * percentage_ + 2f, 4f) / 2f);

                return Linear.None(newPercentage, start_, end_);
            }
        }
    }
}