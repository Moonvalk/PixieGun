using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Animation
{
    /// <summary>
    /// Base class for animating textures on mesh instances.
    /// </summary>
    [RegisteredType(nameof(TextureAnimation))]
    public class TextureAnimation : BaseTextureAnimation<Texture>
    {
        // ...
    }
}