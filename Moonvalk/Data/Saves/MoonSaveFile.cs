// #define __DEBUG

using System.Linq;
using Godot;
using Godot.Collections;

namespace Moonvalk.Data
{
    /// <summary>
    /// Base class representing a singular save file that will store various save data.
    /// </summary>
    public class MoonSaveFile
    {
        #region Data Fields

        /// <summary>
        /// A dictionary of all save data by string name (category) that will be stored within this save file.
        /// </summary>
        public System.Collections.Generic.Dictionary<string, IMoonSaveData> SaveItems { get; protected set; }

        /// <summary>
        /// The corresponding file path where this save will be located.
        /// </summary>
        public string FilePath { get; protected set; }

        /// <summary>
        /// File object used for loading / saving to the system.
        /// </summary>
        public File SaveFile { get; protected set; } = new File();

        /// <summary>
        /// The hardcoded user file location where app data will be stored (ex. %appdata%/Godot/Project/...).
        /// </summary>
        protected const string UserDataLocation = "user://";

        #endregion

        #region Public Methods

        /// <summary>
        /// Default constructor for a new save file object.
        /// </summary>
        /// <param name="filePath_">The name and location of this save file.</param>
        public MoonSaveFile(string filePath_ = "Save.json")
        {
            FilePath = filePath_;
            SaveItems = new System.Collections.Generic.Dictionary<string, IMoonSaveData>();
        }

        /// <summary>
        /// Adds new save data to this container of the specified unit type and returns it. This method
        /// also accepts existing save data objects as input.
        /// </summary>
        /// <typeparam name="Unit">The unit used to store game data.</typeparam>
        /// <param name="category_">The name of the category where this save data will be located.</param>
        /// <param name="saveData_">An optional existing save data object to be stored here.</param>
        /// <returns>Returns the save data object stored for the specified category.</returns>
        public BaseMoonSaveData<Unit> AddSaveData<Unit>(string category_, BaseMoonSaveData<Unit> saveData_ = null)
        {
            var formattedKey = category_.ToLower();
            if (SaveItems.ContainsKey(formattedKey))
            {
                // Remove existing data if it exists.
                SaveItems.Remove(formattedKey);
            }

            if (saveData_ == null) saveData_ = new BaseMoonSaveData<Unit>();

            SaveItems.Add(formattedKey, saveData_);
            return saveData_;
        }

        /// <summary>
        /// Called to get any existing save data for the provided category name.
        /// </summary>
        /// <typeparam name="Unit">The unit used to store game data.</typeparam>
        /// <param name="category_">The name of the category where this save data will be located.</param>
        /// <returns>Returns the save data object stored for the specified category, if it exists.</returns>
        public BaseMoonSaveData<Unit> GetSaveData<Unit>(string category_)
        {
            var formattedKey = category_.ToLower();
            if (SaveItems.ContainsKey(formattedKey)) return SaveItems[formattedKey] as BaseMoonSaveData<Unit>;

            return null;
        }

        /// <summary>
        /// Sets individual values stored within this save file by category, data name, and expected value for storage.
        /// </summary>
        /// <typeparam name="Unit">The unit used to store game data in this category.</typeparam>
        /// <param name="category_">The category name.</param>
        /// <param name="settings_">Array of settings pairings (data name and value).</param>
        /// <returns>Returns the save data object where these settings were stored.</returns>
        public BaseMoonSaveData<Unit> Set<Unit>(string category_, params (string name_, Unit value_)[] settings_)
        {
            var formattedKey = category_.ToLower();
            BaseMoonSaveData<Unit> saveData = null;
            if (SaveItems.ContainsKey(formattedKey))
                saveData = SaveItems[formattedKey] as BaseMoonSaveData<Unit>;

            if (saveData == null)
                saveData = AddSaveData(formattedKey, new BaseMoonSaveData<Unit>());

            foreach (var setting in settings_)
            {
                saveData.SetValue((setting.name_, setting.value_));
            }

            return saveData;
        }

        /// <summary>
        /// Gets the value for the corresponding category and setting name, if applicable.
        /// </summary>
        /// <typeparam name="Unit">The type of unit used to store game data in this category.</typeparam>
        /// <param name="category_">The category name.</param>
        /// <param name="setting_">The name of the setting a value is expected for.</param>
        /// <returns>Returns the matching value if it exists.</returns>
        public Unit Get<Unit>(string category_, string setting_)
        {
            var formattedKey = category_.ToLower();
            if (SaveItems.ContainsKey(formattedKey))
            {
                var saveData = SaveItems[formattedKey] as BaseMoonSaveData<Unit>;
                return saveData.GetValue(setting_);
            }

            return default;
        }

        /// <summary>
        /// Called to save this file with all current data.
        /// </summary>
        public void Save()
        {
            var error = SaveFile.Open(UserDataLocation + FilePath, File.ModeFlags.Write);
            if (error != Error.Ok)
            {
#if (__DEBUG)
                    GD.Print("Error saving. Could not open save file.");
#endif
                return;
            }

            var format = new System.Collections.Generic.Dictionary<string, string>();
            var keys = SaveItems.Keys.ToArray();
            for (var index = 0; index < keys.Length; index++)
            {
                format.Add(keys[index].ToLower(), SaveItems[keys[index]].GetJson());
            }

            var jsonString = JSON.Print(format);

#if (__DEBUG)
                GD.Print("JSON String is: " + jsonString);
#endif
            SaveFile.StoreString(jsonString);
            SaveFile.Close();
        }

        /// <summary>
        /// Called to load this file and store all data stored there within this container.
        /// </summary>
        public void Load()
        {
            var error = SaveFile.Open(UserDataLocation + FilePath, File.ModeFlags.Read);
            if (error != Error.Ok)
            {
#if (__DEBUG)
                    GD.Print("Error loading. Could not open save file.");
#endif
                return;
            }

            var content = SaveFile.GetAsText();
            SaveFile.Close();

#if (__DEBUG)
                GD.Print("Content is: " + content);
#endif
            var result = (Dictionary)JSON.Parse(content).Result;

            var keys = result.Keys.Cast<string>().ToArray();
            var values = result.Values.Cast<string>().ToArray();
            for (var index = 0; index < keys.Length; index++)
            {
#if (__DEBUG)
                    GD.Print("Found key " + keys[index]);
                    GD.Print("Found value " + values[index]);
#endif

                var categories = SaveItems.Keys.ToArray();
                for (var item = 0; item < categories.Length; item++)
                {
                    if (categories[item] == keys[index])
                    {
                        SaveItems[categories[item]].ParseJson(values[index]);
                        break;
                    }
                }
            }
        }

        #endregion
    }
}