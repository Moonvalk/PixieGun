namespace Moonvalk.Animation
{
    /// <summary>
    /// Contract for a BaseMoonTweenHandler to follow.
    /// </summary>
    public interface IMoonTweenHandler
    {
        /// <summary>
        /// Gets the Tween found within this container casted to the requested type if applicable.
        /// </summary>
        /// <typeparam name="Type">The type of value being animated.</typeparam>
        /// <returns>Returns the Tween casted to the requested type if available.</returns>
        IMoonTween<Type> GetTween<Type>();

        /// <summary>
        /// Deletes the Tween found within this container.
        /// </summary>
        void Delete();

        /// <summary>
        /// Starts the Tween found within this container.
        /// </summary>
        void Start();
    }
}