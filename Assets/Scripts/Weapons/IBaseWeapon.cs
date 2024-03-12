using Interfaces;
using System.Collections;

namespace Weapons {
    public interface IBaseWeapon {
        
        public void PerformAttack();
        public void ResetAttack();

        public bool CanAttack();

        public float GetRange();
        
        public WeaponModel CalculateStats();

        public void Equip(IAttacker wielder);
    }
}