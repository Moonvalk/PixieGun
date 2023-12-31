using Moonvalk.Resources;

namespace Moonvalk.Data
{
    /// <summary>
    /// Pairs a string name with an integer value.
    /// </summary>
    [RegisteredType(nameof(MoonInt))]
    public class MoonInt : MoonValue<int>
    {
        // ...
    }
}