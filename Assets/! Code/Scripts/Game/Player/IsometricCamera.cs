using UnityEngine;

public class IsometricCamera : MonoBehaviour
{
    [Header("Ustawienia kamery")]
    public Transform target; // Obiekt, za którym ma pod¹¿aæ kamera
    public Vector3 offset = new Vector3(0, 10, -10); // Przesuniêcie kamery wzglêdem celu
    [Range(0.01f, 1f)]
    public float smoothTime = 0.125f; // Czas wyg³adzania ruchu kamery (mniejsza wartoœæ = szybsza reakcja)
    public float orthoSize = 5f;      // Rozmiar ortograficzny kamery
    public float lookAheadDistance = 2f; // Odleg³oœæ przesuniêcia kamery w kierunku ruchu gracza

    private Vector3 velocity = Vector3.zero; // zmienna pomocnicza do SmoothDamp
    private Vector3 lastTargetPosition;      // zapamiêtana poprzednia pozycja celu

    void Start()
    {
        // Ustawienie kamery w tryb ortograficzny
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = orthoSize;

        // Ustawienie k¹ta dla efektu izometrycznego
        transform.rotation = Quaternion.Euler(30f, 45f, 0f);

        // Zainicjuj zapamiêtan¹ pozycjê
        if (target != null)
        {
            lastTargetPosition = target.position;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Obliczamy bazow¹ pozycjê kamery
        Vector3 targetPosition = target.position + offset;

        // Wyliczamy wektor ruchu gracza miêdzy klatkami
        Vector3 movementDirection = (target.position - lastTargetPosition);

        // Jeœli gracz siê poruszy³, dodajemy look-ahead
        if (movementDirection.sqrMagnitude > 0.001f)
        {
            // normalizujemy wektor, ¿eby dodaæ zawsze tê sam¹ odleg³oœæ w przód
            movementDirection.Normalize();
            targetPosition += movementDirection * lookAheadDistance;
        }

        // P³ynne przesuniêcie kamery w pozycjê docelow¹
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );

        // Zapamiêtujemy obecn¹ pozycjê gracza na nastêpn¹ klatkê
        lastTargetPosition = target.position;
    }
}
