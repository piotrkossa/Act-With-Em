using UnityEngine;
using UnityEngine.Events;


public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float playerDetectionRange;

    private Transform _player;
    private Transform thisTransform;

    public UnityEvent<bool> onSetActive;
    public UnityEvent<Vector3> onSetTarget;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        thisTransform = this.transform;
    }

    private bool isPlayerInRange = false;
    private void Update()
    {
        if (Vector3.Distance(thisTransform.position, _player.position) < playerDetectionRange) {
            if (!isPlayerInRange) {
                isPlayerInRange = true;
                onSetActive?.Invoke(true);
            }
        }
        else if (isPlayerInRange){ 
            isPlayerInRange = false;
            onSetActive?.Invoke(false);
        }
        
        if (isPlayerInRange) {
            onSetTarget?.Invoke(_player.transform.position);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, playerDetectionRange);
    }
}
