#if (TOOLS)
using System.Collections.ObjectModel;
using System.Linq;
using Godot;
using Godot.Collections;

namespace Moonvalk.Resources
{
    /// <summary>
    /// Settings available for the ResourceRegistryPlugin used in the Godot Project settings.
    /// </summary>
    public static class ResourceRegistrySettings
    {
        /// <summary>
        /// Available search types used when finding new resources to register.
        /// </summary>
        public enum ResourceSearchType
        {
            Recursive = 0,
            Namespace = 1
        }

        /// <summary>
        /// A prefix expected for classes that will be registered.
        /// </summary>
        public static string ClassPrefix => GetSettings(nameof(ClassPrefix)).ToString();

        /// <summary>
        /// The type of search that will be used when registering custom types.
        /// </summary>
        public static ResourceSearchType SearchType => (ResourceSearchType)GetSettings(nameof(SearchType));

        /// <summary>
        /// A collection of all resource directories to search for types within.
        /// </summary>
        public static ReadOnlyCollection<string> ResourceScriptDirectories =>
            new ReadOnlyCollection<string>(((Array)GetSettings(nameof(ResourceScriptDirectories))).Cast<string>().ToList());

        /// <summary>
        /// Called to initialize these settings.
        /// </summary>
        public static void Init()
        {
            AddSetting(nameof(ClassPrefix), Variant.Type.String, "");
            AddSetting(nameof(SearchType), Variant.Type.Int, ResourceSearchType.Recursive, PropertyHint.Enum, "Recursive,Namespace");

            AddSetting(nameof(ResourceScriptDirectories), Variant.Type.StringArray, new Array<string>("res://"));
        }

        /// <summary>
        /// Gets the current setting matching the input title, if available.
        /// </summary>
        /// <param name="title_">The title of the setting.</param>
        /// <returns>Returns the setting object.</returns>
        private static object GetSettings(string title_)
        {
            return ProjectSettings.GetSetting($"{nameof(ResourceRegistryPlugin)}/{title_}");
        }

        /// <summary>
        /// Adds a new setting within the Godot project settings menu.
        /// </summary>
        /// <param name="title_">The title of the setting.</param>
        /// <param name="type_">The type of the setting.</param>
        /// <param name="value_">The value used for the setting.</param>
        /// <param name="hint_">A hint used to display inputs for the setting.</param>
        /// <param name="hintString_">A string used as a hint when hovering the setting.</param>
        private static void AddSetting(string title_, Variant.Type type_, object value_, PropertyHint hint_ = PropertyHint.None, string hintString_ = "")
        {
            title_ = SettingPath(title_);
            if (!ProjectSettings.HasSetting(title_)) ProjectSettings.SetSetting(title_, value_);

            var info = new Dictionary
            {
                ["name"] = title_,
                ["type"] = type_,
                ["hint"] = hint_,
                ["hint_string"] = hintString_
            };

            ProjectSettings.AddPropertyInfo(info);
        }

        /// <summary>
        /// Gets the settings path.
        /// </summary>
        /// <param name="title_">The title of the setting to get a path for.</param>
        /// <returns>Returns the full path where this setting is located.</returns>
        private static string SettingPath(string title_)
        {
            return $"{nameof(ResourceRegistryPlugin)}/{title_}";
        }
    }
}
#endif