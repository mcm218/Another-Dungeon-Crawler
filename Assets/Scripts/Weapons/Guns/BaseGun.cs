using Interfaces;
using UnityEngine;

namespace Weapons {
    public abstract class BaseGun : MonoBehaviour, IBaseWeapon {
        public          GunWeapon model;
        public abstract void        PerformAttack();
        public abstract void        ResetAttack();
        public abstract bool        CanAttack();
        public abstract float       GetRange(); 
        
        public abstract void Equip(IAttacker wielder);

        public WeaponModel CalculateStats() => model.CalculateStats();
        
    }
}