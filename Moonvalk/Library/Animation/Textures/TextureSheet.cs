using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Animation
{
    /// <summary>
    /// Container representing a texture sheet.
    /// </summary>
    [RegisteredType(nameof(TextureSheet))]
    public class TextureSheet : Resource
    {
        /// <summary>
        /// </summary>
        [Export] public Texture Texture { get; set; }

        /// <summary>
        /// </summary>
        [Export] public Vector2 FrameSize { get; set; } = new Vector2(24, 24);

        /// <summary>
        /// </summary>
        [Export] public int ColumnCount { get; set; } = 5;
    }
}