using UnityEngine;

public class Destrutible : MonoBehaviour
{
    [SerializeField] private GameObject destroyVFX;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<DamageSource>() || other.GetComponent<Projectile>()) {
            PickUpSpawner pickUpSpawner = GetComponent<PickUpSpawner>();
            pickUpSpawner?.DropItem();
            Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject, .1f);
        }
    }
}
