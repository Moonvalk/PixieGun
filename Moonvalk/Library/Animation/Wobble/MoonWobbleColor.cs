using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Animation {
	/// <summary>
	/// A basic Wobble which handles Color values.
	/// </summary>
	public class MoonWobbleColor : BaseMoonWobble<Godot.Color> {
		/// <summary>
		/// Default constructor made without setting up references.
		/// </summary>
		public MoonWobbleColor() : base() {
			// ...
		}

		/// <summary>
		/// Constructor for creating a new Wobble.
		/// </summary>
		/// <param name="referenceValues_">Array of references to float values.</param>
		public MoonWobbleColor(params Ref<float>[] referenceValues_) : base(referenceValues_) {
			// ...
		}

		/// <summary>
		/// Method used to update all properties available to this object.
		/// </summary>
		protected override void updateProperties() {
			// Apply easing and set properties.
			float wave = Mathf.Sin(this.Time * this.Frequency) * this.Amplitude * this._strength;
			for (int index = 0; index < this.Properties.Length; index += 4) {
				if (this.Properties[index] == null) {
					this.Delete();
					break;
				}
				this.Properties[index]() = this.StartValues[index].r + (wave * this.Percentage.r);
				this.Properties[index + 1]() = this.StartValues[index].g + (wave * this.Percentage.g);
				this.Properties[index + 2]() = this.StartValues[index].b + (wave * this.Percentage.b);
				this.Properties[index + 3]() = this.StartValues[index].a + (wave * this.Percentage.a);
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