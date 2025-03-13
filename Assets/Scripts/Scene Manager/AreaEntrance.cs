using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [Tooltip("Имя выхода в соседней локации")]
   [SerializeField] private string transitionName;

   private void Start() {
    if(transitionName == SceneManagement.Instance.SceneTransitionName) {
        PlayerController.Instance.transform.position = this.transform.position;
        CameraController.Instance.SetPlayerCameraFollow();
        UIFade.Instance.FadeToClear();
    }
   }
}
