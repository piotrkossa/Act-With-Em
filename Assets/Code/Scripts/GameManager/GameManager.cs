using UnityEngine;

public enum GameStates {
    Player,
    Dialogue,
    Pause
}

public class GameManager: MonoBehaviour
{
    private GameStates GameState = GameStates.Player;

    private static GameManager _instance;

    private static GameManager Instance {
        get {
            if (_instance == null) {
                _instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    private void SetPlayer() {
        GameState = GameStates.Player;
        InputManager.Instance.SetPlayerInput();
    }

    private void SetPause() {
        GameState = GameStates.Pause;
        InputManager.Instance.SetUIInput();
    }

    private void SetDialogue() {
        GameState = GameStates.Dialogue;
        InputManager.Instance.SetDialogueInput();
    }


    public static void SetState(GameStates gameState) {
        switch (gameState) {
            case GameStates.Player:
                Instance.SetPlayer();
                break;

            case GameStates.Dialogue:
                Instance.SetDialogue();
                break;

            case GameStates.Pause:
                Instance.SetPause();
                break;
        }
    }

    public static GameStates GetGameState() {
        return Instance.GameState;
    }
}
