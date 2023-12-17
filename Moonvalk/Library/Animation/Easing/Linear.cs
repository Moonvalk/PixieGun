namespace Moonvalk.Animation
{
    /// <summary>
    /// Supplies Easing functions that affect how a value traverses from a start to an end point.
    /// </summary>
    public static partial class Easing
    {
        /// <summary>
        /// Static class containing Linear interpolation methods.
        /// </summary>
        public static class Linear
        {
            /// <summary>
            /// A basic linear interpolation function that provides constant traversal to the target value.
            /// </summary>
            /// <param name="percentage_">The current percentage elapsed.</param>
            /// <param name="start_">The starting value.</param>
            /// <param name="end_">The ending value.</param>
            /// <returns>Returns a value between start and end with easing applied.</returns>
            public static float None(float percentage_, float start_, float end_)
            {
                return start_ + (end_ - start_) * percentage_;
            }
        }
    }
}