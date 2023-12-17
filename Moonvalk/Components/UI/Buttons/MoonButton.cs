using Godot;
using Godot.Collections;
using Moonvalk.Animation;

namespace Moonvalk.UI
{
	/// <summary>
	/// Base class for an extended button with hover animations.
	/// </summary>
	public class MoonButton : Button
	{
		#region Data Fields
		/// <summary>
		/// Stores the path to the container element.
		/// </summary>
		[Export] protected NodePath PContainer { get; set; }

		/// <summary>
		/// The scale used when hovering this button element.
		/// </summary>
		[Export] public float HoveredScale { get; protected set; } = 1.2f;

		/// <summary>
		/// Stores reference to the container element.
		/// </summary>
		public TextureRect Container { get; protected set; }

		/// <summary>
		/// Flag that determines if this button is focused.
		/// </summary>
		public bool IsFocused { get; protected set; }

		/// <summary>
		/// Signal that is emitted once focus has entered on this element.
		/// </summary>
		[Signal] public delegate void OnFocusEnter();

		/// <summary>
		/// Signal that is emitted once focus has exited on this element.
		/// </summary>
		[Signal] public delegate void OnFocusExit();
		#endregion

		#region Godot Events
		/// <summary>
		/// Called when this object is first initialized.
		/// </summary>
		public override void _Ready()
		{
			Container = GetNode<TextureRect>(PContainer);
			this.CenterPivot();
			Container.CenterPivot();

			Connect("pressed", this, nameof(HandlePress));
			Connect("mouse_entered", this, nameof(HandleChangeFocus), new Array { true });
			Connect("focus_entered", this, nameof(HandleChangeFocus), new Array { true });
			Connect("focus_exited", this, nameof(HandleChangeFocus), new Array { false });
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Handles updating the focused state of this button when an event occurs.
		/// </summary>
		/// <param name="isFocused_">Flag that determines if this button is currently focused or not.</param>
		protected void HandleChangeFocus(bool isFocused_)
		{
			if (IsFocused == isFocused_) return;
			
			IsFocused = isFocused_;
			if (IsFocused)
			{
				GrabFocus();
				Container.ScaleTo(Vector2.One * HoveredScale, new MoonTweenParams { Duration = 0.5f, EasingType = Easing.Types.ElasticOut });
				Container.ColorTo(new Color(1.1f, 1.1f, 1.1f), new MoonTweenParams { Duration = 0.25f });
			}
			else
			{
				Container.ScaleTo(Vector2.One, new MoonTweenParams { Duration = 0.25f });
				Container.ColorTo(new Color(0.9f, 0.9f, 0.9f), new MoonTweenParams { Duration = 0.25f });
			}
			
			EmitSignal(IsFocused ? nameof(OnFocusEnter) : nameof(OnFocusExit));
		}

		/// <summary>
		/// Called to handle press animations when this button is pressed.
		/// </summary>
		protected void HandlePress()
		{
			IsFocused = true;
			Container.RectScale = Vector2.One * HoveredScale;
			Container.ScaleTo(Vector2.One * 0.9f, new MoonTweenParams
			{
				Duration = 0.5f, EasingFunction = Easing.Elastic.Out,
			}).Then(() =>
			{
				HandleChangeFocus(false);
			});
		}
		#endregion
	}
}
