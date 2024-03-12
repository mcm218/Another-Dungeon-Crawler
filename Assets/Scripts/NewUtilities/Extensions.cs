using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class Extensions {
    /// <summary>
    /// Returns a new Vector3 with the specified values.
    /// </summary>
    public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null) {
        return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
    }

    /// <summary>
    /// Returns a new Vector3 with the specified values added to the original.
    /// </summary>
    public static Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null) {
        return new Vector3(vector.x + (x ?? 0), vector.y + (y ?? 0), vector.z + (z ?? 0));
    }

    /// <summary>
    /// Returns a boolean indicating whether the current Vector3 is within a specified range of another Vector3.
    /// </summary>
    public static bool InRangeOf(this Vector3 current, Vector3 target, float range) {
        return (current - target).sqrMagnitude <= range * range;
    }

    /// <summary>
    /// Retrieves all the children of a given Transform.
    /// </summary>
    /// <remarks>
    /// This method can be used with LINQ to perform operations on all child Transforms. For example,
    /// you could use it to find all children with a specific tag, to disable all children, etc.
    /// Transform implements IEnumerable and the GetEnumerator method which returns an IEnumerator of all its children.
    /// </remarks>
    /// <param name="parent">The Transform to retrieve children from.</param>
    /// <returns>An IEnumerable&lt;Transform&gt; containing all the child Transforms of the parent.</returns> 
    public static IEnumerable<Transform> Children(this Transform transform) {
        foreach (Transform child in transform) {
            yield return child;
        }
    }
    
    /// <summary>
    /// Resets transform's position, scale and rotation
    /// </summary>
    /// <param name="transform">Transform to use</param>
    public static void Reset(this Transform transform) {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Destroys all children of the transform
    /// </summary>
    /// <param name="parent">Transform whose children are to be destroyed.</param>
    public static void DestroyChildren(this Transform parent) {
        parent.ForEveryChild(child => GameObject.Destroy(child.gameObject));
    }

    public static void ForEveryChild(this Transform parent, Action<Transform> action) {
        for (var i = parent.childCount - 1; i >= 0; i--) {
            action(parent.GetChild(i));
        }
    }

    /// <summary>
    /// Disables all child GameObjects associated with the given Transform.
    /// </summary>
    /// <param name="parent">Transform whose child GameObjects are to be disabled.</param>
    public static void DisableChildren(this Transform parent) {
        parent.ForEveryChild(child => child.gameObject.SetActive(false));
    }

    /// <summary>
    /// Enables all child GameObjects associated with the given Transform.
    /// </summary>
    /// <param name="parent">Transform whose child GameObjects are to be enabled.</param>
    public static void EnableChildren(this Transform parent) {
        parent.ForEveryChild(child => child.gameObject.SetActive(true));
    }

    /// <summary>
    /// Immediately destroys all children of the given Transform.
    /// </summary>
    /// <param name="parent">Transform whose children are to be destroyed.</param>
    public static void DestroyChildrenImmediate(this Transform parent) {
        parent.ForEveryChild(child => GameObject.DestroyImmediate(child.gameObject));
    }

/// <summary>
    /// Gets a component of the given type attached to the GameObject. If that type of component does not exist, it adds one.
    /// </summary>
    /// <remarks>
    /// This method is useful when you don't know if a GameObject has a specific type of component,
    /// but you want to work with that component regardless. Instead of checking and adding the component manually,
    /// you can use this method to do both operations in one line.
    /// </remarks>
    /// <typeparam name="T">The type of the component to get or add.</typeparam>
    /// <param name="gameObject">The GameObject to get the component from or add the component to.</param>
    /// <returns>The existing component of the given type, or a new one if no such component exists.</returns>    
    public static T GetOrAdd<T> (this GameObject gameObject) where T : Component {
        T component = gameObject.GetComponent<T>();
        if (!component) component = gameObject.AddComponent<T>();
        
        return component;
    }

    /// <summary>
    /// Returns the object itself if it exists, null otherwise.
    /// </summary>
    /// <remarks>
    /// This method helps differentiate between a null reference and a destroyed Unity object. Unity's "== null" check
    /// can incorrectly return true for destroyed objects, leading to misleading behaviour. The OrNull method use
    /// Unity's "null check", and if the object has been marked for destruction, it ensures an actual null reference is returned,
    /// aiding in correctly chaining operations and preventing NullReferenceExceptions.
    /// </remarks>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="obj">The object being checked.</param>
    /// <returns>The object itself if it exists and not destroyed, null otherwise.</returns>
    public static T OrNull<T> (this T obj) where T : Object => obj ? obj : null;
    
    /// <summary>
    /// Destroys all children of the game object
    /// </summary>
    /// <param name="gameObject">GameObject whose children are to be destroyed.</param>
    public static void DestroyChildren(this GameObject gameObject) {
        gameObject.transform.DestroyChildren();
    }
    
    /// <summary>
    /// Immediately destroys all children of the given GameObject.
    /// </summary>
    /// <param name="gameObject">GameObject whose children are to be destroyed.</param>
    public static void DestroyChildrenImmediate(this GameObject gameObject) {
        gameObject.transform.DestroyChildrenImmediate();
    }
    
    /// <summary>
    /// Enables all child GameObjects associated with the given GameObject.
    /// </summary>
    /// <param name="gameObject">GameObject whose child GameObjects are to be enabled.</param>
    public static void EnableChildren(this GameObject gameObject) {
        gameObject.transform.EnableChildren();
    }
        
    /// <summary>
    /// Disables all child GameObjects associated with the given GameObject.
    /// </summary>
    /// <param name="gameObject">GameObject whose child GameObjects are to be disabled.</param>
    public static void DisableChildren(this GameObject gameObject) {
        gameObject.transform.DisableChildren();
    }        

    /// <summary>
    /// Resets the GameObject's transform's position, rotation, and scale to their default values.
    /// </summary>
    /// <param name="gameObject">GameObject whose transformation is to be reset.</param>
    public static void ResetTransformation(this GameObject gameObject) {
        gameObject.transform.Reset();
    }

    /// <summary>
    /// Converts an IEnumerator<T> to an IEnumerable<T>.
    /// </summary>
    /// <param name="e">An instance of IEnumerator<T>.</param>
    /// <returns>An IEnumerable<T> with the same elements as the input instance.</returns>    
    public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> e) {
        while (e.MoveNext()) {
            yield return e.Current;
        }
    }
}
