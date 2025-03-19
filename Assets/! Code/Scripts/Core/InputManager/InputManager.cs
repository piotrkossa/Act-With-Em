using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManager", menuName = "InputManager/InputManager")]
public class InputManager : ScriptableObject, GameInput.IPlayerActions, GameInput.IUIActions, GameInput.IDialogueActions, GameInput.IInventoryActions, GameInput.IMiniGameActions
{
    private static InputManager _instance;

    public static InputManager Instance {
        get {
            if (_instance == null)
                _instance = Resources.Load<InputManager>("InputManager/InputManager");
            return _instance;
        }
        private set {}
    }

    private GameInput _gameInput;

    private void OnEnable() {
        if (_gameInput == null) {
            _gameInput = new GameInput();

            _gameInput.Player.SetCallbacks(this);
            _gameInput.UI.SetCallbacks(this);
            _gameInput.Dialogue.SetCallbacks(this);
            _gameInput.Inventory.SetCallbacks(this);
            _gameInput.MiniGame.SetCallbacks(this);
        }
        SetPlayerInput();
        Debug.Log(1);
    }

    private void OnDisable() {
        DisableAll();
    }


    public void SetUIInput() {
        DisableAll();

        _gameInput.UI.Enable();
    }

    public void SetPlayerInput() {
        DisableAll();

        _gameInput.Player.Enable();
    }

    public void SetDialogueInput() {
        DisableAll();

        _gameInput.Dialogue.Enable();
    }

    public void SetInventoryInput() {
        DisableAll();

        _gameInput.Inventory.Enable();
    }

    public void SetMiniGameInput() {
        DisableAll();

        _gameInput.MiniGame.Enable();
    }

    public void DisableAll() {
        _gameInput.Inventory.Disable();
        _gameInput.Player.Disable();
        _gameInput.UI.Disable();
        _gameInput.Dialogue.Disable();
        _gameInput.MiniGame.Disable();
    }


    // Player
    public class Player {
        public static UnityEvent<Vector2> MoveEvent = new UnityEvent<Vector2>();
        public static UnityEvent AttackEvent = new UnityEvent();
        public static UnityEvent DodgeEvent = new UnityEvent();
        public static UnityEvent PauseEvent = new UnityEvent();
        public static UnityEvent InteractEvent = new UnityEvent();
        public static UnityEvent OnInvEnter = new UnityEvent();
    }

    // UI
    public class UI {
        public static UnityEvent ResumeEvent = new UnityEvent();
    }

    // Dialogue
    public class Dialogue {
        public static UnityEvent OnSkipEvent = new UnityEvent();
    }

    // Inventory
    public class Inventory {
        public static UnityEvent OnInvExit = new UnityEvent();
    }

    // HotBar
    public class HotBar {
        public static UnityEvent<int> OnHotBarItemChoosen = new UnityEvent<int>();
    }

    // MiniGame
    public class MiniGame {
        public static UnityEvent LeftClick = new UnityEvent();
    }


    #region Input Interface Implementations

    //Player
    void GameInput.IPlayerActions.OnMove(InputAction.CallbackContext context)
    {
        Player.MoveEvent?.Invoke(context.ReadValue<Vector2>());
        Debug.Log(context.ReadValue<Vector2>());
    }

    void GameInput.IPlayerActions.OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            Player.AttackEvent?.Invoke();
    }

    void GameInput.IPlayerActions.OnDodge(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            Player.DodgeEvent?.Invoke();
    }

    void GameInput.IPlayerActions.OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            Player.PauseEvent?.Invoke();
        }
    }

    void GameInput.IPlayerActions.OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Player.InteractEvent?.Invoke();
        }
    }

    void GameInput.IPlayerActions.OnInvOpen(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            Player.OnInvEnter?.Invoke();
        }
    }


    // UI
    void GameInput.IUIActions.OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            UI.ResumeEvent?.Invoke();
        }
    }


    // Dialogue
    void GameInput.IDialogueActions.OnSkip(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            Dialogue.OnSkipEvent?.Invoke();
        }
    }

    // Inventory
    void GameInput.IInventoryActions.OnInvClose(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            Inventory.OnInvExit?.Invoke();
        }
    }

    void GameInput.IPlayerActions.OnHotBar1(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            HotBar.OnHotBarItemChoosen?.Invoke(0);
        }
    }

    void GameInput.IPlayerActions.OnHotBar2(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            HotBar.OnHotBarItemChoosen?.Invoke(1);
        }
    }

    void GameInput.IPlayerActions.OnHotBar3(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            HotBar.OnHotBarItemChoosen?.Invoke(2);
        }
    }

    void GameInput.IMiniGameActions.OnClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            MiniGame.LeftClick?.Invoke();
        }
    }

    #endregion
}
