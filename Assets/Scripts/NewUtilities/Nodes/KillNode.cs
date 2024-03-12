using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Utilities {
    public class KillNode : MonoBehaviour, INode {
        [SerializeField, Required]
        private new CapsuleCollider collider;
        
        [SerializeField]
        private List<string> tagsToKill = new List<string>();


        private void Awake() {
            if (collider == null) {
                collider = GetComponent<CapsuleCollider>();
            }
        }

        public Color GetNodeColor() {
            return Color.red;
        }
        
        private void OnDrawGizmos() {
            this.DrawNodeGizmo(collider);
        }
        
        private void OnTriggerEnter(Collider other) {
            Debug.Log($"Kill node entered: {other.gameObject.tag}");
            if (tagsToKill.Contains(other.gameObject.tag)) {
                Debug.Log($"Killing {other.gameObject.name}");
                Destroy(other.gameObject);
            }
        }
        
    }
}