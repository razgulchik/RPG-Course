using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [Tooltip("Имя выхода на этой локации")]
    [SerializeField] private string sceneTransitionName;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<PlayerController>()) {
            StartCoroutine(LoadSceneRoutine());
            
        }
    }

    private IEnumerator LoadSceneRoutine() {
        UIFade.Instance.FadeToBlack();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToLoad);
        SceneManagement.Instance.SetTransitionName(sceneTransitionName);
    }
}
