using System;
using Godot;
using Moonvalk.Animation;
using Moonvalk.Audio;
using Moonvalk.Utilities;

namespace Moonvalk.Components
{
	public class BaseSceneTransition : Control
	{
		/// <summary>
		/// All states available to a screen transition.
		/// </summary>
		public enum TransitionState
		{
			Idle,
			Intro,
			Covered,
			Outro,
			Complete,
		}
		
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
		public TransitionState CurrentState { get; protected set; } = TransitionState.Idle;

		/// <summary>
		/// A map of events based on the state of this object which other objects can add actions to.
		/// </summary>
		public MoonActionMap<TransitionState> Events { get; protected set; } = new MoonActionMap<TransitionState>();

		/// <summary>
		/// The current progress of this transition animation.
		/// </summary>
		protected float _progress;

		/// <summary>
		/// Gets the current progress of this transition.
		/// </summary>
		public float Progress => _progress;
		#endregion

		#region Public Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="onComplete_"></param>
		public void PlayIntro(Action onComplete_)
		{
			if (CurrentState == TransitionState.Covered) return;
			
			Events.AddAction(TransitionState.Covered, onComplete_);
			AudioEnter.PlaySound();
			setState(TransitionState.Intro);
			animateProgress(1f, () =>
			{
				setState(TransitionState.Covered);
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="onComplete_"></param>
		public void PlayOutro(Action onComplete_)
		{
			if (CurrentState == TransitionState.Complete) return;

			Events.AddAction(TransitionState.Complete, onComplete_);
			AudioEnter.PlaySound();
			setState(TransitionState.Outro);
			animateProgress(-1f, () =>
			{
				setState(TransitionState.Complete);
			});
		}

		/// <summary>
		/// Called to snap this transition to the specified state.
		/// </summary>
		/// <param name="state_">The state to snap to.</param>
		public void SnapStateTo(TransitionState state_)
		{
			switch (state_)
			{
				case TransitionState.Covered:
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
		protected void setProgress()
		{
			Material.Set("shader_param/progress", _progress);
		}

		/// <summary>
		/// Handles playing the animation on progress and updating the shader material.
		/// </summary>
		/// <param name="onComplete_">An action that will be invoked on completion.</param>
		protected void animateProgress(float direction_, Action onComplete_ = null)
		{
			_progress = 0f;
			MoonTween.CustomTweenTo<MoonTween>(() => ref _progress, 1f, TransitionParams, false)
				.OnComplete(onComplete_)
				.OnStart(() =>
				{
					Material.Set("shader_param/direction", direction_);
				})
				.OnUpdate(setProgress)
				.Start();
		}

		/// <summary>
		/// Sets the state of this object and emits it to listeners.
		/// </summary>
		/// <param name="state_">The new state to be set.</param>
		protected void setState(TransitionState state_)
		{
			CurrentState = state_;
			Events.Run(CurrentState, true);
			
			if (CurrentState == TransitionState.Complete) setState(TransitionState.Idle);
		}
		#endregion
	}
}