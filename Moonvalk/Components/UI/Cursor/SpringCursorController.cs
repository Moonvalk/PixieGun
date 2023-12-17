using System;
using Godot;
using Moonvalk.Animation;
using Moonvalk.Components;

namespace Moonvalk.Nodes
{
    /// <summary>
    /// Handles displaying a custom cursor that follows mouse position with the use of spring animations.
    /// </summary>
    public class SpringCursorController : Node
    {
        #region Data Fields
        /// <summary>
        /// Parameters used to dictate how the cursor will move.
        /// </summary>
        [Export] public MoonSpringParams MoveParameters { get; protected set; }

        /// <summary>
        /// Reference to the cursor display node.
        /// </summary>
        public Control Cursor { get; protected set; }

        /// <summary>
        /// A spring used to handle moving the cursor on screen. This reference is local to this controller
        /// as we want to continue applying previous forces rather than snapping immediately.
        /// </summary>
        public MoonSpringVec2 MovementSpring { get; protected set; }

        /// <summary>
        /// The target position to move towards that will update to new mouse positions.
        /// </summary>
        public Vector2 TargetPosition { get; protected set; }

        /// <summary>
        /// The current position which we will animate and snap the Cursor to each frame.
        /// </summary>
        protected Vector2 _currentPosition;
        
        /// <summary>
        /// A tolerance value used to check floating point comparisons when determining if the mouse is targeted properly.
        /// </summary>
        private const float MousePositionCheckTolerance = 0.001f;
        #endregion

        #region Godot Events
        /// <summary>
        /// Called when this object is first initialized.
        /// </summary>
        public override void _Ready()
        {
            Cursor = this.GetComponent<TextureRect>();

            Input.MouseMode = Input.MouseModeEnum.Hidden;
            TargetPosition = _currentPosition = GetTree().Root.GetMousePosition();

            Cursor.RectPosition = TargetPosition;

            MovementSpring = new MoonSpringVec2(() => ref _currentPosition.x, () => ref _currentPosition.y) { StartOnTargetAssigned = true };

            MovementSpring.SetParameters(MoveParameters ?? new MoonSpringParams())
                .OnUpdate(() => { Cursor.RectPosition = _currentPosition - Cursor.RectPivotOffset; });
        }

        /// <summary>
        /// Called each game tick.
        /// </summary>
        /// <param name="delta_">The time elapsed between last and current frame.</param>
        public override void _Process(float delta_)
        {
            var mouse = GetTree().Root.GetMousePosition();

            if (!(Math.Abs(TargetPosition.x - mouse.x) > MousePositionCheckTolerance) &&
                !(Math.Abs(TargetPosition.y - mouse.y) > MousePositionCheckTolerance)) return;

            TargetPosition = mouse;
            MovementSpring.To(TargetPosition);
        }
        #endregion
    }
}