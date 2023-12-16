using System;
using Godot;
using Moonvalk.Nodes;

namespace Moonvalk.Components.UI
{
	/// <summary>
	/// Controller for handling transition animations between scene swaps.
	/// </summary>
	public class MoonTransitionController : Control
	{
		#region Data Fields
		/// <summary>
		/// A prefab to be instantiated as a transition animation.
		/// </summary>
		[Export] protected PackedScene prefab_Transition { get; set; }

		/// <summary>
		/// Path to the spinner node.
		/// </summary>
		[Export] protected NodePath p_spinner { get; set; }

		/// <summary>
		/// Stores reference to the spinner node used to play animations during loading.
		/// </summary>
		public MoonSceneLoadSpinner Spinner { get; protected set; }

		/// <summary>
		/// Stores reference to the transition object used to play transition animations between scene swaps.
		/// </summary>
		public BaseMoonTransition Transition { get; protected set; }
		#endregion

		#region Godot Events
		/// <summary>
		/// Called when this object is first initialized.
		/// </summary>
		public override void _Ready() {
			Transition = this.AddInstance<BaseMoonTransition>(prefab_Transition);
			MoveChild(Transition, 0);

			Spinner = GetNode<MoonSceneLoadSpinner>(p_spinner);
			Transition.SnapState(MoonTransitionState.Covered);
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Plays the transition animation intro.
		/// </summary>
		/// <param name="onCovered_">An optional action to be executed on completion.</param>
		public void PlayTransitionIntro(Action onCovered_ = null) {
			Transition.Events.AddAction(MoonTransitionState.Covered, () =>
			{
				Spinner.Play();
				onCovered_?.Invoke();
			});
			
			Transition.PlayIntro();
		}

		/// <summary>
		/// Plays the transition animation outro.
		/// </summary>
		/// <param name="onComplete_">An optional action to be executed on completion.</param>
		public void PlayTransitionOutro(Action onComplete_ = null) {
			Transition.Events.AddAction(MoonTransitionState.Complete, onComplete_);
			Spinner.Stop();
			Transition.PlayOutro();
		}
		#endregion
	}
}
