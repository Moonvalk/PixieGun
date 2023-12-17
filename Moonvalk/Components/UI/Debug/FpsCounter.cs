using Godot;

namespace Moonvalk.Debug
{
    /// <summary>
    /// Updates a Label node each frame with the current FPS.
    /// </summary>
    public class FpsCounter : Label
    {
        /// <summary>
        /// Called each game tick.
        /// </summary>
        /// <param name="delta_">The time elapsed between current and last frame.</param>
        public override void _Process(float delta_)
        {
            Text = "FPS: " + Engine.GetFramesPerSecond();
        }
    }
}