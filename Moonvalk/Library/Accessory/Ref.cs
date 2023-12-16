
namespace Moonvalk.Accessory {
	/// <summary>
	/// Contract for a method that will return a reference to a field similar to that of a pointer.
	/// </summary>
	/// <typeparam name="Type">The type of value or object that will be stored here.</typeparam>
	/// <returns>Returns a value or object of type T.</returns>
	public delegate ref Type Ref<Type>();
}
