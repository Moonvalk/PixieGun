using Godot;
using Moonvalk.Nodes;

namespace Moonvalk.Animation
{
	/// <summary>
	/// Contains extension methods for manipulating Godot.Control nodes with animations.
	/// </summary>
	public static class NodeMoonAnimationExtensions
	{
		/// <summary>
		/// Called to clear all animations found on the specified node.
		/// </summary>
		/// <param name="node_">The node to clear animations for.</param>
		public static void ClearAnimations(this Node node_)
		{
			MoonTweenExtensions.Delete(node_);
			MoonSpringExtensions.Delete(node_);
			MoonWobbleExtensions.Delete(node_);
		}

		/// <summary>
		/// Called to clear all animations found on the specified node and all of its children.
		/// </summary>
		/// <param name="node_">The node to clear animations for itself and its children.</param>
		public static void ClearAnimationsForAll(this Node node_)
		{
			node_.ClearAnimations();
			node_.ApplyActionToChildren(ClearAnimationsForAll);
		}
	}
}
