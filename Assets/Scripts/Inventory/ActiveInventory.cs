using System;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNum = 0;
    private InputSystem_Actions inputSystem;

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
        inputSystem.UI.Inventory.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
    }

    public void EquipStartingWeapon() {
        ToggleActiveHighlight(0);
    }

    private void ToggleActiveSlot(int numValue) {
        ToggleActiveHighlight(numValue);
    }

    private void ToggleActiveHighlight(int indexNum) {
        activeSlotIndexNum = indexNum;
        foreach (Transform inventorySlot in this.transform) {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }
        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon() {
        if (PlayerHealth.Instance.IsDead) { return; }
        
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null) {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();
        GameObject weaponToSpawn = null;
        
        if(weaponInfo == null) {
            ActiveWeapon.Instance.WeaponNull();
            return;
        } else {
            weaponToSpawn = weaponInfo.weaponPrefab;
        }
              
        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        newWeapon.transform.parent = ActiveWeapon.Instance.transform;
        
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
