using Godot;

namespace Moonvalk.Accessory
{
    /// <summary>
    /// Static class containing helper functions for handling SceneTreeTweens in Godot.
    /// </summary>
    public static class SceneTreeTweenHelpers
    {
        /// <summary>
        /// Called to nullify a SceneTreeTween.
        /// </summary>
        /// <param name="tween_">The SceneTreeTween to be stopped and nullified.</param>
        public static void CancelTween(ref SceneTreeTween tween_)
        {
            if (tween_ == null) return;

            tween_.Stop();
            tween_ = null;
        }

        /// <summary>
        /// Initializes a new SceneTreeTween in place of the provided reference.
        /// This ensures that an existing tween is stopped and nullified before creating a new one.
        /// </summary>
        /// <param name="node_">The object used to create/manage Tweens.</param>
        /// <param name="tween_">The Tween reference to be reinitialized.</param>
        /// <param name="transition_">Optional transition type.</param>
        /// <param name="ease_">Optional easing type.</param>
        /// <returns>Returns the original Tween reference holding a newly created SceneTreeTween.</returns>
        public static SceneTreeTween InitTween(this Node node_, ref SceneTreeTween tween_, Tween.TransitionType transition_ = Tween.TransitionType.Cubic,
            Tween.EaseType ease_ = Tween.EaseType.InOut)
        {
            CancelTween(ref tween_);
            tween_ = node_.CreateTween().SetTrans(transition_).SetEase(ease_);

            return tween_;
        }
    }
}