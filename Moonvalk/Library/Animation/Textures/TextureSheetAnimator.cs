using System.Collections.Generic;
using Godot;
using Moonvalk.Utilities;

namespace Moonvalk.Animation {
	/// <summary>
	/// Handler for animating texture sheets on a mesh instance.
	/// </summary>
	public class TextureSheetAnimator : BaseTextureAnimator<TextureSheetAnimation, int> {
		#region Private Methods
		/// <summary>
		/// Adjusts the texture on the stored mesh instance.
		/// </summary>
		protected override void AdjustTexture() {
			var material = (this.Mesh.GetActiveMaterial(0) as SpatialMaterial);
			material.AlbedoTexture = this.CurrentAnimation.Sheet.Texture;

			var rect = this.CurrentAnimation.GetFrameData(this.CurrentAnimation.Frames[this.CurrentFrame]);
			material.Uv1Offset = new Vector3(rect.Position.x, rect.Position.y, 0f);
			material.Uv1Scale = new Vector3(rect.Size.x, rect.Size.y, 0f);
		}
		#endregion
	}
}
