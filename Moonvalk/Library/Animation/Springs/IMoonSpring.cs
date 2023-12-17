using System;
using Moonvalk.Accessory;
using Moonvalk.Systems;

namespace Moonvalk.Animation
{
    /// <summary>
    /// Contract that a Spring object must fulfill.
    /// </summary>
    public interface IMoonSpring<Unit> : IQueueItem
    {
        /// <summary>
        /// Sets all reference values that this Spring will manipulate.
        /// </summary>
        /// <param name="referenceValues_">Array of references to values.</param>
        /// <returns>Returns reference to this Spring object</returns>
        BaseMoonSpring<Unit> SetReferences(params Ref<float>[] referenceValues_);

        /// <summary>
        /// Starts this Spring with the current settings if there is a need to apply forces.
        /// </summary>
        /// <returns>Returns reference to this Spring object</returns>
        BaseMoonSpring<Unit> Start();

        /// <summary>
        /// Stops this Spring.
        /// </summary>
        /// <returns>Returns reference to this Spring object</returns>
        BaseMoonSpring<Unit> Stop();

        /// <summary>
        /// Sets the dampening factor applied to this spring.
        /// </summary>
        /// <param name="dampening_">New dampening factor.</param>
        /// <returns>Returns reference to this spring.</returns>
        BaseMoonSpring<Unit> SetDampening(float dampening_);

        /// <summary>
        /// Sets the tension factor applied to this spring.
        /// </summary>
        /// <param name="tension_">New tension factor.</param>
        /// <returns>Returns reference to this spring.</returns>
        BaseMoonSpring<Unit> SetTension(float tension_);

        /// <summary>
        /// Sets all parameters from a reference object.
        /// </summary>
        /// <param name="parameters_">All properties to be assigned for this Spring.</param>
        /// <returns>Returns this Spring object.</returns>
        BaseMoonSpring<Unit> SetParameters(MoonSpringParams parameters_);

        /// <summary>
        /// Applies a new target spring height and begins animating towards reaching that value.
        /// </summary>
        /// <param name="targetProperties_">Target spring heights for each property.</param>
        /// <returns>Returns reference to this spring.</returns>
        BaseMoonSpring<Unit> To(params Unit[] targetProperties_);

        /// <summary>
        /// Snaps each spring property to the provided target values.
        /// </summary>
        /// <param name="targetProperties_">Target spring heights for each property.</param>
        /// <returns>Returns reference to this spring.</returns>
        BaseMoonSpring<Unit> Snap(params Unit[] targetProperties_);

        /// <summary>
        /// Removes this Spring on the following game tick.
        /// </summary>
        /// <returns>Returns reference to this Spring object</returns>
        BaseMoonSpring<Unit> Delete();

        /// <summary>
        /// Defines Actions that will occur when this Spring begins.
        /// </summary>
        /// <param name="tasksToAdd_">Array of Actions to add.</param>
        /// <returns>Returns this Spring object.</returns>
        BaseMoonSpring<Unit> OnStart(params Action[] tasksToAdd_);

        /// <summary>
        /// Defines Actions that will occur when this Spring updates.
        /// </summary>
        /// <param name="tasksToAdd_">Array of Actions to add.</param>
        /// <returns>Returns this Spring object.</returns>
        BaseMoonSpring<Unit> OnUpdate(params Action[] tasksToAdd_);

        /// <summary>
        /// Defines Actions that will occur once this Spring has completed.
        /// </summary>
        /// <param name="tasksToAdd_">Array of Actions to add.</param>
        /// <returns>Returns this Spring object.</returns>
        BaseMoonSpring<Unit> OnComplete(params Action[] tasksToAdd_);

        /// <summary>
        /// Gets the current state of this Spring object.
        /// </summary>
        /// <returns>Returns the current state.</returns>
        MoonSpringState GetCurrentState();
    }
}