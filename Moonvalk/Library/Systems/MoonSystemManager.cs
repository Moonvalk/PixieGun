using System.Collections.Generic;

namespace Moonvalk.Systems
{
    /// <summary>
    /// A manager for handling all MoonSystems.
    /// </summary>
    public class MoonSystemManager
    {
        #region Data Fields

        /// <summary>
        /// A map that stores reference to all MoonSystems.
        /// </summary>
        public List<IMoonSystem> SystemMap { get; protected set; } = new List<IMoonSystem>();

        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MoonSystemManager()
        {
            Global.Systems = this;
            Global.RegisterSystems();
        }

        #region Public Methods

        /// <summary>
        /// Update method that runs each MoonSystem in order stored within _systemMap.
        /// </summary>
        /// <param name="delta_">The duration of time between last and current frame.</param>
        public void Update(float delta_)
        {
            foreach (var system in this.SystemMap)
            {
                system.Execute(delta_);
            }
        }

        /// <summary>
        /// Registers a new MoonSystem here.
        /// </summary>
        /// <param name="system_">The MoonSystem object to be registered.</param>
        public void RegisterSystem(IMoonSystem system_)
        {
            this.SystemMap.Add(system_);
        }

        /// <summary>
        /// Gets an MoonSystem stored within this manager by type.
        /// </summary>
        /// <typeparam name="Type">The type of the MoonSystem to find.</typeparam>
        /// <returns>Returns the matching MoonSystem of the type T, if possible.</returns>
        public IMoonSystem Get<Type>()
        {
            foreach (var system in this.SystemMap)
            {
                if (system.GetType() == typeof(Type))
                {
                    return system;
                }
            }

            return null;
        }

        /// <summary>
        /// Clears all System queues at once.
        /// </summary>
        public void ClearAllSystems()
        {
            foreach (var system in this.SystemMap)
            {
                system.Clear();
            }
        }

        #endregion
    }
}