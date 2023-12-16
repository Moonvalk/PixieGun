using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Data
{
	/// <summary>
	/// A resource that pairs a name with a resource object that can be instantiated and casted.
	/// </summary>
	[RegisteredType(nameof(MoonResource))]
	public class MoonResource : MoonValue<Resource>
	{
		/// <summary>
		/// Gets the value stored within this resource casted to the requested type.
		/// </summary>
		/// <typeparam name="Type">The type to cast resources to.</typeparam>
		/// <returns>Returns the stored resource casted to type, if available.</returns>
		public Type GetAs<Type>() where Type : Resource
		{
			return Value as Type;
		}
	}
}
