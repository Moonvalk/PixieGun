using System.Collections.Generic;
using Godot;
using Moonvalk.Nodes;
using Moonvalk.Utilities;

namespace Moonvalk.Animation
{
    /// <summary>
    /// Handler for animating textures on a mesh instance.
    /// </summary>
    /// <typeparam name="AnimationType">The type of animation expected to be animated.</typeparam>
    /// <typeparam name="FrameType">The type of frame data expected.</typeparam>
    public abstract class BaseTextureAnimator<AnimationType, FrameType> : Node
        where AnimationType : BaseTextureAnimation<FrameType>
    {
        #region Godot Events
        /// <summary>
        /// Called when this object is first initialized.
        /// </summary>
        public override void _Ready()
        {
            Mesh = GetNode<MeshInstance>(PMeshInstance);
            if (Animations.Count > 0)
            {
                CurrentAnimation = Animations[0];
                Play("walk");
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Adjusts the texture on the stored mesh instance.
        /// </summary>
        protected abstract void AdjustTexture();
        #endregion

        #region Data Fields
        /// <summary>
        /// Path to the mesh instance that will have its texture adjusted.
        /// </summary>
        [Export] protected NodePath PMeshInstance { get; set; }

        /// <summary>
        /// Stores references to all available animations.
        /// </summary>
        [Export] public List<AnimationType> Animations { get; protected set; }

        /// <summary>
        /// Stores the current state of this animator.
        /// </summary>
        public TextureAnimatorState CurrentState { get; protected set; } = TextureAnimatorState.Stopped;

        /// <summary>
        /// Reference to the mesh instance that will have its texture adjusted.
        /// </summary>
        public MeshInstance Mesh { get; protected set; }

        /// <summary>
        /// Stores the current animation being played.
        /// </summary>
        public AnimationType CurrentAnimation { get; protected set; }

        /// <summary>
        /// Stores the index of the current frame.
        /// </summary>
        public int CurrentFrame { get; protected set; }

        /// <summary>
        /// </summary>
        protected MoonTimer _timer;
        #endregion

        #region Public Methods
        /// <summary>
        /// Called to play the current animation.
        /// </summary>
        public void Play(string animationName_ = null)
        {
            if (animationName_ != null)
            {
                var formattedName = animationName_.ToLower();
                for (var index = 0; index < Animations.Count; index++)
                    if (Animations[index].Name.ToLower() == formattedName)
                    {
                        CurrentAnimation = Animations[index];
                        break;
                    }
            }

            CurrentState = TextureAnimatorState.Playing;
            CurrentFrame = CurrentAnimation.Frames.Count - 1;
            NextFrame();
        }

        /// <summary>
        /// Called to jump to the next available frame.
        /// </summary>
        public void NextFrame()
        {
            if (CurrentState == TextureAnimatorState.Stopped) return;

            CurrentFrame++;
            if (CurrentFrame > CurrentAnimation.Frames.Count - 1) CurrentFrame = 0;

            if (!Mesh.Validate())
            {
                Stop();
                return;
            }

            AdjustTexture();
            if (_timer == null) _timer = new MoonTimer(NextFrame);

            _timer.Start(CurrentAnimation.FrameDurations[CurrentFrame]);
        }

        /// <summary>
        /// Called to stop the current animation.
        /// </summary>
        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }

            CurrentState = TextureAnimatorState.Stopped;
        }
        #endregion
    }
}