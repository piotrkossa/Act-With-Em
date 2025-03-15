using UnityEngine;
using UnityEngine.SceneManagement;

public class TavernDoorsInside : MonoBehaviour
{
    private bool playerInRange = false;

    private void OnEnable()
    {
        InputManager.Instance.InteractEvent += SubmitPressed;
    }

    private void OnDisable()
    {
        InputManager.Instance.InteractEvent -= SubmitPressed;
    }

    private void SubmitPressed()
    {
        if (playerInRange)
        {
            SceneManager.LoadScene("MainGame");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
