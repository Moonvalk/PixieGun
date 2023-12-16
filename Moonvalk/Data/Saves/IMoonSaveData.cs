
namespace Moonvalk.Data {
    /// <summary>
    /// Contract for save data to follow.
    /// </summary>
    public interface IMoonSaveData {
        /// <summary>
        /// Prints this container as a JSON storage string value.
        /// </summary>
        /// <returns>Returns a JSON string matching the data found within this container.</returns>
        string GetJSON();

        /// <summary>
        /// Called to parse a JSON string into usable data within this container.
        /// </summary>
        /// <param name="jsonData_">The JSON string to be parsed.</param>
        void ParseJSON(string jsonData_);
    }
}
