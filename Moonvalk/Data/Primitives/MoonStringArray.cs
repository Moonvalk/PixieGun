using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Data
{
    /// <summary>
    /// Contains an array of string values with corresponding titles.
    /// </summary>
    [RegisteredType(nameof(MoonStringArray), "", nameof(Resource))]
    public class MoonStringArray : MoonValueArray<MoonString>
    {
        // ...
    }
}