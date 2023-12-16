using System;
using Godot;

namespace Moonvalk.Components
{
	public abstract class BaseSceneLoadAnimation : Control
	{
		#region Data Fields
		#endregion

		#region Godot Events
		/// <summary>
		/// Called when this object is first initialized.
		/// </summary>
		public override void _Ready() {
			Visible = false;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Called to play the animation loop on this spinner object.
		/// </summary>
		public abstract void Play();

		/// <summary>
		/// Called to stop the animation loop on this spinner object.
		/// </summary>
		/// <param name="onComplete_">An optional action that will be completed when done.</param>
		public abstract void Stop(Action onComplete_ = null);
		#endregion
	}
}
