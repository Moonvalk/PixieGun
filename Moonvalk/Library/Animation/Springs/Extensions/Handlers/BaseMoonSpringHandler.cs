using System;

namespace Moonvalk.Animation {
	/// <summary>
	/// Base class for MoonSpringHandlers. These are containers that automate updating
	/// Node data each game tick with the use of extension methods.
	/// </summary>
	/// <typeparam name="Unit">The type of unit that will be animated.</typeparam>
	/// <typeparam name="SpringType">The type of MoonSpring object that will be used.</typeparam>
	/// <typeparam name="ParentType">The type of Node which is being animated.</typeparam>
	public abstract class BaseMoonSpringHandler<Unit, SpringType, ParentType> : IMoonSpringHandler
		where SpringType : BaseMoonSpring<Unit>, new() {
		/// <summary>
		/// The value used for animating properties.
		/// </summary>
		protected float[] _values;

		/// <summary>
		/// The property this container will animate upon.
		/// </summary>
		public MoonSpringProperty Property { get; protected set; }

		/// <summary>
		/// Any actions that will occur during Spring updates managed by this container.
		/// </summary>
		public Action OnUpdate { get; protected set; }

		/// <summary>
		/// Stores the Spring object being managed.
		/// </summary>
		public SpringType Spring { get; protected set; }

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
		protected BaseMoonSpringHandler(
			ref ParentType object_,
			MoonSpringProperty property_,
			Unit target_,
			MoonSpringParams parameters_,
			bool start_,
			Action onComplete_
		) {
			this.bindData(ref object_, property_);
			this.setupSpring(target_, parameters_, start_, onComplete_);
		}

		/// <summary>
		/// Gets the Spring found within this container casted to the requested type if applicable.
		/// </summary>
		/// <typeparam name="Type">The type of value being animated.</typeparam>
		/// <returns>Returns the Spring casted to the requested type if available.</returns>
		public IMoonSpring<Type> GetSpring<Type>() {
			return (IMoonSpring<Type>)this.Spring;
		}

		/// <summary>
		/// Deletes the Spring found within this container.
		/// </summary>
		public void Delete() {
			this.Spring.Delete();
			this.Spring = null;
			this.OnUpdate = null;
		}
		
		/// <summary>
		/// Starts the Spring found within this container.
		/// </summary>
		public void Start() {
			this.Spring.Start();
		}

		/// <summary>
		/// Called to bind required data to this container on initialization.
		/// </summary>
		/// <param name="object_">The object that this handler will manipulate.</param>
		/// <param name="property_">The property that will be adjusted.</param>
		protected void bindData(ref ParentType object_, MoonSpringProperty property_) {
			this._values = this.getInitialPropertyValues(ref object_, property_);
			this.OnUpdate = this.assignUpdateAction(ref object_, property_);
		}

		/// <summary>
		/// Called to set up new Spring objects managed by this container.
		/// </summary>
		/// <param name="target_">The target value to be animated to.</param>
		/// <param name="parameters_">Parameters used to manipulate how the animation will play.</param>
		/// <param name="start_">Flag that determines if this animation should begin immediately.</param>
		/// <param name="onComplete_">Action to be run once the animation is complete.</param>
		protected abstract void setupSpring(Unit target_, MoonSpringParams parameters_, bool start_, Action onComplete_);

		/// <summary>
		/// Called to assign a new Action called each game tick during animations that will
		/// manipulate the Node property.
		/// </summary>
		/// <param name="object_">The object that this handler will manipulate.</param>
		/// <param name="property_">The property to be adjusted.</param>
		/// <returns>Returns a new Action.</returns>
		protected abstract Action assignUpdateAction(ref ParentType object_, MoonSpringProperty property_);

		/// <summary>
		/// Gets the starting value for the Spring object to begin at.
		/// </summary>
		/// <param name="object_">The object that this handler will manipulate.</param>
		/// <param name="property_">The property to be adjusted.</param>
		/// <returns>Returns the initial value.</returns>
		protected abstract float[] getInitialPropertyValues(ref ParentType object_, MoonSpringProperty property_);
	}
}
