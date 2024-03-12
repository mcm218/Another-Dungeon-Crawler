using UnityEngine;

namespace _Scripts.Utilities {
    [RequireComponent(typeof(Light))]
    public class PersistentLight : PersistentSingleton<PersistentLight> {
        // Used to ensure multiple lights aren't created when loading a scene
    }
}