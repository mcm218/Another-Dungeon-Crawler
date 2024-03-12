using UnityEngine;

namespace _Scripts.Utilities {
    public interface INode {
        Color GetNodeColor();
    }

    public enum TransitionDirection {
        EntranceToExit = 0,
        ExitToEntrance = 1,
    }
    
    public static class NodeExtensions {
        public static void DrawNodeGizmo(this INode node, CapsuleCollider collider) {
            Gizmos.color = node.GetNodeColor();
            Gizmos.DrawWireSphere(collider.transform.position, collider.radius);
        }
    }
}