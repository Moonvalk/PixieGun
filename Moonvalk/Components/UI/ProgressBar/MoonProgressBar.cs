using Godot;
using Moonvalk.Accessory;
using Moonvalk.Animation;

namespace Moonvalk.Components.UI
{
    /// <summary>
    /// Handler for displaying a progress bar which moves a texture to display percentage.
    /// </summary>
    public class MoonProgressBar : TextureRect
    {
        #region Godot Events
        /// <summary>
        /// Called when this object is first initialized.
        /// </summary>
        public override void _Ready()
        {
            ProgressFront = GetNode<TextureRect>(PProgressFront);
            ProgressLabel = GetNode<Label>(PProgressLabel);

            ProgressFront.Material = ProgressFront.Material.Duplicate() as Material;
            OriginalProgressPosition = ProgressFront.RectPosition;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the progress displayed on this element.
        /// </summary>
        /// <param name="percentage_">The new percentage value.</param>
        /// <param name="snap_">True if the value should be snapped to, false if the bar should animate.</param>
        public void SetProgress(float percentage_, bool snap_ = false)
        {
            percentage_ = Mathf.Clamp(percentage_, 0f, 1f);
            if (Progress == percentage_) return;

            Progress = percentage_;
            EmitSignal(nameof(OnProgressChange), Progress);

            var target = OriginalProgressPosition + Vector2.Left * ((1f - Progress) * ProgressFront.RectSize * BarOffsetPercentage * ProgressFront.RectScale);

            if (snap_)
            {
                ProgressFront.RectPosition = target;
                _displayedProgress = Progress;
                UpdateProgressLabel();
                _barColor = GetTargetColor();
                ProgressFront.Material.Set("shader_param/color", _barColor);
                return;
            }

            AnimateProgress(target);
        }
        #endregion

        #region Data Fields
        /// <summary>
        /// Path to the texture node for the front progress image.
        /// </summary>
        [Export] protected NodePath PProgressFront { get; set; }

        /// <summary>
        /// Path to the label node for the progress percentage display.
        /// </summary>
        [Export] protected NodePath PProgressLabel { get; set; }

        /// <summary>
        /// An array of colors that will be displayed on the bar in order of escalating percentage.
        /// </summary>
        [Export] public Vector3[] Colors { get; protected set; }

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
        protected Vector2 OriginalProgressPosition { get; set; }

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
        protected float _displayedProgress;

        /// <summary>
        /// Stores the current bar color applied to the progress bar.
        /// </summary>
        protected Vector3 _barColor = Vector3.One;

        /// <summary>
        /// Event emitted when progress changes on this element.
        /// </summary>
        /// <param name="value_"></param>
        [Signal]
        public delegate void OnProgressChange(float value_);
        #endregion

        #region Private Methods
        /// <summary>
        /// Animates the progress bar offset to the target location.
        /// </summary>
        /// <param name="target_">The target offset location to animate towards.</param>
        protected void AnimateProgress(Vector2 target_)
        {
            ProgressFront.SpringMoveTo(target_, new MoonSpringParams
            {
                Tension = 125f, Dampening = 6f
            });

            MoonTween.CustomTweenTo<MoonTween>(() => ref _displayedProgress, Progress, new MoonTweenParams
                {
                    Duration = 0.5f, EasingFunction = Easing.Cubic.Out
                })
                .OnUpdate(UpdateProgressLabel);

            var refs = new Ref<float>[3] { () => ref _barColor.x, () => ref _barColor.y, () => ref _barColor.z };
            MoonTweenVec3.CustomTweenTo<MoonTweenVec3>(refs, GetTargetColor(), new MoonTweenParams
                {
                    Duration = 0.5f, EasingFunction = Easing.Cubic.InOut
                })
                .OnUpdate(() => { ProgressFront.Material.Set("shader_param/color", _barColor); });
        }

        /// <summary>
        /// Updates the label displaying progress.
        /// </summary>
        protected void UpdateProgressLabel()
        {
            ProgressLabel.Text = Mathf.Round(_displayedProgress * 100f) + "%";
        }

        /// <summary>
        /// Gets the target color based on current percentage.
        /// </summary>
        /// <returns>Returns a Vector3 representing an RGB color value applied to shaders.</returns>
        protected Vector3 GetTargetColor()
        {
            return Colors[(int)Mathf.Clamp(Mathf.Floor(Progress * (Colors.Length - 0.2f)), 0f, Colors.Length - 1f)];
        }
        #endregion
    }
}