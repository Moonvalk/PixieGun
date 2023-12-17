using System.Collections.Generic;
using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Animation {
	/// <summary>
	/// Base class for animating texture sheets on mesh instances.
	/// </summary>
	[RegisteredType(nameof(TextureSheetAnimation), "", nameof(Resource))]
	public class TextureSheetAnimation : BaseTextureAnimation<int> {
		/// <summary>
		/// The sheet that will be used to grab frame data.
		/// </summary>
		[Export] public TextureSheet Sheet { get; protected set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index_"></param>
		/// <returns></returns>
		public Rect2 GetFrameData(int index_) {
			var frameUvSize = (this.Sheet.FrameSize / this.Sheet.Texture.GetSize());
			var frameX = frameUvSize.x * (index_ % this.Sheet.ColumnCount);
			var frameY = frameUvSize.y * Mathf.Floor((float)index_ / this.Sheet.ColumnCount);
			return new Rect2(frameX, frameY, frameUvSize);
		}
	}
}
