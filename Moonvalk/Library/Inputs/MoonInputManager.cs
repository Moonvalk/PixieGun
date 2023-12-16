using Godot;

namespace Moonvalk.Inputs {
	/// <summary>
	/// Manager for handling creating and removing player inputs.
	/// </summary>
	public static class MoonInputManager {
		/// <summary>
		/// Adds new inputs to the map.
		/// </summary>
		/// <param name="inputPairs_">The pairs to be added.</param>
		public static void Add(params InputKeyPair[] inputPairs_) {
			foreach (InputKeyPair pair in inputPairs_) {
				if (InputMap.HasAction(pair.Name)) {
					InputMap.EraseAction(pair.Name);
				}
				InputMap.AddAction(pair.Name);
				InputMap.ActionAddEvent(pair.Name, new InputEventKey() { Scancode = (uint)pair.Key });
			}
		}
		
		/// <summary>
		/// Removes an action from the map.
		/// </summary>
		/// <param name="inputNames_">The names to be removed.</param>
		public static void Remove(params string[] inputNames_) {
			foreach (string name in inputNames_) {
				InputMap.EraseAction(name);
			}
		}
	}
}