using Godot;
using Moonvalk.Data;
using Moonvalk.Nodes;
using Moonvalk.Resources;

namespace Moonvalk.Components
{
	/// <summary>
	/// Base class for an object that will track and instantiate a variety of different scenes via path.
	/// </summary>
	/// <typeparam name="SceneType">The type of scene element packed scenes should be casted to.</typeparam>
	public abstract class MoonScenePathLoader<SceneType> : BaseSceneLoader<MoonStringArray, SceneType>
		where SceneType : Node
	{
		#region Private Methods
		/// <summary>
		/// Internal execution to remove the current scene and swap to a new one. When finished the display
		/// event will be invoked to inform subscribers.
		/// </summary>
		/// <param name="sceneIndex_">The scene index to swap to.</param>
		protected override void swapToScene(int sceneIndex_)
		{
			MoonResourceLoader.Load(getScenePath(sceneIndex_), (PackedScene scene_) =>
			{
				this.RemoveChildren(CurrentScene);
				instantiateScene(scene_);
				EmitSignal(nameof(OnDisplay));
			});
		}

		/// <summary>
		/// Handles instancing a new scene as a child of this node.
		/// </summary>
		/// <param name="packedScene_">The scene to instance.</param>
		protected virtual void instantiateScene(PackedScene packedScene_)
		{
			CurrentScene = this.AddInstance<SceneType>(packedScene_);
		}

		/// <summary>
		/// Gets the matching scene path to be loaded for the specified index.
		/// </summary>
		/// <param name="sceneIndex_">The index to load.</param>
		/// <returns>Returns the scene path within the virtual file system.</returns>
		protected string getScenePath(int sceneIndex_)
		{
			int index = Mathf.Clamp(sceneIndex_, 0, Scenes.Length - 1);
			return Scenes.Items[index].AsPath;
		}
		#endregion
	}
}
