using Moonvalk.Accessory;

namespace Moonvalk.Animation {
	/// <summary>
	/// A basic Tween which handles singular float value properties.
	/// </summary>
	public class MoonTween : BaseMoonTween<float> {
		/// <summary>
		/// Default constructor made without setting up references.
		/// </summary>
		public MoonTween() : base() {
			// ...
		}

		/// <summary>
		/// Constructor for creating a new Tween.
		/// </summary>
		/// <param name="referenceValues_">Array of references to float values.</param>
		public MoonTween(params Ref<float>[] referenceValues_) : base(referenceValues_) {
			// ...
		}

		/// <summary>
		/// Method used to update all properties available to this object.
		/// </summary>
		protected override void updateProperties() {
			// Apply easing and set properties.
			for (int index = 0; index < this.Properties.Length; index++) {
				if (this.Properties[index] == null) {
					this.Delete();
					break;
				}
				this.Properties[index]() = this.EasingFunctions[index](this.Percentage, this.StartValues[index], this.TargetValues[index]);
			}
		}

		/// <summary>
		/// Updates all starting values set the reference property values.
		/// </summary>
		protected override void updateStartValues() {
			for (int index = 0; index < this.Properties.Length; index++) {
				this.StartValues[index] = this.Properties[index]();
			}
		}
	}
}