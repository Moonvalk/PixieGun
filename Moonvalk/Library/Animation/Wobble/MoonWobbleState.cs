
namespace Moonvalk.Animation {
	/// <summary>
	/// Provides an index for each available Wobble state.
	/// </summary>
	public enum MoonWobbleState {
		/// <summary> This Wobble is idle and waiting for instruction. </summary>
		Idle,
		/// <summary> This Wobble has just begun. </summary>
		Start,
		/// <summary> This Wobble is actively updating. </summary>
		Update,
		/// <summary> This Wobble has just completed. </summary>
		Complete,
		/// <summary> This Wobble is idle and is now considered stopped. </summary>
		Stopped,
	}
}
