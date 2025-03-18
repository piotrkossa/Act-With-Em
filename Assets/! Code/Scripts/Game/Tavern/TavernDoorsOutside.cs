using System.Threading.Tasks;
using UnityEngine;

public class TavernDoorsOutside : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject player;

    public void Interact()
    {
        Task task = SubmitPressed();
    }

    private async Task SubmitPressed()
    {
        await TransitionManager.Instance.StartTransiiton();

        player.transform.position = new Vector3(-275f, 3.12f, -40f);

        AudioManager.Instance.PlayTavernMusic();
    }
}
