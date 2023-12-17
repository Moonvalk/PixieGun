// #define __DEBUG

using System.Linq;
using Godot;
using Godot.Collections;

namespace Moonvalk.Data
{
    /// <summary>
    /// Base class for save data of generic type that can be stored / parsed from JSON.
    /// </summary>
    /// <typeparam name="Unit">The unit type that is expected to be stored here.</typeparam>
    public class BaseMoonSaveData<Unit> : IMoonSaveData
    {
        /// <summary>
        /// Default constructor for a new save data container.
        /// </summary>
        public BaseMoonSaveData()
        {
            Data = new System.Collections.Generic.Dictionary<string, Unit>();
        }

        /// <summary>
        /// A dictionary of all save data accessible by string names.
        /// </summary>
        public System.Collections.Generic.Dictionary<string, Unit> Data { get; protected set; }

        /// <summary>
        /// Prints this container as a JSON storage string value.
        /// </summary>
        /// <returns>Returns a JSON string matching the data found within this container.</returns>
        public string GetJson()
        {
            var formattedData = JSON.Print(Data);
            return formattedData;
        }

        /// <summary>
        /// Called to parse a JSON string into usable data within this container.
        /// </summary>
        /// <param name="jsonData_">The JSON string to be parsed.</param>
        public void ParseJson(string jsonData_)
        {
            var result = (Dictionary)JSON.Parse(jsonData_).Result;
#if (__DEBUG)
                GD.Print("ParseJSON got string: " + result.ToString());
#endif

            var keys = result.Keys.Cast<string>().ToArray();

            var values = result.Values.Cast<Unit>().ToArray();

            for (var index = 0; index < keys.Length; index++)
            {
#if (__DEBUG)
                    GD.Print("Found key " + keys[index]);
                    GD.Print("Found value " + values[index]);
#endif

                var settings = Data.Keys.ToArray();
                for (var item = 0; item < settings.Length; item++)
                    if (settings[item] == keys[index])
                    {
                        Data[settings[item]] = values[index];
                        break;
                    }
            }
        }

        /// <summary>
        /// Called to set values within this save data by string / unit pairing.
        /// </summary>
        /// <param name="pairs_">All pairs of data to be set within this object.</param>
        public BaseMoonSaveData<Unit> SetValue(params (string name_, Unit value_)[] pairs_)
        {
            foreach (var pair in pairs_)
            {
                var formattedKey = pair.name_.ToLower();

                if (Data.ContainsKey(formattedKey))
                {
                    Data[formattedKey] = pair.value_;
                    continue;
                }

                Data.Add(formattedKey, pair.value_);
            }

            return this;
        }

        /// <summary>
        /// Called to get the unit value for the matching data name, if it exists.
        /// </summary>
        /// <param name="name_">The save data name to find a unit value for.</param>
        /// <returns>Returns the value stored for the requested data name.</returns>
        public Unit GetValue(string name_)
        {
            var formattedKey = name_.ToLower();
            if (Data.TryGetValue(formattedKey, out var value)) return value;

            return default;
        }
    }
}