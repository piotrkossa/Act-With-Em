using System.Threading.Tasks;
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

        player.isKinematic = true;
        player.position = new Vector3(-274.75f, 3.052f, -43.463f);
        player.isKinematic = false;

        AudioManager.Instance.PlayTavernMusic();
    }
}
