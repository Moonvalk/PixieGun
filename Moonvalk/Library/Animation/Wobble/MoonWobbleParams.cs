using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Animation
{
    /// <summary>
    /// A grouping of parameters which change how MoonWobbles operate.
    /// </summary>
    [RegisteredType(nameof(MoonWobbleParams))]
    public class MoonWobbleParams : Resource
    {
        /// <summary>
        /// The duration in seconds of the Wobble.
        /// </summary>
        [Export] public float Duration { get; set; } = -1f;

        /// <summary>
        /// The frequency of the sin wave applied to achieve animation.
        /// </summary>
        [Export] public float Frequency { get; set; } = 5f;

        /// <summary>
        /// The amplitude of the sin wave applied to achieve animation.
        /// </summary>
        [Export] public float Amplitude { get; set; } = 10f;

        /// <summary>
        /// Easing settings applied to the start of the animation.
        /// </summary>
        [Export] public MoonTweenParams EaseIn { get; set; }

        /// <summary>
        /// Easing settings applied to the end of the animation.
        /// </summary>
        [Export] public MoonTweenParams EaseOut { get; set; }
    }
}