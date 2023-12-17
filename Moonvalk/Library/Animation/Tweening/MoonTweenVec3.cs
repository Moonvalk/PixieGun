using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Animation
{
    /// <summary>
    /// A Tween object which handles Vector3 values.
    /// </summary>
    public class MoonTweenVec3 : BaseMoonTween<Vector3>
    {
        /// <summary>
        /// Default constructor made without setting up references.
        /// </summary>
        public MoonTweenVec3()
        {
            // ...
        }

        /// <summary>
        /// Constructor for creating a new Tween.
        /// </summary>
        /// <param name="referenceValues_">Array of references to float values.</param>
        public MoonTweenVec3(params Ref<float>[] referenceValues_) : base(referenceValues_)
        {
            // ...
        }

        /// <summary>
        /// Method used to update all properties available to this object.
        /// </summary>
        protected override void UpdateProperties()
        {
            // Apply easing and set properties.
            for (var index = 0; index < Properties.Length; index += 3)
            {
                if (Properties[index] == null)
                {
                    Delete();
                    break;
                }

                Properties[index]() = EasingFunctions[index](Percentage, StartValues[index].x, TargetValues[index].x);

                Properties[index + 1]() = EasingFunctions[index](Percentage, StartValues[index].y, TargetValues[index].y);

                Properties[index + 2]() = EasingFunctions[index](Percentage, StartValues[index].z, TargetValues[index].z);
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