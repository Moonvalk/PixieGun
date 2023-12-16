using Godot;
using Moonvalk.Nodes;
using System;
using System.Collections.Generic;

namespace Moonvalk.Animation {
	/// <summary>
	/// Contains extension methods made for animating Godot.Node objects with the use of MoonTweens.
	/// </summary>
	public static class MoonTweenExtensions {
		/// <summary>
		/// Stores reference to all MoonTweenGroups used to manage MoonTweenHandlers by the manipulated Godot.Node.
		/// Each node being animated is assigned its own group.
		/// </summary>
		public static Dictionary<Node, MoonTweenGroup> TweenGroups { get; set; } = new Dictionary<Node, MoonTweenGroup>();

		/// <summary>
		/// Gets the matching MoonTweenGroup for the given Godot object, when available. If no group
		/// exists a new one will be created.
		/// </summary>
		/// <param name="object_">The object to receive a group for.</param>
		/// <returns>Returns the matching MoonTweenGroup for the input object.</returns>
		public static MoonTweenGroup GetMoonTweenGroup(Node object_) {
			MoonTweenGroup group;
			if (MoonTweenExtensions.TweenGroups.ContainsKey(object_)) {
				group = MoonTweenExtensions.TweenGroups[object_];
			} else {
				group = new MoonTweenGroup();
				MoonTweenExtensions.TweenGroups.Add(object_, group);
			}
			return group;
		}

		/// <summary>
		/// Gets an Action used to remove the provided object / property pair Tween handler once an
		/// animation is finished.
		/// </summary>
		/// <param name="object_">The object to create an action for.</param>
		/// <param name="property_">The property to create an action for.</param>
		/// <returns>A new Action that will remove properties from the map once complete.</returns>
		public static Action GetRemoveAction(Node object_, MoonTweenProperty property_) {
			return () => {
				if (!object_.Validate()) {
					MoonTweenExtensions.Delete(object_);
					return;
				}
				if (MoonTweenExtensions.TweenGroups.ContainsKey(object_)) {
					MoonTweenGroup group = MoonTweenExtensions.TweenGroups[object_];
					if (group.TweenHandlers.Remove(property_) && group.TweenHandlers.Count == 0) {
						MoonTweenExtensions.TweenGroups.Remove(object_);
					}
				}
			};
		}

		/// <summary>
		/// Clears all Tweens currently being applied to nodes.
		/// </summary>
		public static void ClearAll() {
			if (MoonTweenExtensions.TweenGroups != null) {
				foreach (MoonTweenGroup group in MoonTweenExtensions.TweenGroups.Values) {
					group?.Clear();
				}
				MoonTweenExtensions.TweenGroups.Clear();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="node_"></param>
		public static void Delete(Node node_) {
			if (MoonTweenExtensions.TweenGroups.ContainsKey(node_)) {
				MoonTweenExtensions.TweenGroups[node_].Clear();
				MoonTweenExtensions.TweenGroups.Remove(node_);
			}
		}
	}
}
