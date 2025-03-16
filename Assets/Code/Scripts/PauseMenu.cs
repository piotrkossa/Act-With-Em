using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settings;


    void Start()
    {
        menu.SetActive(false);
        settings.SetActive(false);
        InputManager.Instance.ResumeEvent += ResumeGame;
        InputManager.Instance.PauseEvent += PauseGame;
    }


    public void PauseGame() {
        menu.SetActive(true);
    }

    public void ResumeGame() {
        menu.SetActive(false);
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
