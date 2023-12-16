using System;
using Godot;
using Moonvalk.Animation;
using Moonvalk.Audio;
using Moonvalk.Utilities;

namespace Moonvalk.Components.UI {
	/// <summary>
	/// Base class for a transition element played between scene changes.
	/// </summary>
	public class BaseMoonTransition : Panel {
		#region Data Fields
		/// <summary>
		/// Properties that change how the transition will animate.
		/// </summary>
		[Export] public MoonTweenParams TransitionParams { get; protected set; }

		/// <summary>
		/// Path to the sound queue for the transition intro.
		/// </summary>
		[Export] protected NodePath p_audioEnter { get; set; }

		/// <summary>
		/// Path to the sound queue for the transition outro.
		/// </summary>
		[Export] protected NodePath p_audioExit { get; set; }

		/// <summary>
		/// Sound queue to be played when the transition intro is played.
		/// </summary>
		public SoundQueue AudioEnter { get; protected set; }

		/// <summary>
		/// Sound queue to be played when the transition outro is played.
		/// </summary>
		public SoundQueue AudioExit { get; protected set; }

		/// <summary>
		/// Stores the current state of this transition element.
		/// </summary>
		public MoonTransitionState CurrentState { get; protected set; } = MoonTransitionState.Idle;

		/// <summary>
		/// A map of events based on the state of this object which other objects can add actions to.
		/// </summary>
		public MoonActionMap<MoonTransitionState> Events { get; protected set; } = new MoonActionMap<MoonTransitionState>();

		/// <summary>
		/// The current progress of this transition animation.
		/// </summary>
		protected float _progress = 0f;

		/// <summary>
		/// Gets the current progress of this transition.
		/// </summary>
		public float Progress {
			get {
				return this._progress;
			}
		}
		#endregion

		#region Godot Events
		/// <summary>
		/// Called when this object is first initialized.
		/// </summary>
		public override void _Ready() {
			this.AudioEnter = this.GetNode<SoundQueue>(p_audioEnter);
			this.AudioExit = this.GetNode<SoundQueue>(p_audioExit);
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Plays the intro animation.
		/// </summary>
		public void PlayIntro() {
			this.AudioEnter.PlaySound();
			this.setState(MoonTransitionState.Intro);
			this.animateProgress(1f, () => {
				this.setState(MoonTransitionState.Covered);
			});
		}

		/// <summary>
		/// Plays the outro animation.
		/// </summary>
		public void PlayOutro() {
			this.AudioExit.PlaySound();
			this.setState(MoonTransitionState.Outro);
			this.animateProgress(-1f, () => {
				this.setState(MoonTransitionState.Complete);
			});
		}

		/// <summary>
		/// Called to snap this transition to the specified state.
		/// </summary>
		/// <param name="state_">The state to snap to.</param>
		public void SnapState(MoonTransitionState state_) {
			switch (state_) {
				case MoonTransitionState.Covered:
					this._progress = 0f;
					this.Material.Set("shader_param/direction", -1f);
					break;
			}
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Called to set the progress value on the transition shader.
		/// </summary>
		protected void setProgress() {
			this.Material.Set("shader_param/progress", this._progress);
		}

		/// <summary>
		/// Handles playing the animation on progress and updating the shader material.
		/// </summary>
		/// <param name="onComplete_">An action that will be invoked on completion.</param>
		protected void animateProgress(float direction_, Action onComplete_ = null) {
			this._progress = 0f;
			MoonTween.CustomTweenTo<MoonTween>(() => ref this._progress, 1f, this.TransitionParams, false)
				.OnComplete(onComplete_).OnStart(() => {
					this.Material.Set("shader_param/direction", direction_);
				}).OnUpdate(this.setProgress).Start();
		}

		/// <summary>
		/// Sets the state of this object and emits it to listeners.
		/// </summary>
		/// <param name="state_">The new state to be set.</param>
		protected void setState(MoonTransitionState state_) {
			this.CurrentState = state_;
			this.Events.Run(this.CurrentState, true);
			if (this.CurrentState == MoonTransitionState.Complete) {
				this.setState(MoonTransitionState.Idle);
			}
		}
		#endregion
	}
}
