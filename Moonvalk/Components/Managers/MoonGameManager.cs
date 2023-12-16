using Godot;
using Moonvalk.Data;
using Moonvalk.Nodes;

namespace Moonvalk.Components
{
	/// <summary>
	/// Main game manager behavior.
	/// </summary>
	public partial class MoonGameManager : Node
	{
		#region Data Fields
		/// <summary>
		/// Singleton instance of GameManager.
		/// </summary>
		public static MoonGameManager Instance { get; private set; }

		/// <summary>
		/// Stores reference to the world environment.
		/// </summary>
		[Export] public Environment WorldEnvironment { get; protected set; }
		#endregion

		/// <summary>
		/// A maximum time elapsed allowed to be sent for game ticks. This helps to
		/// minimize lag spikes applied to systems.
		/// </summary>
		public const float MAXIMUM_FRAME_DELTA = 0.033f;

		#region Godot Events
		/// <summary>
		/// Occurs once this object is initialized.
		/// </summary>
		public override void _Ready()
		{
			MoonGameManager.Instance = this.MakeSingleton<MoonGameManager>(MoonGameManager.Instance);
			new MoonGameSettings("GlobalGameSettings.json");
		}
		
		/// <summary>
		/// Called each game tick.
		/// </summary>
		/// <param name="delta_">The time elapsed since last frame.</param>
		public override void _Process(float delta_)
		{
			float deltaCapped = Mathf.Min(delta_, MAXIMUM_FRAME_DELTA);
			Global.Systems.Update(deltaCapped);
		}
		#endregion
	}
}
