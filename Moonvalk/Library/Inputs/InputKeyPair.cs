using Godot;

namespace Moonvalk.Inputs {
	/// <summary>
	/// Container for pairing a name and key.
	/// </summary>
	public struct InputKeyPair {
		/// <summary>
		/// The name of the input.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The key associated with this input.
		/// </summary>
		public KeyList Key { get; set; }

		/// <summary>
		/// Constructs a new input name / key pairing.
		/// </summary>
		/// <param name="name_">The name to be stored.</param>
		/// <param name="key_">The key to the paired with this name.</param>
		public InputKeyPair(string name_, KeyList key_) {
			this.Name = name_;
			this.Key = key_;
		}
	}
}