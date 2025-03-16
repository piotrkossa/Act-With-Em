using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settings;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider fullscreenToggle;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume")) {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
        if (PlayerPrefs.HasKey("sfxVolume")) {
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        }
    }

    public void MainMenu() {
        settings.SetActive(false);
        menu.SetActive(true);
    }

    public void SetMusicVolume(float musicVolume) {
        audioMixer.SetFloat("music", musicVolume*8);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }

    public void SetSFXVolume(float sfxVolume) {
        audioMixer.SetFloat("sfx", sfxVolume*8);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }

    public void SetFullscreen(float isFullscreen) {
        if (isFullscreen == 1) {
            Screen.fullScreen = true;
        } else {
            Screen.fullScreen = false;
        }
    }
}
