using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Animation
{
    /// <summary>
    /// A basic Wobble which handles float values.
    /// </summary>
    public class MoonWobble : BaseMoonWobble<float>
    {
        /// <summary>
        /// Default constructor made without setting up references.
        /// </summary>
        public MoonWobble()
        {
            // ...
        }

        /// <summary>
        /// Constructor for creating a new Wobble.
        /// </summary>
        /// <param name="referenceValues_">Array of references to float values.</param>
        public MoonWobble(params Ref<float>[] referenceValues_) : base(referenceValues_)
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
            for (var index = 0; index < Properties.Length; index++)
            {
                if (Properties[index] == null)
                {
                    Delete();
                    break;
                }

                Properties[index]() = StartValues[index] + wave * Percentage;
            }
        }

        /// <summary>
        /// Updates all starting values set the reference property values.
        /// </summary>
        protected override void UpdateStartValues()
        {
            for (var index = 0; index < Properties.Length; index++) StartValues[index] = Properties[index]();
        }
    }
}