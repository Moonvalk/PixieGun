using System;
using Godot;
using Moonvalk.Animation;
using Object = Godot.Object;

namespace Moonvalk.Nodes
{
    /// <summary>
    /// Common extension methods for Godot Nodes.
    /// </summary>
    public static class NodeExtensions
    {
        /// <summary>
        /// Queues this node and all children found within the tree for deletion.
        /// </summary>
        /// <param name="node_">The node to be queued for deletion.</param>
        public static void QueueFreeAll(this Node node_)
        {
            node_.QueueFree();
            node_.ApplyActionToChildren(QueueFreeAll);
        }

        /// <summary>
        /// Checks if this node is still valid within the context of Godot's current tree.
        /// </summary>
        /// <param name="node_">The node to check for validity.</param>
        /// <returns>Returns true when this node is still valid, false when it is deleted or queued for deletion.</returns>
        public static bool Validate(this Node node_)
        {
            return node_ != null && Object.IsInstanceValid(node_) && !node_.IsQueuedForDeletion();
        }

        /// <summary>
        /// Called to add an instance of a packed scene as a child to this node.
        /// </summary>
        /// <param name="node_">The node to add an instance to.</param>
        /// <param name="scene_">The packed scene to instantiate and add as a child.</param>
        /// <returns>Returns the newly instantiated node.</returns>
        public static NodeType AddInstance<NodeType>(this Node node_, PackedScene scene_)
            where NodeType : Node
        {
            var instance = (NodeType)scene_.Instance();
            node_.AddChild(instance);
            return instance;
        }

        /// <summary>
        /// Called to move a child node. This ensures that the child is still valid, stops all animations,
        /// queues each element for deletion in memory, and finally removes the element from the parent,
        /// </summary>
        /// <param name="node_">The parent node to remove a child from.</param>
        /// <param name="item_">The child to be removed, when applicable.</param>
        public static void RemoveChildren(this Node node_, Node item_)
        {
            if (item_.Validate())
            {
                item_.ClearAnimationsForAll();
                item_.QueueFreeAll();
                node_.RemoveChild(item_);
            }
        }

        /// <summary>
        /// Called to ensure that the provided instance reference is made a singleton. If an instance has already been
        /// assigned to the provided reference this node will be queued for deletion.
        /// </summary>
        /// <typeparam name="NodeType">The type of node expected to be returned when no singleton exists yet.</typeparam>
        /// <param name="node_">The node that will be provided a singleton instance.</param>
        /// <param name="instance_">The reference for checking whether a singleton instance exists.</param>
        /// <returns>Returns the singleton instance.</returns>
        public static NodeType MakeSingleton<NodeType>(this Node node_, NodeType instance_)
            where NodeType : Node
        {
            if (instance_ != null)
            {
                node_.QueueFree();
                if (node_.GetParent().Validate())
                    node_.GetParent().RemoveChild(node_);

                return instance_;
            }

            return node_ as NodeType;
        }

        /// <summary>
        /// Applies the provided action to all child node elements.
        /// </summary>
        /// <param name="node_">The input parent node.</param>
        /// <param name="action_">The action to be applied recursively.</param>
        public static void ApplyActionToChildren(this Node node_, Action<Node> action_)
        {
            var children = node_.GetChildren();
            for (var index = 0; index < children.Count; index++) action_.Invoke(children[index] as Node);
        }
    }
}