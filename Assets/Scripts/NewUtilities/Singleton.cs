using UnityEngine;


/// <summary>
/// Similar to a singleton, but doesn't destroy will override the current instance if a new
/// instance is created.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake () => Instance = this as T;

    protected virtual void OnApplicationQuit ()
    {
        Instance = null;
        Destroy (this.gameObject);
    }
}

public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake ()
    {
        // Is there already an Instance? If so, destroy this new object and return
        if (Instance != null)
        {
            Destroy (this.gameObject);
            return;
        }

        base.Awake ();
    }

}

public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake ()
    {
        base.Awake ();
        DontDestroyOnLoad (this.gameObject);
    }
}


