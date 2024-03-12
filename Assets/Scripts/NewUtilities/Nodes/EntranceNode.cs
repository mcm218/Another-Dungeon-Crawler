using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace _Scripts.Utilities {
    public class EntranceNode : MonoBehaviour, INode {
        [InfoBox("Negative values will be ignored.")]
        public int previousSceneId = -1;
        
        [SerializeField, Required]
        private new CapsuleCollider collider;

        private bool isActive = false;
        
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
            return Color.green;
        }

        /// <summary>
        /// Only sets the previousSceneId if it hasn't been set yet.
        /// </summary>
        /// <param name="sceneIndex"></param>
        public void SetPreviousSceneIndexOnce(int sceneIndex) {
            if (previousSceneId < 0) { previousSceneId = sceneIndex; }
        }


        private void OnTriggerEnter(Collider other) {
            Debug.Log("EntranceNode trigger enter");
            if (!hasTriggered && other.gameObject.CompareTag("Player")) {
                Debug.Log("Player has entered the node.");
                if (previousSceneId >= 0 && isActive) {
                    BernaLoader.Instance.SoftLoad(previousSceneId, sourceSceneId, TransitionDirection.ExitToEntrance);
                    hasTriggered = true;
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            Debug.Log("EntranceNode trigger exit");
            if (other.gameObject.CompareTag("Player")) {
                Debug.Log("Player has exited the node.");
                if (previousSceneId >= 0 && !isActive) {
                    isActive = true;
                }
            }
        }

    }
}