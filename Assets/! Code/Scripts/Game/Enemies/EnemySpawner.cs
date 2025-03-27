using System.Collections.Generic;
using UnityEngine;

public interface IEnemy {
    void SetTarget(Vector3 target);
    void SetActive(bool active);
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<GameObject> enemiesList; // Hold Reference to Enemies Prefabs
    private IEnemy[] enemiesInterfaces; // Hold Reference to Enemy Interfaces to Interact During Runtime
    [SerializeField] private float playerDetectionRange; // Range from Spawner Position That Detects Player

    private Transform _player; //  Hold Reference to Player Transform
    private Transform thisTransform; // Hold Reference to Spawner Object Transform


    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        thisTransform = this.transform;

        enemiesInterfaces = new IEnemy [enemiesList.Count];
        for (int i = 0; i < enemiesList.Count; i++) {
            enemiesInterfaces[i] = Instantiate(enemiesList[i], thisTransform).GetComponent<IEnemy>();
            enemiesInterfaces[i].SetActive(false);
        }
    }


    private void Update()
    {
        if (Vector3.Distance(thisTransform.position, _player.position) < playerDetectionRange) {
            SetAllEnemiesActive(true);
        }
        else { 
            SetAllEnemiesActive(false);
        }
        if (isActive) {
            SetAllEnemiesTarget();
        }
    }



    private bool isActive = false;
    private void SetAllEnemiesActive(bool active) {
        if (isActive == active) return;
        isActive = active;
        foreach (var ienemy in enemiesInterfaces) {
            ienemy.SetActive(active);
        }
    }

    private void SetAllEnemiesTarget() {
        foreach (var ienemy in enemiesInterfaces) {
            ienemy.SetTarget(_player.transform.position);
        }
    }
}
