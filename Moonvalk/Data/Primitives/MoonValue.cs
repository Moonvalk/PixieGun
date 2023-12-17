using Godot;

namespace Moonvalk.Data
{
    /// <summary>
    /// Container used for pairing titles with different data types as a resource.
    /// </summary>
    public abstract class MoonValue<Unit> : Resource, IMoonValue
    {
        /// <summary>
        /// The name of the resource.
        /// </summary>
        [Export]
        public string Name { get; protected set; } = "";

        /// <summary>
        /// The value stored here.
        /// </summary>
        [Export]
        public Unit Value { get; protected set; }
    }
}