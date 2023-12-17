using System;
using Godot;
using Moonvalk.Nodes;

namespace Moonvalk.Components.UI
{
    /// <summary>
    /// Base handler for a root menu scene.
    /// </summary>
    public class BaseRootMenu : MoonScenePathLoader<BaseMenuPageController>
    {
        #region Public Methods
        /// <summary>
        /// Called to hide the current menu page.
        /// </summary>
        public override void Hide(Action onComplete_)
        {
            // Play an animation and when complete remove the scene.
            CurrentScene.HidePage(() =>
            {
                HideCurrentScene();
                onComplete_?.Invoke();
            });
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Displays the scene at the given index.
        /// </summary>
        /// <param name="sceneIndex_">The scene index to be displayed.</param>
        protected override void DisplayScene(int sceneIndex_)
        {
            // Animate away the current page first, if necessary.
            if (CurrentScene.Validate())
            {
                CurrentScene.HidePage(() => { SwapToScene(sceneIndex_); });

                return;
            }

            SwapToScene(sceneIndex_);
        }

        /// <summary>
        /// Instantiates a new scene at the requested index.
        /// </summary>
        /// <param name="packedScene_">The scene element to instance.</param>
        protected override void InstantiateScene(PackedScene packedScene_)
        {
            base.InstantiateScene(packedScene_);
            CurrentScene.Connect(nameof(BaseMenuPageController.OnExitMenu), this, nameof(this.Hide));
            CurrentScene.Connect(nameof(BaseMenuPageController.OnDisplayPage), this, nameof(Show));
            CurrentScene.Connect(nameof(BaseMenuPageController.OnButtonFocus), this, nameof(HandleButtonFocus));
            CurrentScene.Connect(nameof(BaseMenuPageController.OnButtonPress), this, nameof(HandleButtonPress));
            CurrentScene.Connect(nameof(BaseMenuPageController.OnAnimateElement), this, nameof(HandleAnimateElement));
        }

        /// <summary>
        /// Handles a button press event.
        /// </summary>
        protected void HandleButtonPress()
        {
            MoonAudioManager.Instance.PlaySound("press");
        }

        /// <summary>
        /// Handles a button focus event.
        /// </summary>
        protected void HandleButtonFocus()
        {
            MoonAudioManager.Instance.PlaySound("hover");
        }

        /// <summary>
        /// Handles an element animation event.
        /// </summary>
        protected void HandleAnimateElement()
        {
            MoonAudioManager.Instance.PlaySound("element");
        }
        #endregion
    }
}