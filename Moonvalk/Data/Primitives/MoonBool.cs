using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Data {
	/// <summary>
	/// Pairs a string name with a boolean value.
	/// </summary>
	[RegisteredType(nameof(MoonBool), "", nameof(Resource))]
	public class MoonBool : MoonValue<bool> {
		// ...
	}
}