using Godot;
using Moonvalk.Components;
using System.Collections.Generic;

namespace Moonvalk.Audio
{
	/// <summary>
	/// Container for holding a pool of SoundQueue objects. This pool can be 
	/// cycled through for randomized playback.
	/// </summary>
	[Tool]
	public class SoundPool : Node
	{
		/// <summary>
		/// List of all sound queues that are stored as child nodes.
		/// </summary>
		public List<SoundQueue> Sounds { get; private set; } = new List<SoundQueue>();

		/// <summary>
		/// Generator for getting random sound effects.
		/// </summary>
		public RandomNumberGenerator Random { get; } = new RandomNumberGenerator();

		/// <summary>
		/// Stores reference to the previously played index.
		/// </summary>
		public int PreviousIndex { get; private set; } = -1;

		/// <summary>
		/// Called when this object is initialized.
		/// </summary>
		public override void _Ready()
		{
			this.Sounds = this.GetAllComponents<SoundQueue>();
		}

		/// <summary>
		/// Called by the Godot editor to get potential warnings with setup.
		/// </summary>
		/// <returns>A string representing a warning with current configuration.</returns>
		public override string _GetConfigurationWarning()
		{
			int numberOfSoundQueues = 0;
			foreach (var child in GetChildren())
			{
				if (child is SoundQueue)
				{
					numberOfSoundQueues++;
				}
			}
			
			if (numberOfSoundQueues < 2) return "Expected two or more children of type SoundQueue.";
			
			return "";
		}

		/// <summary>
		/// Called to play a new random sound effect within this pool.
		/// </summary>
		public void PlayRandomSound()
		{
			int index;
			do
			{
				index = this.Random.RandiRange(0, this.Sounds.Count - 1);
			}
			while (index == this.PreviousIndex);
			
			this.PreviousIndex = index;
			this.Sounds[index].PlaySound();
		}
	}
}
