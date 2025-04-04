using UnityEngine;
using UnityEngine.Events;

public enum PlayerStates {
    idle,
    moveUp,
    moveDown,
    moveLeft,
    moveRight,
    attacking,
    knocked
}

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerStates currentState {get; private set;} = PlayerStates.idle;

    public bool canMove {get; private set;} = true;

    public UnityEvent onStateChanged;

    private int blockCount = 0;

    public void SetState(PlayerStates state) {
        if (CheckIfCanMove(state)) {
            blockCount--;
            if (blockCount <= 0) {
                canMove = true;
                blockCount = 0;
            }
        }
        else {
            blockCount++;
            canMove = false;
        }

        if (state == currentState) return;
        currentState = state;

        onStateChanged?.Invoke();
    }

    private bool CheckIfCanMove(PlayerStates state) {
        if (state == PlayerStates.attacking || state == PlayerStates.knocked) {
            return false;
        }
        return true;
    }
}
