using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Animation {
	/// <summary>
	/// A Tween object which handles Vector2 values.
	/// </summary>
	public class MoonTweenVec2 : BaseMoonTween<Vector2> {
		/// <summary>
		/// Default constructor made without setting up references.
		/// </summary>
		public MoonTweenVec2() : base() {
			// ...
		}

		/// <summary>
		/// Constructor for creating a new Tween.
		/// </summary>
		/// <param name="referenceValues_">Array of references to float values.</param>
		public MoonTweenVec2(params Ref<float>[] referenceValues_) : base(referenceValues_) {
			// ...
		}

		/// <summary>
		/// Method used to update all properties available to this object.
		/// </summary>
		protected override void updateProperties() {
			// Apply easing and set properties.
			for (int index = 0; index < this.Properties.Length; index += 2) {
				if (this.Properties[index] == null) {
					this.Delete();
					break;
				}
				this.Properties[index]() = this.EasingFunctions[index](this.Percentage, this.StartValues[index].x, this.TargetValues[index].x);
				this.Properties[index + 1]() = this.EasingFunctions[index](this.Percentage, this.StartValues[index].y, this.TargetValues[index].y);
			}
		}

		/// <summary>
		/// Updates all starting values set the reference property values.
		/// </summary>
		protected override void updateStartValues() {
			for (int index = 0; index < this.Properties.Length; index += 2) {
				this.StartValues[index].x = this.Properties[index]();
				this.StartValues[index].y = this.Properties[index + 1]();
			}
		}
	}
}