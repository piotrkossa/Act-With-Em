using System.Threading.Tasks;
using UnityEngine;

public class TavernDoorsInside : MonoBehaviour, IInteractable
{
    private Rigidbody player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    public void Interact()
    {
        Task task = SubmitPressed();
    }

    private async Task SubmitPressed()
    {
        await TransitionManager.Instance.StartTransiiton();

        player.position = new Vector3(-1.25f, 1.38f, -15.01f);

        AudioManager.Instance.PlayForestMusic();
    }
}
