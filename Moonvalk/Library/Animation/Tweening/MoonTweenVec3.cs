using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Animation {
	/// <summary>
	/// A Tween object which handles Vector3 values.
	/// </summary>
	public class MoonTweenVec3 : BaseMoonTween<Vector3> {
		/// <summary>
		/// Default constructor made without setting up references.
		/// </summary>
		public MoonTweenVec3() : base() {
			// ...
		}

		/// <summary>
		/// Constructor for creating a new Tween.
		/// </summary>
		/// <param name="referenceValues_">Array of references to float values.</param>
		public MoonTweenVec3(params Ref<float>[] referenceValues_) : base(referenceValues_) {
			// ...
		}

		/// <summary>
		/// Method used to update all properties available to this object.
		/// </summary>
		protected override void UpdateProperties() {
			// Apply easing and set properties.
			for (var index = 0; index < this.Properties.Length; index += 3) {
				if (this.Properties[index] == null) {
					this.Delete();
					break;
				}
				this.Properties[index]() = this.EasingFunctions[index](this.Percentage, this.StartValues[index].x, this.TargetValues[index].x);
				this.Properties[index + 1]() = this.EasingFunctions[index](this.Percentage, this.StartValues[index].y, this.TargetValues[index].y);
				this.Properties[index + 2]() = this.EasingFunctions[index](this.Percentage, this.StartValues[index].z, this.TargetValues[index].z);
			}
		}

		/// <summary>
		/// Updates all starting values set the reference property values.
		/// </summary>
		protected override void UpdateStartValues() {
			for (var index = 0; index < this.Properties.Length; index += 3) {
				this.StartValues[index].x = this.Properties[index]();
				this.StartValues[index].y = this.Properties[index + 1]();
				this.StartValues[index].z = this.Properties[index + 2]();
			}
		}
	}
}