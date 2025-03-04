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
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Tworzymy wektor wejœciowy
        inputVector = new Vector3(moveX, 0f, moveZ);

        // Jeœli wektor jest d³u¿szy ni¿ 1, normalizujemy (¿eby nie przekraczaæ maksymalnej prêdkoœci)
        if (inputVector.magnitude > 1f)
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
