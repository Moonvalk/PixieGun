// #define __DEBUG

using System;
using Godot;
using Moonvalk.Components.UI;
using Moonvalk.Data;
using Moonvalk.Nodes;
using Moonvalk.Resources;
using Moonvalk.Utilities;

namespace Moonvalk.Components
{
	/// <summary>
	/// A manager for loading game scenes with transitions.
	/// </summary>
	public class MoonSceneLoadManager : Node
	{
		#region Data Fields
		/// <summary>
		/// A list of all root scenes available to this game.
		/// </summary>
		[Export] public MoonStringArray SceneList { get; protected set; }

		/// <summary>
		/// The default scene to be instantiated on load.
		/// </summary>
		[Export] public string DefaultScene { get; protected set; } = "";

		/// <summary>
		/// Path to the transition controller object.
		/// </summary>
		[Export] protected NodePath p_transitionController { get; set; }

		/// <summary>
		/// Singleton instance of this manager for quick access.
		/// </summary>
		public static MoonSceneLoadManager Instance { get; protected set; }

		/// <summary>
		/// Stores reference to the main scene when active.
		/// </summary>
		public Node MainScene { get; protected set; }

		/// <summary>
		/// Stores reference to the transition controller responsible for playing animations
		/// between main scene swaps.
		/// </summary>
		public MoonTransitionController TransitionController { get; protected set; }

		/// <summary>
		/// Flag that determines if this scene loader is actively loading.
		/// </summary>
		public bool IsLoading { get; protected set; }
		#endregion

		#region Godot Events
		/// <summary>
		/// Called when this object is first initialized.
		/// </summary>
		public override void _Ready()
		{
			MoonSceneLoadManager.Instance = this.MakeSingleton<MoonSceneLoadManager>(MoonSceneLoadManager.Instance);
			this.TransitionController = this.GetNode<MoonTransitionController>(p_transitionController);
			this.LoadScene(this.DefaultScene);
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Called to load a scene by name.
		/// </summary>
		/// <param name="sceneName_">The scene name to load.</param>
		/// <param name="onLoad_">An action that will be called when the scene is loaded.</param>
		public void LoadScene(string sceneName_, Action onLoad_ = null)
		{
			if (this.IsLoading) return;
			
			this.IsLoading = true;
			if (this.MainScene.Validate())
			{
				this.TransitionController.PlayTransitionIntro(() => {
					this.startSceneLoad(sceneName_, onLoad_);
				});
				return;
			}
			
			this.startSceneLoad(sceneName_, onLoad_);
		}

		/// <summary>
		/// Gets a matching resource file path for the requested scene name.
		/// </summary>
		/// <param name="sceneName_">The scene name to be loaded.</param>
		/// <returns>Returns the corresponding file path.</returns>
		public string GetScenePath(string sceneName_)
		{
			for (int index = 0; index < this.SceneList.Length; index++)
			{
				if (this.SceneList.Items[index].Name.ToLower() == sceneName_.ToLower())
					return this.SceneList.Items[index].Value;
			}
		
			#if (__DEBUG)
				GD.Print("Could not find file path for scene: " + sceneName_);
			#endif
			return this.SceneList.Items[0].Value;
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Starts loading the requested scene name.
		/// </summary>
		/// <param name="sceneName_">The scene to be loaded.</param>
		/// <param name="onLoad_">An action to be run when the scene is finished being loaded.</param>
		protected void startSceneLoad(string sceneName_, Action onLoad_)
		{
			MoonResourceLoader.Load<PackedScene>(this.GetScenePath(sceneName_), (PackedScene scene_) => {
				this.instanceScene(scene_, onLoad_);
			}, initialPollDelay_: 1f);
		}

		/// <summary>
		/// Called to instantiate a new main scene when the resource is loaded and available.
		/// </summary>
		/// <param name="scene_">The packed scene to instantiate.</param>
		/// <param name="onLoad_">An action to be run when the scene swap is complete.</param>
		protected void instanceScene(PackedScene scene_, Action onLoad_)
		{
			#if (__DEBUG)
				GD.Print("Instantiating scene: " + scene_);
			#endif
			
			this.RemoveChildren(this.MainScene);
			this.MainScene = this.AddInstance<Node>(scene_);
			this.IsLoading = false;
			onLoad_?.Invoke();
			this.TransitionController.PlayTransitionOutro();
		}
		#endregion
	}
}
