using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Data {
	/// <summary>
	/// Contains a list of MoonResources. These are Resources paired with string names that can be casted to any
	/// other Godot Resource type.
	/// </summary>
	[RegisteredType(nameof(MoonResourceArray), "", nameof(Resource))]
	public class MoonResourceArray : MoonValueArray<MoonResource> {
		/// <summary>
		/// Gets the requested index item casted to the requested resource type.
		/// </summary>
		/// <typeparam name="Type">The type to cast resources to.</typeparam>
		/// <param name="index_">The resource index to return.</param>
		/// <returns>Returns the stored resource at the requested index casted to type, if available.</returns>
		public Type GetAs<Type>(int index_) where Type : Resource {
			if (index_ > -1 && index_ < this.Length) {
				return this.Items[index_] as Type;
			}
			return default(Type);
		}
	}
}
