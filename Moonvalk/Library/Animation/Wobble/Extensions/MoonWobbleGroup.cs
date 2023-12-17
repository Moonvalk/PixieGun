using System;
using System.Collections.Generic;
using Godot;

namespace Moonvalk.Animation
{
    /// <summary>
    /// Container for managing MoonWobbleHandlers animating a specific Godot.Node object.
    /// </summary>
    public class MoonWobbleGroup
    {
        /// <summary>
        /// Stores the last property that was adjusted.
        /// </summary>
        protected MoonWobbleProperty _previousProperty;

        /// <summary>
        /// Default constructor for a new group.
        /// </summary>
        public MoonWobbleGroup()
        {
            WobbleHandlers = new Dictionary<MoonWobbleProperty, IMoonWobbleHandler>();
        }

        /// <summary>
        /// Stores reference to all relevant Wobble handlers by property being adjusted on this object.
        /// </summary>
        public Dictionary<MoonWobbleProperty, IMoonWobbleHandler> WobbleHandlers { get; }

        /// <summary>
        /// Adds a new MoonWobbleHandler to this group for the specified property.
        /// </summary>
        /// <typeparam name="ParentType">The object type that will be manipulated.</typeparam>
        /// <typeparam name="Unit">The unit type that will be animated.</typeparam>
        /// <param name="object_">The object that will be manipulated.</param>
        /// <param name="property_">The property on _object to be animated.</param>
        /// <param name="percentage_">
        /// The percentage of the property that will be affected. This is useful for
        /// multi-axis values that need to be affected differently.
        /// </param>
        /// <param name="parameters_">Properties that determine how the animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should begin immediately.</param>
        /// <param name="onComplete_">
        /// An action to be run when this Wobble is complete. This is primarily used
        /// to remove a Wobble reference once finished.
        /// </param>
        /// <returns>Returns the new Wobble object.</returns>
        public BaseMoonWobble<Unit> AddWobble<ParentType, Unit>(ref ParentType object_, MoonWobbleProperty property_, Unit percentage_,
            MoonWobbleParams parameters_, bool start_, Action onComplete_)
        {
            RemoveProperty<Unit>(property_);
            IMoonWobbleHandler handler = null;
            switch (percentage_)
            {
                case float value:
                    handler = new MoonWobbleHandler<ParentType>(ref object_, property_, value, parameters_, start_, onComplete_);

                    break;
                case Vector2 value:
                    handler = new MoonWobbleVec2Handler<ParentType>(ref object_, property_, value, parameters_, start_, onComplete_);

                    break;
                case Vector3 value:
                    handler = new MoonWobbleVec3Handler<ParentType>(ref object_, property_, value, parameters_, start_, onComplete_);

                    break;
                case Color value:
                    handler = new MoonWobbleColorHandler<ParentType>(ref object_, property_, value, parameters_, start_, onComplete_);

                    break;
            }

            if (handler != null) return AddProperty<Unit>(property_, handler);

            return null;
        }

        /// <summary>
        /// Gets the Wobble for the requested property, when available.
        /// </summary>
        /// <typeparam name="Unit">The expected unit that will be used to Wobble the input property.</typeparam>
        /// <param name="property_">The property used to animate.</param>
        /// <returns>Returns the MoonWobble object, when applicable.</returns>
        public IMoonWobble<Unit> GetWobble<Unit>(MoonWobbleProperty property_)
        {
            if (WobbleHandlers.ContainsKey(property_))
                return WobbleHandlers[property_]?.GetWobble<Unit>();

            return null;
        }

        /// <summary>
        /// Clears this group by deleting all Wobbles. This clears all Wobbles previously stored for this object.
        /// </summary>
        public void Clear()
        {
            if (WobbleHandlers != null)
            {
                foreach (var handler in WobbleHandlers.Values) handler?.Delete();

                WobbleHandlers.Clear();
            }
        }

        /// <summary>
        /// Starts all Wobbles stored in this group.
        /// </summary>
        public void StartAll()
        {
            foreach (var handler in WobbleHandlers.Values) handler.Start();
        }

        /// <summary>
        /// Adds a new handler/property pair to the map.
        /// </summary>
        /// <typeparam name="Unit">The unit type used for animating.</typeparam>
        /// <param name="property_">The property that will be animated.</param>
        /// <param name="wobbleHandler_">Reference to the new Wobble handler created for the property.</param>
        /// <returns>Returns the MoonWobble used for animating.</returns>
        protected BaseMoonWobble<Unit> AddProperty<Unit>(MoonWobbleProperty property_, IMoonWobbleHandler wobbleHandler_)
        {
            WobbleHandlers.Add(property_, wobbleHandler_);
            _previousProperty = property_;
            return (BaseMoonWobble<Unit>)wobbleHandler_.GetWobble<Unit>();
        }

        /// <summary>
        /// Called to remove a handler/property pair from the map. This is done to ensure that MoonWobbles do
        /// not conflict with each other when adjusting similar properties.
        /// </summary>
        /// <typeparam name="Unit">The unit type used for animating.</typeparam>
        /// <param name="property_">The property that will be animated.</param>
        protected void RemoveProperty<Unit>(MoonWobbleProperty property_)
        {
            if (WobbleHandlers.ContainsKey(property_))
            {
                WobbleHandlers[property_].GetWobble<Unit>().Delete();

                WobbleHandlers.Remove(property_);
            }
        }
    }
}