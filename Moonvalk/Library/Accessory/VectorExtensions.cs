using Godot;

namespace Moonvalk.Accessory {
	/// <summary>
	/// Static extensions class for the Godot Vector3 data type.
	/// </summary>
	public static class VectorExtensions {
		/// <summary>
		/// Returns the Vector3 value with reassigned X, Y, and/or Z.
		/// </summary>
		/// <param name="vector_">The input vector to be adjusted.</param>
		/// <param name="x_">A new X value to be used.</param>
		/// <param name="y_">A new Y value to be used.</param>
		/// <param name="z_">A new Z value to be used.</param>
		/// <returns>Returns the modified input vector.</returns>
		public static Vector3 With(this Vector3 vector_, float? x_ = null, float? y_ = null, float? z_ = null) {
			return new Vector3(x_ ?? vector_.x, y_ ?? vector_.y, z_ ?? vector_.z);
		}

		/// <summary>
		/// Returns the Vector2 value with reassigned X and/or Y.
		/// </summary>
		/// <param name="vector_">The input vector to be adjusted.</param>
		/// <param name="x_">A new X value to be used.</param>
		/// <param name="y_">A new Y value to be used.</param>
		/// <returns>Returns the modified input vector.</returns>
		public static Vector2 With(this Vector2 vector_, float? x_ = null, float? y_ = null) {
			return new Vector2(x_ ?? vector_.x, y_ ?? vector_.y);
		}

		/// <summary>
		/// Returns the Vector3 value with added X, Y, and/or Z values.
		/// </summary>
		/// <param name="vector_">The input vector to be adjusted.</param>
		/// <param name="x_">The x value to add.</param>
		/// <param name="y_">The y value to add.</param>
		/// <param name="z_">The z value to add.</param>
		/// <returns>Returns the modified input vector.</returns>
		public static Vector3 Add(this Vector3 vector_, float? x_ = null, float? y_ = null, float? z_ = null) {
			return new Vector3(vector_.x + (x_ ?? 0f), vector_.y + (y_ ?? 0f), vector_.z + (z_ ?? 0f));
		}

		/// <summary>
		/// Returns the Vector2 value with added X and/or Y values.
		/// </summary>
		/// <param name="vector_">The input vector to be adjusted.</param>
		/// <param name="x_">The x value to add.</param>
		/// <param name="y_">The y value to add.</param>
		/// <returns>Returns the modified input vector.</returns>
		public static Vector2 Add(this Vector2 vector_, float? x_ = null, float? y_ = null) {
			return new Vector2(vector_.x + (x_ ?? 0f), vector_.y + (y_ ?? 0f));
		}
	}
}
