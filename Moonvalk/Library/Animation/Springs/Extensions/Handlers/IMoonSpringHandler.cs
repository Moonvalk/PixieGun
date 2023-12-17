
namespace Moonvalk.Animation
{
	/// <summary>
	/// Contract for a BaseMoonSpringHandler to follow.
	/// </summary>
	public interface IMoonSpringHandler
	{
		/// <summary>
		/// Gets the Spring found within this container casted to the requested type if applicable.
		/// </summary>
		/// <typeparam name="Type">The type of value being animated.</typeparam>
		/// <returns>Returns the Spring casted to the requested type if available.</returns>
		IMoonSpring<Type> GetSpring<Type>();

		/// <summary>
		/// Deletes the Spring found within this container.
		/// </summary>
		void Delete();

		/// <summary>
		/// Starts the Spring found within this container.
		/// </summary>
		void Start();
	}
}
