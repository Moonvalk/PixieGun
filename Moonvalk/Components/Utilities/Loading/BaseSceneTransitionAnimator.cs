using System;
using Godot;

namespace Moonvalk.Components {
	public class BaseSceneTransitionAnimator : Node {
		[Export] public BaseSceneTransition Transition { get; protected set; }
		[Export] public bool StartCovered { get; protected set; } = false;

		public override void _Ready() {
			if (this.StartCovered) {
				this.Transition.SnapStateTo(BaseSceneTransition.TransitionState.Covered);
			}
		}

		public void PlayIntro(Action onComplete_) {
			this.Transition.PlayIntro(onComplete_);
		}

		public void PlayOutro(Action onComplete_) {
			this.Transition.PlayOutro(onComplete_);
		}
	}
}