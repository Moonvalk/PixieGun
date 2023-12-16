using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Data {
	/// <summary>
	/// Contains an array of boolean values with corresponding titles.
	/// </summary>
	[RegisteredType(nameof(MoonBoolArray), "", nameof(Resource))]
	public class MoonBoolArray : MoonValueArray<MoonBool> {
		// ...
	}
}
