using UnityEngine.Events;
using Weapons;

namespace Interfaces {
    public interface IWeapon {
        
        public WeaponType Type { get; }
        public WeaponModel CalculateStats();
    }

    public enum WeaponType {
        Sword,
        Gun,
    }
}