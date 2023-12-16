// #define __DEBUG

using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Moonvalk.Data {
    /// <summary>
    /// Base class for save data of generic type that can be stored / parsed from JSON.
    /// </summary>
    /// <typeparam name="Unit">The unit type that is expected to be stored here.</typeparam>
    public class BaseMoonSaveData<Unit> : IMoonSaveData {
        /// <summary>
        /// A dictionary of all save data accessible by string names.
        /// </summary>
        public Dictionary<string, Unit> Data { get; protected set; }

        /// <summary>
        /// Default constructor for a new save data container.
        /// </summary>
        public BaseMoonSaveData() {
            this.Data = new Dictionary<string, Unit>();
        }

        /// <summary>
        /// Called to set values within this save data by string / unit pairing.
        /// </summary>
        /// <param name="pairs_">All pairs of data to be set within this object.</param>
        public BaseMoonSaveData<Unit> SetValue(params (string name_, Unit value_)[] pairs_) {
            foreach ((string name_, Unit value_) pair in pairs_) {
                string formattedKey = pair.name_.ToLower();
                if (this.Data.ContainsKey(formattedKey)) {
                    this.Data[formattedKey] = pair.value_;
                    continue;
                }
                this.Data.Add(formattedKey, pair.value_);
            }
            return this;
        }

        /// <summary>
        /// Called to get the unit value for the matching data name, if it exists.
        /// </summary>
        /// <param name="name_">The save data name to find a unit value for.</param>
        /// <returns>Returns the value stored for the requested data name.</returns>
        public Unit GetValue(string name_) {
            string formattedKey = name_.ToLower();
            if (this.Data.ContainsKey(formattedKey)) {
                return this.Data[formattedKey];
            }
            return default(Unit);
        }

        /// <summary>
        /// Prints this container as a JSON storage string value.
        /// </summary>
        /// <returns>Returns a JSON string matching the data found within this container.</returns>
        public string GetJSON() {
            string formattedData = JSON.Print(this.Data);
            return formattedData;
        }

        /// <summary>
        /// Called to parse a JSON string into usable data within this container.
        /// </summary>
        /// <param name="jsonData_">The JSON string to be parsed.</param>
        public void ParseJSON(string jsonData_) {
            Godot.Collections.Dictionary result = (Godot.Collections.Dictionary)JSON.Parse(jsonData_).Result;
            #if (__DEBUG)
                GD.Print("ParseJSON got string: " + result.ToString());
            #endif

            string[] keys = result.Keys.Cast<string>().ToArray<string>();
            Unit[] values = result.Values.Cast<Unit>().ToArray<Unit>();
            for (int index = 0; index < keys.Length; index++) {
                #if (__DEBUG)
                    GD.Print("Found key " + keys[index]);
                    GD.Print("Found value " + values[index]);
                #endif

                string[] settings = this.Data.Keys.ToArray<string>();
                for (int item = 0; item < settings.Length; item++) {
                    if (settings[item] == keys[index]) {
                        this.Data[settings[item]] = values[index];
                        break;
                    }
                }
            }
        }
    }
}
