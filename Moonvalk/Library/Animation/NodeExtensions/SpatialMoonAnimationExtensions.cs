using Godot;
using Moonvalk.Nodes;

namespace Moonvalk.Animation {
	/// <summary>
	/// Contains extension methods for manipulating Godot.Control nodes with animations
	/// </summary>
	public static class SpatialMoonAnimationExtensions {
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
		public static TweenType TweenTo<Unit, TweenType>(
			this Spatial object_,
			MoonTweenProperty property_,
			Unit target_,
			MoonTweenParams parameters_ = null,
			bool start_ = true
		) where TweenType : BaseMoonTween<Unit>, new() {
			if (!object_.Validate()) {
				MoonTweenExtensions.Delete(object_);
				return null;
			}
			MoonTweenGroup group = MoonTweenExtensions.GetMoonTweenGroup(object_);
			return (TweenType)group.AddTween<Spatial, Unit>(object_, property_, target_, parameters_ ?? new MoonTweenParams(),
				start_, MoonTweenExtensions.GetRemoveAction(object_, property_));
		}

		/// <summary>
		/// Rotates the object to the requested target value.
		/// </summary>
		/// <param name="object_">The object that will be animated.</param>
		/// <param name="target_">The target value to be animated to.</param>
		/// <param name="parameters_">Properties that adjust how the animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should start immediately.</param>
		/// <returns>Returns the newly created MoonTween object for further adjustments / handling.</returns>
		public static MoonTweenVec3 RotateTo(this Spatial object_, Vector3 target_, MoonTweenParams parameters_ = null, bool start_ = true) {
			return SpatialMoonAnimationExtensions.TweenTo<Vector3, MoonTweenVec3>(object_, MoonTweenProperty.Rotation, target_, parameters_, start_);
		}

		/// <summary>
		/// Scales the object to the requested target value.
		/// </summary>
		/// <param name="object_">The object that will be animated.</param>
		/// <param name="target_">The target value to be animated to.</param>
		/// <param name="parameters_">Properties that adjust how the animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should start immediately.</param>
		/// <returns>Returns the newly created MoonTween object for further adjustments / handling.</returns>
		public static MoonTweenVec3 ScaleTo(this Spatial object_, Vector3 target_, MoonTweenParams parameters_ = null, bool start_ = true) {
			return SpatialMoonAnimationExtensions.TweenTo<Vector3, MoonTweenVec3>(object_, MoonTweenProperty.Scale, target_, parameters_, start_);
		}

		/// <summary>
		/// Moves the object to the requested target value.
		/// </summary>
		/// <param name="object_">The object that will be animated.</param>
		/// <param name="target_">The target value to be animated to.</param>
		/// <param name="parameters_">Properties that adjust how the animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should start immediately.</param>
		/// <returns>Returns the newly created MoonTween object for further adjustments / handling.</returns>
		public static MoonTweenVec3 MoveTo(this Spatial object_, Vector3 target_, MoonTweenParams parameters_ = null, bool start_ = true) {
			return SpatialMoonAnimationExtensions.TweenTo<Vector3, MoonTweenVec3>(object_, MoonTweenProperty.Position, target_, parameters_, start_);
		}

		/// <summary>
		/// Gets the matching MoonTween for the selected object/property pair when available.
		/// </summary>
		/// <typeparam name="Unit">The type of unit expected being used for this Tween.</typeparam>
		/// <param name="object_">The object to get a Tween instance for.</param>
		/// <param name="property_">The property to get a Tween instance for.</param>
		/// <returns>Returns the MoonTween object if it exists.</returns>
		public static IMoonTween<Unit> GetTween<Unit>(this Spatial object_, MoonTweenProperty property_) {
			return MoonTweenExtensions.GetMoonTweenGroup(object_)?.GetTween<Unit>(property_);
		}

		/// <summary>
		/// Gets the matching MoonTweenGroup for the selected object.
		/// </summary>
		/// <param name="object_">The object to get a group for.</param>
		/// <returns>Returns the MoonTweenGroup if it exists.</returns>
		public static MoonTweenGroup GetTweenGroup(this Spatial object_) {
			return MoonTweenExtensions.GetMoonTweenGroup(object_);
		}
		#endregion

		#region MoonSpring Methods
		/// <summary>
		/// The base Spring method used for creating animations on objects.
		/// </summary>
		/// <typeparam name="Unit">The unit used for springing.</typeparam>
		/// <typeparam name="SpringType">The type of Spring that will be used.</typeparam>
		/// <param name="object_">The object that will be animated.</param>
		/// <param name="property_">The property available on _object to be changed.</param>
		/// <param name="target_">The target value to be animated to.</param>
		/// <param name="parameters_">Properties that adjust how the animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should start immediately.</param>
		/// <returns>Returns the newly created MoonSpring object for further adjustments / handling.</returns>
		public static SpringType SpringTo<Unit, SpringType>(
			this Spatial object_,
			MoonSpringProperty property_,
			Unit target_,
			MoonSpringParams parameters_ = null,
			bool start_ = true
		) where SpringType : BaseMoonSpring<Unit>, new() {
			if (!object_.Validate()) {
				MoonSpringExtensions.Delete(object_);
				return null;
			}
			MoonSpringGroup group = MoonSpringExtensions.GetMoonSpringGroup(object_);
			return (SpringType)group.AddSpring<Spatial, Unit>(ref object_, property_, target_, parameters_ ?? new MoonSpringParams(),
				start_, MoonSpringExtensions.GetRemoveAction(object_, property_));
		}

		/// <summary>
		/// Rotates the object to the requested target value.
		/// </summary>
		/// <param name="object_">The object that will be animated.</param>
		/// <param name="target_">The target value to be animated to.</param>
		/// <param name="parameters_">Properties that adjust how the animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should start immediately.</param>
		/// <returns>Returns the newly created MoonTween object for further adjustments / handling.</returns>
		public static MoonSpringVec3 SpringRotateTo(this Spatial object_, Vector3 target_, MoonSpringParams parameters_ = null, bool start_ = true) {
			return SpatialMoonAnimationExtensions.SpringTo<Vector3, MoonSpringVec3>(object_, MoonSpringProperty.Rotation, target_, parameters_, start_);
		}

		/// <summary>
		/// Scales the object to the requested target value.
		/// </summary>
		/// <param name="object_">The object that will be animated.</param>
		/// <param name="target_">The target value to be animated to.</param>
		/// <param name="parameters_">Properties that adjust how the animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should start immediately.</param>
		/// <returns>Returns the newly created MoonTween object for further adjustments / handling.</returns>
		public static MoonSpringVec3 SpringScaleTo(this Spatial object_, Vector3 target_, MoonSpringParams parameters_ = null, bool start_ = true) {
			return SpatialMoonAnimationExtensions.SpringTo<Vector3, MoonSpringVec3>(object_, MoonSpringProperty.Scale, target_, parameters_, start_);
		}

		/// <summary>
		/// Moves the object to the requested target value.
		/// </summary>
		/// <param name="object_">The object that will be animated.</param>
		/// <param name="target_">The target value to be animated to.</param>
		/// <param name="parameters_">Properties that adjust how the animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should start immediately.</param>
		/// <returns>Returns the newly created MoonTween object for further adjustments / handling.</returns>
		public static MoonSpringVec3 SpringMoveTo(this Spatial object_, Vector3 target_, MoonSpringParams parameters_ = null, bool start_ = true) {
			return SpatialMoonAnimationExtensions.SpringTo<Vector3, MoonSpringVec3>(object_, MoonSpringProperty.Position, target_, parameters_, start_);
		}

		/// <summary>
		/// Gets the matching MoonSpring for the selected object/property pair when available.
		/// </summary>
		/// <typeparam name="Unit">The type of unit expected being used for this Tween.</typeparam>
		/// <param name="object_">The object to get a Spring instance for.</param>
		/// <param name="property_">The property to get a Spring instance for.</param>
		/// <returns>Returns the MoonSpring object if it exists.</returns>
		public static IMoonSpring<Unit> GetSpring<Unit>(this Spatial object_, MoonSpringProperty property_) {
			return MoonSpringExtensions.GetMoonSpringGroup(object_)?.GetSpring<Unit>(property_);
		}

		/// <summary>
		/// Gets the matching MoonSpringGroup for the selected object.
		/// </summary>
		/// <param name="object_">The object to get a group for.</param>
		/// <returns>Returns the MoonSpringGroup if it exists.</returns>
		public static MoonSpringGroup GetSpringGroup(this Spatial object_) {
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
		/// <param name="percentage_">The percentage of the property that will be affected. This is useful for
		/// multi-axis values that need to be affected differently.</param>
		/// <param name="parameters_">Properties that adjust how the animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should start immediately.</param>
		/// <returns>Returns the newly created MoonWobble object for further adjustments / handling.</returns>
		public static WobbleType Wobble<Unit, WobbleType>(
			this Spatial object_,
			MoonWobbleProperty property_,
			Unit percentage_,
			MoonWobbleParams parameters_ = null,
			bool start_ = true
		) where WobbleType : BaseMoonWobble<Unit>, new() {
			if (!object_.Validate()) {
				MoonWobbleExtensions.Delete(object_);
				return null;
			}
			MoonWobbleGroup group = MoonWobbleExtensions.GetMoonWobbleGroup(object_);
			return (WobbleType)group.AddWobble<Spatial, Unit>(ref object_, property_, percentage_, parameters_ ?? new MoonWobbleParams(),
				start_, MoonWobbleExtensions.GetRemoveAction(object_, property_));
		}

		/// <summary>
		/// Rotates the object with a wobble.
		/// </summary>
		/// <param name="object_">The object that will be animated.</param>
		/// <param name="percentage_">The percentage of the property that will be affected. This is useful for
		/// multi-axis values that need to be affected differently.</param>
		/// <param name="parameters_">Properties that adjust how the animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should start immediately.</param>
		/// <returns>Returns the newly created MoonWobble object for further adjustments / handling.</returns>
		public static MoonWobbleVec3 WobbleRotation(this Spatial object_, Vector3 percentage_, MoonWobbleParams parameters_ = null, bool start_ = true) {
			return SpatialMoonAnimationExtensions.Wobble<Vector3, MoonWobbleVec3>(object_, MoonWobbleProperty.Rotation, percentage_, parameters_, start_);
		}

		/// <summary>
		/// Stops the requested wobble rotation.
		/// </summary>
		/// <param name="object_">The object to stop animating.</param>
		/// <returns>Returns the MoonWobble object for further adjustments.</returns>
		public static MoonWobbleVec3 StopWobbleRotation(this Spatial object_) {
			return (MoonWobbleVec3)MoonWobbleExtensions.GetMoonWobbleGroup(object_)?.GetWobble<Vector3>(MoonWobbleProperty.Rotation).Stop();
		}

		/// <summary>
		/// Applies a scaling affect with a wobble.
		/// </summary>
		/// <param name="object_">The object that will be animated.</param>
		/// <param name="percentage_">The percentage of the property that will be affected. This is useful for
		/// multi-axis values that need to be affected differently.</param>
		/// <param name="parameters_">Properties that adjust how the animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should start immediately.</param>
		/// <returns>Returns the MoonWobble object for further adjustments.</returns>
		public static MoonWobbleVec3 WobbleScale(this Spatial object_, Vector3 percentage_, MoonWobbleParams parameters_ = null, bool start_ = true) {
			return SpatialMoonAnimationExtensions.Wobble<Vector3, MoonWobbleVec3>(object_, MoonWobbleProperty.Scale, percentage_, parameters_, start_);
		}

		/// <summary>
		/// Stops the requested wobble scale.
		/// </summary>
		/// <param name="object_">The object to stop animating.</param>
		/// <returns>Returns the MoonWobble object for further adjustments.</returns>
		public static MoonWobbleVec3 StopWobbleScale(this Spatial object_) {
			return (MoonWobbleVec3)MoonWobbleExtensions.GetMoonWobbleGroup(object_)?.GetWobble<Vector3>(MoonWobbleProperty.Scale).Stop();
		}

		/// <summary>
		/// Applies a movement affect with a wobble.
		/// </summary>
		/// <param name="object_">The object that will be animated.</param>
		/// <param name="percentage_">The percentage of the property that will be affected. This is useful for
		/// multi-axis values that need to be affected differently.</param>
		/// <param name="parameters_">Properties that adjust how the animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should start immediately.</param>
		/// <returns>Returns the MoonWobble object for further adjustments.</returns>
		public static MoonWobbleVec3 WobbleMove(this Spatial object_, Vector3 percentage_, MoonWobbleParams parameters_ = null, bool start_ = true) {
			return SpatialMoonAnimationExtensions.Wobble<Vector3, MoonWobbleVec3>(object_, MoonWobbleProperty.Position, percentage_, parameters_, start_);
		}

		/// <summary>
		/// Stops the requested wobble movement.
		/// </summary>
		/// <param name="object_">The object that will be animated.</param>
		/// <returns>Returns the MoonWobble object for further adjustments.</returns>
		public static MoonWobbleVec3 StopWobbleMove(this Spatial object_) {
			return (MoonWobbleVec3)MoonWobbleExtensions.GetMoonWobbleGroup(object_)?.GetWobble<Vector3>(MoonWobbleProperty.Position).Stop();
		}

		/// <summary>
		/// Gets the matching MoonWobble for the selected object/property pair when available.
		/// </summary>
		/// <typeparam name="Unit">The type of unit expected being used for this object.</typeparam>
		/// <param name="object_">The object to get a Wobble instance for.</param>
		/// <param name="property_">The property to get a Wobble instance for.</param>
		/// <returns>Returns the MoonWobble object if it exists.</returns>
		public static IMoonWobble<Unit> GetWobble<Unit>(this Spatial object_, MoonWobbleProperty property_) {
			return MoonWobbleExtensions.GetMoonWobbleGroup(object_)?.GetWobble<Unit>(property_);
		}

		/// <summary>
		/// Gets the matching MoonWobbleGroup for the selected object.
		/// </summary>
		/// <param name="object_">The object to get a group for.</param>
		/// <returns>Returns the MoonWobbleGroup if it exists.</returns>
		public static MoonWobbleGroup GetWobbleGroup(this Spatial object_) {
			return MoonWobbleExtensions.GetMoonWobbleGroup(object_);
		}
		#endregion
	}
}
