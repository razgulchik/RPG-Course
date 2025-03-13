using System;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goldCoin, healthGlobe, staminaGlobe;

    public void DropItem() {
        int randomNum = UnityEngine.Random.Range(1, 5);

        if (randomNum == 1) {
            Instantiate(healthGlobe, transform.position, Quaternion.identity);
        }
        if (randomNum == 2) {
            Instantiate(staminaGlobe, transform.position, Quaternion.identity);
        }
        if (randomNum == 3) {
            int randomAmountOfGold = UnityEngine.Random.Range(1, 4);
            
            for (int i = 0; i < randomAmountOfGold; i++) {
                Instantiate(goldCoin, transform.position, Quaternion.identity);
            }
        }
    }
}
