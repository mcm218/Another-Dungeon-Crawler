using Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Weapons;

namespace Enemies {
    
    [RequireComponent(typeof(CircleCollider2D), typeof(CircleCollider2D), typeof(SpriteRenderer))]
    public class BasicEnemy : MonoBehaviour, IHealth, IControllable, IAttacker {
        public float Health { get; private set; }

        [SerializeField]
        private Weapon _weapon;

        public IWeapon Weapon => _weapon as IWeapon;

        private CircleCollider2D attackCollider;
        private CircleCollider2D body;
        private SpriteRenderer   sprite;
        
        public Collider2D Body => body;
        
        private List<IHealth> targets = new List<IHealth>();
        
        [field:SerializeField]
        public UnityEvent OnDeath { get; private set; }
        
        private EnemyModel model;

        private void Awake() {
            if (model == null) {
                model = ScriptableObject.CreateInstance<EnemyModel>();
            }
            
            var colliders = GetComponents<CircleCollider2D>();
            attackCollider = colliders[0];
            body = colliders[1];
            
            sprite = GetComponent<SpriteRenderer>();

            attackCollider.radius = Weapon.GetRange();
            attackCollider.isTrigger = true;

            Health = model.health;
        }

        /**
         * This method is called when the enemy is created
         */
        public bool Damage(float damage) {
            Debug.Log(name + " took " + damage + " damage");
            Health -= damage;
            sprite.color = Color.red;
            
            if (Health <= 0) {
                return true;
            }
            
            Invoke(nameof(ResetColor), 0.1f);
            return false;
        }

        public void Destroy() {
            gameObject.SetActive(false);
        }
        
        private void ResetColor() {
            sprite.color = Color.white;
        }

        public void ResetHealth() {
            Health = 100;
        }

        public void Attack(IHealth target) {
            Weapon.OnHit.Invoke(target);
            if (target.Damage(Weapon.CalculateDamage())) {
                target.Destroy();
            }
        }

        private readonly string[] TAGS_TO_ATTACK = {"Player"};
        
        private void OnTriggerEnter2D(Collider2D other) {
            Debug.Log("Trigger with " + other.gameObject.name);
            if (other.gameObject.TryGetComponent<IHealth>(out var health) && Array.Exists(TAGS_TO_ATTACK, tag => tag == other.gameObject.tag)) {
                if (health.Body.IsTouching(attackCollider)) {
                    // targets.Add(health);
                    Attack(health);
                }
            }
        }
        
        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.TryGetComponent<IHealth>(out var health) && Array.Exists(TAGS_TO_ATTACK, tag => tag == other.gameObject.tag)) {
                if (!health.Body.IsTouching(attackCollider)) {
                    // targets.Remove(health);
                }
            }
        }
    }
}