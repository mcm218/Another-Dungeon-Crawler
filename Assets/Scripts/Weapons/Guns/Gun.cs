using Interfaces;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Weapons {
    public class Gun : BaseGun {
        private WeaponModel stats;
        // TODO: Wielder should be able to call an equip method on the gun
        // TODO: Wielder should

        private bool isAttacking = false;
        
        // TODO: See what I can move to BaseSword
        [field:SerializeField]
        public IAttacker Wielder { get; private set; }
        
        [SerializeField]
        private WeaponAttackCollider attackCollider;
        
        public override void Equip(IAttacker wielder) {
            Debug.Log("Gun.cs: Equipping gun!");
            Wielder = wielder;
            
        }
        
        public override void PerformAttack() {
            Debug.Log("Performing attack!");
            isAttacking = true;
            var position            = Wielder.GameObject.transform.position;
            var forward             = Wielder.GameObject.transform.right;
            var distanceFromWielder = 1 + stats.range / 2;
            position += forward * distanceFromWielder;
            attackCollider.SetPositionData(transform.position, position);
            attackCollider.SetActive(true);
            Invoke(nameof(ResetAttack), stats.attackRate);
        }

        public override void ResetAttack() {
            Debug.Log("Resetting attack!");
            isAttacking = false;
            attackCollider.SetActive(false);
        }
        
        private void Attack(IHealth health, GameObject target) {
            if (health == null) {
                throw new NullReferenceException("Gun.cs: Health is null");
            }
            
            Debug.Log("Attacking " + target.name);
            health.Damage(stats.damage);
            OnHit.Invoke(health);
        }

        private void Awake() {
            if (model == null) {
                throw new NullReferenceException("Model is null");
            }
            
            stats = model.CalculateStats();
        }

        private void Start() {
            if (attackCollider == null) {
                throw new NullReferenceException("Attack collider is null");
            }
            var data = new WeaponAttackCollider.GunColliderData(0.1f);
            attackCollider.SetAttackType(data);
            Debug.Log(JsonUtility.ToJson(data));
            
            attackCollider.collider.enabled = false;
            attackCollider.OnTargetEnter.AddListener(Attack);
        }

        public override bool CanAttack() {
            return !isAttacking;
        }

        public override float GetRange() {
            if (stats == null) {
                throw new NullReferenceException("Stats is null");
            }
            
            return stats.range;
        }

        public UnityEvent<IHealth> OnHit { get; } = new UnityEvent<IHealth>();
    }
}