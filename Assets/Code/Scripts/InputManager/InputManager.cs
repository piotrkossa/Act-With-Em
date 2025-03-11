using System;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManager", menuName = "InputManager/InputManager")]
public class InputManager : ScriptableObject, GameInput.IPlayerActions, GameInput.IUIActions, GameInput.IDialogueActions, GameInput.IInventoryActions
{
    [SerializeField] private Asset _object;
    
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
            
            SetPlayerInput();
        }
    }

    private void OnDisable() {
        _gameInput.Player.Disable();
        _gameInput.UI.Disable();
        _gameInput.Dialogue.Disable();
        _gameInput.Inventory.Disable();
    }


    public void SetUIInput() {
        _gameInput.UI.Enable();

        _gameInput.Player.Disable();
        _gameInput.Dialogue.Disable();
        _gameInput.Inventory.Disable();
    }

    public void SetPlayerInput() {
        _gameInput.Player.Enable();
    
        _gameInput.Dialogue.Disable();
        _gameInput.UI.Disable();
        _gameInput.Inventory.Disable();
    }

    public void SetDialogueInput() {
        _gameInput.Dialogue.Enable();

        _gameInput.Player.Disable();
        _gameInput.UI.Disable();
        _gameInput.Inventory.Disable();
    }

    public void SetInventoryInput() {
        _gameInput.Inventory.Enable();

        _gameInput.Player.Disable();
        _gameInput.UI.Disable();
        _gameInput.Dialogue.Disable();
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

    #endregion
}
