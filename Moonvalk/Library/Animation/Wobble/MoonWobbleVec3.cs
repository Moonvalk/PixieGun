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
        public MoonWobbleVec3() : base()
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
            var wave = Mathf.Sin(this.Time * this.Frequency) * this.Amplitude * this._strength;
            for (var index = 0; index < this.Properties.Length; index += 3)
            {
                if (this.Properties[index] == null)
                {
                    this.Delete();
                    break;
                }

                this.Properties[index]() = this.StartValues[index].x + (wave * this.Percentage.x);
                this.Properties[index + 1]() = this.StartValues[index].y + (wave * this.Percentage.y);
                this.Properties[index + 2]() = this.StartValues[index].z + (wave * this.Percentage.z);
            }
        }

        /// <summary>
        /// Updates all starting values set the reference property values.
        /// </summary>
        protected override void UpdateStartValues()
        {
            for (var index = 0; index < this.Properties.Length; index += 3)
            {
                this.StartValues[index].x = this.Properties[index]();
                this.StartValues[index].y = this.Properties[index + 1]();
                this.StartValues[index].z = this.Properties[index + 2]();
            }
        }
    }
}