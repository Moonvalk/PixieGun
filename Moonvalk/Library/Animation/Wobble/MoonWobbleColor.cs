using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Animation
{
    /// <summary>
    /// A basic Wobble which handles Color values.
    /// </summary>
    public class MoonWobbleColor : BaseMoonWobble<Color>
    {
        /// <summary>
        /// Default constructor made without setting up references.
        /// </summary>
        public MoonWobbleColor()
        {
            // ...
        }

        /// <summary>
        /// Constructor for creating a new Wobble.
        /// </summary>
        /// <param name="referenceValues_">Array of references to float values.</param>
        public MoonWobbleColor(params Ref<float>[] referenceValues_) : base(referenceValues_)
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
            for (var index = 0; index < Properties.Length; index += 4)
            {
                if (Properties[index] == null)
                {
                    Delete();
                    break;
                }

                Properties[index]() = StartValues[index].r + wave * Percentage.r;
                Properties[index + 1]() = StartValues[index].g + wave * Percentage.g;
                Properties[index + 2]() = StartValues[index].b + wave * Percentage.b;
                Properties[index + 3]() = StartValues[index].a + wave * Percentage.a;
            }
        }

        /// <summary>
        /// Updates all starting values set the reference property values.
        /// </summary>
        protected override void UpdateStartValues()
        {
            for (var index = 0; index < Properties.Length; index += 4)
            {
                StartValues[index].r = Properties[index]();
                StartValues[index].g = Properties[index + 1]();
                StartValues[index].b = Properties[index + 2]();
                StartValues[index].a = Properties[index + 3]();
            }
        }
    }
}