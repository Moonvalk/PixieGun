using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Animation
{
    /// <summary>
    /// A basic Wobble which handles Vector2 values.
    /// </summary>
    public class MoonWobbleVec2 : BaseMoonWobble<Vector2>
    {
        /// <summary>
        /// Default constructor made without setting up references.
        /// </summary>
        public MoonWobbleVec2()
        {
            // ...
        }

        /// <summary>
        /// Constructor for creating a new Wobble.
        /// </summary>
        /// <param name="referenceValues_">Array of references to float values.</param>
        public MoonWobbleVec2(params Ref<float>[] referenceValues_) : base(referenceValues_)
        {
            // ...
        }

        /// <summary>
        /// Method used to update all properties available to this object.
        /// </summary>
        protected override void UpdateProperties()
        {
            // Apply easing and set properties.
            var wave = Mathf.Sin(Time * Frequency) * Amplitude * _strength;
            for (var index = 0; index < Properties.Length; index += 2)
            {
                if (Properties[index] == null)
                {
                    Delete();
                    break;
                }

                Properties[index]() = StartValues[index].x + wave * Percentage.x;
                Properties[index + 1]() = StartValues[index].y + wave * Percentage.y;
            }
        }

        /// <summary>
        /// Updates all starting values set the reference property values.
        /// </summary>
        protected override void UpdateStartValues()
        {
            for (var index = 0; index < Properties.Length; index += 2)
            {
                StartValues[index].x = Properties[index]();
                StartValues[index].y = Properties[index + 1]();
            }
        }
    }
}