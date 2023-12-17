using System;
using Godot;

namespace Moonvalk.Animation
{
    /// <summary>
    /// Handler for a MoonTween that affects Colors. These are containers that automate updating
    /// Node data each game tick with the use of extension methods.
    /// </summary>
    /// <typeparam name="ParentType">The type of Node which is being animated.</typeparam>
    public class MoonTweenColorHandler<ParentType> : BaseMoonTweenHandler<Color, MoonTweenColor, ParentType>
    {
        /// <summary>
        /// Constructor for a new handler.
        /// </summary>
        /// <param name="object_">The object that will be animated.</param>
        /// <param name="property_">The property found on object_ that will be manipulated.</param>
        /// <param name="target_">The target value to be animated to.</param>
        /// <param name="parameters_">Properties that determine how the animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should begin immediately.</param>
        /// <param name="onComplete_">
        /// An action to be run when this Tween is complete. This is primarily used
        /// to remove a Tween reference once finished.
        /// </param>
        public MoonTweenColorHandler(ParentType object_, MoonTweenProperty property_, Color target_, MoonTweenParams parameters_, bool start_,
            Action onComplete_) : base(object_, property_, target_, parameters_, start_, onComplete_)
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
        protected override Action AssignUpdateAction(ParentType object_, MoonTweenProperty property_)
        {
            switch (object_)
            {
                case Control control:
                    switch (property_)
                    {
                        case MoonTweenProperty.Color:
                            return () => { control.Modulate = new Color(_values[0], _values[1], _values[2], _values[3]); };
                    }

                    break;
                case Node2D node:
                    switch (property_)
                    {
                        case MoonTweenProperty.Color:
                            return () => { node.Modulate = new Color(_values[0], _values[1], _values[2], _values[3]); };
                    }

                    break;
            }

            return null;
        }

        /// <summary>
        /// Gets the starting value for the Tween object to begin at.
        /// </summary>
        /// <param name="object_">The object that this handler will manipulate.</param>
        /// <param name="property_">The property to be adjusted.</param>
        /// <returns>Returns the initial value.</returns>
        protected override float[] GetInitialPropertyValues(ParentType object_, MoonTweenProperty property_)
        {
            switch (object_)
            {
                case Control control:
                    switch (property_)
                    {
                        case MoonTweenProperty.Color:
                            return new[] { control.Modulate.r, control.Modulate.g, control.Modulate.b, control.Modulate.a };
                    }

                    break;
                case Node2D node:
                    switch (property_)
                    {
                        case MoonTweenProperty.Color:
                            return new[] { node.Modulate.r, node.Modulate.g, node.Modulate.b, node.Modulate.a };
                    }

                    break;
            }

            return new float[0];
        }

        /// <summary>
        /// Called to set up new Tween objects managed by this container.
        /// </summary>
        /// <param name="target_">The target value to be animated to.</param>
        /// <param name="parameters_">Parameters used to manipulate how the animation will play.</param>
        /// <param name="start_">Flag that determines if this animation should begin immediately.</param>
        /// <param name="onComplete_">
        /// An action to be run when this Tween is complete. This is primarily used
        /// to remove a Tween reference once finished.
        /// </param>
        protected override void SetupTween(Color target_, MoonTweenParams parameters_, bool start_, Action onComplete_)
        {
            Tween = new MoonTweenColor { StartOnTargetAssigned = start_ };
            Tween.SetReferences(() => ref _values[0], () => ref _values[1], () => ref _values[2], () => ref _values[3]).SetParameters(parameters_);

            Tween.OnUpdate(OnUpdate).OnComplete(() => { onComplete_?.Invoke(); }).To(target_);
        }
    }
}