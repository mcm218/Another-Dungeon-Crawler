using UnityEngine.Events;

namespace Interfaces {
    public interface IWeapon {
        public float CalculateDamage();

        public UnityEvent<IHealth> OnHit { get; }
    }
}