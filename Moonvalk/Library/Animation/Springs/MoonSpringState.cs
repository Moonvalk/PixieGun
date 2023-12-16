
namespace Moonvalk.Animation {
	/// <summary>
	/// Provides an index for each available Spring state.
	/// </summary>
	public enum MoonSpringState {
		/// <summary> This Spring has just begun. </summary>
		Start,
		/// <summary> This Spring is actively updating. </summary>
		Update,
		/// <summary> This Spring has just completed. </summary>
		Complete,
		/// <summary> This Spring is idle and is now considered stopped. </summary>
		Stopped,
		/// <summary> This Spring has just deleted early. </summary>
		Deleted,
	}
}
