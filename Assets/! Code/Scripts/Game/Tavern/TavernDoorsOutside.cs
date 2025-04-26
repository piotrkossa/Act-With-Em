using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class TavernDoorsOutside : MonoBehaviour, IInteractable, ISaveData
{
    private Rigidbody player;
    private int entered = 0;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    public void LoadData(GameData data)
    {
        this.entered = data.entered;
    }

    public void SaveData(ref GameData data)
    {
        data.entered = this.entered;
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

        entered++;
    }
}
