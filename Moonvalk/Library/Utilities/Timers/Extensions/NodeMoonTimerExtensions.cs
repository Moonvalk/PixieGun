using System;
using Godot;
using Moonvalk.Nodes;

namespace Moonvalk.Utilities {
	/// <summary>
	/// Extensions available to MoonTimer and Node objects for interacting with one another.
	/// </summary>
	public static class NodeMoonTimerExtensions {
		/// <summary>
		/// Called to add validations for specified nodes before executing any other completion actions
		/// on the specified timer object. If any node validations fail this timer will not execute when complete.
		/// </summary>
		/// <param name="timer_">The timer to add validations to.</param>
		/// <param name="nodes_">The nodes to be validated.</param>
		/// <returns>Returns the timer object.</returns>
		public static MoonTimer Validate(this MoonTimer timer_, params Node[] nodes_) {
			timer_.OnComplete(0, () => {
				foreach(Node node in nodes_) {
					if (!node.Validate()) {
						timer_.Stop();
						timer_.Reset();
						break;
					}
				}
			});
			return timer_;
		}

		/// <summary>
		/// Called to wait for a duration in seconds before executing tasks. This ensures that the parent node is
		/// valid and not queued for deletion before executing.
		/// </summary>
		/// <param name="node_">The node used for validation.</param>
		/// <param name="duration_">Duration in seconds to wait.</param>
		/// <param name="onComplete_">Completion actions to be run when waiting is complete.</param>
		/// <returns>Returns the new timer object.</returns>
		public static MoonTimer Wait(this Node node_, float duration_, params Action[] onComplete_) {
			return MoonTimer.Wait(duration_, onComplete_).Validate(node_);
		}
	}
}