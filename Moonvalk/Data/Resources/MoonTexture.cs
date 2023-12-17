using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Data
{
    /// <summary>
    /// Pairs a string name with a Texture value.
    /// </summary>
    [RegisteredType(nameof(MoonTexture))]
    public class MoonTexture : MoonValue<Texture>
    {
        // ...
    }
}