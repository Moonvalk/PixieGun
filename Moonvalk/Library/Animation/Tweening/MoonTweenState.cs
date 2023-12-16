
namespace Moonvalk.Animation {
	/// <summary>
	/// Provides an index for each available Tween state.
	/// </summary>
	public enum MoonTweenState {
		/// <summary> This Tween is idle and waiting for instruction. </summary>
		Idle,
		/// <summary> This Tween has just begun. </summary>
		Start,
		/// <summary> This Tween is actively updating. </summary>
		Update,
		/// <summary> This Tween has just completed. </summary>
		Complete,
		/// <summary> This Tween is idle and is now considered stopped. </summary>
		Stopped,
	}
}
