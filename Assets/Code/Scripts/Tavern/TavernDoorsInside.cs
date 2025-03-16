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
        InputManager.Instance.InteractEvent += OnInteractEvent;
    }

    // Odsubskrybowanie eventu InteractEvent, kiedy obiekt jest wy��czony
    private void OnDisable()
    {
        InputManager.Instance.InteractEvent -= OnInteractEvent;
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
    private IEnumerator SubmitPressed()
    {
        // Rozpocz�cie animacji FadeIn
        yield return StartCoroutine(TransitionManager.FadeIn());
        // Zmiana pozycji gracza po zako�czeniu animacji FadeIn
        player.transform.position = new Vector3(7f, 1.38f, -13f);
        yield return new WaitForSeconds(1.5f);

        // Rozpocz�cie animacji FadeOut
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

    // Wykrycie, kiedy gracz opu�ci obszar triggera
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
