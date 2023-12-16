
namespace Moonvalk.Utilities.Algorithms {
	/// <summary>
	/// Algorithms for determining physical motion.
	/// </summary>
	public static class MotionAlgorithms {
		/// <summary>
		/// Uses the provided settings to supply a force that creates simple harmonic motion.
		/// </summary>
		/// <param name="tension_">Tension applied to this spring motion. Determines the amount of force to return to offset.</param>
		/// <param name="offset_">Offset/target height applied to this spring motion.</param>
		/// <param name="dampening_">Dampening factor applied to this spring motion. Determines how quickly motion is slowed.</param>
		/// <param name="velocity_">Current velocity applied to this spring on the previous game tick.</param>
		/// <returns>Returns a new velocity to continue harmonic motion.</returns>
		public static float SimpleHarmonicMotion(float tension_, float offset_, float dampening_, float velocity_) {
			return (tension_ * offset_) - (dampening_ * velocity_);
		}
	}
}
