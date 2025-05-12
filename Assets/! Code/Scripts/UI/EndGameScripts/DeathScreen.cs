using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public void NewGame() {
        Task task = ChangeSceneToMainGame();
        Time.timeScale = 1f;
    }

    public void LoadGame() {
        Task task = ChangeSceneToMainGame();
        Time.timeScale = 1f;
    }

    public void Exit() {
        Application.Quit();
    }

    private async Task ChangeSceneToMainGame() {
        await TransitionManager.Instance.StartTransiiton();

        GameManager.SetState(GameStates.Player);
        SceneManager.LoadScene("MainGame");
    }
}
