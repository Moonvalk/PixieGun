namespace Moonvalk.Animation
{
    /// <summary>
    /// Contract for a BaseMoonWobbleHandler to follow.
    /// </summary>
    public interface IMoonWobbleHandler
    {
        /// <summary>
        /// Gets the Wobble found within this container casted to the requested type if applicable.
        /// </summary>
        /// <typeparam name="Type">The type of value being animated.</typeparam>
        /// <returns>Returns the Wobble casted to the requested type if available.</returns>
        IMoonWobble<Type> GetWobble<Type>();

        /// <summary>
        /// Deletes the Wobble found within this container.
        /// </summary>
        void Delete();

        /// <summary>
        /// Starts the Wobble found within this container.
        /// </summary>
        void Start();
    }
}