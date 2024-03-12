using Interfaces;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Weapons {
    [CreateAssetMenu(fileName = "Sword", menuName = "Weapons/Sword")]
    public class SwordWeapon : ScriptableObject, IWeapon {
        private WeaponType weaponType = WeaponType.Sword;
        
        public WeaponType Type {
            get => weaponType;
        }
        
        [SerializeField]
        private SwordPartModel[] weaponParts;

        public SwordPartModel[] WeaponParts => weaponParts;

        public WeaponModel CalculateStats() => weaponParts.Aggregate(new WeaponModel(), (acc, part) => acc + part);
    }
}