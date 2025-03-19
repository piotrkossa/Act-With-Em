using UnityEngine;


public class InputManagerLoader : MonoBehaviour {
    private void Awake() {
        var inputManager = InputManager.Instance; // To wymusi załadowanie i OnEnable()
    }
}