using System;
using UnityEngine;

namespace Enemies {
    [RequireComponent(typeof(BasicEnemy))]
    public class EnemyController : MonoBehaviour {
        [SerializeField]
        private GameObject target;
        
        private EnemyModel model;

        private void Awake() {
            if (target == null) {
                var potentialTarget = GameObject.FindGameObjectWithTag("Player");
                if (potentialTarget != null) {
                    target = potentialTarget;
                }
            }

            if (model == null) {
                model = ScriptableObject.CreateInstance<EnemyModel>();
            }
        }
        
        private void Update() {
            if (target == null) return;
            var direction = target.transform.position - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, model.speed * Time.deltaTime);
        }

    }
}