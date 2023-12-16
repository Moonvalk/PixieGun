using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Data {
	/// <summary>
	/// Contains an array of integer values with corresponding titles.
	/// </summary>
	[RegisteredType(nameof(MoonIntArray), "", nameof(Resource))]
	public class MoonIntArray : MoonValueArray<MoonInt> {
		// ...
	}
}
