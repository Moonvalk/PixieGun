using System;
using Godot;

namespace Moonvalk.Animation {
	/// <summary>
	/// Handler for a MoonTween that affects Vector3 values. These are containers that automate updating
	/// Node data each game tick with the use of extension methods.
	/// </summary>
	/// <typeparam name="ParentType">The type of Node which is being animated.</typeparam>
	public class MoonTweenVec3Handler<ParentType> : BaseMoonTweenHandler<Vector3, MoonTweenVec3, ParentType> {
		/// <summary>
		/// Constructor for a new handler.
		/// </summary>
		/// <param name="object_">The object that will be animated.</param>
		/// <param name="property_">The property found on object_ that will be manipulated.</param>
		/// <param name="target_">The target value to be animated to.</param>
		/// <param name="parameters_">Properties that determine how the animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should begin immediately.</param>
		/// <param name="onComplete_">An action to be run when this Tween is complete. This is primarily used
		/// to remove a Tween reference once finished.</param>
		public MoonTweenVec3Handler(
			ParentType object_,
			MoonTweenProperty property_,
			Vector3 target_,
			MoonTweenParams parameters_,
			bool start_,
			Action onComplete_
		) : base(object_, property_, target_, parameters_, start_, onComplete_) {
			// ...
		}

		/// <summary>
		/// Called to assign a new Action called each game tick during animations that will
		/// manipulate the Node property.
		/// </summary>
		/// <param name="object_">The object that this handler will manipulate.</param>
		/// <param name="property_">The property to be adjusted.</param>
		/// <returns>Returns a new Action.</returns>
		protected override Action assignUpdateAction(ParentType object_, MoonTweenProperty property_) {
			switch (object_) {
				case Spatial spatial:
					switch (property_) {
						case MoonTweenProperty.Position:
							return () => { spatial.Translation = new Vector3(this._values[0], this._values[1], this._values[2]); };
						case MoonTweenProperty.Scale:
							return () => { spatial.Scale = new Vector3(this._values[0], this._values[1], this._values[2]); };
						case MoonTweenProperty.Rotation:
							return () => { spatial.RotationDegrees = new Vector3(this._values[0], this._values[1], this._values[2]); };
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
		protected override float[] getInitialPropertyValues(ParentType object_, MoonTweenProperty property_) {
			switch (object_) {
				case Spatial spatial:
					switch (property_) {
						case MoonTweenProperty.Position:
							return new float[3] { spatial.Translation.x, spatial.Translation.y, spatial.Translation.z };
						case MoonTweenProperty.Scale:
							return new float[3] { spatial.Scale.x, spatial.Scale.y, spatial.Scale.z };
						case MoonTweenProperty.Rotation:
							return new float[3] { spatial.RotationDegrees.x, spatial.RotationDegrees.y, spatial.RotationDegrees.z };
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
		/// <param name="onComplete_">An action to be run when this Tween is complete. This is primarily used
		/// to remove a Tween reference once finished.</param>
		protected override void setupTween(Vector3 target_, MoonTweenParams parameters_, bool start_, Action onComplete_) {
			this.Tween = new MoonTweenVec3() { StartOnTargetAssigned = start_ };
			this.Tween.SetReferences(() => ref this._values[0], () => ref this._values[1], () => ref this._values[2]).SetParameters(parameters_);
			this.Tween.OnUpdate(this.OnUpdate).OnComplete(() => {
				onComplete_?.Invoke();
			}).To(target_);
		}
	}
}
