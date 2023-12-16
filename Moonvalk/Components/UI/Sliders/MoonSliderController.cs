using Godot;
using Moonvalk.UI;

namespace Moonvalk.Components.UI
{
	/// <summary>
	/// Base controller for a slider element that takes input to move up / down.
	/// </summary>
	public class MoonSliderController : Control
	{
		#region Data Fields
		/// <summary>
		/// Path to the button element for decrementing.
		/// </summary>
		[Export] protected NodePath p_buttonDown { get; set; }

		/// <summary>
		/// Path to the button element for incrementing.
		/// </summary>
		[Export] protected NodePath p_buttonUp { get; set; }

		/// <summary>
		/// Stores reference to the progress bar element.
		/// </summary>
		public MoonProgressBar ProgressBar { get; protected set; }

		/// <summary>
		/// Stores reference to the button for decrementing.
		/// </summary>
		public MoonButton ButtonDown { get; protected set; }

		/// <summary>
		/// Stores reference to the button for incrementing.
		/// </summary>
		public MoonButton ButtonUp { get; protected set; }

		/// <summary>
		/// Event which emits when the slider value has changed.
		/// </summary>
		/// <param name="slider_">Reference to the slider object.</param>
		/// <param name="value_">The new value stored on this slider.</param>
		[Signal] public delegate void OnSliderValueChange(MoonSliderController slider_, float value_);
		#endregion

		#region Godot Events
		/// <summary>
		/// Called when this object is first initialized.
		/// </summary>
		public override void _Ready()
		{
			ProgressBar = this.GetComponent<MoonProgressBar>();
			ButtonDown = GetNode<MoonButton>(p_buttonDown);
			ButtonUp = GetNode<MoonButton>(p_buttonUp);

			ButtonDown.Connect("pressed", this, nameof(handleButtonDownPress));
			ButtonUp.Connect("pressed", this, nameof(handleButtonUpPress));
			ProgressBar.Connect(nameof(MoonProgressBar.OnProgressChange), this, nameof(handleProgressBarValueChange));
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Handles a press on the down button.
		/// </summary>
		protected void handleButtonDownPress()
		{
			updateProgressBar(ProgressBar.Progress - 0.1f);
		}

		/// <summary>
		/// Handles a press on the up button.
		/// </summary>
		protected void handleButtonUpPress()
		{
			updateProgressBar(ProgressBar.Progress + 0.1f);
		}

		/// <summary>
		/// Called to update the progress bar with animation.
		/// </summary>
		/// <param name="percentage_">The new percentage to animate to.</param>
		protected void updateProgressBar(float percentage_)
		{
			ProgressBar.SetProgress(percentage_);
		}

		/// <summary>
		/// Handles emitting an event when the progress bar value has changed.
		/// </summary>
		/// <param name="value_">The value to share.</param>
		protected void handleProgressBarValueChange(float value_)
		{
			EmitSignal(nameof(OnSliderValueChange), this, value_);
		}
		#endregion
	}
}
