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

        player.isKinematic = true;
        player.position = new Vector3(-757.42f, 1.353f, 19.2f);
        player.isKinematic = false;

        AudioManager.Instance.PlayForestMusic();
    }
}
