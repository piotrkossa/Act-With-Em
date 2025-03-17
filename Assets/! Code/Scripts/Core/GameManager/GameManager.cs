using UnityEngine;

public enum GameStates {
    Player,
    Dialogue,
    Pause,
    Inventory,
    MiniGame,
    Other
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

    private void SetInventory() {
        GameState = GameStates.Inventory;
        InputManager.Instance.SetInventoryInput();
    }

    private void SetMiniGame() {
        GameState = GameStates.MiniGame;
        InputManager.Instance.SetMiniGameInput();
    }

    private void SetOther() {
        GameState = GameStates.Other;
        InputManager.Instance.DisableAll();
    }

    public void SetPreviousState() {
        SetState(previousState);
    }

    private GameStates previousState;

    public static void SetState(GameStates gameState) {
        Instance.previousState = Instance.GameState;

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
                
            case GameStates.Inventory:
                Instance.SetInventory();
                break;
            case GameStates.MiniGame:
                Instance.SetMiniGame();
                break;
            case GameStates.Other:
                Instance.SetOther();
                break;
        }
    }

    public static GameStates GetGameState() {
        return Instance.GameState;
    }
}
