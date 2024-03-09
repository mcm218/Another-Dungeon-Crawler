using UnityEngine;
using UnityEngine.Events;

namespace Interfaces {
    public interface IHealth {
        float Health { get; }

        /// <param name="damage"></param>
        /// <returns>If damaged entity was killed</returns>
        public    bool  Damage(float damage);
        public    void  ResetHealth();

        public void Destroy();
        
        public Collider2D Body { get; }
        
        public UnityEvent OnDeath { get; }
    }
}