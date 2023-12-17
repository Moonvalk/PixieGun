using System;

namespace Moonvalk.Resources
{
    /// <summary>
    /// Container representing a type to be registered.
    /// </summary>
    [AttributeUsage(System.AttributeTargets.Class)]
    public class RegisteredTypeAttribute : System.Attribute
    {
        /// <summary>
        /// The name of the Resource type to be registered.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The optional icon path to be displayed for this resource.
        /// </summary>
        public string IconPath { get; protected set; }

        /// <summary>
        /// The base type of the Resource.
        /// </summary>
        public string BaseType { get; protected set; }

        /// <summary>
        /// Constructs a new type to be registered.
        /// </summary>
        /// <param name="name_">The name of the Resource type to be registered.</param>
        /// <param name="iconPath_">Optional icon path.</param>
        /// <param name="baseType_">The base type to inherit this type from.</param>
        public RegisteredTypeAttribute(string name_, string iconPath_ = "", string baseType_ = "Resource")
        {
            this.Name = name_;
            this.IconPath = iconPath_;
            this.BaseType = baseType_;
        }
    }
}