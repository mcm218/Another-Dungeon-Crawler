using System.Collections.Generic;
using UnityEngine;


public static class Helpers {
    /// <summary>
    /// Gets a random point within a box collider
    /// </summary>
    /// <param name="boxCollider"></param>
    /// <returns></returns>
    public static Vector3 GetRandomPointInsideCollider(this BoxCollider2D boxCollider) {
        Vector2 extents = boxCollider.size / 2f;
        Vector2 point = new Vector2(
                                     Random.Range(-extents.x, extents.x),
                                     Random.Range(-extents.y, extents.y)
                                    );

        return boxCollider.transform.TransformPoint(point);
    }

    public static float GetArea(this BoxCollider2D boxCollider) {
        return boxCollider.size.x * boxCollider.size.y;
    }
}