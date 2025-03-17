using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settings;

    public void ContinueGame() {
        StartCoroutine(LoadGame());
    }

    public void Settings() {
        menu.SetActive(false);
        settings.SetActive(true);
    }
    
    public void ExitGame() {
        Application.Quit();
    }

    private bool isWorking = false;
    private IEnumerator LoadGame() {
        if (!isWorking) {
            isWorking = true;
            yield return TransitionManager.FadeIn();
            TransitionManager.FadeOut();
            SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
        }
    }

}
