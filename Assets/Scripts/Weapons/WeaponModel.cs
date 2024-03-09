using UnityEngine;

namespace Weapons {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon")]
    public class WeaponModel : ScriptableObject {
        public float damage = 10f;
        public float range  = 3f;
        public float attackRate = 1f;
        public float impactForce = 30f;
        
        // TODO: Add parts
    }
}