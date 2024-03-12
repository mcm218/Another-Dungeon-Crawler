using Interfaces;
using UnityEngine;

namespace Weapons {
    public abstract class BaseSword : MonoBehaviour, IBaseWeapon {
        // TODO: Break out SwordWeapon into... an abstract class??
        public          SwordWeapon model;
        public abstract void        PerformAttack();
        public abstract void        ResetAttack();
        public abstract bool        CanAttack();
        public abstract float       GetRange();

        public abstract void Equip(IAttacker wielder);

        public WeaponModel CalculateStats() => model.CalculateStats();
    }
}