using Godot;

namespace Moonvalk.Components
{
	/// <summary>
	/// Basic node that queues music playing when initialized.
	/// </summary>
	public class MoonMusicPlayer : Node
	{
		/// <summary>
		/// The name of the audio group to be played.
		/// </summary>
		[Export] public string GroupName { get; protected set; } = "";

		/// <summary>
		/// Optional resource name for a track to be played within the group.
		/// </summary>
		[Export] public string ResourceName { get; protected set; } = "";

		/// <summary>
		/// Called when this object is first initialized.
		/// </summary>
		public override void _Ready()
		{
			MoonAudioManager.Instance?.PlayMusic(GroupName, ResourceName == "" ? null : ResourceName);
		}
	}
}
