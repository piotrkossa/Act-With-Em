using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashDistance = 5f; // Dystans do jakiego gracz się "dasha"
    public float dashCooldown = 1f; // Czas odnowienia dasha
    public float dashDuration = 0.2f; // Jak długo trwa dash
    public KeyCode dashKey = KeyCode.Space; // Klawisz, którym gracz aktywuje dash
    private bool isDashing = false; // Flaga informująca, czy gracz jest w trakcie dasha
    private float dashTime = 0f; // Czas, przez który trwa dash
    private Vector3 dashDirection; // Kierunek, w którym postać wykonuje dash
    private float lastDashTime = -999f; // Czas ostatniego dasha (do obsługi cooldownu)

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Sprawdzamy, czy gracz chce wykonać dash
        if (Input.GetKeyDown(dashKey) && Time.time >= lastDashTime + dashCooldown && !isDashing)
        {
            Dash();
        }

        // Jeśli dash trwa, wykonaj jego logikę
        if (isDashing)
        {
            dashTime += Time.deltaTime;
            rb.MovePosition(transform.position + dashDirection * dashDistance * Time.deltaTime / dashDuration);

            // Zakończenie dasha po upływie odpowiedniego czasu
            if (dashTime >= dashDuration)
            {
                isDashing = false;
                dashTime = 0f;
            }
        }
    }

    void Dash()
    {
        // Pobieramy ruch gracza (input w poziomie i pionie)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Ustalamy kierunek dasha w zależności od wejścia
        if (horizontal > 0) // Ruch w prawo
            dashDirection = transform.right;
        else if (horizontal < 0) // Ruch w lewo
            dashDirection = -transform.right;
        else if (vertical > 0) // Ruch w górę
            dashDirection = transform.forward;
        else if (vertical < 0) // Ruch w dół
            dashDirection = -transform.forward;

        // Aktywujemy dash i ustawiamy czas ostatniego dasha
        isDashing = true;
        lastDashTime = Time.time;
    }
}