using Godot;
using Moonvalk.Nodes;

namespace Moonvalk.Animation
{
    /// <summary>
    /// Contains extension methods for manipulating Godot.Control nodes with animations.
    /// </summary>
    public static class ControlMoonAnimationExtensions
    {
        /// <summary>
        /// Centers the rect pivot point found on a control node.
        /// </summary>
        /// <param name="control_">The control node to center.</param>
        /// <returns>Returns this control node.</returns>
        public static Control CenterPivot(this Control control_)
        {
            control_.RectPivotOffset = control_.RectSize * 0.5f;
            return control_;
        }

        #region MoonTween Methods
        /// <summary>
        /// The base Tween method used for creating animations on objects.
        /// </summary>
        /// <typeparam name="Unit">The unit used for animating.</typeparam>
        /// <typeparam name="TweenType">The type of Tween that will be used.</typeparam>
        /// <param name="object_">The object that will be animated.</param>
        /// <param name="property_">The property available on _object to be changed.</param>
        /// <param name="target_">The target value to be animated to.</param>
        /// <param name="parameters_">Properties that adjust how the animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should start immediately.</param>
        /// <returns>Returns the newly created MoonTween object for further adjustments / handling.</returns>
        private static TweenType TweenTo<Unit, TweenType>(this Control object_, MoonTweenProperty property_, Unit target_, MoonTweenParams parameters_ = null,
            bool start_ = true)
            where TweenType : BaseMoonTween<Unit>, new()
        {
            if (!object_.Validate())
            {
                MoonTweenExtensions.Delete(object_);
                return null;
            }

            var group = MoonTweenExtensions.GetMoonTweenGroup(object_);
            return (TweenType)group.AddTween(object_, property_, target_, parameters_ ?? new MoonTweenParams(), start_,
                MoonTweenExtensions.GetRemoveAction(object_, property_));
        }

        /// <summary>
        /// Rotates the object to the requested target value.
        /// </summary>
        /// <param name="object_">The object that will be animated.</param>
        /// <param name="target_">The target value to be animated to.</param>
        /// <param name="parameters_">Properties that adjust how the animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should start immediately.</param>
        /// <returns>Returns the newly created MoonTween object for further adjustments / handling.</returns>
        public static MoonTween RotateTo(this Control object_, float target_, MoonTweenParams parameters_ = null, bool start_ = true)
        {
            return object_.TweenTo<float, MoonTween>(MoonTweenProperty.Rotation, target_, parameters_, start_);
        }

        /// <summary>
        /// Scales the object to the requested target value.
        /// </summary>
        /// <param name="object_">The object that will be animated.</param>
        /// <param name="target_">The target value to be animated to.</param>
        /// <param name="parameters_">Properties that adjust how the animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should start immediately.</param>
        /// <returns>Returns the newly created MoonTween object for further adjustments / handling.</returns>
        public static MoonTweenVec2 ScaleTo(this Control object_, Vector2 target_, MoonTweenParams parameters_ = null, bool start_ = true)
        {
            return object_.TweenTo<Vector2, MoonTweenVec2>(MoonTweenProperty.Scale, target_, parameters_, start_);
        }

        /// <summary>
        /// Moves the object to the requested target value.
        /// </summary>
        /// <param name="object_">The object that will be animated.</param>
        /// <param name="target_">The target value to be animated to.</param>
        /// <param name="parameters_">Properties that adjust how the animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should start immediately.</param>
        /// <returns>Returns the newly created MoonTween object for further adjustments / handling.</returns>
        public static MoonTweenVec2 MoveTo(this Control object_, Vector2 target_, MoonTweenParams parameters_ = null, bool start_ = true)
        {
            return object_.TweenTo<Vector2, MoonTweenVec2>(MoonTweenProperty.Position, target_, parameters_, start_);
        }

        /// <summary>
        /// Colors the object to the requested target value.
        /// </summary>
        /// <param name="object_">The object that will be animated.</param>
        /// <param name="target_">The target value to be animated to.</param>
        /// <param name="parameters_">Properties that adjust how the animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should start immediately.</param>
        /// <returns>Returns the newly created MoonTween object for further adjustments / handling.</returns>
        public static MoonTweenColor ColorTo(this Control object_, Color target_, MoonTweenParams parameters_ = null, bool start_ = true)
        {
            return object_.TweenTo<Color, MoonTweenColor>(MoonTweenProperty.Color, target_, parameters_, start_);
        }

        /// <summary>
        /// Gets the matching MoonTween for the selected object/property pair when available.
        /// </summary>
        /// <typeparam name="Unit">The type of unit expected being used for this Tween.</typeparam>
        /// <param name="object_">The object to get a Tween for.</param>
        /// <param name="property_">The property to get a Tween for.</param>
        /// <returns>Returns the MoonTween object if it exists.</returns>
        public static IMoonTween<Unit> GetTween<Unit>(this Control object_, MoonTweenProperty property_)
        {
            return MoonTweenExtensions.GetMoonTweenGroup(object_)?.GetTween<Unit>(property_);
        }

        /// <summary>
        /// Gets the matching MoonTweenGroup for the selected object.
        /// </summary>
        /// <param name="object_">The object to get a group for.</param>
        /// <returns>Returns the MoonTweenGroup if it exists.</returns>
        public static MoonTweenGroup GetTweenGroup(this Control object_)
        {
            return MoonTweenExtensions.GetMoonTweenGroup(object_);
        }
        #endregion

        #region MoonSpring Methods
        /// <summary>
        /// The base Spring method used for creating animations on objects.
        /// </summary>
        /// <typeparam name="Unit">The unit used for animating.</typeparam>
        /// <typeparam name="SpringType">The type of Spring that will be used.</typeparam>
        /// <param name="object_">The object that will be animated.</param>
        /// <param name="property_">The property available on _object to be changed.</param>
        /// <param name="target_">The target value to be animated to.</param>
        /// <param name="parameters_">Properties that adjust how the animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should start immediately.</param>
        /// <returns>Returns the newly created MoonSpring object for further adjustments / handling.</returns>
        private static SpringType SpringTo<Unit, SpringType>(this Control object_, MoonSpringProperty property_, Unit target_,
            MoonSpringParams parameters_ = null, bool start_ = true)
            where SpringType : BaseMoonSpring<Unit>, new()
        {
            if (!object_.Validate())
            {
                MoonSpringExtensions.Delete(object_);
                return null;
            }

            var group = MoonSpringExtensions.GetMoonSpringGroup(object_);
            return (SpringType)group.AddSpring(ref object_, property_, target_, parameters_ ?? new MoonSpringParams(), start_,
                MoonSpringExtensions.GetRemoveAction(object_, property_));
        }

        /// <summary>
        /// Rotates the object to the requested target value.
        /// </summary>
        /// <param name="object_">The object that will be animated.</param>
        /// <param name="target_">The target value to be animated to.</param>
        /// <param name="parameters_">Properties that adjust how the animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should start immediately.</param>
        /// <returns>Returns the newly created MoonSpring object for further adjustments / handling.</returns>
        public static MoonSpring SpringRotateTo(this Control object_, float target_, MoonSpringParams parameters_ = null, bool start_ = true)
        {
            return object_.SpringTo<float, MoonSpring>(MoonSpringProperty.Rotation, target_, parameters_, start_);
        }

        /// <summary>
        /// Scales the object to the requested target value.
        /// </summary>
        /// <param name="object_">The object that will be animated.</param>
        /// <param name="target_">The target value to be animated to.</param>
        /// <param name="parameters_">Properties that adjust how the animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should start immediately.</param>
        /// <returns>Returns the newly created MoonSpring object for further adjustments / handling.</returns>
        public static MoonSpringVec2 SpringScaleTo(this Control object_, Vector2 target_, MoonSpringParams parameters_ = null, bool start_ = true)
        {
            return object_.SpringTo<Vector2, MoonSpringVec2>(MoonSpringProperty.Scale, target_, parameters_, start_);
        }

        /// <summary>
        /// Moves the object to the requested target value.
        /// </summary>
        /// <param name="object_">The object that will be animated.</param>
        /// <param name="target_">The target value to be animated to.</param>
        /// <param name="parameters_">Properties that adjust how the animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should start immediately.</param>
        /// <returns>Returns the newly created MoonSpring object for further adjustments / handling.</returns>
        public static MoonSpringVec2 SpringMoveTo(this Control object_, Vector2 target_, MoonSpringParams parameters_ = null, bool start_ = true)
        {
            return object_.SpringTo<Vector2, MoonSpringVec2>(MoonSpringProperty.Position, target_, parameters_, start_);
        }

        /// <summary>
        /// Gets the matching MoonSpring for the selected object/property pair when available.
        /// </summary>
        /// <typeparam name="Unit">The type of unit expected being used for this Spring.</typeparam>
        /// <param name="object_">The object to get a Spring instance for.</param>
        /// <param name="property_">The property to get a Spring instance for.</param>
        /// <returns>Returns the MoonSpring object if it exists.</returns>
        public static IMoonSpring<Unit> GetSpring<Unit>(this Control object_, MoonSpringProperty property_)
        {
            return MoonSpringExtensions.GetMoonSpringGroup(object_)?.GetSpring<Unit>(property_);
        }

        /// <summary>
        /// Gets the matching MoonSpringGroup for the selected object.
        /// </summary>
        /// <param name="object_">The object to get a group for.</param>
        /// <returns>Returns the MoonSpringGroup if it exists.</returns>
        public static MoonSpringGroup GetSpringGroup(this Control object_)
        {
            return MoonSpringExtensions.GetMoonSpringGroup(object_);
        }
        #endregion

        #region MoonWobble Methods
        /// <summary>
        /// Base method for applying a Wobble to a node.
        /// </summary>
        /// <typeparam name="Unit">The unit used for animating.</typeparam>
        /// <typeparam name="WobbleType">The MoonWobble type expected.</typeparam>
        /// <param name="object_">The object that will be animated.</param>
        /// <param name="property_">The property available on _object to be changed.</param>
        /// <param name="percentage_">
        /// The percentage of the property that will be affected. This is useful for
        /// multi-axis values that need to be affected differently.
        /// </param>
        /// <param name="parameters_">Properties that adjust how the animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should start immediately.</param>
        /// <returns>Returns the newly created MoonWobble object for further adjustments / handling.</returns>
        private static WobbleType Wobble<Unit, WobbleType>(this Control object_, MoonWobbleProperty property_, Unit percentage_,
            MoonWobbleParams parameters_ = null, bool start_ = true)
            where WobbleType : BaseMoonWobble<Unit>, new()
        {
            if (!object_.Validate())
            {
                MoonWobbleExtensions.Delete(object_);
                return null;
            }

            var group = MoonWobbleExtensions.GetMoonWobbleGroup(object_);
            return (WobbleType)group.AddWobble(ref object_, property_, percentage_, parameters_ ?? new MoonWobbleParams(), start_,
                MoonWobbleExtensions.GetRemoveAction(object_, property_));
        }

        /// <summary>
        /// Rotates the object with a wobble.
        /// </summary>
        /// <param name="object_">The object that will be animated.</param>
        /// <param name="percentage_">
        /// The percentage of the property that will be affected. This is useful for
        /// multi-axis values that need to be affected differently.
        /// </param>
        /// <param name="parameters_">Properties that adjust how the animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should start immediately.</param>
        /// <returns>Returns the newly created MoonWobble object for further adjustments / handling.</returns>
        public static MoonWobble WobbleRotation(this Control object_, float percentage_, MoonWobbleParams parameters_ = null, bool start_ = true)
        {
            return object_.Wobble<float, MoonWobble>(MoonWobbleProperty.Rotation, percentage_, parameters_, start_);
        }

        /// <summary>
        /// Stops the requested wobble rotation.
        /// </summary>
        /// <param name="object_">The object to stop animating.</param>
        /// <returns>Returns the MoonWobble object for further adjustments.</returns>
        public static MoonWobble StopWobbleRotation(this Control object_)
        {
            return (MoonWobble)MoonWobbleExtensions.GetMoonWobbleGroup(object_)?.GetWobble<float>(MoonWobbleProperty.Rotation).Stop();
        }

        /// <summary>
        /// Applies a scaling affect with a wobble.
        /// </summary>
        /// <param name="object_">The object that will be animated.</param>
        /// <param name="percentage_">
        /// The percentage of the property that will be affected. This is useful for
        /// multi-axis values that need to be affected differently.
        /// </param>
        /// <param name="parameters_">Properties that adjust how the animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should start immediately.</param>
        /// <returns>Returns the MoonWobble object for further adjustments.</returns>
        public static MoonWobbleVec2 WobbleScale(this Control object_, Vector2 percentage_, MoonWobbleParams parameters_ = null, bool start_ = true)
        {
            return object_.Wobble<Vector2, MoonWobbleVec2>(MoonWobbleProperty.Scale, percentage_, parameters_, start_);
        }

        /// <summary>
        /// Stops the requested wobble scale.
        /// </summary>
        /// <param name="object_">The object to stop animating.</param>
        /// <returns>Returns the MoonWobble object for further adjustments.</returns>
        public static MoonWobbleVec2 StopWobbleScale(this Control object_)
        {
            return (MoonWobbleVec2)MoonWobbleExtensions.GetMoonWobbleGroup(object_)?.GetWobble<Vector2>(MoonWobbleProperty.Scale).Stop();
        }

        /// <summary>
        /// Applies a movement affect with a wobble.
        /// </summary>
        /// <param name="object_">The object that will be animated.</param>
        /// <param name="percentage_">
        /// The percentage of the property that will be affected. This is useful for
        /// multi-axis values that need to be affected differently.
        /// </param>
        /// <param name="parameters_">Properties that adjust how the animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should start immediately.</param>
        /// <returns>Returns the MoonWobble object for further adjustments.</returns>
        public static MoonWobbleVec2 WobbleMove(this Control object_, Vector2 percentage_, MoonWobbleParams parameters_ = null, bool start_ = true)
        {
            return object_.Wobble<Vector2, MoonWobbleVec2>(MoonWobbleProperty.Position, percentage_, parameters_, start_);
        }

        /// <summary>
        /// Stops the requested wobble movement.
        /// </summary>
        /// <param name="object_">The object that will be animated.</param>
        /// <returns>Returns the MoonWobble object for further adjustments.</returns>
        public static MoonWobbleVec2 StopWobbleMove(this Control object_)
        {
            return (MoonWobbleVec2)MoonWobbleExtensions.GetMoonWobbleGroup(object_)?.GetWobble<Vector2>(MoonWobbleProperty.Position).Stop();
        }

        /// <summary>
        /// Applies a color affect with a wobble.
        /// </summary>
        /// <param name="object_">The object that will be animated.</param>
        /// <param name="percentage_">
        /// The percentage of the property that will be affected. This is useful for
        /// multi-axis values that need to be affected differently.
        /// </param>
        /// <param name="parameters_">Properties that adjust how the animation will look.</param>
        /// <param name="start_">Flag that determines if this animation should start immediately.</param>
        /// <returns>Returns the MoonWobble object for further adjustments.</returns>
        public static MoonWobbleColor WobbleColor(this Control object_, Color percentage_, MoonWobbleParams parameters_ = null, bool start_ = true)
        {
            return object_.Wobble<Color, MoonWobbleColor>(MoonWobbleProperty.Color, percentage_, parameters_, start_);
        }

        /// <summary>
        /// Stops the requested wobble color.
        /// </summary>
        /// <param name="object_">The object that will be animated.</param>
        /// <returns>Returns the MoonWobble object for further adjustments.</returns>
        public static MoonWobbleColor StopWobbleColor(this Control object_)
        {
            return (MoonWobbleColor)MoonWobbleExtensions.GetMoonWobbleGroup(object_)?.GetWobble<Color>(MoonWobbleProperty.Color).Stop();
        }

        /// <summary>
        /// Gets the matching MoonWobble for the selected object/property pair when available.
        /// </summary>
        /// <typeparam name="Unit">The type of unit expected being used for this object.</typeparam>
        /// <param name="object_">The object to get a Wobble instance for.</param>
        /// <param name="property_">The property to get a Wobble instance for.</param>
        /// <returns>Returns the MoonWobble object if it exists.</returns>
        public static IMoonWobble<Unit> GetWobble<Unit>(this Control object_, MoonWobbleProperty property_)
        {
            return MoonWobbleExtensions.GetMoonWobbleGroup(object_)?.GetWobble<Unit>(property_);
        }

        /// <summary>
        /// Gets the matching MoonWobbleGroup for the selected object.
        /// </summary>
        /// <param name="object_">The object to get a group for.</param>
        /// <returns>Returns the MoonWobbleGroup if it exists.</returns>
        public static MoonWobbleGroup GetWobbleGroup(this Control object_)
        {
            return MoonWobbleExtensions.GetMoonWobbleGroup(object_);
        }
        #endregion
    }
}