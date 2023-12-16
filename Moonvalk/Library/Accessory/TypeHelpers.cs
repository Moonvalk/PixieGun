
namespace Moonvalk.Accessory {
	/// <summary>
	/// Static class containing helper functions for basic type checks.
	/// </summary>
	public static class TypeHelpers {
		/// <summary>
		/// Determines if the object being checked is of type T.
		/// </summary>
		/// <typeparam name="Type">The type that is expected for this check to return true.</typeparam>
		/// <param name="obj_">The object being tested.</param>
		/// <returns>Returns true when the object is of type T.</returns>
		public static bool IsType<Type>(this object obj_) {
			return obj_ is Type;
		}
	}
}