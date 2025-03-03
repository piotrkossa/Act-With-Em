using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ustawienia ruchu")]
    public float speed = 5f; // Szybkoœæ poruszania siê gracza

    void Update()
    {
        // Pobranie danych wejœciowych z klawiatury
        float moveX = Input.GetAxis("Horizontal"); // ruch w lewo/prawo
        float moveZ = Input.GetAxis("Vertical");   // ruch do przodu/ty³u

        // Pobranie kierunków ruchu wzglêdem kamery, aby sterowanie by³o intuicyjne
        Transform cam = Camera.main.transform;
        Vector3 camForward = cam.forward;
        camForward.y = 0; // ignorujemy pionow¹ sk³adow¹
        camForward.Normalize();
        Vector3 camRight = cam.right;
        camRight.y = 0;
        camRight.Normalize();

        // Wyliczenie kierunku ruchu gracza na podstawie wejœcia i ustawieñ kamery
        Vector3 movement = (camRight * moveX + camForward * moveZ);
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }

        // Przemieszczenie gracza w przestrzeni œwiata
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}
