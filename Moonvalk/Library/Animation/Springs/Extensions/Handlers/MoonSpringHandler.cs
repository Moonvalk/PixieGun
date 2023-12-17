using System;
using Godot;

namespace Moonvalk.Animation
{
	/// <summary>
	/// Handler for a MoonSpring (float). These are containers that automate updating
	/// Node data each game tick with the use of extension methods.
	/// </summary>
	/// <typeparam name="ParentType">The type of object this handler will be applied to.</typeparam>
	public class MoonSpringHandler<ParentType> : BaseMoonSpringHandler<float, MoonSpring, ParentType> {
		/// <summary>
		/// Constructor for a new handler.
		/// </summary>
		/// <param name="object_">The object that will be animated.</param>
		/// <param name="property_">The property found on object_ that will be manipulated.</param>
		/// <param name="target_">The target value to be animated to.</param>
		/// <param name="parameters_">Properties that determine how the animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should begin immediately.</param>
		/// <param name="onComplete_">An action to be run when this Spring is complete. This is primarily used
		/// to remove a Spring reference once finished.</param>
		public MoonSpringHandler(
			ref ParentType object_,
			MoonSpringProperty property_,
			float target_,
			MoonSpringParams parameters_,
			bool start_,
			Action onComplete_
		) : base(ref object_, property_, target_, parameters_, start_, onComplete_) {
			// ...
		}

		/// <summary>
		/// Called to assign a new Action called each game tick during animations that will
		/// manipulate the Node property.
		/// </summary>
		/// <param name="object_">The object that this handler will manipulate.</param>
		/// <param name="property_">The property to be adjusted.</param>
		/// <returns>Returns a new Action.</returns>
		protected override Action AssignUpdateAction(ref ParentType object_, MoonSpringProperty property_) {
			switch (object_) {
				case Control control:
					switch (property_) {
						case MoonSpringProperty.Rotation:
							return () => { control.RectRotation = this._values[0]; };
					}
					break;
				case Node2D node:
					switch (property_) {
						case MoonSpringProperty.Rotation:
							return () => { node.RotationDegrees = this._values[0]; };
					}
					break;
			}
			return null;
		}

		/// <summary>
		/// Gets the starting value for the Spring object to begin at.
		/// </summary>
		/// <param name="object_">The object that this handler will manipulate.</param>
		/// <param name="property_">The property to be adjusted.</param>
		/// <returns>Returns the initial value.</returns>
		protected override float[] GetInitialPropertyValues(ref ParentType object_, MoonSpringProperty property_) {
			switch (object_) {
				case Control control:
					switch (property_) {
						case MoonSpringProperty.Rotation:
							return new float[1] { control.RectRotation };
					}
					break;
				case Node2D node:
					switch (property_) {
						case MoonSpringProperty.Rotation:
							return new float[1] { node.RotationDegrees };
					}
					break;
			}
			return new float[0];
		}

		/// <summary>
		/// Called to set up new Spring objects managed by this container.
		/// </summary>
		/// <param name="target_">The target value to be animated to.</param>
		/// <param name="parameters_">Parameters used to manipulate how the animation will play.</param>
		/// <param name="start_">Flag that determines if this animation should begin immediately.</param>
		/// <param name="onComplete_">Action to be run once the animation is complete.</param>
		protected override void SetupSpring(float target_, MoonSpringParams parameters_, bool start_, Action onComplete_) {
			this.Spring = new MoonSpring() { StartOnTargetAssigned = start_ };
			this.Spring.SetReferences(() => ref this._values[0]).SetParameters(parameters_);
			this.Spring.To(target_).OnUpdate(this.OnUpdate).OnComplete(() => {
				onComplete_?.Invoke();
			});
		}
	}
}
