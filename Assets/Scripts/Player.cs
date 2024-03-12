using Interfaces;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Weapons;

[RequireComponent(  typeof(Rigidbody2D), typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour, IHealth, IAttacker  {
    [SerializeField]
    private float initialHealth = 100f;

    public     GameObject GameObject => gameObject;

    [field: SerializeField, ReadOnly]
    public float Health {
        get;
        private set;
    }

    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 5f;
    [SerializeField] private float maxSpeed = 20f;
    
    
    private Rigidbody2D rb;
    
    private SpriteRenderer sprite;
    
    private InputManager inputManager;

    
    [field:SerializeField]
    public UnityEvent OnDeath { get; private set; }

    [FormerlySerializedAs("weapon"),SerializeField]
    private Sword sword;
    
    [SerializeField]
    private Gun gun;
    
    public Sword Sword => sword;
    
    public Gun Gun => gun;
    
    public bool Damage(float damage) {
        Debug.Log(name + " took " + damage + " damage");
        Health       -= damage;
        sprite.color =  Color.red;
        if (Health <= 0) {
            OnDeath.Invoke();
            return true;
        }
        Invoke(nameof(ResetColor), 0.1f);
        return false;
    }
    
    private void ResetColor() {
        sprite.color = Color.white;
    }
    
    
    public void EquipWeapon(Sword newSword) {
        sword = newSword;
    }

    public void ResetHealth() {
        Health = initialHealth;
    }
    
    private BoxCollider2D bodyCollider;

    public Collider2D Body => bodyCollider;
    
    private List<IHealth> targets = new List<IHealth>();

    private void Awake() {
        rb           = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        inputManager = new InputManager();

        Health = initialHealth;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible   = true;
    }

    private void Start() {
        if (Sword != null) {
            Debug.Log("Equipping sword...");
            Sword.Equip(this);
        }
        if (Gun != null) {
            Debug.Log("Equipping gun...");
            Gun.Equip(this);
        }
    }

    private void OnEnable() {
        inputManager.Enable();
    }
    
    private void OnDisable() {
        inputManager.Disable();
    }

    // TODO: Point player towards mouse
    private void FixedUpdate() {
        Vector2 movementInput = inputManager.KBM.Movement.ReadValue<Vector2>();
        if (movementInput != Vector2.zero) {
            Vector2 currentMovementDirection = rb.velocity.normalized;
            
            float   dotProduct     = Vector2.Dot(currentMovementDirection, movementInput);
            float   lerpValue      = (dotProduct + 1) / 2;
            float   forceMagnitude = Mathf.Lerp(acceleration, deceleration, lerpValue);
            Vector2 force          = movementInput.normalized * forceMagnitude;


            rb.AddForce(force, ForceMode2D.Force);
            
            if (rb.velocity.magnitude > maxSpeed) {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
        else {
            rb.AddForce(-rb.velocity * deceleration, ForceMode2D.Force);
        }

        bool attackInput = inputManager.KBM.Attack.ReadValue<float>() > 0f;
        if (attackInput && gun != null && gun.CanAttack()) {
            Debug.Log("Attacking with gun...");
            gun.PerformAttack();
        }
        
        if (attackInput && sword != null && sword.CanAttack()) {
            Debug.Log("Attacking with sword...");
            sword.PerformAttack();
        }
        
        Vector2 screenPosition = inputManager.KBM.Aim.ReadValue<Vector2>();
        transform.rotation = Quaternion.Euler(0, 0, GetLookAngle(screenPosition));
    }
    
    public float GetLookAngle(Vector2 position) {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
        Vector2 direction     = worldPosition - (Vector2)transform.position;
        float   angle         = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return angle;
    }
    
    #if UNITY_EDITOR

    private void OnDrawGizmos() {
        if (!Application.isPlaying) return;
        
        if (sword != null) {
            // Draw attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sword.GetRange());
        }

        // Draw look direction - NOT CURRENTLY WORKING
        // Vector2 screenPosition = inputManager.KBM.Aim.ReadValue<Vector2>();
        // Vector2 worldPosition  = Camera.main.ScreenToWorldPoint(screenPosition);
        // Gizmos.DrawLine(transform.position, worldPosition);
    }

    #endif

    public void Destroy() {
        gameObject.SetActive(false);
    }
    
    private string[] TAGS_TO_ATTACK = {"Enemy"};
}