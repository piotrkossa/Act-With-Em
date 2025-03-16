using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;

    [Header("Music")]
    [SerializeField] private AudioClip tavernMusic;
    [SerializeField] private AudioClip forestMusic;

    private void Start() {
        musicSource.clip = forestMusic;
        musicSource.Play();
    }

    public void PlayTavernMusic() {
        musicSource.clip = tavernMusic;
        musicSource.Play();
    }

    public void PlayForestMusic() {
        musicSource.clip = forestMusic;
        musicSource.Play();
    }
}
