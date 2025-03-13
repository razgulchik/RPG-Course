using System.Collections;
//using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina>
{
    public int CurrentStamina { get; private set;}

    [SerializeField] private Sprite fullStaminaImage, emptyStaminaImage;
    [SerializeField] private int timeBetweenStaminaRefresh = 3;

    private Transform staminaConteiner;
    private int startingStamina = 3;
    private int maxStamina;
    const string STAMINA_CONTEINER_TEXT = "Stamina Container";

    protected override void Awake() {
        base.Awake();

        maxStamina = startingStamina;
        CurrentStamina = startingStamina;
    }

    private void Start() {
        staminaConteiner = GameObject.Find(STAMINA_CONTEINER_TEXT).transform;
    }
    
    public void UseStamina() {
        CurrentStamina--;
        UpdateStaminaImages();
        StopAllCoroutines();
        StartCoroutine(SlowRefreshStaminaRoutine());
    }

    public void RefreshStamina() {
        if (CurrentStamina < maxStamina || !PlayerHealth.Instance.IsDead) {
            CurrentStamina++;
        }
        UpdateStaminaImages();
    }

    public void ReplenishStaminaOnDeath() {
        CurrentStamina = startingStamina;
        UpdateStaminaImages();
    }

    private IEnumerator SlowRefreshStaminaRoutine() {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenStaminaRefresh);
            RefreshStamina();
        }
    } 

    private void UpdateStaminaImages() {
        for (int i = 0; i < maxStamina; i++) {
            Transform child = staminaConteiner.GetChild(i);
            Image image = child?.GetComponent<Image>();

            if (i <= CurrentStamina - 1) {
                image.sprite = fullStaminaImage;
            } else {
                image.sprite = emptyStaminaImage;
            }
        }
    }
}
