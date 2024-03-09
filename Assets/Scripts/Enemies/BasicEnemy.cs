using Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Weapons;

namespace Enemies {
    
    [RequireComponent( typeof(SpriteRenderer))]
    public class BasicEnemy : MonoBehaviour, IHealth, IControllable, IAttacker {
        // public  float            Health { get; set; }
        public float            Health { get; set; } = 100;
        
        private CircleCollider2D body;
        private CircleCollider2D attackCollider;
        private SpriteRenderer   sprite;

        [SerializeField]
        private Weapon _weapon;

        public IWeapon Weapon => _weapon as IWeapon;

        public Collider2D       Body           => body;
        public CircleCollider2D AttackCollider => attackCollider;
        
        
        [field:SerializeField]
        public UnityEvent OnDeath { get; private set; }
        private void Awake() {
            var colliders = GetComponents<CircleCollider2D>();
            attackCollider = colliders[0];
            body = colliders[1];
            
            sprite = GetComponent<SpriteRenderer>();
        }
        
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

    }
}