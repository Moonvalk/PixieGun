using System.Collections.Generic;
using Godot;
using Moonvalk.Nodes;
using Moonvalk.Utilities;

namespace Moonvalk.Animation {
	/// <summary>
	/// Handler for animating textures on a mesh instance.
	/// </summary>
	/// <typeparam name="AnimationType">The type of animation expected to be animated.</typeparam>
	/// <typeparam name="FrameType">The type of frame data expected.</typeparam>
	public abstract class BaseTextureAnimator<AnimationType, FrameType> : Node where AnimationType : BaseTextureAnimation<FrameType> {
		#region Data Fields
		/// <summary>
		/// Path to the mesh instance that will have its texture adjusted.
		/// </summary>
		[Export] protected NodePath p_meshInstance { get; set; }

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
		public int CurrentFrame { get; protected set; } = 0;

		/// <summary>
		/// 
		/// </summary>
		protected MoonTimer _timer;
		#endregion

		#region Godot Events
		/// <summary>
		/// Called when this object is first initialized.
		/// </summary>
		public override void _Ready() {
			this.Mesh = this.GetNode<MeshInstance>(p_meshInstance);
			if (this.Animations.Count > 0) {
				this.CurrentAnimation = this.Animations[0];
				this.Play("walk");
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Called to play the current animation.
		/// </summary>
		public void Play(string animationName_ = null) {
			if (animationName_ != null) {
				string formattedName = animationName_.ToLower();
				for (int index = 0; index < this.Animations.Count; index++) {
					if (this.Animations[index].Name.ToLower() == formattedName) {
						this.CurrentAnimation = this.Animations[index];
						break;
					}
				}
			}
			this.CurrentState = TextureAnimatorState.Playing;
			this.CurrentFrame = this.CurrentAnimation.Frames.Count - 1;
			this.NextFrame();
		}

		/// <summary>
		/// Called to jump to the next available frame.
		/// </summary>
		public void NextFrame() {
			if (this.CurrentState == TextureAnimatorState.Stopped) {
				return;
			}
			this.CurrentFrame++;
			if (this.CurrentFrame > this.CurrentAnimation.Frames.Count - 1) {
				this.CurrentFrame = 0;
			}
			if (!this.Mesh.Validate()) {
				this.Stop();
				return;
			}
			this.adjustTexture();
			if (this._timer == null) {
				this._timer = new MoonTimer(this.NextFrame);
			}
			this._timer.Start(this.CurrentAnimation.FrameDurations[this.CurrentFrame]);
		}

		/// <summary>
		/// Called to stop the current animation.
		/// </summary>
		public void Stop() {
			if (this._timer != null) {
				this._timer.Stop();
				this._timer = null;
			}
			this.CurrentState = TextureAnimatorState.Stopped;
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Adjusts the texture on the stored mesh instance.
		/// </summary>
		protected abstract void adjustTexture();
		#endregion
	}
}
