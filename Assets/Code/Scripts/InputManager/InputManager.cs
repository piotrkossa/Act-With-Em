using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManager", menuName = "InputManager/InputManager")]
public class InputManager : ScriptableObject, GameInput.IPlayerActions, GameInput.IUIActions, GameInput.IDialogueActions, GameInput.IInventoryActions, GameInput.IMiniGameActions
{
    
    private static InputManager _instance;

    public static InputManager Instance {
        get {
            if (_instance == null) {
                _instance = Resources.Load<InputManager>("InputManager/InputManager");
            }
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
            
            SetPlayerInput();
        }
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

    private void DisableAll() {
        _gameInput.Inventory.Disable();
        _gameInput.Player.Disable();
        _gameInput.UI.Disable();
        _gameInput.Dialogue.Disable();
        _gameInput.MiniGame.Disable();
    }


    // Player
    public event Action<Vector2> MoveEvent;
    public event Action AttackEvent;
    public event Action DodgeEvent;
    public event Action PauseEvent;
    public event Action InteractEvent;
    public event Action OnInvEnter;

    // UI
    public event Action ResumeEvent;

    // Dialogue
    public event Action OnSkipEvent;

    // Inventory
    public event Action OnInvExit;

    // HotBar
    public event Action<int> OnHotBarItemChoosen;

    // MiniGame
    public event Action OnMiniGameLeftClick;


    #region Input Interface Implementations

    //Player
    void GameInput.IPlayerActions.OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    void GameInput.IPlayerActions.OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            AttackEvent?.Invoke();
    }

    void GameInput.IPlayerActions.OnDodge(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            DodgeEvent?.Invoke();
    }

    void GameInput.IPlayerActions.OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            PauseEvent?.Invoke();
        }
    }

    void GameInput.IPlayerActions.OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            InteractEvent?.Invoke();
        }
    }

    void GameInput.IPlayerActions.OnInvOpen(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            OnInvEnter?.Invoke();
        }
    }


    // UI
    void GameInput.IUIActions.OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            ResumeEvent?.Invoke();
        }
    }


    // Dialogue
    void GameInput.IDialogueActions.OnSkip(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            OnSkipEvent?.Invoke();
        }
    }

    // Inventory
    void GameInput.IInventoryActions.OnInvClose(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            OnInvExit?.Invoke();
        }
    }

    public void OnHotBar1(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            OnHotBarItemChoosen?.Invoke(0);
        }
    }

    public void OnHotBar2(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            OnHotBarItemChoosen?.Invoke(1);
        }
    }

    public void OnHotBar3(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            OnHotBarItemChoosen?.Invoke(2);
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            OnMiniGameLeftClick?.Invoke();
        }
    }

    #endregion
}
