using System.Collections.Generic;
using UnityEngine;

public interface IEnemy {
    void SetTarget(Vector3 target);
    void SetActive(bool active);
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float playerDetectionRange;

    private Transform _player;
    private Transform thisTransform;

    private IEnemy[] enemiesInterfaces;


    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        thisTransform = this.transform;

        enemiesInterfaces = thisTransform.GetComponentsInChildren<IEnemy>();
    }

    private bool isPlayerInRange = false;
    private void Update()
    {
        if (Vector3.Distance(thisTransform.position, _player.position) < playerDetectionRange) {
            if (!isPlayerInRange) {
                isPlayerInRange = true;
                SetAllEnemiesActive(true);
            }
        }
        else if (isPlayerInRange){ 
            isPlayerInRange = false;
            SetAllEnemiesActive(false);
        }
        
        if (isPlayerInRange) {
            SetAllEnemiesTarget();
        }
    }


    private void SetAllEnemiesActive(bool active) {
        foreach (var ienemy in enemiesInterfaces) {
            ienemy.SetActive(active);
        }
    }

    private void SetAllEnemiesTarget() {
        foreach (var ienemy in enemiesInterfaces) {
            ienemy.SetTarget(_player.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, playerDetectionRange);
    }
}
