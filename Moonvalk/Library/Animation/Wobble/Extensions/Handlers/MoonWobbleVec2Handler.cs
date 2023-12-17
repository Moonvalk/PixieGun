using System;
using Godot;

namespace Moonvalk.Animation
{
    /// <summary>
    /// Handler for a MoonWobble which affects Vector2 values. These are containers that automate updating
    /// Node data each game tick with the use of extension methods.
    /// </summary>
    /// <typeparam name="ParentType">The type of Node which is being animated.</typeparam>
    public class MoonWobbleVec2Handler<ParentType> : BaseMoonWobbleHandler<Vector2, MoonWobbleVec2, ParentType>
    {
        /// <summary>
        /// Constructor for a new handler.
        /// </summary>
        /// <param name="object_">The object that will be animated.</param>
        /// <param name="property_">The property found on object_ that will be manipulated.</param>
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
        public MoonWobbleVec2Handler(ref ParentType object_, MoonWobbleProperty property_, Vector2 percentage_, MoonWobbleParams parameters_, bool start_,
            Action onComplete_) : base(ref object_, property_, percentage_, parameters_, start_, onComplete_)
        {
            // ...
        }

        /// <summary>
        /// Called to assign a new Action called each game tick during animations that will
        /// manipulate the Node property.
        /// </summary>
        /// <param name="object_">The object that this handler will manipulate.</param>
        /// <param name="property_">The property to be adjusted.</param>
        /// <returns>Returns a new Action.</returns>
        protected override Action AssignUpdateAction(ref ParentType object_, MoonWobbleProperty property_)
        {
            switch (object_)
            {
                case Control control:
                    switch (property_)
                    {
                        case MoonWobbleProperty.Position:
                            return () => { control.RectPosition = new Vector2(_values[0], _values[1]); };
                        case MoonWobbleProperty.Scale:
                            return () => { control.RectScale = new Vector2(_values[0], _values[1]); };
                    }

                    break;
                case Node2D node:
                    switch (property_)
                    {
                        case MoonWobbleProperty.Position:
                            return () => { node.Position = new Vector2(_values[0], _values[1]); };
                        case MoonWobbleProperty.Scale:
                            return () => { node.Scale = new Vector2(_values[0], _values[1]); };
                    }

                    break;
            }

            return null;
        }

        /// <summary>
        /// Gets the starting value for the Wobble object to begin at.
        /// </summary>
        /// <param name="object_">The object that this handler will manipulate.</param>
        /// <param name="property_">The property to be adjusted.</param>
        /// <returns>Returns the initial value.</returns>
        protected override float[] GetInitialPropertyValues(ref ParentType object_, MoonWobbleProperty property_)
        {
            switch (object_)
            {
                case Control control:
                    switch (property_)
                    {
                        case MoonWobbleProperty.Position:
                            return new[] { control.RectPosition.x, control.RectPosition.y };
                        case MoonWobbleProperty.Scale:
                            return new[] { control.RectScale.x, control.RectScale.y };
                    }

                    break;
                case Node2D node:
                    switch (property_)
                    {
                        case MoonWobbleProperty.Position:
                            return new[] { node.Position.x, node.Position.y };
                        case MoonWobbleProperty.Scale:
                            return new[] { node.Scale.x, node.Scale.y };
                    }

                    break;
            }

            return new float[0];
        }

        /// <summary>
        /// Called to set up new Wobble objects managed by this container.
        /// </summary>
        /// <param name="parameters_">Parameters used to manipulate how the animation will play.</param>
        /// <param name="percentage_">
        /// The percentage of the property that will be affected. This is useful for
        /// multi-axis values that need to be affected differently.
        /// </param>
        /// <param name="start_">Flag that determines if this animation should begin immediately.</param>
        /// <param name="onComplete_">
        /// An action to be run when this Wobble is complete. This is primarily used
        /// to remove a Wobble reference once finished.
        /// </param>
        protected override void SetupWobble(MoonWobbleParams parameters_, Vector2 percentage_, bool start_, Action onComplete_)
        {
            Wobble = new MoonWobbleVec2();
            Wobble.SetReferences(() => ref _values[0], () => ref _values[1]).SetParameters(parameters_).SetPercentage(percentage_);

            Wobble.OnUpdate(OnUpdate).OnComplete(() => { onComplete_?.Invoke(); });

            if (start_) Wobble.Start();
        }
    }
}