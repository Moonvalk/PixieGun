using Moonvalk.Accessory;

namespace Moonvalk.Animation {
	/// <summary>
	/// A basic Tween which handles Color value properties.
	/// </summary>
	public class MoonTweenColor : BaseMoonTween<Godot.Color> {
		/// <summary>
		/// Default constructor made without setting up references.
		/// </summary>
		public MoonTweenColor() : base() {
			// ...
		}

		/// <summary>
		/// Constructor for creating a new Tween.
		/// </summary>
		/// <param name="referenceValues_">Array of references to float values.</param>
		public MoonTweenColor(params Ref<float>[] referenceValues_) : base(referenceValues_) {
			// ...
		}

		/// <summary>
		/// Method used to update all properties available to this object.
		/// </summary>
		protected override void updateProperties() {
			// Apply easing and set properties.
			for (int index = 0; index < this.Properties.Length; index += 4) {
				if (this.Properties[index] == null) {
					this.Stop();
					break;
				}
				this.Properties[index]() = this.EasingFunctions[index](this.Percentage, this.StartValues[index].r, this.TargetValues[index].r);
				this.Properties[index + 1]() = this.EasingFunctions[index](this.Percentage, this.StartValues[index].g, this.TargetValues[index].g);
				this.Properties[index + 2]() = this.EasingFunctions[index](this.Percentage, this.StartValues[index].b, this.TargetValues[index].b);
				this.Properties[index + 3]() = this.EasingFunctions[index](this.Percentage, this.StartValues[index].a, this.TargetValues[index].a);
			}
		}

		/// <summary>
		/// Updates all starting values set the reference property values.
		/// </summary>
		protected override void updateStartValues() {
			for (int index = 0; index < this.Properties.Length; index += 4) {
				this.StartValues[index].r = this.Properties[index]();
				this.StartValues[index].g = this.Properties[index + 1]();
				this.StartValues[index].b = this.Properties[index + 2]();
				this.StartValues[index].a = this.Properties[index + 3]();
			}
		}
	}
}
