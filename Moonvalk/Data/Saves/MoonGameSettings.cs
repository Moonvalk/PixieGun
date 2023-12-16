using Godot;
using Moonvalk.Accessory;
using Moonvalk.Components;

namespace Moonvalk.Data {
	/// <summary>
	/// Object used to store global game setting data in a save file.
	/// </summary>
	public class MoonGameSettings : MoonSaveFile {
		/// <summary>
		/// A singleton instance of this object.
		/// </summary>
		public static MoonGameSettings Instance { get; protected set; }

		/// <summary>
		/// Default constructor for a global settings file.
		/// </summary>
		/// <param name="filePath_">The path where global game settings will be located locally.</param>
		public MoonGameSettings(string filePath_) : base(filePath_) {
			if (MoonGameSettings.Instance != null) {
				return;
			}
			MoonGameSettings.Instance = this;
			this.setDefaults();
			this.Load();
			
			this.assignAudioSettings();
			this.assignDisplaySettings();
		}

		/// <summary>
		/// Sets the default values expected for game settings.
		/// </summary>
		protected void setDefaults() {
			// Provide default settings before attempting to load.
			BaseMoonSaveData<float> volume = this.AddSaveData<float>("volume", new BaseMoonSaveData<float>());
			for (int index = 0; index < MoonAudioManager.Instance?.AudioBuses.Length; index++) {
				volume.SetValue((MoonAudioManager.Instance?.AudioBuses.Items[index].Name, MoonAudioManager.Instance.AudioBuses.Items[index].Value));
			}
			this.AddSaveData<bool>("fullscreen", new BaseMoonSaveData<bool>()).SetValue(
				("Enabled", false)
			);
			this.AddSaveData<float>("graphics", new BaseMoonSaveData<float>()).SetValue(
				("Screenshake", 0.5f),
				("Bloom", 0.5f)
			);
		}

		/// <summary>
		/// Assigns audio settings to the corresponding managers.
		/// </summary>
		protected void assignAudioSettings() {
			// Assign audio bus levels.
			BaseMoonSaveData<float> volume = this.GetSaveData<float>("volume");
			for (int index = 0; index < MoonAudioManager.Instance?.AudioBuses.Length; index++) {
				MoonAudioManager.Instance?.SetVolume(MoonAudioManager.Instance.AudioBuses.Items[index].Name,
					volume.GetValue(MoonAudioManager.Instance.AudioBuses.Items[index].Name));
			}
		}

		/// <summary>
		/// Assigns display settings to the corresponding managers.
		/// </summary>
		protected void assignDisplaySettings() {
			// Assign graphics settings.
			if (DeviceHelpers.IsDeviceHTML5() || DeviceHelpers.IsDeviceMobile()) {
				this.Set("fullscreen", ("enabled", false));
			}
			OS.WindowFullscreen = this.Get<bool>("fullscreen", "enabled");
			this.SetBloom();
		}

		/// <summary>
		/// Called to set the bloom graphics value
		/// </summary>
		/// <param name="value_">The new value to be stored. If left as default the current loaded value will be applied.</param>
		public void SetBloom(float value_ = -1f) {
			if (value_ >= 0f) {
				this.Set<float>("graphics", ("bloom", value_));
			}
			if (MoonGameManager.Instance != null && MoonGameManager.Instance.WorldEnvironment != null) {
				float bloomMultiplier = 2f;
				MoonGameManager.Instance.WorldEnvironment.GlowIntensity = this.Get<float>("graphics", "bloom") * bloomMultiplier;
			}
		}
	}
}
