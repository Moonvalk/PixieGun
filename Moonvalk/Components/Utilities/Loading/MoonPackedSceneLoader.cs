using Godot;
using Moonvalk.Data;
using Moonvalk.Nodes;

namespace Moonvalk.Components
{
    /// <summary>
    /// Base class for an object that will track and instantiate a variety of different packed scenes.
    /// </summary>
    /// <typeparam name="SceneType">The type of scene element packed scenes should be casted to.</typeparam>
    public abstract class MoonPackedSceneLoader<SceneType> : BaseSceneLoader<MoonResourceArray, SceneType>
        where SceneType : Node
    {
        #region Private Methods
        /// <summary>
        /// Internal execution to remove the current scene and swap to a new one. When finished the display
        /// event will be invoked to inform subscribers.
        /// </summary>
        /// <param name="sceneIndex_">The scene index to swap to.</param>
        protected override void SwapToScene(int sceneIndex_)
        {
            this.RemoveChildren(CurrentScene);
            InstantiateScene(sceneIndex_);
            EmitSignal(nameof(OnDisplay));
        }

        /// <summary>
        /// Called to instantiate the scene at the specified index. If this index is out of bounds it will
        /// wrap to find a usable index instead.
        /// </summary>
        /// <param name="sceneIndex_">The scene index to be instantiated.</param>
        protected virtual void InstantiateScene(int sceneIndex_)
        {
            var index = Mathf.PosMod(sceneIndex_, Scenes.Length);
            CurrentScene = this.AddInstance<SceneType>(Scenes.GetAs<PackedScene>(index));
        }
        #endregion
    }
}