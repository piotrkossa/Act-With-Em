using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settings;


    void Start()
    {
        menu.SetActive(false);
        settings.SetActive(false);
        this.transform.parent.gameObject.SetActive(false);
        InputManager.UI.ResumeEvent.AddListener(ResumeGame);
        InputManager.Player.PauseEvent.AddListener(PauseGame);
    }


    public void PauseGame() {
        GameManager.SetState(GameStates.Pause);
        Time.timeScale = 0f;
        menu.SetActive(true);
        this.transform.parent.gameObject.SetActive(true);
    }

    public void ResumeGame() {
        menu.SetActive(false);
        settings.SetActive(false);
        this.transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1f;
        GameManager.SetState(GameStates.Player);
    }

    public void Settings() {
        menu.SetActive(false);
        settings.SetActive(true);
    }
    
    public void ExitGame() {
        Debug.Log("exit");
        Application.Quit();
    }
}
