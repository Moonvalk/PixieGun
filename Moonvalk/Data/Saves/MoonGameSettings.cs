using Godot;
using Moonvalk.Accessory;
using Moonvalk.Components;

namespace Moonvalk.Data
{
    /// <summary>
    /// Object used to store global game setting data in a save file.
    /// </summary>
    public class MoonGameSettings : MoonSaveFile
    {
        /// <summary>
        /// A singleton instance of this object.
        /// </summary>
        public static MoonGameSettings Instance { get; set; }

        /// <summary>
        /// Default constructor for a global settings file.
        /// </summary>
        /// <param name="filePath_">The path where global game settings will be located locally.</param>
        public MoonGameSettings(string filePath_) : base(filePath_)
        {
            if (Instance != null) return;

            Instance = this;
            SetDefaults();
            Load();

            AssignAudioSettings();
            AssignDisplaySettings();
        }

        /// <summary>
        /// Sets the default values expected for game settings.
        /// </summary>
        protected void SetDefaults()
        {
            // Provide default settings before attempting to load.
            var volume = AddSaveData("volume", new BaseMoonSaveData<float>());
            for (var index = 0; index < MoonAudioManager.Instance?.AudioBuses.Length; index++)
            {
                if (MoonAudioManager.Instance?.AudioBuses.Items[index] != null)
                    volume.SetValue((MoonAudioManager.Instance.AudioBuses.Items[index].Name,
                        MoonAudioManager.Instance.AudioBuses.Items[index].Value));
            }

            AddSaveData("fullscreen", new BaseMoonSaveData<bool>())
                .SetValue(
                    ("Enabled", false)
                );
            AddSaveData("graphics", new BaseMoonSaveData<float>())
                .SetValue(
                    ("Screenshake", 0.5f),
                    ("Bloom", 0.5f)
                );
        }

        /// <summary>
        /// Assigns audio settings to the corresponding managers.
        /// </summary>
        protected void AssignAudioSettings()
        {
            // Assign audio bus levels.
            var volume = GetSaveData<float>("volume");
            for (var index = 0; index < MoonAudioManager.Instance?.AudioBuses.Length; index++)
            {
                MoonAudioManager.Instance?.SetVolume(MoonAudioManager.Instance.AudioBuses.Items[index].Name,
                    volume.GetValue(MoonAudioManager.Instance.AudioBuses.Items[index].Name));
            }
        }

        /// <summary>
        /// Assigns display settings to the corresponding managers.
        /// </summary>
        protected void AssignDisplaySettings()
        {
            // Assign graphics settings.
            if (DeviceHelpers.IsDeviceHtml5() || DeviceHelpers.IsDeviceMobile())
                Set("fullscreen", ("enabled", false));

            OS.WindowFullscreen = Get<bool>("fullscreen", "enabled");
            SetBloom();
        }

        /// <summary>
        /// Called to set the bloom graphics value
        /// </summary>
        /// <param name="value_">The new value to be stored. If left as default the current loaded value will be applied.</param>
        public void SetBloom(float value_ = -1f)
        {
            if (value_ >= 0f) Set("graphics", ("bloom", value_));

            if (MoonGameManager.Instance != null && MoonGameManager.Instance.WorldEnvironment != null)
            {
                var bloomMultiplier = 2f;
                MoonGameManager.Instance.WorldEnvironment.GlowIntensity =
                    Get<float>("graphics", "bloom") * bloomMultiplier;
            }
        }
    }
}