using Interfaces;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Player : MonoBehaviour, IHealth, IAttacker  {
    [SerializeField]
    private float initialHealth = 100f;

    [property: SerializeField, ReadOnly]
    public float Health {
        get;
        private set;
    }

    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 5f;
    [SerializeField] private float maxSpeed = 20f;
    
    
    private Rigidbody2D rb;
    
    private InputManager inputManager;

    [SerializeField]
    private UnityEvent onDeath;

    [property: SerializeField, ReadOnly]
    public IWeapon Weapon { get; private set; }
    
    public void Damage(float damage) {
        Health -= damage;
        if (Health <= 0) {
            onDeath.Invoke();
        }
    }
    
    public void Attack(IHealth target) {
        target.Damage(Weapon.CalculateDamage());
    }
    
    public void EquipWeapon(IWeapon weapon) {
        if (Weapon != null) {
            Weapon.OnHit.RemoveListener(Attack);
        }
        
        Weapon = weapon;
        weapon.OnHit.AddListener(Attack);
    }

    public void ResetHealth() {
        Health = initialHealth;
    }
    
    private void Awake() {
        rb           = GetComponent<Rigidbody2D>();
        inputManager = new InputManager();
        
        Health = initialHealth;
        if (Weapon != null) {
            Weapon.OnHit.AddListener(Attack);
        }
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
            
            float dotProduct = Vector2.Dot(currentMovementDirection, movementInput);
            float lerpValue  = (dotProduct + 1) / 2;
            float forceMagnitude = Mathf.Lerp(acceleration, deceleration, lerpValue);
            Vector2 force = movementInput.normalized * forceMagnitude;
            
            // If changing direction, should use deceleration value
            // float verticalForce   = Math.Abs(Mathf.Sign(movementInput.x) - Mathf.Sign(currentMovementDirection.x)) < 0.1f ? movementInput.y * acceleration : movementInput.y * deceleration;
            // float horizontalForce = Math.Abs(Mathf.Sign(movementInput.y) - Mathf.Sign(currentMovementDirection.y)) < 0.1f ? movementInput.x * acceleration : movementInput.x * deceleration;
            // if (Mathf.Sign(movementInput.x) != Mathf.Sign(currentMovementDirection.x)) Debug.Log("Changing horizontal direction");
            // if (Mathf.Sign(movementInput.y) != Mathf.Sign(currentMovementDirection.y)) Debug.Log("Changing vertical direction");
            
            
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