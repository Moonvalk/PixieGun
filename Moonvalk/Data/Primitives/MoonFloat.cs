using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Data
{
	/// <summary>
	/// Pairs a string name with a float value.
	/// </summary>
	[RegisteredType(nameof(MoonFloat), "", nameof(Resource))]
	public class MoonFloat : MoonValue<float>
	{
		// ...
	}
}