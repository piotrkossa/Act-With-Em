using UnityEngine;

public enum GameStates {
    MainMenu,
    GameMenu,
    Gameplay,
    Loading,
    Inventory,
    Shop,
    Dialog
}

public static class GameManager
{
    public static GameStates gameState {get; private set;}

    public static void ChangeGameState(GameStates gs) {
        gameState = gs;
    }
}
