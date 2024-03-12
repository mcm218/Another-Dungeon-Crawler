using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace _Scripts.Utilities {
    public class DespawnNode : MonoBehaviour, INode {
        [Required]
        public int sceneToDespawn = -1;
        
        [SerializeField, Required]
        private new CapsuleCollider collider;
        
        private bool hasTriggered = false;
        
        private void Awake() {
            if (collider == null) {
                collider = GetComponent<CapsuleCollider>();
            }
        }

        private void OnDrawGizmos() {
            this.DrawNodeGizmo(collider);
        }

        public Color GetNodeColor() {
            return Color.red;
        }

        private void OnTriggerEnter(Collider other) {
            Debug.Log("Despawn node trigger enter.");
            if (!hasTriggered && other.gameObject.CompareTag("Player")) {
                Debug.Log("Player has entered the node.");
                if (sceneToDespawn >= 0 && !hasTriggered) {
                    BernaLoader.Instance.SoftUnload(sceneToDespawn);
                    hasTriggered = true;
                }
            }
        }

        public void SetSceneToDespawnOnce(int activeSceneBuildIndex) {
            if (sceneToDespawn < 0) {
                sceneToDespawn = activeSceneBuildIndex;
            }
        }
    }
}