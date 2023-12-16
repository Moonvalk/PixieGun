// #define __DEBUG

using System;
using Godot;
using Moonvalk.Components.UI;
using Moonvalk.Data;
using Moonvalk.Nodes;
using Moonvalk.Resources;

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
			Instance = this.MakeSingleton(Instance);
			TransitionController = GetNode<MoonTransitionController>(p_transitionController);
			LoadScene(DefaultScene);
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
			if (IsLoading) return;
			
			IsLoading = true;
			if (MainScene.Validate())
			{
				TransitionController.PlayTransitionIntro(() =>
				{
					startSceneLoad(sceneName_, onLoad_);
				});
				return;
			}
			
			startSceneLoad(sceneName_, onLoad_);
		}

		/// <summary>
		/// Gets a matching resource file path for the requested scene name.
		/// </summary>
		/// <param name="sceneName_">The scene name to be loaded.</param>
		/// <returns>Returns the corresponding file path.</returns>
		public string GetScenePath(string sceneName_)
		{
			for (int index = 0; index < SceneList.Length; index++)
			{
				if (SceneList.Items[index].Name.ToLower() == sceneName_.ToLower())
					return SceneList.Items[index].Value;
			}
		
			#if (__DEBUG)
				GD.Print("Could not find file path for scene: " + sceneName_);
			#endif
			return SceneList.Items[0].Value;
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
			MoonResourceLoader.Load(GetScenePath(sceneName_), (PackedScene scene_) =>
			{
				instanceScene(scene_, onLoad_);
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
			
			this.RemoveChildren(MainScene);
			MainScene = this.AddInstance<Node>(scene_);
			IsLoading = false;
			onLoad_?.Invoke();
			TransitionController.PlayTransitionOutro();
		}
		#endregion
	}
}
