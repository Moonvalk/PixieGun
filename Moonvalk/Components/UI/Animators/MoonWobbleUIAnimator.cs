using Godot;
using Moonvalk.Animation;

namespace Moonvalk.Components
{
	/// <summary>
	/// Animates a UI component with wobble animations on load.
	/// </summary>
	public class MoonWobbleUIAnimator : Control
	{
		/// <summary>
		/// The path to the UI element inherited from a Control node.
		/// </summary>
		[Export] protected NodePath p_uiElement { get; set; }

		/// <summary>
		/// Properties for the movement wobble animation.
		/// </summary>
		[Export] public MoonWobbleParams WobbleMoveParams { get; protected set; }

		/// <summary>
		/// The direction of the wobble movement animation.
		/// </summary>
		[Export] public Vector2 WobbleMoveDirection { get; protected set; } = Vector2.Up;

		/// <summary>
		/// Properties for the scaling wobble animation.
		/// </summary>
		[Export] public MoonWobbleParams WobbleScaleParams { get; protected set; }

		/// <summary>
		/// Properties for the rotation wobble animation.
		/// </summary>
		[Export] public MoonWobbleParams WobbleRotateParams { get; protected set; }

		/// <summary>
		/// Stores reference to the element that will be animated.
		/// </summary>
		public Control Element { get; protected set; }

		/// <summary>
		/// Called when this object is first initialized.
		/// </summary>
		public override void _Ready()
		{
			this.Element = this.GetNode<Control>(p_uiElement);
			this.Element.CenterPivot();
			this.Element.WobbleMove(this.WobbleMoveDirection, this.WobbleMoveParams).Start();
			this.Element.WobbleScale(Vector2.One, this.WobbleScaleParams).Start();
			this.Element.WobbleRotation(1f, this.WobbleRotateParams).Start();
		}
	}
}
