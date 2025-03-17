using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TavernDoorsInside : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private bool playerInRange = false;

    // Subskrybujemy event InteractEvent, kiedy obiekt jest w��czony
    private void OnEnable()
    {
        InputManager.Player.InteractEvent.AddListener(OnInteractEvent);
        playerInRange = false;
    }

    // Odsubskrybowanie eventu InteractEvent, kiedy obiekt jest wy��czony
    private void OnDisable()
    {
        InputManager.Player.InteractEvent.RemoveListener(OnInteractEvent);
    }

    private void Start() {
        playerInRange = false;
    }

    // Funkcja wywo�ywana po naci�ni�ciu przycisku InteractEvent
    private void OnInteractEvent()  // Ta metoda teraz uruchamia Coroutine
    {
        if (playerInRange)
        {
            StartCoroutine(SubmitPressed());  // Uruchomienie Coroutine
        }
    }

    // Funkcja wywo�ywana w ramach Coroutine
    private bool isWorking = false;
    private IEnumerator SubmitPressed()
    {
        if (isWorking) yield break;
        isWorking = true;
        // Rozpocz�cie animacji FadeIn
        GameManager.SetState(GameStates.Dialogue);
        playerInRange = false;
        yield return StartCoroutine(TransitionManager.FadeIn());
        // Zmiana pozycji gracza po zako�czeniu animacji FadeIn
        player.transform.position = new Vector3(-1.25f, 1.38f, -15.01f);
        AudioManager.Instance.PlayForestMusic();
        yield return new WaitForSeconds(1.5f);
        player.transform.position = new Vector3(-1.25f, 1.38f, -15.01f);

        // Rozpocz�cie animacji FadeOut
        yield return StartCoroutine(TransitionManager.FadeOut());
        playerInRange = false;
        GameManager.SetState(GameStates.Player);
        isWorking = false;
    }

    // Wykrycie, kiedy gracz wejdzie w obszar triggera
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    // Wykrycie, kiedy gracz opu�ci obszar triggera
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
