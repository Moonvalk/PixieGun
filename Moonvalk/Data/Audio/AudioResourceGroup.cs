using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Data
{
    /// <summary>
    /// A group for managing realtime loading audio resources via paths.
    /// </summary>
    [RegisteredType(nameof(AudioResourceGroup))]
    public class AudioResourceGroup : MoonStringArray
    {
        #region Data Fields

        /// <summary>
        /// The play type used when an audio resource has finished playing and will need to be swapped.
        /// </summary>
        [Export]
        public AudioResourcePlayType LoopType { get; protected set; } = AudioResourcePlayType.Ordered;

        /// <summary>
        /// The initial play type used when an audio resource must be selected.
        /// </summary>
        [Export]
        public AudioResourcePlayType InitialPlayType { get; protected set; } = AudioResourcePlayType.Random;

        /// <summary>
        /// The current index of this group used to manage playlists.
        /// </summary>
        public int CurrentIndex { get; protected set; } = -1;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a random stream path matching a resource within this group.
        /// </summary>
        /// <returns>Returns a new randomized stream path.</returns>
        public string GetRandomStreamPath()
        {
            if (Items.Length < 2) return GetStreamPath(0);

            var rng = new RandomNumberGenerator();
            var index = rng.RandiRange(0, Items.Length - 1);

            while (index == CurrentIndex)
            {
                index = rng.RandiRange(0, Items.Length - 1);
            }

            return GetStreamPath(index);
        }

        /// <summary>
        /// Gets a stream path at the specified index.
        /// </summary>
        /// <param name="index_">The resource index to get a path for.</param>
        /// <returns>Returns the corresponding resource path.</returns>
        public string GetStreamPath(int index_)
        {
            if (index_ > Items.Length - 1) index_ = 0;

            CurrentIndex = index_;
            return Items[CurrentIndex].AsPath;
        }

        /// <summary>
        /// Gets a stream path for the provided resource name, if applicable.
        /// </summary>
        /// <param name="name_">The resource name to find a path for.</param>
        /// <returns>Returns the corresponding stream path.</returns>
        public string GetStreamPath(string name_ = null)
        {
            if (name_ == null)
            {
                switch (CurrentIndex == -1 ? InitialPlayType : LoopType)
                {
                    default:
                    case AudioResourcePlayType.Random:
                        return GetRandomStreamPath();
                    case AudioResourcePlayType.Ordered:
                        return GetStreamPath(CurrentIndex + 1);
                }
            }

            var formattedName = name_.ToLower();
            for (var index = 0; index < Items.Length; index++)
            {
                if (formattedName == Items[index].Name.ToLower()) return GetStreamPath(index);
            }

            return GetStreamPath(0);
        }

        #endregion
    }
}