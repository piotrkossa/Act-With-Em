using System;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManager", menuName = "ScriptableObjects/InputManager")]
public class InputManager : ScriptableObject, GameInput.IPlayerActions, GameInput.IUIActions
{
    [SerializeField] private Asset _object;
    
    private static InputManager _instance;

    public static InputManager Instance {
        get {
            if (_instance == null) {
                _instance = Resources.Load<InputManager>("ScriptableObjects/InputManager");
            }
            return _instance;
        }
    }

    private GameInput _gameInput;

    private void OnEnable() {
        if (_gameInput == null) {
            _gameInput = new GameInput();

            _gameInput.Player.SetCallbacks(this);
            _gameInput.UI.SetCallbacks(this);
            
            SetPlayerInput();
        }
    }

    private void OnDisable() {
        _gameInput.Player.Disable();
        _gameInput.UI.Disable();
    }

    private void SetUIInput() {
        _gameInput.Player.Disable();
        _gameInput.UI.Enable();
    }

    private void SetPlayerInput() {
        _gameInput.Player.Enable();
        _gameInput.UI.Disable();
    }


    // Player
    public event Action<Vector2> MoveEvent;
    public event Action AttackEvent;
    public event Action DodgeEvent;
    public event Action PauseEvent;
    public event Action InteractEvent;


    // UI
    public event Action ResumeEvent;


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
            SetUIInput();
        }
    }


    void GameInput.IUIActions.OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            ResumeEvent?.Invoke();
            SetPlayerInput();
        }
    }

    void GameInput.IPlayerActions.OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            InteractEvent?.Invoke();
        }
    }
}
