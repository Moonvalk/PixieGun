using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Data
{
    /// <summary>
    /// A resource that pairs a name with a PackedScene object that can be instantiated and casted.
    /// </summary>
    [RegisteredType(nameof(MoonPackedScene))]
    public class MoonPackedScene : MoonValue<PackedScene>
    {
        // ...
    }
}