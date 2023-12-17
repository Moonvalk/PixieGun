using System;
using System.Collections.Generic;
using System.Linq;
using Moonvalk.Accessory;

namespace Moonvalk.Utilities
{
    /// <summary>
    /// Base container representing an action map based on enum / state.
    /// </summary>
    /// <typeparam name="EnumType">An enumerator used for tracking events.</typeparam>
    /// <typeparam name="ActionType">The type of action stored for each event.</typeparam>
    public abstract class BaseMoonActionMap<EnumType, ActionType>
    {
        /// <summary>
        /// Default constructor for a new action map.
        /// </summary>
        protected BaseMoonActionMap()
        {
            Events = new Dictionary<EnumType, InitValue<List<ActionType>>>();
            foreach (EnumType item in Enum.GetValues(typeof(EnumType)))
                Events.Add(item, new InitValue<List<ActionType>>(() => new List<ActionType>()));
        }

        /// <summary>
        /// A map of all enumerator values paired with lists of actions that will occur during events.
        /// </summary>
        public Dictionary<EnumType, InitValue<List<ActionType>>> Events { get; protected set; }

        /// <summary>
        /// Adds new actions for the specified event / enum value.
        /// </summary>
        /// <param name="event_">The event / enum value to add new actions for.</param>
        /// <param name="actions_">Actions that will be executed during the event.</param>
        public BaseMoonActionMap<EnumType, ActionType> AddAction(EnumType event_, params ActionType[] actions_)
        {
            foreach (var action in actions_)
            {
                if (action == null) continue;

                Events[event_].Value.Add(action);
            }

            return this;
        }

        /// <summary>
        /// Adds new actions for the specified event / enum value.
        /// </summary>
        /// <param name="event_">The event / enum value to add new actions for.</param>
        /// <param name="removePreviousActions_">
        /// Flag that when true will remove all actions previously stored for
        /// this event.
        /// </param>
        /// <param name="actions_">Actions that will be executed during the event.</param>
        public BaseMoonActionMap<EnumType, ActionType> AddAction(EnumType event_, bool removePreviousActions_ = false, params ActionType[] actions_)
        {
            if (removePreviousActions_) Clear(event_);

            return AddAction(event_, actions_);
        }

        /// <summary>
        /// Adds new actions for the specified event / enum value at the specified index.
        /// </summary>
        /// <param name="event_">The event / enum value to add new actions for.</param>
        /// <param name="index_">The position in the list where to add these tasks.</param>
        /// <param name="actions_">Actions that will be executed during the event.</param>
        public BaseMoonActionMap<EnumType, ActionType> AddAction(EnumType event_, int index_ = -1, params ActionType[] actions_)
        {
            foreach (var action in actions_)
                Events[event_].Value.Insert(index_ > -1 ? index_ : Events[event_].Value.Count, action);

            return this;
        }

        /// <summary>
        /// Clears all actions stored for all event / enum values.
        /// </summary>
        public BaseMoonActionMap<EnumType, ActionType> ClearAll()
        {
            foreach (var key in Events.Keys.ToArray())
                Events[key].Value.Clear();

            return this;
        }

        /// <summary>
        /// Clears all actions stored for the specified event / enum value.
        /// </summary>
        /// <param name="event_">The event / enum value to clear actions for.</param>
        public BaseMoonActionMap<EnumType, ActionType> Clear(EnumType event_)
        {
            Events[event_].Value.Clear();

            return this;
        }
    }
}