using System;
using System.Collections;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Timeline;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private WeaponInfo weaponInfo;
    
    private CapsuleCollider2D weaponCollider;
    private Transform slashAnimSpawnPoint;
    private Animator myAnimator;
    private GameObject slashAnim;

    public WeaponInfo WeaponInfo { get => weaponInfo; set => weaponInfo = value; }

    private void Awake() {
        myAnimator = GetComponent<Animator>();
        weaponCollider = GetComponent<CapsuleCollider2D>();
        slashAnimSpawnPoint = PlayerController.Instance.GetSlashAnimSpawnPoint();
    }

    private void Start() {
        //weaponCollider = PlayerController.Instance.GetWeaponCollider();
        weaponCollider.enabled = false;
    }
    
    private void Update() {
        MouseFollowWithOffset();
    }

    public void Attack() {
        weaponCollider.enabled = true;;
        myAnimator.SetTrigger("Attack");
        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        slashAnim.transform.parent = this.transform.parent;
    }

    public void DoneAttakingAnimEvent() {
        weaponCollider.enabled = false;
    }

    public void SwingUpFlipAnimEvent() {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
        if (PlayerController.Instance.FacingLeft) {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimEvent() {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (PlayerController.Instance.FacingLeft) {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset()
    {
        Vector2 mousePosition = ActiveWeapon.Instance.GetMousePosition();
        Vector2 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePosition.y - playerScreenPoint.y, Math.Abs(mousePosition.x - playerScreenPoint.x)) * Mathf.Rad2Deg;
        angle = Math.Clamp(angle, -50, 40);
        if(mousePosition.x < playerScreenPoint.x) {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        } else {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public WeaponInfo GetWeaponInfo(){
        return weaponInfo;
    }
}
