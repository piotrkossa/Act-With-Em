using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;

    private float lastHorizontal = 0f;
    private float lastVertical = 0f;

    void Update()
    {
        // Odczytujemy wartości ruchu gracza w przestrzeni 3D
        float horizontal = Input.GetAxisRaw("Horizontal"); // Ruch w lewo/prawo (X)
        float vertical = Input.GetAxisRaw("Vertical");     // Ruch w przód/tył (Z)

        // Sprawdzamy, czy postać się porusza
        if (horizontal != 0 || vertical != 0)
        {
            // Jeżeli ruch w poziomie (X) jest silniejszy, wybieramy animację lewo/prawo
            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
            {
                if (horizontal > 0 && lastHorizontal <= 0)
                {
                    animator.SetTrigger("RightTrigger");
                }
                else if (horizontal < 0 && lastHorizontal >= 0)
                {
                    animator.SetTrigger("LeftTrigger");
                }
            }
            // Jeżeli ruch w pionie (Z) dominuje, wybieramy animację przód/tył
            else
            {
                if (vertical > 0 && lastVertical <= 0)
                {
                    animator.SetTrigger("UpTrigger"); // Ruch do przodu
                }
                else if (vertical < 0 && lastVertical >= 0)
                {
                    animator.SetTrigger("DownTrigger"); // Ruch do tyłu
                }
            }
        }
        else
        {
            animator.SetTrigger("IdleTrigger"); // Jeśli postać stoi, uruchom Idle
        }

        // Zapamiętujemy poprzednie wartości
        lastHorizontal = horizontal;
        lastVertical = vertical;
    }
}