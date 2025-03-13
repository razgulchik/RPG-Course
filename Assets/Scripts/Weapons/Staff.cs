using System;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject magicLaserPrefab;
    [SerializeField] private Transform magicLaserSpawnPoint;

    private Animator myAnimator;
    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private void Awake() {
        myAnimator = GetComponent<Animator>();
    }

    private void Update() {
        MouseFollow();
    }

    public void Attack() {
        myAnimator.SetTrigger(FIRE_HASH);
    }
    
    public WeaponInfo GetWeaponInfo(){
        return weaponInfo;
    }
    
    private void MouseFollow() {
        Vector2 mousePosition = ActiveWeapon.Instance.GetMousePosition();
        
        Vector2 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
        
        float angle = Mathf.Atan2(mousePosition.y - playerScreenPoint.y, Math.Abs(mousePosition.x - playerScreenPoint.x)) * Mathf.Rad2Deg;
        angle = Math.Clamp(angle, -50, 40);
        if(mousePosition.x < playerScreenPoint.x) {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
        } else {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void SpawnStaffProjectileAnimEvent() {
        GameObject newMagicLaser = Instantiate(magicLaserPrefab, magicLaserSpawnPoint.position, Quaternion.identity);
        newMagicLaser.GetComponent<MagicLaser>().UpdateLaserRange(weaponInfo.weaponRange);
    }
}
