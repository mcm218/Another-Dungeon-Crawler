using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace _Scripts.Utilities {
    [ExecuteInEditMode]
    public class Follower : MonoBehaviour {
        [SerializeField]
        private Transform target;
        
        [SerializeField]
        private Vector3 offset;
        
        [SerializeField,Range(0f,1f), InfoBox("The higher the value, the faster the camera will follow the target")]
        private float smoothSpeed = 0.125f;
        
        [SerializeField]
        private bool followRotation = true;

        private void LateUpdate() {
            if (target == null) return;
            
            #if UNITY_EDITOR
            // If not in play mode, immediately set position to target position.
            if (!Application.isPlaying) {
                transform.position = target.position + offset;
                return;
            }
            #endif
            
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            if (followRotation) transform.rotation = target.rotation;
        }

        private void Awake() {
            if (target == null) {
                var potentialTarget = GameObject.FindGameObjectWithTag("MainCamera")?.transform;
                if (potentialTarget != null) {
                    target = potentialTarget;
                }
            }
        }

        public void SetTarget(Transform transform1) {
            target = transform1;
        }
    }
}