using Godot;
using Moonvalk.Accessory;
using Moonvalk.Utilities.Algorithms;

namespace Moonvalk.Animation
{
    /// <summary>
    /// Object that handles Spring calculations for a singular float value.
    /// </summary>
    public class MoonSpring : BaseMoonSpring<float>
    {
        /// <summary>
        /// Default constructor made without setting up references.
        /// </summary>
        public MoonSpring()
        {
            // ...
        }

        /// <summary>
        /// Constructor for creating a new Spring.
        /// </summary>
        /// <param name="referenceValues_">Array of references to float values.</param>
        public MoonSpring(params Ref<float>[] referenceValues_) : base(referenceValues_)
        {
            // ...
        }

        /// <summary>
        /// Calculates the necessary velocities to be applied to all Spring properties each game tick.
        /// </summary>
        protected override void CalculateForces()
        {
            for (var i = 0; i < Properties.Length; i++)
            {
                if (Properties[i] == null)
                {
                    Delete();
                    break;
                }

                var displacement = TargetProperties[i] - Properties[i]();
                CurrentForce[i] = MotionAlgorithms.SimpleHarmonicMotion(Tension, displacement, Dampening, Speed[i]);
            }
        }

        /// <summary>
        /// Applies force to properties each frame.
        /// </summary>
        /// <param name="deltaTime_">The time elapsed between last and current game tick.</param>
        protected override void ApplyForces(float deltaTime_)
        {
            for (var i = 0; i < Properties.Length; i++)
            {
                Speed[i] += CurrentForce[i] * deltaTime_;
                Properties[i]() += Speed[i] * deltaTime_;
            }
        }

        /// <summary>
        /// Determines if the minimum forces have been met to continue calculating Spring forces.
        /// </summary>
        /// <returns>Returns true if the minimum forces have been met.</returns>
        protected override bool MinimumForcesMet()
        {
            for (var index = 0; index < CurrentForce.Length; index++)
            {
                var metTarget = Mathf.Abs(TargetProperties[index] - Properties[index]()) >= MinimumForce[index];

                var metMinimumForce = Mathf.Abs(CurrentForce[index] + Speed[index]) >= MinimumForce[index];

                if (metTarget && metMinimumForce) return true;
            }

            return false;
        }

        /// <summary>
        /// Assigns the minimum force required until the Spring is completed based on inputs.
        /// </summary>
        protected override void SetMinimumForce()
        {
            MinimumForce = new float[Properties.Length];
            for (var index = 0; index < Properties.Length; index++)
                MinimumForce[index] = DefaultMinimumForcePercentage * Mathf.Abs(TargetProperties[index] - Properties[index]());
        }

        /// <summary>
        /// Determines if there is a need to apply force to this Spring to meet target values.
        /// </summary>
        /// <returns>Returns true if forces need to be applied</returns>
        protected override bool NeedToApplyForce()
        {
            for (var index = 0; index < Properties.Length; index++)
                if (Properties[index]() != TargetProperties[index])
                    return true;

            return false;
        }

        /// <summary>
        /// Snaps all Spring properties directly to their target values.
        /// </summary>
        protected override void SnapSpringToTarget()
        {
            for (var index = 0; index < Properties.Length; index++) Properties[index]() = TargetProperties[index];
        }
    }
}