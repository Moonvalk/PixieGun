using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Animation {
	/// <summary>
	/// A basic Wobble which handles Vector2 values.
	/// </summary>
	public class MoonWobbleVec2 : BaseMoonWobble<Vector2> {
		/// <summary>
		/// Default constructor made without setting up references.
		/// </summary>
		public MoonWobbleVec2() : base() {
			// ...
		}

		/// <summary>
		/// Constructor for creating a new Wobble.
		/// </summary>
		/// <param name="referenceValues_">Array of references to float values.</param>
		public MoonWobbleVec2(params Ref<float>[] referenceValues_) : base(referenceValues_) {
			// ...
		}

		/// <summary>
		/// Method used to update all properties available to this object.
		/// </summary>
		protected override void UpdateProperties() {
			// Apply easing and set properties.
			var wave = Mathf.Sin(this.Time * this.Frequency) * this.Amplitude * this._strength;
			for (var index = 0; index < this.Properties.Length; index += 2) {
				if (this.Properties[index] == null) {
					this.Delete();
					break;
				}
				this.Properties[index]() = this.StartValues[index].x + (wave * this.Percentage.x);
				this.Properties[index + 1]() = this.StartValues[index].y + (wave * this.Percentage.y);
			}
		}

		/// <summary>
		/// Updates all starting values set the reference property values.
		/// </summary>
		protected override void UpdateStartValues() {
			for (var index = 0; index < this.Properties.Length; index += 2) {
				this.StartValues[index].x = this.Properties[index]();
				this.StartValues[index].y = this.Properties[index + 1]();
			}
		}
	}
}