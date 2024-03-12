using Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Weapons;

namespace Enemies {
    
    [RequireComponent( typeof(SpriteRenderer), typeof(CircleCollider2D), typeof(CircleCollider2D))]
    public class BasicEnemy : MonoBehaviour, IHealth, IControllable, IAttacker {
        // public  float            Health { get; set; }
        public float            Health { get; set; } = 100;


        public GameObject GameObject {
            get => gameObject;
        }

        private CircleCollider2D body;
        private CircleCollider2D attackCollider;
        private SpriteRenderer   sprite;

        [FormerlySerializedAs("weapon"),SerializeField]
        private Sword sword;

        public Sword Sword => sword;

        [SerializeField]
        private Gun gun;
        public Gun              Gun            => Gun;
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
            Health -= damage;
            Debug.Log("Setting color...");
            sprite.color = Color.red;
            
            if (Health <= 0) {
                return true;
            }

            StartCoroutine(ResetColor());
            // Invoke(nameof(ResetColor), 0.1f);
            return false;
        }

        public void Destroy() {
            gameObject.SetActive(false);
        }
        
        private IEnumerator ResetColor() {
            yield return new WaitForSeconds(0.1f);
            
            Debug.Log("Reseting color");
            sprite.color = Color.white;
        }

        public void ResetHealth() {
            Health = 100;
        }

    }
}