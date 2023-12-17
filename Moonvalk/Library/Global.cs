using Moonvalk.Animation;
using Moonvalk.Systems;
using Moonvalk.Utilities;

namespace Moonvalk
{
    /// <summary>
    /// Global class for accessing Systems and game engine components.
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// Global System manager for MoonSystems.
        /// </summary>
        public static MoonSystemManager Systems { get; set; } = new MoonSystemManager();

        /// <summary>
        /// Gets a specific MoonSystem found within the Global MoonSystemSystemManager.
        /// </summary>
        /// <typeparam name="Type">The type of System being searched for.</typeparam>
        /// <returns>Returns the MoonSystem matching type searched for, if found.</returns>
        public static IMoonSystem GetSystem<Type>()
        {
            return Systems.Get<Type>();
        }

        /// <summary>
        /// Called to register custom systems once the MoonSystemManager has been initialized.
        /// </summary>
        public static void RegisterSystems()
        {
            new MoonTimerSystem();
            new MoonTweenSystem();
            new MoonSpringSystem();
            new MoonWobbleSystem();
        }
    }
}