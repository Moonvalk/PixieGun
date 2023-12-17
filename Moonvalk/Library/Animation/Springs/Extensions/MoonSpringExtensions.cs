using Godot;
using Moonvalk.Nodes;
using System;
using System.Collections.Generic;

namespace Moonvalk.Animation {
	/// <summary>
	/// Contains extension methods made for animating Godot.Node objects with the use of MoonSprings.
	/// </summary>
	public static class MoonSpringExtensions {
		/// <summary>
		/// Stores reference to all MoonSpringGroups used to manage MoonSpringHandlers by the manipulated Godot.Node.
		/// Each node being animated is assigned its own group.
		/// </summary>
		public static Dictionary<Node, MoonSpringGroup> SpringGroups { get; set; } = new Dictionary<Node, MoonSpringGroup>();

		/// <summary>
		/// Gets the matching MoonSpringGroup for the given Godot object, when available. If no group
		/// exists a new one will be created.
		/// </summary>
		/// <param name="object_">The object to receive a group for.</param>
		/// <returns>Returns the matching MoonSpringGroup for the input object.</returns>
		public static MoonSpringGroup GetMoonSpringGroup(Node object_) {
			MoonSpringGroup group;
			if (MoonSpringExtensions.SpringGroups.ContainsKey(object_)) {
				group = MoonSpringExtensions.SpringGroups[object_];
			} else {
				group = new MoonSpringGroup();
				MoonSpringExtensions.SpringGroups.Add(object_, group);
			}
			return group;
		}

		/// <summary>
		/// Gets an Action used to remove the provided object / property pair Spring handler once an
		/// animation is finished.
		/// </summary>
		/// <param name="object_">The object to create an action for.</param>
		/// <param name="property_">The property to create an action for.</param>
		/// <returns>A new Action that will remove properties from the map once complete.</returns>
		public static Action GetRemoveAction(Node object_, MoonSpringProperty property_) {
			return () => {
				if (!object_.Validate()) {
					MoonSpringExtensions.Delete(object_);
					return;
				}
				if (MoonSpringExtensions.SpringGroups.ContainsKey(object_)) {
					var group = MoonSpringExtensions.SpringGroups[object_];
					if (group.SpringHandlers.Remove(property_) && group.SpringHandlers.Count == 0) {
						MoonSpringExtensions.SpringGroups.Remove(object_);
					}
				}
			};
		}

		/// <summary>
		/// Clears all Springs currently being applied to nodes.
		/// </summary>
		public static void ClearAll() {
			if (MoonSpringExtensions.SpringGroups != null) {
				foreach (var group in MoonSpringExtensions.SpringGroups.Values) {
					group?.Clear();
				}
				MoonSpringExtensions.SpringGroups.Clear();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="node_"></param>
		public static void Delete(Node node_) {
			if (MoonSpringExtensions.SpringGroups.ContainsKey(node_)) {
				MoonSpringExtensions.SpringGroups[node_].Clear();
				MoonSpringExtensions.SpringGroups.Remove(node_);
			}
		}
	}
}
