using System;
using Godot;
using Moonvalk.Data;
using Moonvalk.Nodes;
using Moonvalk.Resources;

namespace Moonvalk.Components
{
    public class BaseSceneLoadAnimator : Control
    {
        #region Godot Events
        /// <summary>
        /// Called when this object is first initialized.
        /// </summary>
        public override void _Ready()
        {
            Visible = false;
            MoonResourceLoader.Load(PathAnimation.AsPath, (PackedScene scene_) => { Animation = this.AddInstance<BaseSceneLoadAnimation>(scene_); });

            Play();
        }
        #endregion

        #region Data Fields
        /// <summary>
        /// Stores reference to the current animation.
        /// </summary>
        [Export] protected MoonString PathAnimation { get; set; }

        /// <summary>
        /// Stores reference to the current animation.
        /// </summary>
        public BaseSceneLoadAnimation Animation { get; protected set; }

        /// <summary>
        /// Flag that determines if the animation is actively playing.
        /// </summary>
        public bool IsPlaying { get; protected set; }
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