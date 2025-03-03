using UnityEngine;

public class IsometricCamera : MonoBehaviour
{
    [Header("Ustawienia kamery")]
    public Transform target; // Obiekt, za którym ma pod¹¿aæ kamera
    public Vector3 offset = new Vector3(0, 10, -10); // Przesuniêcie kamery wzglêdem celu
    [Range(0.01f, 1f)]
    public float smoothSpeed = 0.125f; // P³ynnoœæ pod¹¿ania kamery
    public float orthoSize = 5f; // Rozmiar ortograficzny kamery

    void Start()
    {
        // Ustawienie kamery w tryb ortograficzny
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = orthoSize;

        // Ustawienie izometrycznego k¹ta widzenia
        // Typowa konfiguracja: obrót o 30 stopni wokó³ osi X i 45 stopni wokó³ osi Y
        transform.rotation = Quaternion.Euler(30f, 45f, 0f);
    }

    void LateUpdate()
    {
        // Jeœli zosta³ przypisany cel, kamera p³ynnie pod¹¿a za nim
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
