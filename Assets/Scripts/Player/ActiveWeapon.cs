using System.Collections;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }
    private float timeBetweenAttacks;
    private InputSystem_Actions inputSystem;

    private bool attackButtonDown, isAttacking = false;

    protected override void Awake() {
        base.Awake();

        inputSystem = new InputSystem_Actions();
    }

    private void OnEnable() {
        inputSystem.Enable();
    }
    // private void OnDisable() {
    //     inputSystem.Disable();
    // }

    private void Start() {
        inputSystem.Player.Attack.started += _ => StartAttacking();
        inputSystem.Player.Attack.canceled += _ => StopAttacking();

        AttackCooldown();
    }

    public void NewWeapon(MonoBehaviour newWeapon) {
        CurrentActiveWeapon = newWeapon;
        AttackCooldown();
        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
    }

    public void WeaponNull() {
        CurrentActiveWeapon = null;
    }

    public Vector2 GetMousePosition(){
        return inputSystem.UI.Point.ReadValue<Vector2>();
    }

    private void Update() {
        Attack();
    }

    private void StartAttacking() {
        attackButtonDown = true;
    }

    private void StopAttacking() {
        attackButtonDown = false;
    }

    private void Attack() {
        if(attackButtonDown && !isAttacking && CurrentActiveWeapon) {
            AttackCooldown();
            (CurrentActiveWeapon as IWeapon).Attack();
        } 
    }
    
    private void AttackCooldown() {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private IEnumerator TimeBetweenAttacksRoutine() {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }
}
