using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Player : MonoBehaviour {
    [SerializeField]
    private float health = 100f;
    
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 5f;
    [SerializeField] private float maxSpeed = 20f;
    
    
    private Rigidbody2D rb;
    
    private InputManager inputManager;
    
    private void Awake() {
        rb           = GetComponent<Rigidbody2D>();
        inputManager = new InputManager();
    }

    private void OnEnable() {
        inputManager.Enable();
    }
    
    private void OnDisable() {
        inputManager.Disable();
    }

    private void FixedUpdate() {
        Vector2 movementInput = inputManager.KBM.Movement.ReadValue<Vector2>();
        if (movementInput != Vector2.zero) {
            Vector2 currentMovementDirection = rb.velocity.normalized;
            
            // If changing direction, should use deceleration value
            float verticalForce   = Math.Abs(Mathf.Sign(movementInput.x) - Mathf.Sign(currentMovementDirection.x)) < 0.1f ? movementInput.y * acceleration : movementInput.y * deceleration;
            float horizontalForce = Math.Abs(Mathf.Sign(movementInput.y) - Mathf.Sign(currentMovementDirection.y)) < 0.1f ? movementInput.x * acceleration : movementInput.x * deceleration;
            // if (Mathf.Sign(movementInput.x) != Mathf.Sign(currentMovementDirection.x)) Debug.Log("Changing horizontal direction");
            // if (Mathf.Sign(movementInput.y) != Mathf.Sign(currentMovementDirection.y)) Debug.Log("Changing vertical direction");
            
            Vector2 force = new Vector2(horizontalForce, verticalForce);
            
            rb.AddForce(force, ForceMode2D.Force);
            
            if (rb.velocity.magnitude > maxSpeed) {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
        else {
            rb.AddForce(-rb.velocity * deceleration, ForceMode2D.Force);
        }
    }
}