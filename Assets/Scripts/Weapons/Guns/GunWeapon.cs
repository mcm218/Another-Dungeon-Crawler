using Interfaces;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Weapons {
    [CreateAssetMenu(fileName = "Gun", menuName = "Weapons/Gun")]
    public class GunWeapon : ScriptableObject, IWeapon {
        private WeaponType weaponType = WeaponType.Gun;
        
        public WeaponType Type {
            get => weaponType;
        }
        
        [SerializeField]
        private GunPartModel[] weaponParts;

        public GunPartModel[] WeaponParts => weaponParts;

        public WeaponModel CalculateStats() => weaponParts.Aggregate(new WeaponModel(), (acc, part) => acc + part);
    }
}