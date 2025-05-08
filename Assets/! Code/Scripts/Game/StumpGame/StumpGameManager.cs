using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

[SelectionBase]
public class StumpGameManager : MonoBehaviour, IInteractable
{
    public static StumpGameManager Instance {get; private set;}

    [SerializeField] private CinemachineCamera cinemachineCamera;

    private HashSet<String> completed = new();

    private Stump[] stumps;

    private int clickedStump = -1;

    private void Start() {
        Instance = this;
        stumps = this.transform.Find("Stumps").GetComponentsInChildren<Stump>();
    }

    private bool havePlayed = false;
    public void Interact()
    {
        if (havePlayed) return;

        GameSetup();
        StartCoroutine(PerformGame());

        havePlayed = true;
    }


    private IEnumerator PerformGame() {
        foreach (var stump in stumps) {
            if (stump.IsRune) stump.CanBeInteracted = true;
        }

        while (true) {
            yield return new WaitUntil(() => clickedStump != -1 && stumps[clickedStump].IsRune);

            yield return StartCoroutine(StartPath(stumps[clickedStump].ColorMaterial));

            if (completed.Count == 4) {
                break;
            }
        }

        PlayerSetup();
        foreach (var stump in stumps) {
            stump.CanBeInteracted = false;
        }
    }

    private IEnumerator StartPath(Material pathMaterial) {
        ClearPath(pathMaterial);
        if (completed.Contains(pathMaterial.name)) {
            completed.Remove(pathMaterial.name);
        }

        var start = clickedStump;
        
        while (true) {
            var neighbors = GetNeighbors(clickedStump, pathMaterial);
            foreach (int n in neighbors) {
                stumps[n].CanBeInteracted = true;
            }
            clickedStump = -1;

            yield return new WaitUntil(() => neighbors.Contains(clickedStump) || (clickedStump != -1 && stumps[clickedStump].IsRune));

            DisableAllInteractables();

            if (stumps[clickedStump].IsRune) {
                if (stumps[clickedStump].ColorMaterial.name == pathMaterial.name && clickedStump != start && neighbors.Contains(clickedStump)) {
                    completed.Add(pathMaterial.name);
                    clickedStump = -1;
                }
                else {
                    ClearPath(pathMaterial);
                    if (completed.Contains(pathMaterial.name)) {
                        completed.Remove(pathMaterial.name);
                    }
                }
                yield break;
            }
            else {
                stumps[clickedStump].ColorMaterial = pathMaterial;
            }
        }
    }

    public void StumpClicked(int index) {
        clickedStump = index;
    }

    private void DisableAllInteractables() {
        foreach (var stump in stumps) {
            if (!stump.IsRune) {
                stump.CanBeInteracted = false;
            }
        }
    }

    private void ClearPath(Material material) {
        foreach (var stump in stumps) {
            if (stump.ColorMaterial == null) continue;
            if (stump.ColorMaterial.name == material.name && !stump.IsRune) {
                stump.ColorMaterial = null;
            }
        }
    }

    private int[] GetNeighbors(int index, Material pathMaterial) {
        List<int> neighbors = new();
        if (index % 4 != 0) {
            neighbors.Add(index-1);
        }
        if (index % 4 != 3) {
            neighbors.Add(index+1);
        }
        if (index - 4 >= 0) {
            neighbors.Add(index-4);
        }
        if (index + 4 <= 15) {
            neighbors.Add(index+4);
        }

        var corrected = new List<int>();

        foreach (var n in neighbors) {
            if (stumps[n].ColorMaterial == null || (stumps[n].IsRune && stumps[n].ColorMaterial.name == pathMaterial.name)) {
                corrected.Add(n);
            }
        }

        return corrected.ToArray();
    }


    private void GameSetup() {
        cinemachineCamera.Follow = this.transform;
        cinemachineCamera.GetComponent<CinemachineRotationComposer>().TargetOffset.y = -1;
        GameManager.SetState(GameStates.MiniGame);
        GameObject.FindGameObjectWithTag("Player").transform.localScale = Vector3.zero;
    }

    private void PlayerSetup() {
        GameObject.FindGameObjectWithTag("Player").transform.localScale = Vector3.one;
        cinemachineCamera.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        cinemachineCamera.GetComponent<CinemachineRotationComposer>().TargetOffset.y = 0;
        GameManager.SetState(GameStates.Player);
    }
}
