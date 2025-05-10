using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// 1. Dodaj implementacjê nowego interfejsu IQuestLogActions
[CreateAssetMenu(fileName = "InputManager", menuName = "InputManager/InputManager")]
public class InputManager : ScriptableObject, GameInput.IPlayerActions, GameInput.IUIActions, GameInput.IDialogueActions, GameInput.IInventoryActions, GameInput.IMiniGameActions, GameInput.IQuestLogActions
{
    private static InputManager _instance;

    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<InputManager>("InputManager/InputManager");
            return _instance;
        }
        private set { }
    }

    private GameInput _gameInput;

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();

            _gameInput.Player.SetCallbacks(this);
            // Zachowaj, jeœli mapa UI jest u¿ywana do innych akcji (np. Pause)
            _gameInput.UI.SetCallbacks(this);
            _gameInput.Dialogue.SetCallbacks(this);
            _gameInput.Inventory.SetCallbacks(this);
            _gameInput.MiniGame.SetCallbacks(this);
            // 2. Ustaw Callbacks dla nowej mapy QuestLog
            _gameInput.QuestLog.SetCallbacks(this);
        }
        // Domyœlnie ustawiamy stan gracza
        SetPlayerInput();
        Debug.Log(1);
    }

    private void OnDisable()
    {
        DisableAll();
    }

    // Metoda do ustawienia stanu UI (np. dla menu pauzy, jeœli mapa UI do tego s³u¿y)
    public void SetUIInput()
    {
        DisableAll();
        _gameInput.UI.Enable();
    }

    // Metoda do ustawienia stanu gracza
    public void SetPlayerInput()
    {
        DisableAll();
        _gameInput.Player.Enable();
        // Jeœli mapa UI jest u¿ywana do innych akcji (np. Pause), mo¿esz j¹ tu równie¿ w³¹czyæ
        // _gameInput.UI.Enable();
        // 5. W³¹cz mapê QuestLog w trybie gracza, aby mo¿na by³o otworzyæ dziennik
        _gameInput.QuestLog.Enable();
    }

    // Metoda do ustawienia stanu dialogu
    public void SetDialogueInput()
    {
        DisableAll();
        _gameInput.Dialogue.Enable();
        // Jeœli potrzebujesz akcji z mapy UI (np. Resume) lub QuestLog (np. Toggle) w stanie dialogu, w³¹cz je tutaj
        // _gameInput.UI.Enable();
        // _gameInput.QuestLog.Enable();
    }

    // Metoda do ustawienia stanu ekwipunku
    public void SetInventoryInput()
    {
        DisableAll();
        _gameInput.Inventory.Enable();
        // Jeœli potrzebujesz akcji z mapy UI (np. Resume) lub QuestLog (np. Toggle) w stanie ekwipunku, w³¹cz je tutaj
        // _gameInput.UI.Enable();
        // _gameInput.QuestLog.Enable();
    }

    // Metoda do ustawienia stanu minigry
    public void SetMiniGameInput()
    {
        DisableAll();
        _gameInput.MiniGame.Enable();
        // Jeœli potrzebujesz akcji z mapy UI lub QuestLog w stanie minigry, w³¹cz je tutaj
        // _gameInput.UI.Enable();
        // _gameInput.QuestLog.Enable();
    }

    // 5. Dodaj now¹ metodê do ustawienia stanu dziennika zadañ
    public void SetQuestLogInput()
    {
        DisableAll();
        // W³¹cz mapê QuestLog, aby mo¿na by³o zamkn¹æ dziennik klawiszem Q
        _gameInput.QuestLog.Enable();
        // Jeœli nawigacja wewn¹trz dziennika korzysta z akcji z mapy Dialogue lub UI, w³¹cz je tutaj:
        // _gameInput.Dialogue.Enable();
        // _gameInput.UI.Enable();
    }


    // 3. Dodaj now¹ mapê QuestLog do dezaktywacji
    public void DisableAll()
    {
        _gameInput.Inventory.Disable();
        _gameInput.Player.Disable();
        _gameInput.UI.Disable();
        _gameInput.Dialogue.Disable();
        _gameInput.MiniGame.Disable();
        _gameInput.QuestLog.Disable(); // Dodaj tê liniê
    }


    // Player
    public class Player
    {
        public static UnityEvent<Vector2> MoveEvent = new UnityEvent<Vector2>();
        public static UnityEvent AttackEvent = new UnityEvent();
        public static UnityEvent DodgeEvent = new UnityEvent();
        public static UnityEvent PauseEvent = new UnityEvent();
        public static UnityEvent InteractEvent = new UnityEvent();
        public static UnityEvent OnInvEnter = new UnityEvent();
    }

    // UI
    public class UI
    {
        public static UnityEvent ResumeEvent = new UnityEvent();
        // To zdarzenie jest wywo³ywane niezale¿nie od tego, która mapa akcji wywo³a³a toggle
        public static UnityEvent QuestLogToggleEvent = new UnityEvent();
    }

    // Dialogue
    public class Dialogue
    {
        public static UnityEvent OnSkipEvent = new UnityEvent();
    }

    // Inventory
    public class Inventory
    {
        public static UnityEvent OnInvExit = new UnityEvent();
    }

    // HotBar
    public class HotBar
    {
        public static UnityEvent<int> OnHotBarItemChoosen = new UnityEvent<int>();
    }

    // MiniGame
    public class MiniGame
    {
        public static UnityEvent LeftClick = new UnityEvent();
    }


    #region Input Interface Implementations

    //Player
    void GameInput.IPlayerActions.OnMove(InputAction.CallbackContext context)
    {
        Player.MoveEvent?.Invoke(context.ReadValue<Vector2>());
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
        if (context.phase == InputActionPhase.Performed)
        {
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
        if (context.phase == InputActionPhase.Performed)
        {
            Player.OnInvEnter?.Invoke();
        }
    }


    // UI
    void GameInput.IUIActions.OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            UI.ResumeEvent?.Invoke();
        }
    }

    // === USUÑ TÊ METODÊ, PONIEWA¯ AKCJA QUESTLOGTOGGLE JEST TERAZ W INNEJ MAPIE ===
    //void GameInput.IUIActions.OnQuestLogToggle(InputAction.CallbackContext context)
    //{
    //    if (context.phase == InputActionPhase.Performed)
    //    {
    //        UI.QuestLogToggleEvent?.Invoke();
    //    }
    //}
    // ===============================================================================


    // Dialogue
    void GameInput.IDialogueActions.OnSkip(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Dialogue.OnSkipEvent?.Invoke();
        }
    }

    // Inventory
    void GameInput.IInventoryActions.OnInvClose(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Inventory.OnInvExit?.Invoke();
        }
    }

    void GameInput.IPlayerActions.OnHotBar1(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            HotBar.OnHotBarItemChoosen?.Invoke(0);
        }
    }

    void GameInput.IPlayerActions.OnHotBar2(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            HotBar.OnHotBarItemChoosen?.Invoke(1);
        }
    }

    void GameInput.IPlayerActions.OnHotBar3(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            HotBar.OnHotBarItemChoosen?.Invoke(2);
        }
    }

    void GameInput.IMiniGameActions.OnClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            MiniGame.LeftClick?.Invoke();
        }
    }

    void GameInput.IQuestLogActions.OnQuestLogToggle(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // Nadal wywo³ujemy to samo zdarzenie w klasie UI
            UI.QuestLogToggleEvent?.Invoke();
        }
    }

    #endregion
}