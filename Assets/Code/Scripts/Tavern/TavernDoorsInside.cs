using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TavernDoorsInside : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private bool playerInRange = false;

    // Subskrybujemy event InteractEvent, kiedy obiekt jest w³¹czony
    private void OnEnable()
    {
        InputManager.Instance.InteractEvent += OnInteractEvent;
    }

    // Odsubskrybowanie eventu InteractEvent, kiedy obiekt jest wy³¹czony
    private void OnDisable()
    {
        InputManager.Instance.InteractEvent -= OnInteractEvent;
    }

    // Funkcja wywo³ywana po naciœniêciu przycisku InteractEvent
    private void OnInteractEvent()  // Ta metoda teraz uruchamia Coroutine
    {
        if (playerInRange)
        {
            StartCoroutine(SubmitPressed());  // Uruchomienie Coroutine
        }
    }

    // Funkcja wywo³ywana w ramach Coroutine
    private IEnumerator SubmitPressed()
    {
        // Rozpoczêcie animacji FadeIn
        yield return StartCoroutine(TransitionManager.FadeIn());

        // Zmiana pozycji gracza po zakoñczeniu animacji FadeIn
        player.transform.position = new Vector3(7f, player.transform.position.y, -13f);

        // Rozpoczêcie animacji FadeOut
        yield return StartCoroutine(TransitionManager.FadeOut());
    }

    // Wykrycie, kiedy gracz wejdzie w obszar triggera
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    // Wykrycie, kiedy gracz opuœci obszar triggera
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
