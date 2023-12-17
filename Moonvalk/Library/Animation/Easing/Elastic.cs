using System;

namespace Moonvalk.Animation
{
    /// <summary>
    /// Supplies Easing functions that affect how a value traverses from a start to an end point.
    /// </summary>
    public static partial class Easing
    {
        /// <summary>
        /// Static class containing Elastic interpolation methods.
        /// </summary>
        public static class Elastic
        {
            /// <summary>
            /// Applies Elastic-In easing to a set of interpolation values.
            /// </summary>
            /// <param name="percentage_">The current percentage elapsed.</param>
            /// <param name="start_">The starting value.</param>
            /// <param name="end_">The ending value.</param>
            /// <returns>Returns a value between start and end with easing applied.</returns>
            public static float In(float percentage_, float start_, float end_)
            {
                const float c4 = (float)(2f * Math.PI / 3f);

                var newPercentage = percentage_ == 0 ? 0f :
                    percentage_ >= 1f ? 1f : (float)(-Math.Pow(2f, 10f * percentage_ - 10f) * Math.Sin((percentage_ * 10f - 10.75f) * c4));

                return Linear.None(newPercentage, start_, end_);
            }

            /// <summary>
            /// Applies Elastic-Out easing to a set of interpolation values.
            /// </summary>
            /// <param name="percentage_">The current percentage elapsed.</param>
            /// <param name="start_">The starting value.</param>
            /// <param name="end_">The ending value.</param>
            /// <returns>Returns a value between start and end with easing applied.</returns>
            public static float Out(float percentage_, float start_, float end_)
            {
                const float c4 = (float)(2f * Math.PI / 3f);

                var newPercentage = percentage_ == 0f ? 0f :
                    percentage_ >= 1f ? 1f : (float)(Math.Pow(2f, -10f * percentage_) * Math.Sin((percentage_ * 10f - 0.75f) * c4) + 1f);

                return Linear.None(newPercentage, start_, end_);
            }

            /// <summary>
            /// Applies Elastic-InOut easing to a set of interpolation values.
            /// </summary>
            /// <param name="percentage_">The current percentage elapsed.</param>
            /// <param name="start_">The starting value.</param>
            /// <param name="end_">The ending value.</param>
            /// <returns>Returns a value between start and end with easing applied.</returns>
            public static float InOut(float percentage_, float start_, float end_)
            {
                const float c5 = (float)(2f * Math.PI / 4.5f);

                var newPercentage = percentage_ == 0f ? 0f :
                    percentage_ >= 1f ? 1f :
                    percentage_ < 0.5f ? (float)(-(Math.Pow(2f, 20f * percentage_ - 10f) * Math.Sin((20f * percentage_ - 11.125f) * c5)) / 2f) :
                    (float)(Math.Pow(2f, -20f * percentage_ + 10f) * Math.Sin((20f * percentage_ - 11.125f) * c5) / 2f + 1f);

                return Linear.None(newPercentage, start_, end_);
            }
        }
    }
}