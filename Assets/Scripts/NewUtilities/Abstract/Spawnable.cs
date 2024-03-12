using UnityEngine;

namespace _Scripts.Utilities {
    public abstract class Spawnable : MonoBehaviour {
        public abstract GameObject Spawn(Vector3 coordinates);
    }

}