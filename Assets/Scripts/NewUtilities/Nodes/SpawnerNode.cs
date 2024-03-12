using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts.Utilities {
    public class SpawnerNode : MonoBehaviour, INode {
        
        [SerializeField, Required]
        private new CapsuleCollider collider;
        
        [ShowInInspector]
        public List<Spawnable> spawnables = new List<Spawnable>();
        
        private List<GameObject> spawnablesInArea = new List<GameObject>();

        public Color GetNodeColor() {
            return Color.magenta;
        }

        private void OnDrawGizmosSelected() {
            this.DrawNodeGizmo(collider);
        }

        public void SpawnAll() {
            if (spawnablesInArea.Count > 0) {
                Debug.Log("There are still spawnables in the area.");
                return;
            }
            
            var _spawnedObjects = spawnables.ConvertAll(spawnable => spawnable.Spawn(transform.position));
            _spawnedObjects.ForEach(spawnedObject => Debug.Log($"Spawned {spawnedObject.name}"));
            spawnablesInArea.AddRange(_spawnedObjects);
        }
        
        public void SpawnRandom() {
            if (spawnablesInArea.Count > 0) {
                Debug.Log("There are still spawnables in the area.");
                return;
            }
            
            var _spawnedObject = spawnables[UnityEngine.Random.Range(0, spawnables.Count)].Spawn(transform.position);
            Debug.Log($"Spawned {_spawnedObject.name}");
            spawnablesInArea.Add(_spawnedObject);
        }
        
        public void AddSpawnable(Spawnable spawnable) {
            spawnables.Add(spawnable);
        }
        
        public int RemoveSpawnable(Spawnable spawnable) {
            return spawnables.RemoveAll(currentSpawnable => currentSpawnable == spawnable);
        }

        private void OnTriggerExit(Collider other) {
            if (spawnablesInArea.Contains(other.gameObject)) {
                spawnablesInArea.Remove(other.gameObject);
            }
            
            if (spawnablesInArea.Count == 0) {
                Debug.Log("All spawnables have left the area.");
            }
        }

        [Button]
        public void ForceSpawnRandom() {
            SpawnRandom();
        }
        
        [Button]
        public void ForceSpawnAll() {
            SpawnAll();
        }
    }
}