namespace Moonvalk.Systems
{
    /// <summary>
    /// Abstract contract for any system to follow that will be managed by the MoonSystemManager.
    /// </summary>
    /// <typeparam name="Type">The type of system. This is required in order to access Systems globally by type.</typeparam>
    public abstract class MoonSystem<Type> : IMoonSystem
    {
        #region Constructors

        /// <summary>
        /// Constructs a new MoonSystem with the default identity.
        /// </summary>
        protected MoonSystem()
        {
            this.Initialize();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes this new System.
        /// </summary>
        protected void Initialize()
        {
            RegisterSelf();
        }

        /// <summary>
        /// Registers this System within the MoonSystemManager.
        /// </summary>
        protected void RegisterSelf()
        {
            Global.Systems.RegisterSystem(this);
        }

        /// <summary>
        /// Execution method for each System.
        /// </summary>
        /// <param name="delta_">The duration in time between last and current frame.</param>
        public abstract void Execute(float delta_);

        /// <summary>
        /// Clears the current queue applied to a system.
        /// </summary>
        public abstract void Clear();

        #endregion
    }
}