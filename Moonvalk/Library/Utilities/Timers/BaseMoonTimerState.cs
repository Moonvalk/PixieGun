
namespace Moonvalk.Utilities {
	/// <summary>
	/// Provides an index for each available BaseTimer state.
	/// </summary>
	public enum BaseMoonTimerState {
		/// <summary> This BaseTimer is idle and waiting for instruction. </summary>
		Idle,
		/// <summary> This BaseTimer has just begun. </summary>
		Start,
		/// <summary> This BaseTimer is actively updating. </summary>
		Update,
		/// <summary> This BaseTimer has just completed. </summary>
		Complete,
		/// <summary> This BaseTimer is idle and is now considered stopped. </summary>
		Stopped,
	}
}
