using System;
using Godot;

namespace Moonvalk.Components.UI
{
	/// <summary>
	/// Base class for a scene load spinner which will animate during transitions.
	/// </summary>
	public class MoonSceneLoadSpinner : Control
	{
		#region Data Fields
		/// <summary>
		/// Flag that determines if the animation is actively playing.
		/// </summary>
		public bool IsPlaying { get; protected set; }
		#endregion

		#region Godot Events
		/// <summary>
		/// Called when this object is first initialized.
		/// </summary>
		public override void _Ready()
		{
			Visible = false;
			Play();
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Called to play the animation loop on this spinner object.
		/// </summary>
		public virtual void Play()
		{
			IsPlaying = true;
			Visible = true;
		}

		/// <summary>
		/// Called to stop the animation loop on this spinner object.
		/// </summary>
		/// <param name="onComplete_">An optional action that will be completed when done.</param>
		public virtual void Stop(Action onComplete_ = null)
		{
			IsPlaying = false;
			onComplete_?.Invoke();
		}
		#endregion
	}
}
