namespace Moonvalk.Accessory
{
    /// <summary>
    /// A container for a value that will force initializing itself before being used.
    /// </summary>
    /// <typeparam name="Type">The type of value that will be stored.</typeparam>
    public class InitValue<Type>
    {
        #region Data Fields

        /// <summary>
        /// A contract for a function expected to initialize a value within this container.
        /// </summary>
        public delegate Type InitFunction();

        /// <summary>
        /// A flag that determines whether this container has initialized itself before use.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Reference to a method used to initialize the value stored within this container.
        /// </summary>
        protected InitFunction _initializationMethod;

        /// <summary>
        /// The value stored by this container.
        /// </summary>
        protected Type _value;

        #endregion

        /// <summary>
        /// Default constructor that takes an initialization method to be run before a value can be returned via this container.
        /// </summary>
        /// <param name="initializationMethod_">Function to be run to initialize a new value.</param>
        public InitValue(InitFunction initializationMethod_)
        {
            _initializationMethod = initializationMethod_;
        }

        /// <summary>
        /// Gets/sets the value stored within this container.
        /// </summary>
        /// <value>Value of type T stored by this container.</value>
        public Type Value
        {
            get
            {
                if (!IsInitialized)
                {
                    _value = _initializationMethod();
                    IsInitialized = true;
                }

                return _value;
            }
            set
            {
                IsInitialized = true;
                _value = value;
            }
        }
    }
}