using Godot;

namespace Moonvalk.Data {
	/// <summary>
	/// Container for storing a group of MoonValue items of generic Resource type.
	/// </summary>
	public abstract class MoonValueArray<Unit> : Resource, IMoonValue where Unit : IMoonValue {
		/// <summary>
		/// The name of the resource.
		/// </summary>
		[Export] public string Name { get; protected set; } = "";
		
		/// <summary>
		/// Array of all items stored here.
		/// </summary>
		[Export] public Unit[] Items { get; protected set; }

		/// <summary>
		/// Gets the current item count.
		/// </summary>
		public int Length { get => this.Items.Length; }
	}
}
