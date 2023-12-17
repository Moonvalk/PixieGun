using Godot;
using Moonvalk.Accessory;
using Moonvalk.Utilities.Algorithms;

namespace Moonvalk.Animation
{
    /// <summary>
    /// Object that handles Spring calculations for a Vector2 value.
    /// </summary>
    public class MoonSpringVec2 : BaseMoonSpring<Vector2>
    {
        /// <summary>
        /// Default constructor made without setting up references.
        /// </summary>
        public MoonSpringVec2()
        {
            // ...
        }

        /// <summary>
        /// Constructor for creating a new Spring.
        /// </summary>
        /// <param name="referenceValues_">Array of references to Vector2 values.</param>
        public MoonSpringVec2(params Ref<float>[] referenceValues_) : base(referenceValues_)
        {
            // ...
        }

        /// <summary>
        /// Calculates the necessary velocities to be applied to all Spring properties each game tick.
        /// </summary>
        protected override void CalculateForces()
        {
            for (var index = 0; index < Properties.Length; index += 2)
            {
                if (Properties[index] == null)
                {
                    Delete();
                    break;
                }

                var displacement = TargetProperties[index].x - Properties[index]();
                CurrentForce[index].x = MotionAlgorithms.SimpleHarmonicMotion(Tension, displacement, Dampening, Speed[index].x);

                displacement = TargetProperties[index].y - Properties[index + 1]();
                CurrentForce[index].y = MotionAlgorithms.SimpleHarmonicMotion(Tension, displacement, Dampening, Speed[index].y);
            }
        }

        /// <summary>
        /// Applies force to properties each frame.
        /// </summary>
        /// <param name="deltaTime_">The time elapsed between last and current game tick.</param>
        protected override void ApplyForces(float deltaTime_)
        {
            for (var index = 0; index < Properties.Length; index += 2)
            {
                Speed[index].x += CurrentForce[index].x * deltaTime_;
                Speed[index].y += CurrentForce[index].y * deltaTime_;
                Properties[index]() += Speed[index].x * deltaTime_;
                Properties[index + 1]() += Speed[index].y * deltaTime_;
            }
        }

        /// <summary>
        /// Determines if the minimum forces have been met to continue calculating Spring forces.
        /// </summary>
        /// <returns>Returns true if the minimum forces have been met.</returns>
        protected override bool MinimumForcesMet()
        {
            for (var index = 0; index < CurrentForce.Length; index += 2)
            {
                var current = new Vector2(Properties[index](), Properties[index + 1]());
                var metTarget = ConversionHelpers.Abs(TargetProperties[index] - current) >= MinimumForce[index];

                var metMinimumForce = ConversionHelpers.Abs(CurrentForce[index] + Speed[index]) >= MinimumForce[index];

                if (metTarget && metMinimumForce) return true;
            }

            return false;
        }

        /// <summary>
        /// Assigns the minimum force required until the Spring is completed based on inputs.
        /// </summary>
        protected override void SetMinimumForce()
        {
            MinimumForce = new Vector2[Properties.Length];
            for (var index = 0; index < Properties.Length; index += 2)
            {
                var current = new Vector2(Properties[index](), Properties[index + 1]());
                MinimumForce[index] = MoonSpring.DefaultMinimumForcePercentage * ConversionHelpers.Abs(TargetProperties[index] - current);
            }
        }

        /// <summary>
        /// Determines if there is a need to apply force to this Spring to meet target values.
        /// </summary>
        /// <returns>Returns true if forces need to be applied</returns>
        protected override bool NeedToApplyForce()
        {
            for (var index = 0; index < Properties.Length; index += 2)
            {
                var current = new Vector2(Properties[index](), Properties[index + 1]());
                if (current != TargetProperties[index]) return true;
            }

            return false;
        }

        /// <summary>
        /// Snaps all Spring properties directly to their target values.
        /// </summary>
        protected override void SnapSpringToTarget()
        {
            for (var index = 0; index < Properties.Length; index += 2)
            {
                Properties[index]() = TargetProperties[index].x;
                Properties[index + 1]() = TargetProperties[index].y;
            }
        }
    }
}