using Interfaces;
using UnityEngine;

namespace Weapons {
    [CreateAssetMenu(fileName = "Sword", menuName = "Weapons/Sword Part")]
    public class SwordPartModel : ScriptableObject {
        public readonly WeaponType weaponType  = WeaponType.Sword;
        public          float      damage      = 10f;
        public          float      range       = 3f;
        public          float      attackRate  = 1f;
        public          float      impactForce = 30f;
        
        // + operator
        public static WeaponModel operator +(SwordPartModel a, SwordPartModel b) {
            return new WeaponModel {
                damage = a.damage + b.damage,
                range = a.range + b.range,
                attackRate = a.attackRate + b.attackRate,
                impactForce = a.impactForce + b.impactForce
            };
        }
    }
}