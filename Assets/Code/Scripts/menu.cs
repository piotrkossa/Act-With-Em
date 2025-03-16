using UnityEngine;

public class Menu : MonoBehaviour
{

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settings;

    public void ContinueGame() {
        
    }

    public void NewGame() {

    }

    public void LoadGame() {

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
