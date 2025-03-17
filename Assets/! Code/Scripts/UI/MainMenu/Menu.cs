using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settings;

    public void ContinueGame() {
        Task task = LoadGame();
    }

    public void Settings() {
        menu.SetActive(false);
        settings.SetActive(true);
    }
    
    public void ExitGame() {
        Application.Quit();
    }

    private bool isWorking = false;

    private async Task LoadGame() {
        if (!isWorking) {
            isWorking = true;
            await TransitionManager.Instance.StartTransiiton();
            SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
        }
    }

}
