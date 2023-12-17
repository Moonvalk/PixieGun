using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Animation {
	/// <summary>
	/// A basic Wobble which handles float values.
	/// </summary>
	public class MoonWobble : BaseMoonWobble<float> {
		/// <summary>
		/// Default constructor made without setting up references.
		/// </summary>
		public MoonWobble() : base() {
			// ...
		}

		/// <summary>
		/// Constructor for creating a new Wobble.
		/// </summary>
		/// <param name="referenceValues_">Array of references to float values.</param>
		public MoonWobble(params Ref<float>[] referenceValues_) : base(referenceValues_) {
			// ...
		}

		/// <summary>
		/// Method used to update all properties available to this object.
		/// </summary>
		protected override void UpdateProperties() {
			// Apply easing and set properties.
			var wave = Mathf.Sin(this.Time * this.Frequency) * this.Amplitude * this._strength;
			for (var index = 0; index < this.Properties.Length; index++) {
				if (this.Properties[index] == null) {
					this.Delete();
					break;
				}
				this.Properties[index]() = this.StartValues[index] + (wave * this.Percentage);
			}
		}

		/// <summary>
		/// Updates all starting values set the reference property values.
		/// </summary>
		protected override void UpdateStartValues() {
			for (var index = 0; index < this.Properties.Length; index++) {
				this.StartValues[index] = this.Properties[index]();
			}
		}
	}
}