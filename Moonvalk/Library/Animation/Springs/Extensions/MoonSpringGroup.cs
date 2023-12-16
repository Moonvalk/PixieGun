using System;
using System.Collections.Generic;
using Godot;

namespace Moonvalk.Animation {
	/// <summary>
	/// Container for managing MoonSpringHandlers animating a specific Godot.Node object.
	/// </summary>
	public class MoonSpringGroup {
		/// <summary>
		/// Stores reference to all relevant Spring handlers by property being adjusted on this object.
		/// </summary>
		public Dictionary<MoonSpringProperty, IMoonSpringHandler> SpringHandlers { get; private set; }

		/// <summary>
		/// Stores the last property that was adjusted.
		/// </summary>
		protected MoonSpringProperty _previousProperty;

		/// <summary>
		/// Default constructor for a new group.
		/// </summary>
		public MoonSpringGroup() {
			this.SpringHandlers = new Dictionary<MoonSpringProperty, IMoonSpringHandler>();
		}

		/// <summary>
		/// Adds a new MoonSpringHandler to this group for the specified property.
		/// </summary>
		/// <typeparam name="ParentType">The object type that will be manipulated.</typeparam>
		/// <typeparam name="Unit">The unit type that will be animated.</typeparam>
		/// <param name="object_">The object that will be manipulated.</param>
		/// <param name="property_">The property on _object to be animated.</param>
		/// <param name="target_">The target value to be animated to.</param>
		/// <param name="parameters_">Properties that change the look of the animation.</param>
		/// <param name="start_">Flag that determines if this animation should begin immediately.</param>
		/// <param name="onComplete_">An action to be run when this Spring is complete. This is primarily used
		/// to remove a Spring reference once finished.</param>
		/// <returns>Returns the new Spring object.</returns>
		public BaseMoonSpring<Unit> AddSpring<ParentType, Unit>(
			ref ParentType object_,
			MoonSpringProperty property_,
			Unit target_,
			MoonSpringParams parameters_,
			bool start_,
			Action onComplete_
		) {
			this.removeProperty<Unit>(property_);
			IMoonSpringHandler handler = null;
			switch (target_) {
				case float value:
					handler = new MoonSpringHandler<ParentType>(ref object_, property_, value, parameters_, start_, onComplete_);
					break;
				case Vector2 value:
					handler = new MoonSpringVec2Handler<ParentType>(ref object_, property_, value, parameters_, start_, onComplete_);
					break;
				case Vector3 value:
					handler = new MoonSpringVec3Handler<ParentType>(ref object_, property_, value, parameters_, start_, onComplete_);
					break;
			}
			if (handler != null) {
				return this.addProperty<Unit>(property_, handler);
			}
			return null;
		}

		/// <summary>
		/// Gets the Spring for the requested property, when available.
		/// </summary>
		/// <typeparam name="Unit">The expected unit that will be used to animate the input property.</typeparam>
		/// <param name="property_">The property used to animate.</param>
		/// <returns>Returns the MoonSpring object, when applicable.</returns>
		public IMoonSpring<Unit> GetSpring<Unit>(MoonSpringProperty property_) {
			if (this.SpringHandlers.ContainsKey(property_)) {
				return this.SpringHandlers[property_]?.GetSpring<Unit>();
			}
			return null;
		}

		/// <summary>
		/// Clears this group by deleting all Springs. This clears all Springs previously stored for this object.
		/// </summary>
		public void Clear() {
			if (this.SpringHandlers != null) {
				foreach (IMoonSpringHandler handler in this.SpringHandlers.Values) {
					handler?.Delete();
				}
				this.SpringHandlers.Clear();
			}
		}

		/// <summary>
		/// Starts all Springs stored in this group.
		/// </summary>
		public void StartAll() {
			foreach (IMoonSpringHandler handler in this.SpringHandlers.Values) {
				handler.Start();
			}
		}

		/// <summary>
		/// Adds a new handler/property pair to the map.
		/// </summary>
		/// <typeparam name="Unit">The unit type used for animating.</typeparam>
		/// <param name="property_">The property that will be animated.</param>
		/// <param name="springHandler_">Reference to the new Spring handler created for the property.</param>
		/// <returns>Returns the MoonSpring used for animating.</returns>
		protected BaseMoonSpring<Unit> addProperty<Unit>(MoonSpringProperty property_, IMoonSpringHandler springHandler_) {
			this.SpringHandlers.Add(property_, springHandler_);
			this._previousProperty = property_;
			return (BaseMoonSpring<Unit>)springHandler_.GetSpring<Unit>();
		}

		/// <summary>
		/// Called to remove a handler/property pair from the map. This is done to ensure that MoonSprings do
		/// not conflict with each other when adjusting similar properties.
		/// </summary>
		/// <typeparam name="Unit">The unit type used for animating.</typeparam>
		/// <param name="property_">The property that will be animated.</param>
		protected void removeProperty<Unit>(MoonSpringProperty property_) {
			if (this.SpringHandlers.ContainsKey(property_)) {
				this.SpringHandlers[property_].GetSpring<Unit>().Delete();
				this.SpringHandlers.Remove(property_);
			}
		}
	}
}
