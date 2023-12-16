using Godot;

namespace Moonvalk.Accessory
{
	/// <summary>
	/// Static class containing basic extended conversion functions.
	/// </summary>
	public static class ConversionHelpers
	{
		/// <summary>
		/// Converts a boolean value to an integer.
		/// </summary>
		/// <param name="flag_">The input boolean flag to convert.</param>
		/// <returns>Returns 1 if true or 0 if false.</returns>
		public static int ToInt(bool flag_)
		{
			return flag_ ? 1 : 0;
		}
		
		/// <summary>
		/// Deconstructs a Vector3 value into a float array.
		/// </summary>
		/// <param name="vector_">The input Vector3 to deconstruct.</param>
		/// <returns>Returns a float array representing the 3 values stored in a Vector3 (xyz).</returns>
		public static float[] Deconstruct(Godot.Vector3 vector_)
		{
			float[] values = new float[3];
			values[0] = vector_.x;
			values[1] = vector_.y;
			values[2] = vector_.z;
			return values;
		}

		/// <summary>
		/// Deconstructs a Vector2 value into a float array.
		/// </summary>
		/// <param name="vector_">The input Vector2 to deconstruct.</param>
		/// <returns>Returns a float array representing the 2 values stored in a Vector2 (xy).</returns>
		public static float[] Deconstruct(Godot.Vector2 vector_)
		{
			float[] values = new float[2];
			values[0] = vector_.x;
			values[1] = vector_.y;
			return values;
		}

		/// <summary>
		/// Gets the absolute value of the input Vector2.
		/// </summary>
		/// <param name="vector_">An input Vector to convert.</param>
		/// <returns>Returns the absolute value of the input Vector2.</returns>
		public static Godot.Vector2 Abs(Godot.Vector2 vector_)
		{
			return new Godot.Vector2(Mathf.Abs(vector_.x), Mathf.Abs(vector_.y));
		}

		/// <summary>
		/// Gets the absolute value of the input Vector3.
		/// </summary>
		/// <param name="vector_">An input Vector to convert.</param>
		/// <returns>Returns the absolute value of the input Vector3.</returns>
		public static Godot.Vector3 Abs(Godot.Vector3 vector_)
		{
			return new Godot.Vector3(Mathf.Abs(vector_.x), Mathf.Abs(vector_.y), Mathf.Abs(vector_.z));
		}
	}
}
