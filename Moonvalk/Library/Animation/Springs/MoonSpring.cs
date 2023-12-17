using Moonvalk.Utilities.Algorithms;
using Moonvalk.Accessory;
using Godot;

namespace Moonvalk.Animation {
	/// <summary>
	/// Object that handles Spring calculations for a singular float value.
	/// </summary>
	public class MoonSpring : BaseMoonSpring<float> {
		/// <summary>
		/// Default constructor made without setting up references.
		/// </summary>
		public MoonSpring() : base() {
			// ...
		}

		/// <summary>
		/// Constructor for creating a new Spring.
		/// </summary>
		/// <param name="referenceValues_">Array of references to float values.</param>
		public MoonSpring(params Ref<float>[] referenceValues_) : base(referenceValues_) {
			// ...
		}

		/// <summary>
		/// Calculates the necessary velocities to be applied to all Spring properties each game tick.
		/// </summary>
		protected override void CalculateForces() {
			for (var i = 0; i < this.Properties.Length; i++) {
				if (this.Properties[i] == null) {
					this.Delete();
					break;
				}
				var displacement = (this.TargetProperties[i] - this.Properties[i]());
				this.CurrentForce[i] = MotionAlgorithms.SimpleHarmonicMotion(this.Tension, displacement, this.Dampening, this.Speed[i]);
			}
		}

		/// <summary>
		/// Applies force to properties each frame.
		/// </summary>
		/// <param name="deltaTime_">The time elapsed between last and current game tick.</param>
		protected override void ApplyForces(float deltaTime_) {
			for (var i = 0; i < this.Properties.Length; i++) {
				this.Speed[i] += this.CurrentForce[i] * deltaTime_;
				this.Properties[i]() += this.Speed[i] * deltaTime_;
			}
		}

		/// <summary>
		/// Determines if the minimum forces have been met to continue calculating Spring forces.
		/// </summary>
		/// <returns>Returns true if the minimum forces have been met.</returns>
		protected override bool MinimumForcesMet() {
			for (var index = 0; index < CurrentForce.Length; index++) {
				var metTarget = (Mathf.Abs(this.TargetProperties[index] - this.Properties[index]()) >= this.MinimumForce[index]);
				var metMinimumForce = (Mathf.Abs(this.CurrentForce[index] + this.Speed[index]) >= this.MinimumForce[index]);
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
			this.MinimumForce = new float[this.Properties.Length];
			for (var index = 0; index < this.Properties.Length; index++) {
				this.MinimumForce[index] = MoonSpring.DefaultMinimumForcePercentage * Mathf.Abs(this.TargetProperties[index] - this.Properties[index]());
			}
		}

		/// <summary>
		/// Determines if there is a need to apply force to this Spring to meet target values.
		/// </summary>
		/// <returns>Returns true if forces need to be applied</returns>
		protected override bool NeedToApplyForce() {
			for (var index = 0; index < this.Properties.Length; index++) {
				if (this.Properties[index]() != this.TargetProperties[index]) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Snaps all Spring properties directly to their target values. 
		/// </summary>
		protected override void SnapSpringToTarget() {
			for (var index = 0; index < this.Properties.Length; index++) {
				this.Properties[index]() = this.TargetProperties[index];
			}
		}
	}
}
