using UnityEngine;

public enum GameStates
{
    Player,
    Dialogue,
    Pause,
    Inventory,
    MiniGame,
    Other
}

public class GameManager : MonoBehaviour
{
    private GameStates GameState = GameStates.Player;
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                _instance = go.AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void SetPlayer()
    {
        GameState = GameStates.Player;
        if (InputManager.Instance != null) InputManager.Instance.SetPlayerInput();
    }

    private void SetPause()
    {
        GameState = GameStates.Pause;
        if (InputManager.Instance != null) InputManager.Instance.SetUIInput();
    }

    private void SetDialogue()
    {
        GameState = GameStates.Dialogue;
        if (InputManager.Instance != null) InputManager.Instance.SetDialogueInput();
    }

    private void SetInventory()
    {
        GameState = GameStates.Inventory;
        if (InputManager.Instance != null) InputManager.Instance.SetInventoryInput();
    }

    private void SetMiniGame()
    {
        GameState = GameStates.MiniGame;
        if (InputManager.Instance != null) InputManager.Instance.SetMiniGameInput();
    }

    private void SetOther()
    {
        GameState = GameStates.Other;
        if (InputManager.Instance != null) InputManager.Instance.DisableAll();
    }

    public static void SetPreviousState()
    {
        if (Instance != null)
        {
            SetState(Instance.previousState);
        }
    }

    private GameStates previousState;

    public static void SetState(GameStates gameState)
    {
        if (Instance == null)
        {
            Debug.LogError("[GameManager] Instance is null. Cannot set state.");
            return;
        }

        Instance.previousState = Instance.GameState;
        Instance.GameState = gameState;

        switch (gameState)
        {
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

    public static GameStates GetGameState()
    {
        if (Instance == null)
        {
            Debug.LogWarning("[GameManager] Instance is null when trying to GetGameState. Returning default (Player).");
            return GameStates.Player;
        }
        return Instance.GameState;
    }
}