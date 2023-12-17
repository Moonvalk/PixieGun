using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Animation
{
    /// <summary>
    /// A basic Tween which handles Color value properties.
    /// </summary>
    public class MoonTweenColor : BaseMoonTween<Color>
    {
        /// <summary>
        /// Default constructor made without setting up references.
        /// </summary>
        public MoonTweenColor()
        {
            // ...
        }

        /// <summary>
        /// Constructor for creating a new Tween.
        /// </summary>
        /// <param name="referenceValues_">Array of references to float values.</param>
        public MoonTweenColor(params Ref<float>[] referenceValues_) : base(referenceValues_)
        {
            // ...
        }

        /// <summary>
        /// Method used to update all properties available to this object.
        /// </summary>
        protected override void UpdateProperties()
        {
            // Apply easing and set properties.
            for (var index = 0; index < Properties.Length; index += 4)
            {
                if (Properties[index] == null)
                {
                    Stop();
                    break;
                }

                Properties[index]() = EasingFunctions[index](Percentage, StartValues[index].r, TargetValues[index].r);

                Properties[index + 1]() = EasingFunctions[index](Percentage, StartValues[index].g, TargetValues[index].g);

                Properties[index + 2]() = EasingFunctions[index](Percentage, StartValues[index].b, TargetValues[index].b);

                Properties[index + 3]() = EasingFunctions[index](Percentage, StartValues[index].a, TargetValues[index].a);
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