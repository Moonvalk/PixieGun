using System;
using Godot;
using Moonvalk.Animation;
using Moonvalk.Audio;
using Moonvalk.Utilities;

namespace Moonvalk.Components.UI
{
	/// <summary>
	/// Base class for a transition element played between scene changes.
	/// </summary>
	public class BaseMoonTransition : Panel
	{
		#region Data Fields
		/// <summary>
		/// Properties that change how the transition will animate.
		/// </summary>
		[Export] public MoonTweenParams TransitionParams { get; protected set; }

		/// <summary>
		/// Path to the sound queue for the transition intro.
		/// </summary>
		[Export] protected NodePath PAudioEnter { get; set; }

		/// <summary>
		/// Path to the sound queue for the transition outro.
		/// </summary>
		[Export] protected NodePath PAudioExit { get; set; }

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
		protected float _progress;

		/// <summary>
		/// Gets the current progress of this transition.
		/// </summary>
		public float Progress => _progress;
		#endregion

		#region Godot Events
		/// <summary>
		/// Called when this object is first initialized.
		/// </summary>
		public override void _Ready()
		{
			AudioEnter = GetNode<SoundQueue>(PAudioEnter);
			AudioExit = GetNode<SoundQueue>(PAudioExit);
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Plays the intro animation.
		/// </summary>
		public void PlayIntro()
		{
			AudioEnter.PlaySound();
			SetState(MoonTransitionState.Intro);
			AnimateProgress(1f, () =>
			{
				SetState(MoonTransitionState.Covered);
			});
		}

		/// <summary>
		/// Plays the outro animation.
		/// </summary>
		public void PlayOutro()
		{
			AudioExit.PlaySound();
			SetState(MoonTransitionState.Outro);
			AnimateProgress(-1f, () =>
			{
				SetState(MoonTransitionState.Complete);
			});
		}

		/// <summary>
		/// Called to snap this transition to the specified state.
		/// </summary>
		/// <param name="state_">The state to snap to.</param>
		public void SnapState(MoonTransitionState state_)
		{
			switch (state_)
			{
				case MoonTransitionState.Covered:
					_progress = 0f;
					Material.Set("shader_param/direction", -1f);
					break;
			}
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Called to set the progress value on the transition shader.
		/// </summary>
		protected void SetProgress()
		{
			Material.Set("shader_param/progress", _progress);
		}

		/// <summary>
		/// Handles playing the animation on progress and updating the shader material.
		/// </summary>
		/// <param name="onComplete_">An action that will be invoked on completion.</param>
		protected void AnimateProgress(float direction_, Action onComplete_ = null)
		{
			_progress = 0f;
			MoonTween.CustomTweenTo<MoonTween>(() => ref _progress, 1f, TransitionParams, false)
				.OnComplete(onComplete_).OnStart(() =>
				{
					Material.Set("shader_param/direction", direction_);
				})
				.OnUpdate(SetProgress)
				.Start();
		}

		/// <summary>
		/// Sets the state of this object and emits it to listeners.
		/// </summary>
		/// <param name="state_">The new state to be set.</param>
		protected void SetState(MoonTransitionState state_)
		{
			CurrentState = state_;
			Events.Run(CurrentState, true);
			
			if (CurrentState == MoonTransitionState.Complete) SetState(MoonTransitionState.Idle);
		}
		#endregion
	}
}
