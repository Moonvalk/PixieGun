using System;
using Godot;

namespace Moonvalk.Animation {
	/// <summary>
	/// Handler for a MoonTween that affects Vector2 values. These are containers that automate updating
	/// Node data each game tick with the use of extension methods.
	/// </summary>
	/// <typeparam name="ParentType">The type of Node which is being animated.</typeparam>
	public class MoonTweenVec2Handler<ParentType> : BaseMoonTweenHandler<Vector2, MoonTweenVec2, ParentType> {
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
		public MoonTweenVec2Handler(
			ParentType object_,
			MoonTweenProperty property_,
			Vector2 target_,
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
				case Control control:
					switch (property_) {
						case MoonTweenProperty.Position:
							return () => { control.RectPosition = new Vector2(this._values[0], this._values[1]); };
						case MoonTweenProperty.Scale:
							return () => { control.RectScale = new Vector2(this._values[0], this._values[1]); };
					}
					break;
				case Node2D node:
					switch (property_) {
						case MoonTweenProperty.Position:
							return () => { node.Position = new Vector2(this._values[0], this._values[1]); };
						case MoonTweenProperty.Scale:
							return () => { node.Scale = new Vector2(this._values[0], this._values[1]); };
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
				case Control control:
					switch (property_) {
						case MoonTweenProperty.Position:
							return new float[2] { control.RectPosition.x, control.RectPosition.y };
						case MoonTweenProperty.Scale:
							return new float[2] { control.RectScale.x, control.RectScale.y };
					}
					break;
				case Node2D node:
					switch (property_) {
						case MoonTweenProperty.Position:
							return new float[2] { node.Position.x, node.Position.y };
						case MoonTweenProperty.Scale:
							return new float[2] { node.Scale.x, node.Scale.y };
					}
					break;
			}
			return default;
		}

		/// <summary>
		/// Called to set up new Tween objects managed by this container.
		/// </summary>
		/// <param name="target_">The target value to be animated to.</param>
		/// <param name="parameters_">Parameters used to manipulate how the animation will play.</param>
		/// <param name="start_">Flag that determines if this animation should begin immediately.</param>
		/// <param name="onComplete_">An action to be run when this Tween is complete. This is primarily used
		/// to remove a Tween reference once finished.</param>
		protected override void setupTween(Vector2 target_, MoonTweenParams parameters_, bool start_, Action onComplete_) {
			this.Tween = new MoonTweenVec2() { StartOnTargetAssigned = start_ };
			this.Tween.SetReferences(() => ref this._values[0], () => ref this._values[1]).SetParameters(parameters_);
			this.Tween.OnUpdate(this.OnUpdate).OnComplete(() => {
				onComplete_?.Invoke();
			}).To(target_);
		}
	}
}
