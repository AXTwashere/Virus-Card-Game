using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public BattleManager battleManager;
    CanvasGroup canvasGroup;

    public void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public void StartGame() {
        battleManager.GameStart();
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void QuitGame() {
        Application.Quit();
    }
}
