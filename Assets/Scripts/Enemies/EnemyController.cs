using Interfaces;
using System;
using UnityEngine;

namespace Enemies {
    [RequireComponent(typeof(CircleCollider2D), typeof(CircleCollider2D), typeof(BasicEnemy))]
    public class EnemyController : MonoBehaviour {
        [SerializeField]
        private GameObject target;
        [SerializeField]
        private IHealth lockedTarget;
        
        private EnemyModel       model;
        private BasicEnemy       enemy;
        
        private CircleCollider2D AttackCollider => enemy.AttackCollider;
        private Collider2D Body => enemy.Body;
        private IWeapon Weapon => enemy.Weapon;
        
        private float timeSinceLastAttack = 0f;
        
        
        private void Awake() {
            enemy = GetComponent<BasicEnemy>();
            
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

        private void Start() {
            AttackCollider.radius   = enemy.Weapon.GetRange();
            AttackCollider.isTrigger = true;

            enemy.Health = model.health;
        }
        

        private void Update() {
            if (target == null) return;
            var direction = target.transform.position - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, model.speed * Time.deltaTime);
            
            if (lockedTarget != null) {
                timeSinceLastAttack += Time.deltaTime;
                if (timeSinceLastAttack >= Weapon.GetAttackRate()) {
                    enemy.Attack(lockedTarget);
                    timeSinceLastAttack = 0f;
                }
            }
        }
        
        private readonly string[] TAGS_TO_ATTACK = {"Player"};
        
        private void OnTriggerEnter2D(Collider2D other) {
            Debug.Log("Trigger with " + other.gameObject.name);
            if (other.gameObject.TryGetComponent<IHealth>(out var health) && Array.Exists(TAGS_TO_ATTACK, tag => tag == other.gameObject.tag)) {
                if (health.Body.IsTouching(AttackCollider)) {
                    lockedTarget = health;
                    enemy.Attack(health);
                }
            }
        }
        
        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.TryGetComponent<IHealth>(out var health) && Array.Exists(TAGS_TO_ATTACK, tag => tag == other.gameObject.tag)) {
                if (!health.Body.IsTouching(AttackCollider)) {
                    lockedTarget = null;
                }
            }
        }

    }
}