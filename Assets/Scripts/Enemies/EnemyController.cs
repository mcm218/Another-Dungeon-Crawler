using Interfaces;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Enemies {
    public class EnemyController : MonoBehaviour {
        [SerializeField]
        private GameObject target;
        [SerializeField]
        private IHealth lockedTarget;
        
        private EnemyModel       model;
        [Required]
        public BasicEnemy       enemy;
        
        private CircleCollider2D AttackCollider => enemy.AttackCollider;
        private Collider2D Body => enemy.Body;
        private Sword Sword => enemy.Sword;
        
        private Gun Gun => enemy.Gun;
        
        public List<Transform> children    = new List<Transform>();
        public Vector3         childOffset = Vector3.zero;
        
        
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

        private void Start() {
            AttackCollider.isTrigger = true;
            enemy.Health = model.health;
            // if (Sword != null) {
            //     AttackCollider.radius    = Sword.GetRange();
            //     Sword.Equip(enemy);
            // }
            // if (Gun != null) {
            //     AttackCollider.radius = Gun.GetRange();
            //     Gun.Equip(enemy);
            // }
            randomOffset = Random.Range(0, 100f);
        }
        
        float         wiggleDistance = 5f;
        float         wiggleSpeed    = 5;
        private float randomOffset;
        
        private void Update() {
            if (target == null) return;
            // var direction = target.transform.position - transform.position;
            // var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.Euler(0, 0, angle);
            
            enemy.GameObject.transform.position = Vector3.MoveTowards(enemy.GameObject.transform.position, target.transform.position, model.speed * Time.deltaTime);
            children.ForEach(child => child.position = enemy.GameObject.transform.position + childOffset);
            
            // if (lockedTarget != null && Gun != null && Gun.CanAttack()) {
            //     // Gun.PerformAttack();
            // }
            //
            // if (lockedTarget != null && Sword != null && Sword.CanAttack()) {
            //     // Sword.PerformAttack();
            // }
            // float yPosition = Mathf.Sin((Time.time + randomOffset) * wiggleSpeed) * wiggleDistance;
            // enemy.GameObject.transform.localRotation = Quaternion.Euler(0, 0, yPosition);
            // children.ForEach(child => child.localRotation = Quaternion.Euler(0, 0, -yPosition));
        }
        
 
        
        private readonly string[] TAGS_TO_ATTACK = {"Player"};
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.TryGetComponent<IHealth>(out var health) && Array.Exists(TAGS_TO_ATTACK, tag => tag == other.gameObject.tag)) {
                if (health.Body.IsTouching(AttackCollider)) {
                    lockedTarget = health;
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