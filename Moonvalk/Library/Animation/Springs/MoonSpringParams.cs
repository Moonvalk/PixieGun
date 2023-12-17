using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Animation
{
    /// <summary>
    /// A grouping of parameters which change how MoonSprings operate.
    /// </summary>
    [RegisteredType(nameof(MoonSpringParams))]
    public class MoonSpringParams : Resource
    {
        /// <summary>
        /// A factor applied each game tick to slow down forces made by tension.
        /// </summary>
        [Export] public float Dampening { get; set; } = 10f;

        /// <summary>
        /// The amount of tension applied to spring forces. This causes a Spring to snap into resting place.
        /// </summary>
        [Export] public float Tension { get; set; } = 250f;
    }
}