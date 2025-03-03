using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ustawienia ruchu")]
    public float speed = 5f; // Szybkoœæ poruszania siê gracza

    private Rigidbody rb;
    private Vector3 inputVector; // przechowuje dane wejœciowe pobrane w Update

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

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
        inputVector = (camRight * moveX + camForward * moveZ);
        if (inputVector.magnitude > 1)
        {
            inputVector.Normalize();
        }
    }

    void FixedUpdate()
    {
        // Nowa pozycja gracza przy uwzglêdnieniu ruchu i prêdkoœci
        Vector3 newPos = rb.position + inputVector * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }
}
