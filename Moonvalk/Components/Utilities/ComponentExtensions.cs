using System;
using System.Collections.Generic;
using Godot;
using Array = Godot.Collections.Array;

namespace Moonvalk.Components
{
	/// <summary>
	/// Extension methods for accessing and manipulating component nodes.
	/// </summary>
	public static class ComponentExtensions
	{
		/// <summary>
		/// Gets the requested component node if it is a child of this node.
		/// </summary>
		/// <typeparam name="NodeType">The component type expected.</typeparam>
		/// <param name="node_">This node object.</param>
		/// <returns>Returns the child component node, if found.</returns>
		public static NodeType GetComponent<NodeType>(this Node node_)
			where NodeType : Node
		{
			foreach (Node child in node_.GetChildren())
			{
				if (child.GetType() == typeof(NodeType)) return child as NodeType;
			}
			
			return default(NodeType);
		}

		/// <summary>
		/// Gets all existing requested component nodes if they are a child of this node.
		/// </summary>
		/// <typeparam name="NodeType">The component type expected.</typeparam>
		/// <param name="node_">This node object.</param>
		/// <param name="ignore_">An array of node types to ignore when found.</param>
		/// <returns>Returns all child component nodes, if found.</returns>
		public static List<NodeType> GetAllComponents<NodeType>(this Node node_, params Type[] ignore_)
			where NodeType : Node
		{
			var nodes = new List<NodeType>();
			GetAllComponentsHelper(node_, ref nodes, ignore_);
			
			return nodes;
		}

		/// <summary>
		/// Helper method for traversing all children nodes in order to build a list of all components of specified type.
		/// </summary>
		/// <typeparam name="NodeType">The component type expected.</typeparam>
		/// <param name="node_">The parent node to check first.</param>
		/// <param name="nodeList_">A list actively being built of all components.</param>
		/// <param name="ignore_">An array of node types to ignore when found.</param>
		private static void GetAllComponentsHelper<NodeType>(Node node_, ref List<NodeType> nodeList_, params Type[] ignore_)
			where NodeType : Node
		{
			var nodeType = node_.GetType();
			for (var index = 0; index < ignore_.Length; index++)
			{
				if (nodeType == ignore_[index]) return;
			}
			
			if (nodeType == typeof(NodeType)) nodeList_.Add(node_ as NodeType);
			
			var children = node_.GetChildren();
			for (var index = 0; index < children.Count; index++)
			{
				GetAllComponentsHelper(children[index] as Node, ref nodeList_, ignore_);
			}
		}
	}
}
