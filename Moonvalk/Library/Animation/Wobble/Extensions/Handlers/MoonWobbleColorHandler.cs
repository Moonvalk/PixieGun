using System;
using Godot;

namespace Moonvalk.Animation
{
    /// <summary>
    /// Handler for a MoonWobble which affects Color values. These are containers that automate updating
    /// Node data each game tick with the use of extension methods.
    /// </summary>
    /// <typeparam name="ParentType">The type of Node which is being animated.</typeparam>
    public class MoonWobbleColorHandler<ParentType> : BaseMoonWobbleHandler<Color, MoonWobbleColor, ParentType>
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
        public MoonWobbleColorHandler(ref ParentType object_, MoonWobbleProperty property_, Color percentage_, MoonWobbleParams parameters_, bool start_,
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
                        case MoonWobbleProperty.Color:
                            return () => { control.Modulate = new Color(_values[0], _values[1], _values[2], _values[3]); };
                    }

                    break;
                case Node2D node:
                    switch (property_)
                    {
                        case MoonWobbleProperty.Color:
                            return () => { node.Modulate = new Color(_values[0], _values[1], _values[2], _values[3]); };
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
                        case MoonWobbleProperty.Color:
                            return new[] { control.Modulate.r, control.Modulate.g, control.Modulate.b, control.Modulate.a };
                    }

                    break;
                case Node2D node:
                    switch (property_)
                    {
                        case MoonWobbleProperty.Color:
                            return new[] { node.Modulate.r, node.Modulate.g, node.Modulate.b, node.Modulate.a };
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
        protected override void SetupWobble(MoonWobbleParams parameters_, Color percentage_, bool start_, Action onComplete_)
        {
            Wobble = new MoonWobbleColor();
            Wobble.SetReferences(() => ref _values[0], () => ref _values[1], () => ref _values[2], () => ref _values[3])
                .SetParameters(parameters_)
                .SetPercentage(percentage_);

            Wobble.OnUpdate(OnUpdate).OnComplete(() => { onComplete_?.Invoke(); });

            if (start_) Wobble.Start();
        }
    }
}