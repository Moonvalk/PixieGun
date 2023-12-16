using Godot;
using Moonvalk.Accessory;
using Moonvalk.Animation;

namespace Moonvalk.Components.UI {
	/// <summary>
	/// Handler for displaying a progress bar which moves a texture to display percentage.
	/// </summary>
	public class MoonProgressBar : TextureRect {
		#region Data Fields
		/// <summary>
		/// Path to the texture node for the front progress image.
		/// </summary>
		[Export] protected NodePath p_progressFront { get; set; }

		/// <summary>
		/// Path to the label node for the progress percentage display.
		/// </summary>
		[Export] protected NodePath p_progressLabel { get; set; }

		/// <summary>
		/// An array of colors that will be displayed on the bar in order of escalating percentage.
		/// </summary>
		[Export] public Godot.Vector3[] Colors { get; protected set; }

		/// <summary>
		/// A multiplier that will be applied to the offset on the progress bar.
		/// </summary>
		[Export] public float BarOffsetPercentage { get; protected set; } = 0.5f;

		/// <summary>
		/// Stores reference to the texture image for displaying progress.
		/// </summary>
		public TextureRect ProgressFront { get; protected set; }

		/// <summary>
		/// Stores the original position of the progress bar to offset from.
		/// </summary>
		protected Godot.Vector2 _originalProgressPosition { get; set; }

		/// <summary>
		/// Stores the label used to display percentage.
		/// </summary>
		public Label ProgressLabel { get; protected set; }

		/// <summary>
		/// Stores the current progress applied.
		/// </summary>
		public float Progress { get; protected set; } = -1f;

		/// <summary>
		/// Stores the displayed progress value on the matching label.
		/// </summary>
		protected float _displayedProgress = 0f;

		/// <summary>
		/// Stores the current bar color applied to the progress bar.
		/// </summary>
		protected Godot.Vector3 _barColor = Godot.Vector3.One;

		/// <summary>
		/// Event emitted when progress changes on this element.
		/// </summary>
		/// <param name="value_"></param>
		[Signal] public delegate void OnProgressChange(float value_);
		#endregion
		
		#region Godot Events
		/// <summary>
		/// Called when this object is first initialized.
		/// </summary>
		public override void _Ready() {
			this.ProgressFront = this.GetNode<TextureRect>(p_progressFront);
			this.ProgressLabel = this.GetNode<Label>(p_progressLabel);

			this.ProgressFront.Material = this.ProgressFront.Material.Duplicate() as Material;
			this._originalProgressPosition = this.ProgressFront.RectPosition;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Sets the progress displayed on this element.
		/// </summary>
		/// <param name="percentage_">The new percentage value.</param>
		/// <param name="snap_">True if the value should be snapped to, false if the bar should animate.</param>
		public void SetProgress(float percentage_, bool snap_ = false) {
			percentage_ = Mathf.Clamp(percentage_, 0f, 1f);
			if (this.Progress == percentage_) {
				return;
			}
			this.Progress = percentage_;
			this.EmitSignal(nameof(OnProgressChange), this.Progress);

			Godot.Vector2 target = this._originalProgressPosition + Godot.Vector2.Left * ((1f - this.Progress) *
				this.ProgressFront.RectSize * this.BarOffsetPercentage * this.ProgressFront.RectScale);
			if (snap_) {
				this.ProgressFront.RectPosition = target;
				this._displayedProgress = this.Progress;
				this.updateProgressLabel();
				this._barColor = this.getTargetColor();
				this.ProgressFront.Material.Set("shader_param/color", this._barColor);
				return;
			}
			this.animateProgress(target);
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Animates the progress bar offset to the target location.
		/// </summary>
		/// <param name="target_">The target offset location to animate towards.</param>
		protected void animateProgress(Godot.Vector2 target_) {
			this.ProgressFront.SpringMoveTo(target_, new MoonSpringParams() {
				Tension = 125f, Dampening = 6f,
			});
			MoonTween.CustomTweenTo<MoonTween>(() => ref this._displayedProgress, this.Progress, new MoonTweenParams() {
				Duration = 0.5f, EasingFunction = Easing.Cubic.Out,
			}).OnUpdate(this.updateProgressLabel);

			Ref<float>[] refs = new Ref<float>[3] { () => ref this._barColor.x, () => ref this._barColor.y, () => ref this._barColor.z };
			MoonTweenVec3.CustomTweenTo<MoonTweenVec3>(refs, this.getTargetColor(), new MoonTweenParams() {
				Duration = 0.5f, EasingFunction = Easing.Cubic.InOut,
			}).OnUpdate(() => {
				this.ProgressFront.Material.Set("shader_param/color", this._barColor);
			});
		}
		
		/// <summary>
		/// Updates the label displaying progress.
		/// </summary>
		protected void updateProgressLabel() {
			this.ProgressLabel.Text = Mathf.Round(this._displayedProgress * 100f) + "%";
		}

		/// <summary>
		/// Gets the target color based on current percentage.
		/// </summary>
		/// <returns>Returns a Vector3 representing an RGB color value applied to shaders.</returns>
		protected Godot.Vector3 getTargetColor() {
			return this.Colors[(int)Mathf.Clamp(Mathf.Floor(this.Progress * (this.Colors.Length - 0.2f)),
				0f, this.Colors.Length - 1f)];
		}
		#endregion
	}
}
