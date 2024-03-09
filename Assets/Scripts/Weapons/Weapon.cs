using Interfaces;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Weapons {
    public class Weapon : MonoBehaviour, IWeapon {
        
        [SerializeField]
        private WeaponModel model;
        
        public float CalculateDamage() {
            return model.damage;
        }
        
        public float GetRange() {
            return model.range;
        }
        
        public float GetAttackRate() {
            return model.attackRate;
        }

        private void Awake() {
            if (model == null) {
                model = ScriptableObject.CreateInstance<WeaponModel>();
            }
        }

        public UnityEvent<IHealth> OnHit { get; } = new UnityEvent<IHealth>();
    }
}