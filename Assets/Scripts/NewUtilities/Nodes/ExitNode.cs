using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace _Scripts.Utilities {
    public class ExitNode : MonoBehaviour, INode {
        
        [Required]
        public int nextSceneId;
        
        [SerializeField, Required]
        private new CapsuleCollider collider;

        private bool hasTriggered = false;
        
        private int sourceSceneId;
        
        private void Awake() {
            if (collider == null) {
                collider = GetComponent<CapsuleCollider>();
            }
            
            sourceSceneId = gameObject.scene.buildIndex;
        }

        private void OnDrawGizmos() {
            this.DrawNodeGizmo(collider);
        }

        public Color GetNodeColor() {
            return Color.blue;
        }

        private void OnTriggerEnter(Collider other) {
            Debug.Log("ExitNode trigger enter");
            if (!hasTriggered && other.gameObject.CompareTag("Player")) {
                Debug.Log("Player has entered the node.");
                BernaLoader.Instance.SoftLoad(nextSceneId, sourceSceneId);
                hasTriggered = true;
            }
        }
    }

}