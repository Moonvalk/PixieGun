using Moonvalk.Utilities.Algorithms;
using Moonvalk.Accessory;
using Godot;

namespace Moonvalk.Animation {
	/// <summary>
	/// Object that handles Spring calculations for a Vector2 value.
	/// </summary>
	public class MoonSpringVec2 : BaseMoonSpring<Vector2> {
		/// <summary>
		/// Default constructor made without setting up references.
		/// </summary>
		public MoonSpringVec2() : base() {
			// ...
		}

		/// <summary>
		/// Constructor for creating a new Spring.
		/// </summary>
		/// <param name="referenceValues_">Array of references to Vector2 values.</param>
		public MoonSpringVec2(params Ref<float>[] referenceValues_) : base(referenceValues_) {
			// ...
		}

		/// <summary>
		/// Calculates the necessary velocities to be applied to all Spring properties each game tick.
		/// </summary>
		protected override void CalculateForces() {
			for (var index = 0; index < this.Properties.Length; index += 2) {
				if (this.Properties[index] == null) {
					this.Delete();
					break;
				}
				var displacement = (this.TargetProperties[index].x - this.Properties[index]());
				this.CurrentForce[index].x = MotionAlgorithms.SimpleHarmonicMotion(this.Tension, displacement, this.Dampening, this.Speed[index].x);
				displacement = (this.TargetProperties[index].y - this.Properties[index + 1]());
				this.CurrentForce[index].y = MotionAlgorithms.SimpleHarmonicMotion(this.Tension, displacement, this.Dampening, this.Speed[index].y);
			}
		}

		/// <summary>
		/// Applies force to properties each frame.
		/// </summary>
		/// <param name="deltaTime_">The time elapsed between last and current game tick.</param>
		protected override void ApplyForces(float deltaTime_) {
			for (var index = 0; index < this.Properties.Length; index += 2) {
				this.Speed[index].x += this.CurrentForce[index].x * deltaTime_;
				this.Speed[index].y += this.CurrentForce[index].y * deltaTime_;
				this.Properties[index]() += this.Speed[index].x * deltaTime_;
				this.Properties[index + 1]() += this.Speed[index].y * deltaTime_;
			}
		}

		/// <summary>
		/// Determines if the minimum forces have been met to continue calculating Spring forces.
		/// </summary>
		/// <returns>Returns true if the minimum forces have been met.</returns>
		protected override bool MinimumForcesMet() {
			for (var index = 0; index < CurrentForce.Length; index += 2) {
				var current = new Vector2(this.Properties[index](), this.Properties[index + 1]());
				var metTarget = (ConversionHelpers.Abs(this.TargetProperties[index] - current) >= this.MinimumForce[index]);
				var metMinimumForce = (ConversionHelpers.Abs(this.CurrentForce[index] + this.Speed[index]) >= this.MinimumForce[index]);
				if (metTarget && metMinimumForce) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Assigns the minimum force required until the Spring is completed based on inputs.
		/// </summary>
		protected override void SetMinimumForce() {
			this.MinimumForce = new Vector2[this.Properties.Length];
			for (var index = 0; index < this.Properties.Length; index += 2) {
				var current = new Vector2(this.Properties[index](), this.Properties[index + 1]());
				this.MinimumForce[index] = MoonSpring.DefaultMinimumForcePercentage *
					ConversionHelpers.Abs(this.TargetProperties[index] - current);
			}
		}

		/// <summary>
		/// Determines if there is a need to apply force to this Spring to meet target values.
		/// </summary>
		/// <returns>Returns true if forces need to be applied</returns>
		protected override bool NeedToApplyForce() {
			for (var index = 0; index < this.Properties.Length; index += 2) {
				var current = new Vector2(this.Properties[index](), this.Properties[index + 1]());
				if (current != this.TargetProperties[index]) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Snaps all Spring properties directly to their target values. 
		/// </summary>
		protected override void SnapSpringToTarget() {
			for (var index = 0; index < this.Properties.Length; index += 2) {
				this.Properties[index]() = this.TargetProperties[index].x;
				this.Properties[index + 1]() = this.TargetProperties[index].y;
			}
		}
	}
}
