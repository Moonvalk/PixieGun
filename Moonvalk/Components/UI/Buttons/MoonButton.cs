using Godot;
using Godot.Collections;
using Moonvalk.Animation;

namespace Moonvalk.UI
{
	/// <summary>
	/// Base class for an extended button with hover animations.
	/// </summary>
	public class MoonButton : Button {
		#region Data Fields
		/// <summary>
		/// Stores the path to the container element.
		/// </summary>
		[Export] protected NodePath p_container { get; set; }

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
		public bool IsFocused { get; protected set; } = false;

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
		public override void _Ready() {
			this.Container = this.GetNode<TextureRect>(p_container);
			this.CenterPivot();
			this.Container.CenterPivot();

			this.Connect("pressed", this, nameof(this.handlePress));
			this.Connect("mouse_entered", this, nameof(this.handleChangeFocus), new Array() { true });
			this.Connect("focus_entered", this, nameof(this.handleChangeFocus), new Array() { true });
			this.Connect("focus_exited", this, nameof(this.handleChangeFocus), new Array() { false });
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Handles updating the focused state of this button when an event occurs.
		/// </summary>
		/// <param name="isFocused_">Flag that determines if this button is currently focused or not.</param>
		protected void handleChangeFocus(bool isFocused_) {
			if (this.IsFocused == isFocused_) {
				return;
			}
			this.IsFocused = isFocused_;
			if (this.IsFocused) {
				this.GrabFocus();
				this.Container.ScaleTo(Vector2.One * this.HoveredScale, new MoonTweenParams() { Duration = 0.5f, EasingType = Easing.Types.ElasticOut });
				this.Container.ColorTo(new Color(1.1f, 1.1f, 1.1f), new MoonTweenParams() { Duration = 0.25f });
			} else {
				this.Container.ScaleTo(Vector2.One, new MoonTweenParams() { Duration = 0.25f });
				this.Container.ColorTo(new Color(0.9f, 0.9f, 0.9f), new MoonTweenParams() { Duration = 0.25f });
			}
			this.EmitSignal(this.IsFocused ? nameof(OnFocusEnter) : nameof(OnFocusExit));
		}

		/// <summary>
		/// Called to handle press animations when this button is pressed.
		/// </summary>
		protected void handlePress() {
			this.IsFocused = true;
			this.Container.RectScale = Vector2.One * this.HoveredScale;
			this.Container.ScaleTo(Vector2.One * 0.9f, new MoonTweenParams() {
				Duration = 0.5f, EasingFunction = Easing.Elastic.Out,
			}).Then(() => {
				this.handleChangeFocus(false);
			});
		}
		#endregion
	}
}
