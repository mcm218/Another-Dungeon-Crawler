using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Utilities {
    public class TriggerNode : MonoBehaviour, INode {
        
        [SerializeField, Required]
        private new CapsuleCollider collider;

        public UnityEvent<string> onEnterActions;
        
        public UnityEvent<string> onExitActions;
        
        [SerializeField]
        private List<string> tagsToTrigger = new List<string>();

        public Color GetNodeColor() {
            return Color.yellow;
        }

        private void OnTriggerEnter(Collider other) {
            Debug.Log($"Trigger node entered: {other.gameObject.tag}");
            if (tagsToTrigger.Contains(other.gameObject.tag)) {
                Debug.Log($"Triggered Enter");
                onEnterActions.Invoke(other.gameObject.tag);
            }
        }
        
        private void OnTriggerExit(Collider other) {
            Debug.Log($"Trigger node exited: {other.gameObject.tag}");
            if (tagsToTrigger.Contains(other.gameObject.tag)) {
                Debug.Log($"Triggered Exit");
                onExitActions.Invoke(other.gameObject.tag);
            }
        }

        private void OnDrawGizmos() {
            this.DrawNodeGizmo(collider);
        }

        [Button]
        private void TriggerEnter() {
            onEnterActions.Invoke("Test");
        }
        
        [Button]
        private void TriggerExit() {
            onExitActions.Invoke("Test");
        }
    }
}