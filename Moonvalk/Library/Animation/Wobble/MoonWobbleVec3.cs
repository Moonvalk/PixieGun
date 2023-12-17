using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Animation
{
    /// <summary>
    /// A basic Wobble which handles Vector3 values.
    /// </summary>
    public class MoonWobbleVec3 : BaseMoonWobble<Vector3>
    {
        /// <summary>
        /// Default constructor made without setting up references.
        /// </summary>
        public MoonWobbleVec3()
        {
            // ...
        }

        /// <summary>
        /// Constructor for creating a new Wobble.
        /// </summary>
        /// <param name="referenceValues_">Array of references to float values.</param>
        public MoonWobbleVec3(params Ref<float>[] referenceValues_) : base(referenceValues_)
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
            for (var index = 0; index < Properties.Length; index += 3)
            {
                if (Properties[index] == null)
                {
                    Delete();
                    break;
                }

                Properties[index]() = StartValues[index].x + wave * Percentage.x;
                Properties[index + 1]() = StartValues[index].y + wave * Percentage.y;
                Properties[index + 2]() = StartValues[index].z + wave * Percentage.z;
            }
        }

        /// <summary>
        /// Updates all starting values set the reference property values.
        /// </summary>
        protected override void UpdateStartValues()
        {
            for (var index = 0; index < Properties.Length; index += 3)
            {
                StartValues[index].x = Properties[index]();
                StartValues[index].y = Properties[index + 1]();
                StartValues[index].z = Properties[index + 2]();
            }
        }
    }
}