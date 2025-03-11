using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    // public float playerHP = 100;
    [SerializeField] private Image healthBarFill;

    // void Update()
    // {
    //     healthbar(playerHP);
    // }

    private void healthbar(float hp) {
        healthBarFill.fillAmount = hp / 100f;
    }
}
