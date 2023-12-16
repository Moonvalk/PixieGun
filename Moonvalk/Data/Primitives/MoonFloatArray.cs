using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Data
{
	/// <summary>
	/// Contains an array of float values with corresponding titles.
	/// </summary>
	[RegisteredType(nameof(MoonFloatArray), "", nameof(Resource))]
	public class MoonFloatArray : MoonValueArray<MoonFloat>
	{
		// ...
	}
}
