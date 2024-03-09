using UnityEngine.Events;

namespace Interfaces {
    public interface IWeapon {
        public float CalculateDamage();

        public float GetRange();

        public UnityEvent<IHealth> OnHit { get; }

        float GetAttackRate();
    }
}