using System.Collections;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements.Experimental;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft {get {return facingLeft;} set {facingLeft = value;}} //Пример класса с геттерами и сеттерами

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private float dashDuration = .2f;
    [SerializeField] private float dashCD = 1f;
    [SerializeField] TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private Transform slashAnimSpawnPoint;
    
    private Knockback knockback;
    private InputSystem_Actions inputSystem;
    private Vector2 movement;
    private Vector2 mousePosition;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private float startingSpeed;

    private bool facingLeft = false;
    private bool isDashing = false;

    protected override void Awake() {
        base.Awake();

        inputSystem = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
    }

    private void OnEnable() {
        inputSystem.Enable();
    }

    private void OnDisable() {
        inputSystem.Disable();
    }

    private void Start() {
        inputSystem.Player.Dash.performed += _ => Dash();

        startingSpeed = moveSpeed;

        ActiveInventory.Instance.EquipStartingWeapon();
    }
    
    private void Update() {
        PlayerInput();
    }

    private void FixedUpdate() {
        AdjustPlayerFacingDirection();
        Move();
    }
    
    public Transform GetWeaponCollider() {
        return weaponCollider;
    }

    public Transform GetSlashAnimSpawnPoint() {
        return slashAnimSpawnPoint;
    }

    private void PlayerInput() {
        movement = inputSystem.Player.Move.ReadValue<Vector2>();
        mousePosition = inputSystem.UI.Point.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move() {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.IsDead) { return; }
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void AdjustPlayerFacingDirection() {
        Vector2 playerPosition = Camera.main.WorldToScreenPoint(transform.position);
        if(mousePosition.x < playerPosition.x)
        {
            mySpriteRenderer.flipX = true;
            FacingLeft = true;
        }
        else
        {
            mySpriteRenderer.flipX = false;
            FacingLeft = false;
        }
    }

    private void Dash() {
        if(!isDashing && Stamina.Instance.CurrentStamina > 0) {
            Stamina.Instance.UseStamina();
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndOfDashingRoutine());
        }
    }

    private IEnumerator EndOfDashingRoutine() {
        yield return new WaitForSeconds(dashDuration);
        moveSpeed = startingSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}
