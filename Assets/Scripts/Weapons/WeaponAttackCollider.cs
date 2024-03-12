using Interfaces;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Weapons {
    // TODO: Break out into factory that returs SwordAttackCollider, GunAttackCollider, etc.
    [RequireComponent(typeof(CircleCollider2D))]
    public class WeaponAttackCollider : MonoBehaviour {
        public Collider2D collider;
        
        public UnityEvent<IHealth,GameObject> OnTargetEnter;
        
        public List<IHealth> hitTargets = new List<IHealth>();
        
        // private Collider2D[] hits = new Collider2D[10];
        
        private SpriteRenderer sprite;
        
        private Vector3            source;
        private Vector3            target;
        private IAttackColliderData data;

        public interface IAttackColliderData {
            public WeaponType WeaponType { get; }
            public Type       ColliderType { get; }
        }

        public class SwordColliderData : IAttackColliderData {
            public              float angle;
            public WeaponType WeaponType => WeaponType.Sword;
            public Type  ColliderType => typeof(CircleCollider2D);
            
            public SwordColliderData(float angle) {
                this.angle = angle;
            }
        }
        
        public class GunColliderData : IAttackColliderData {
            public              float width;
            public WeaponType WeaponType => WeaponType.Gun;
            public Type  ColliderType => typeof(BoxCollider2D);
            
            public GunColliderData(float width) {
                this.width = width;
            }
        }

        private void Awake() {
            collider = GetComponent<CircleCollider2D>();
            sprite = GetComponent<SpriteRenderer>();
            
            collider.enabled = false;
            if (sprite != null) {
                sprite.enabled = false;
            }
        }
        
        public void SetAttackType(IAttackColliderData data) {
            this.data = data;
            
            // TODO: Instantiate collider
            if (data.ColliderType == typeof(CircleCollider2D)) {
                collider = gameObject.AddComponent<CircleCollider2D>();
            } else if (data.ColliderType == typeof(BoxCollider2D)) {
                collider = gameObject.AddComponent<BoxCollider2D>();
            }
        }

        private void FixedUpdate() {
            if (collider.enabled) CheckForEnemies();
        }
        
        public void CheckForEnemies() {
            Debug.Log(data.WeaponType);
            switch (data.WeaponType) {
                case WeaponType.Sword:
                    CheckForSwordEnemies();
                    break;
                case WeaponType.Gun:
                    CheckForGunEnemies();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("WeaponType not set");
            }
        }
        
        private void CheckForSwordEnemies() {
            Assert.IsTrue(data.WeaponType == WeaponType.Sword, "Weapon type is not sword");
            CircleCollider2D collider = (CircleCollider2D) this.collider;
            SwordColliderData swordData = data as SwordColliderData;
            
            // TODO: Update to handle angle
            var hits = (Physics2D.OverlapCircleAll(transform.position, collider.radius)).ToList();
            hits.RemoveAll(hit => {
                var angle = Math.Abs(Vector3.Angle(transform.position, hit.transform.position));
                var distance = Math.Abs(Vector3.Distance(transform.position, hit.transform.position));
                return angle > swordData.angle / 2 || distance > collider.radius;
            });
            
            HandleHits(hits.ToArray());
            
            #if UNITY_EDITOR
            Debug.DrawLine(source, target, Color.red, 0.1f);
            #endif
        }

        private void CheckForGunEnemies() {
            Assert.IsTrue(data.WeaponType == WeaponType.Gun, "Weapon type is not gun");
            GunColliderData gunData = (GunColliderData) data;

            // Check box from source to target, width of 0.01
            var center = (source + target)                                     / 2;
            var angle  = Mathf.Atan2(target.y - source.y, target.x - source.x) * Mathf.Rad2Deg;
            var distance = Vector3.Distance(source, target);
            HandleHits(Physics2D.OverlapBoxAll(center, new Vector2(distance, gunData.width), angle));
        }
        
        #if UNITY_EDITOR

        private void OnDrawGizmos() {
            if (!Application.isPlaying) return;
            
            if (data.WeaponType == WeaponType.Sword) {
                CircleCollider2D collider = (CircleCollider2D) this.collider;
                
                
                // Draw attack range
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, collider.radius);
            }
            
            if (data.WeaponType == WeaponType.Gun) {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(source, target);
            }
        }

        #endif

        private void HandleHits(Collider2D[] hits) {
            var hitsList = hits.ToList();
            hitsList.RemoveAll(hit => {
                return hit == null || hit.GetComponent<IHealth>() == null;
            });
            foreach (var hit in hitsList) {
                var enemyHealth = hit.GetComponent<IHealth>();
                if (enemyHealth == null || hitTargets.Contains(enemyHealth)) continue;
                
                OnTargetEnter.Invoke(enemyHealth, hit.gameObject);
                hitTargets.Add(enemyHealth);
            }
            
        }
        
        public void SetActive(bool active) {
            collider.enabled = active;
            if (sprite != null) {
                sprite.enabled = active;
            }
            hitTargets.Clear();
        }

        public void SetColliderRadius(float radius) {
            // collider.radius = radius;
            transform.localScale = new Vector3(radius / 2, radius / 2, 1);
        }
        
        public void SetPositionData(Vector3 source, Vector3 target) {
            transform.position = target;
            this.source        = source;
            this.target        = target;
        }
    }
}