using System;
using Godot;
using Moonvalk.Data;
using Moonvalk.Nodes;

namespace Moonvalk.Components
{
    /// <summary>
    /// A base load manager for Godot scenes.
    /// </summary>
    /// <typeparam name="DataType">The type of serializable data used to provide scenes.</typeparam>
    /// <typeparam name="SceneType">The type of scene element packed scenes should be casted to.</typeparam>
    public abstract class BaseSceneLoader2<DataType, SceneType> : Node
        where SceneType : Node where DataType : MoonValueArray<DataType>
    {
        #region Godot Events
        /// <summary>
        /// Called when this object is first initialized.
        /// </summary>
        public override void _Ready()
        {
            if (ShowDefaultOnLoad) Display(DefaultSceneIndex);

            TransitionAnimator = this.GetComponent<BaseSceneTransitionAnimator>();
            LoadAnimator = this.GetComponent<BaseSceneLoadAnimator>();
        }
        #endregion

        #region Data Fields
        /// <summary>
        /// All scenes available to this object to swap between.
        /// </summary>
        [Export] public MoonValueArray<DataType> Scenes { get; protected set; }

        /// <summary>
        /// Flag that determines if the default scene should be shown on load.
        /// </summary>
        [Export] public bool ShowDefaultOnLoad { get; protected set; } = true;

        /// <summary>
        /// The default scene index to be displayed.
        /// </summary>
        [Export] public int DefaultSceneIndex { get; protected set; }

        /// <summary>
        /// Stores reference to a transition animator if applicable.
        /// </summary>
        public BaseSceneTransitionAnimator TransitionAnimator { get; protected set; }

        /// <summary>
        /// Stores reference to a load animator if applicable.
        /// </summary>
        public BaseSceneLoadAnimator LoadAnimator { get; protected set; }

        /// <summary>
        /// Stores the current scene.
        /// </summary>
        public SceneType CurrentScene { get; protected set; }

        /// <summary>
        /// Stores the current scene index being loaded.
        /// </summary>
        public int LoadedIndex { get; protected set; } = -1;

        /// <summary>
        /// Event emitted when this element hides itself.
        /// </summary>
        [Signal]
        public delegate void OnHide();

        /// <summary>
        /// Event emitted when this element displays a new scene.
        /// </summary>
        [Signal]
        public delegate void OnDisplay();
        #endregion

        #region Public Methods
        /// <summary>
        /// Called to show the scene at the specified index.
        /// </summary>
        /// <param name="sceneIndex_">The scene index to load.</param>
        /// <returns>Returns true if the scene request is unique, false if the request was already made.</returns>
        public bool Display(int sceneIndex_ = -1)
        {
            var index = sceneIndex_ == -1 ? DefaultSceneIndex : sceneIndex_;
            if (LoadedIndex == index) return false;

            LoadedIndex = index;
            DisplayScene(index);
            return true;
        }

        /// <summary>
        /// Called to show the scene with the provided name. Accepts non case-sensitive strings.
        /// </summary>
        /// <param name="sceneName_">The name of the scene to load.</param>
        /// <returns>Returns true if the scene is able to be swapped to, false if the scene is already loaded or not found.</returns>
        public bool Display(string sceneName_)
        {
            var formattedName = sceneName_.ToLower();
            for (var index = 0; index < Scenes.Length; index++)
                if (formattedName == Scenes.Items[index].Name.ToLower())
                    return Display(index);

            return false;
        }

        /// <summary>
        /// Hides the current scene with no callback.
        /// </summary>
        public virtual void Hide()
        {
            Hide(null);
        }

        /// <summary>
        /// Called when this object hides its current scene.
        /// </summary>
        /// <param name="onComplete_">An action to be called when the hide animation is complete.</param>
        public virtual void Hide(Action onComplete_)
        {
            if (TransitionAnimator != null)
            {
                TransitionAnimator.PlayIntro(() =>
                {
                    HideCurrentScene();
                    onComplete_?.Invoke();
                });

                return;
            }

            HideCurrentScene();
            onComplete_?.Invoke();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Internal execution to remove the current scene and inform subscribers.
        /// </summary>
        protected void HideCurrentScene()
        {
            this.RemoveChildren(CurrentScene);
            EmitSignal(nameof(OnHide));
        }

        /// <summary>
        /// Called to display the scene at the specified index.
        /// </summary>
        /// <param name="sceneIndex_">The scene index to swap to.</param>
        protected virtual void DisplayScene(int sceneIndex_)
        {
            SwapToScene(sceneIndex_);
        }

        /// <summary>
        /// Internal execution to remove the current scene and swap to a new one. When finished the display
        /// event will be invoked to inform subscribers.
        /// </summary>
        /// <param name="sceneIndex_">The scene index to swap to.</param>
        protected abstract void SwapToScene(int sceneIndex_);
        #endregion
    }
}