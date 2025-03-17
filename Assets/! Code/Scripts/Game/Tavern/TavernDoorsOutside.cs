using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TavernDoorsOutside : MonoBehaviour
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
            Task task = SubmitPressed();  // Uruchomienie Coroutine
        }
    }


    private async Task SubmitPressed()
    {
        await TransitionManager.Instance.StartTransiiton();

        player.transform.position = new Vector3(-275f, 3.12f, -40f);

        AudioManager.Instance.PlayTavernMusic();
        playerInRange = false;
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
