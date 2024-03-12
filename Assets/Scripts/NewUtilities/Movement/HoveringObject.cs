using UnityEngine;

/// <summary>
/// Provides a hovering effect to an object using sinusoidal movement.
/// </summary>
public class HoveringEffect : MonoBehaviour
{
    /// <summary>
    /// The amplitude of the hover effect, determining how much the object will move up and down.
    /// </summary>
    [Tooltip("Defines how much the object will move up and down.")]
    public float hoverAmplitude = 0.05f;

    /// <summary>
    /// The frequency of the hover effect, determining how fast the object will move up and down.
    /// </summary>
    [Tooltip("Defines how fast the object will move up and down.")]
    public float hoverFrequency = 1f;

    /// <summary>
    /// The initial position of the object before applying the hovering effect.
    /// </summary>
    private Vector3 initialPosition;

    /// <summary>
    /// Captures the initial position of the object when the script starts.
    /// </summary>
    private void Start()
    {
        initialPosition = transform.position;
    }

    /// <summary>
    /// Updates the position of the object based on a sine function to create a hovering effect.
    /// </summary>
    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        transform.position = initialPosition + new Vector3(0, yOffset, 0);
    }
}