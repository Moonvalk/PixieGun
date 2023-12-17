using System;
using System.Collections.Generic;
using Godot;

namespace Moonvalk.Animation
{
    /// <summary>
    /// Container for managing MoonTweenHandlers animating a specific Godot.Node object.
    /// </summary>
    public class MoonTweenGroup
    {
        /// <summary>
        /// Stores the last property that was adjusted.
        /// </summary>
        protected MoonTweenProperty _previousProperty;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MoonTweenGroup()
        {
            TweenHandlers = new Dictionary<MoonTweenProperty, IMoonTweenHandler>();
        }

        /// <summary>
        /// Stores reference to all relevant tween handlers by property being adjusted on this object.
        /// </summary>
        public Dictionary<MoonTweenProperty, IMoonTweenHandler> TweenHandlers { get; }

        /// <summary>
        /// Adds a new MoonTweenHandler to this group for the specified property.
        /// </summary>
        /// <typeparam name="ParentType">The object type that will be manipulated.</typeparam>
        /// <typeparam name="Unit">The unit type that will be animated.</typeparam>
        /// <param name="object_">The object that will be manipulated.</param>
        /// <param name="property_">The property on _object to be animated.</param>
        /// <param name="target_">The target value to be animated to.</param>
        /// <param name="parameters_">Properties that change the look of the animation.</param>
        /// <param name="start_">Flag that determines if this animation should begin immediately.</param>
        /// <param name="onComplete_">
        /// An action to be run when this Tween is complete. This is primarily used
        /// to remove a Tween reference once finished.
        /// </param>
        /// <returns>Returns the new Tween object.</returns>
        public BaseMoonTween<Unit> AddTween<ParentType, Unit>(ParentType object_, MoonTweenProperty property_, Unit target_, MoonTweenParams parameters_,
            bool start_, Action onComplete_)
        {
            RemoveProperty<Unit>(property_);
            IMoonTweenHandler handler = null;
            switch (target_)
            {
                case float value:
                    handler = new MoonTweenHandler<ParentType>(object_, property_, value, parameters_, start_, onComplete_);

                    break;
                case Vector2 value:
                    handler = new MoonTweenVec2Handler<ParentType>(object_, property_, value, parameters_, start_, onComplete_);

                    break;
                case Vector3 value:
                    handler = new MoonTweenVec3Handler<ParentType>(object_, property_, value, parameters_, start_, onComplete_);

                    break;
                case Color value:
                    handler = new MoonTweenColorHandler<ParentType>(object_, property_, value, parameters_, start_, onComplete_);

                    break;
            }

            if (handler != null) return AddProperty<Unit>(property_, handler);

            return null;
        }

        /// <summary>
        /// Gets the Tween for the requested property, when available.
        /// </summary>
        /// <typeparam name="Unit">The expected unit that will be used to tween the input property.</typeparam>
        /// <param name="property_">The property used to animate.</param>
        /// <returns>Returns the MoonTween object, when applicable.</returns>
        public IMoonTween<Unit> GetTween<Unit>(MoonTweenProperty property_)
        {
            if (TweenHandlers.ContainsKey(property_))
                return TweenHandlers[property_]?.GetTween<Unit>();

            return null;
        }

        /// <summary>
        /// Clears this group by deleting all Tweens. This clears all Tweens previously stored for this object.
        /// </summary>
        public void Clear()
        {
            if (TweenHandlers != null)
            {
                foreach (var handler in TweenHandlers.Values) handler.Delete();

                TweenHandlers.Clear();
            }
        }

        /// <summary>
        /// Starts all Tweens stored in this group.
        /// </summary>
        public void StartAll()
        {
            foreach (var handler in TweenHandlers.Values) handler.Start();
        }

        /// <summary>
        /// Adds a new handler/property pair to the map.
        /// </summary>
        /// <typeparam name="Unit">The unit type used for animating.</typeparam>
        /// <param name="property_">The property that will be animated.</param>
        /// <param name="tweenHandler_">Reference to the new Tween handler created for the property.</param>
        /// <returns>Returns the MoonTween used for animating.</returns>
        protected BaseMoonTween<Unit> AddProperty<Unit>(MoonTweenProperty property_, IMoonTweenHandler tweenHandler_)
        {
            TweenHandlers.Add(property_, tweenHandler_);
            _previousProperty = property_;
            return (BaseMoonTween<Unit>)tweenHandler_.GetTween<Unit>();
        }

        /// <summary>
        /// Called to remove a handler/property pair from the map. This is done to ensure that MoonTweens do
        /// not conflict with each other when adjusting similar properties.
        /// </summary>
        /// <typeparam name="Unit">The unit type used for animating.</typeparam>
        /// <param name="property_">The property that will be animated.</param>
        protected void RemoveProperty<Unit>(MoonTweenProperty property_)
        {
            if (TweenHandlers.ContainsKey(property_))
            {
                TweenHandlers[property_].GetTween<Unit>().Delete();

                TweenHandlers.Remove(property_);
            }
        }
    }
}