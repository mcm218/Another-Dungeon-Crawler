using Interfaces;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Weapons;

[RequireComponent(  typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour, IHealth, IAttacker  {
    [SerializeField]
    private float initialHealth = 100f;

    [field: SerializeField, ReadOnly]
    public float Health {
        get;
        private set;
    }

    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 5f;
    [SerializeField] private float maxSpeed = 20f;
    
    
    private Rigidbody2D rb;
    
    private CircleCollider2D attackCollider;

    private SpriteRenderer sprite;
    
    private InputManager inputManager;

    
    [field:SerializeField]
    public UnityEvent OnDeath { get; private set; }

    [SerializeField]
    private Weapon _weapon;

    public IWeapon Weapon => _weapon as IWeapon;
    
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
    
    public void Attack(IHealth target) {
        target.Damage(Weapon.CalculateDamage());
    }
    
    public void EquipWeapon(Weapon weapon) {
        if (_weapon != null) {
            Weapon.OnHit.RemoveListener(Attack);
        }

        _weapon = weapon;
        Weapon.OnHit.AddListener(Attack);
    }

    public void ResetHealth() {
        Health = initialHealth;
    }
    
    private BoxCollider2D bodyCollider;

    public Collider2D Body => bodyCollider;
    
    private bool isAttacking = false;
    
    private List<IHealth> targets = new List<IHealth>();

    private void Awake() {
        rb           = GetComponent<Rigidbody2D>();
        attackCollider = GetComponent<CircleCollider2D>();
        bodyCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        inputManager = new InputManager();

        Health = initialHealth;
        
        attackCollider.radius = Weapon.GetRange();
        attackCollider.isTrigger = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
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
        if (attackInput) {
            if (isAttacking) return;
            Debug.Log("Attacking...");
            isAttacking = true;
            var killedTargets = new List<IHealth>();
            foreach (var target in targets) {
                if (target.Damage(Weapon.CalculateDamage())) {
                    killedTargets.Add(target);
                }
            }

            killedTargets.ForEach(target => {
                targets.Remove(target);
                target.Destroy();
            });
            
        }
        else {
            isAttacking = false;
        }
    }

    public void Destroy() {
        gameObject.SetActive(false);
    }
    
    private string[] TAGS_TO_ATTACK = {"Enemy"};

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Trigger with " + other.gameObject.name);
        if (other.gameObject.TryGetComponent<IHealth>(out var health) && Array.Exists(TAGS_TO_ATTACK, tag => tag == other.gameObject.tag)) {
            if (health.Body.IsTouching(attackCollider)) {
                targets.Add(health);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.TryGetComponent<IHealth>(out var health) && Array.Exists(TAGS_TO_ATTACK, tag => tag == other.gameObject.tag)) {
            if (!health.Body.IsTouching(attackCollider)) {
                targets.Remove(health);
            }
        }
    }
}