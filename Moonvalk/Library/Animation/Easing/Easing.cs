using System.Collections.Generic;

namespace Moonvalk.Animation
{
    /// <summary>
    /// Supplies Easing functions that affect how a value traverses from a start to an end point.
    /// </summary>
    public static partial class Easing
    {
        /// <summary>
        /// A look-up able to convert Easing.Types into usable EasingFunctions.
        /// </summary>
        /// <value>Gets an EasingFunction that can be passed to animations.</value>
        public static Dictionary<Types, EasingFunction> Functions { get; private set; } = new Dictionary<Types, EasingFunction>
        {
            { Types.BackIn, Back.In },
            { Types.BackOut, Back.Out },
            { Types.BackInOut, Back.InOut },
            { Types.BounceIn, Bounce.In },
            { Types.BounceOut, Bounce.Out },
            { Types.BounceInOut, Bounce.InOut },
            { Types.CircularIn, Circular.In },
            { Types.CircularOut, Circular.Out },
            { Types.CircularInOut, Circular.InOut },
            { Types.CubicIn, Cubic.In },
            { Types.CubicOut, Cubic.Out },
            { Types.CubicInOut, Cubic.InOut },
            { Types.ElasticIn, Elastic.In },
            { Types.ElasticOut, Elastic.Out },
            { Types.ElasticInOut, Elastic.InOut },
            { Types.ExponentialIn, Exponential.In },
            { Types.ExponentialOut, Exponential.Out },
            { Types.ExponentialInOut, Exponential.InOut },
            { Types.Linear, Linear.None },
            { Types.None, Linear.None },
            { Types.QuadraticIn, Quadratic.In },
            { Types.QuadraticOut, Quadratic.Out },
            { Types.QuadraticInOut, Quadratic.InOut },
            { Types.QuarticIn, Quartic.In },
            { Types.QuarticOut, Quartic.Out },
            { Types.QuarticInOut, Quartic.InOut },
            { Types.QuinticIn, Quintic.In },
            { Types.QuinticOut, Quintic.Out },
            { Types.QuinticInOut, Quintic.InOut },
            { Types.SinusoidalIn, Sinusoidal.In },
            { Types.SinusoidalOut, Sinusoidal.Out },
            { Types.SinusoidalInOut, Sinusoidal.InOut }
        };
    }
}