using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public void NewGame() {
        SaveDataManager.instance.NewGame();
        Task task = ChangeSceneToMainGame();
    }

    public void LoadGame() {
        Task task = ChangeSceneToMainGame();
    }

    public void Exit() {
        Application.Quit();
    }

    private async Task ChangeSceneToMainGame() {
        await TransitionManager.Instance.StartTransiiton();

        GameManager.SetState(GameStates.Player);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainGame");
    }
}
