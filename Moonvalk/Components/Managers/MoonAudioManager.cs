using Godot;
using Moonvalk.Audio;
using Moonvalk.Data;
using Moonvalk.Nodes;
using Moonvalk.Resources;
using System.Collections.Generic;

namespace Moonvalk.Components
{
	/// <summary>
	/// A basic audio manager for handling bus volume and music playback.
	/// </summary>
	public class MoonAudioManager : Node
	{
		#region Data Fields
		/// <summary>
		/// Stores a singleton instance of this class for accessing its features.
		/// </summary>
		public static MoonAudioManager Instance { get; protected set; }

		/// <summary>
		/// Stores a global list of all audio resource groups used for playing music.
		/// </summary>
		[Export] public MoonResourceArray MusicList { get; protected set; }

		/// <summary>
		/// A list of all global sound effects.
		/// </summary>
		[Export] public MoonResourceArray GlobalSoundList { get; protected set; }

		/// <summary>
		/// Map of all global sounds loaded.
		/// </summary>
		public Dictionary<string, SoundPool> GlobalSounds { get; protected set; }

		/// <summary>
		/// Stores all case sensitive audio bus names available to be modified and their default starting values.
		/// </summary>
		[Export] public MoonFloatArray AudioBuses { get; protected set; }

		/// <summary>
		/// Flag that determines if this object is actively playing music or not.
		/// </summary>
		[Export] public bool Activated { get; protected set; } = true;

		/// <summary>
		/// Stores reference to the audio stream player used for playing music.
		/// </summary>
		public AudioStreamPlayer Player { get; protected set; }

		/// <summary>
		/// The current group name being played from.
		/// </summary>
		public string CurrentGroupName { get; protected set; }
		#endregion
		
		#region Godot Events
		/// <summary>
		/// Called when this object is first initialized.
		/// </summary>
		public override void _Ready()
		{
			MoonAudioManager.Instance = this.MakeSingleton<MoonAudioManager>(MoonAudioManager.Instance);
			this.Player = this.GetComponent<AudioStreamPlayer>();
			this.Player.Connect("finished", this, nameof(this.handleAudioStreamFinished));

			// Build a dictionary of sound pools for global playing.
			this.GlobalSounds = new Dictionary<string, SoundPool>();
			for (int index = 0; index < this.GlobalSoundList.Items.Length; index++)
			{
				string name = this.GlobalSoundList.Items[index].Name.ToLower();
				SoundPool pool = this.AddInstance<SoundPool>(this.GlobalSoundList.Items[index].GetAs<PackedScene>());
				
				if (this.GlobalSounds.ContainsKey(name)) this.GlobalSounds.Remove(name);
				
				this.GlobalSounds.Add(name, pool);
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Called to play music from the specified group. If a resource name is provided that specific track 
		/// </summary>
		/// <param name="groupName_">The group name to play music from.</param>
		/// <param name="resourceName_">The resource name to play within this group. If none is provided
		/// a new track will be selected by the group based on setup.</param>
		public void PlayMusic(string groupName_, string resourceName_ = null)
		{
			if (!this.Activated) return;

			string streamPath = null;
			string formattedName = groupName_.ToLower();
			for (int index = 0; index < this.MusicList.Items.Length; index++)
			{
				AudioResourceGroup group = this.MusicList.GetAs<AudioResourceGroup>(index);
				
				if (group.Name.ToLower() == formattedName)
				{
					this.CurrentGroupName = group.Name;
					streamPath = group.GetStreamPath(resourceName_);
					break;
				}
			}
			
			if (streamPath == null) return;

			MoonResourceLoader.Load<AudioStream>(streamPath, (AudioStream stream_) => {
				this.playStream(stream_);
			});
		}

		/// <summary>
		/// Called to play a global sound pool by name.
		/// </summary>
		/// <param name="soundPoolName_">The name of the sound to play.</param>
		public void PlaySound(string soundPoolName_)
		{
			string formattedName = soundPoolName_.ToLower();
			if (this.GlobalSounds.ContainsKey(formattedName)) this.GlobalSounds[formattedName].PlayRandomSound();
		}

		/// <summary>
		/// Sets the volume percentage of the requested audio bus.
		/// </summary>
		/// <param name="audioBus_">The bus to target.</param>
		/// <param name="percentage_">The new percentage to be applied for this bus volume.</param>
		/// <param name="saveSettings_">Should this setting be saved in the global json?</param>
		public void SetVolume(string audioBus_, float percentage_, bool saveSettings_ = true)
		{
			string busName = this.GetAudioBus(audioBus_).Name;
			float volumeDb = GD.Linear2Db(Mathf.Clamp(percentage_, 0f, 1f));
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(busName), volumeDb);
			MoonGameSettings.Instance.Set<float>("volume", (busName, percentage_));

			if (saveSettings_) MoonGameSettings.Instance.Save();
		}

		/// <summary>
		/// Gets a matching AudioBus name / default value pair from a non-case-sensitive request.
		/// </summary>
		/// <param name="name_">The name of the bus to find an exact match for.</param>
		/// <returns>Returns the AudioBus data.</returns>
		public MoonFloat GetAudioBus(string name_)
		{
			string formattedName = name_.ToLower();
			for (int index = 0; index < this.AudioBuses.Length; index++)
			{
				if (formattedName == this.AudioBuses.Items[index].Name.ToLower()) return this.AudioBuses.Items[index];
			}
			
			return this.AudioBuses.Items[0];
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Called to assign and play a new audio stream.
		/// </summary>
		/// <param name="stream_">The new audio stream / file to be played.</param>
		protected void playStream(AudioStream stream_)
		{
			this.Player.Stream = stream_;
			this.Player.Play();
		}

		/// <summary>
		/// Called when the current audio stream is finished playing.
		/// </summary>
		protected void handleAudioStreamFinished()
		{
			this.PlayMusic(this.CurrentGroupName);
		}
		#endregion
	}
}
