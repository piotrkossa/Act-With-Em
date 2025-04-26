using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class TavernDoorsOutside : MonoBehaviour, IInteractable
{
    private Rigidbody player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    public void Interact()
    {
        Task task = SubmitPressed();
    }

    private async Task SubmitPressed()
    {
        await TransitionManager.Instance.StartTransiiton();

        player.position = new Vector3(-275f, 3.12f, -40f);

        AudioManager.Instance.PlayTavernMusic();
    }
}
