using Godot;
using Moonvalk.Nodes;
using System;
using System.Collections.Generic;

namespace Moonvalk.Animation {
	/// <summary>
	/// Contains extension methods made for animating Godot.Node objects with the use of MoonWobbles.
	/// </summary>
	public static class MoonWobbleExtensions {
		/// <summary>
		/// Stores reference to all MoonWobbleGroups used to manage MoonWobbleHandlers by the manipulated Godot.Node.
		/// Each node being animated is assigned its own group.
		/// </summary>
		public static Dictionary<Node, MoonWobbleGroup> WobbleGroups { get; set; } = new Dictionary<Node, MoonWobbleGroup>();

		/// <summary>
		/// Gets the matching MoonWobbleGroup for the given Godot object, when available. If no group
		/// exists a new one will be created.
		/// </summary>
		/// <param name="object_">The object to receive a group for.</param>
		/// <returns>Returns the matching MoonWobbleGroup for the input object.</returns>
		public static MoonWobbleGroup GetMoonWobbleGroup(Node object_) {
			MoonWobbleGroup group;
			if (MoonWobbleExtensions.WobbleGroups.ContainsKey(object_)) {
				group = MoonWobbleExtensions.WobbleGroups[object_];
			} else {
				group = new MoonWobbleGroup();
				MoonWobbleExtensions.WobbleGroups.Add(object_, group);
			}
			return group;
		}

		/// <summary>
		/// Gets an Action used to remove the provided object / property pair Wobble handler once an
		/// animation is finished.
		/// </summary>
		/// <param name="object_">The object to create an action for.</param>
		/// <param name="property_">The property to create an action for.</param>
		/// <returns>A new Action that will remove properties from the map once complete.</returns>
		public static Action GetRemoveAction(Node object_, MoonWobbleProperty property_) {
			return () => {
				if (!object_.Validate()) {
					MoonWobbleExtensions.Delete(object_);
					return;
				}
				if (MoonWobbleExtensions.WobbleGroups.ContainsKey(object_)) {
					var group = MoonWobbleExtensions.WobbleGroups[object_];
					if (group.WobbleHandlers.Remove(property_) && group.WobbleHandlers.Count == 0) {
						MoonWobbleExtensions.WobbleGroups.Remove(object_);
					}
				}
			};
		}

		/// <summary>
		/// Clears all Wobbles currently being applied to nodes.
		/// </summary>
		public static void ClearAll() {
			if (MoonWobbleExtensions.WobbleGroups != null) {
				foreach (var group in MoonWobbleExtensions.WobbleGroups.Values) {
					group?.Clear();
				}
				MoonWobbleExtensions.WobbleGroups.Clear();
			}
		}

		/// <summary>
		/// Called to delete all animations applied to the specified node object.
		/// </summary>
		/// <param name="node_">The node to remove animations from.</param>
		public static void Delete(Node node_) {
			if (MoonWobbleExtensions.WobbleGroups.ContainsKey(node_)) {
				MoonWobbleExtensions.WobbleGroups[node_].Clear();
				MoonWobbleExtensions.WobbleGroups.Remove(node_);
			}
		}
	}
}
