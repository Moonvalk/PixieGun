using System;
using Godot;

namespace Moonvalk.Components
{
    public class BaseSceneTransitionAnimator : Node
    {
        [Export] public BaseSceneTransition Transition { get; protected set; }
        [Export] public bool StartCovered { get; protected set; }

        public override void _Ready()
        {
            if (StartCovered) Transition.SnapStateTo(BaseSceneTransition.TransitionState.Covered);
        }

        public void PlayIntro(Action onComplete_)
        {
            Transition.PlayIntro(onComplete_);
        }

        public void PlayOutro(Action onComplete_)
        {
            Transition.PlayOutro(onComplete_);
        }
    }
}