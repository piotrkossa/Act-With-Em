using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

[SelectionBase]
public class StumpGameManager : MonoBehaviour, IInteractable
{
    public static StumpGameManager Instance { get; private set; }
    public static event System.Action OnStumpGameCompleted;

    [SerializeField] private CinemachineCamera cinemachineCamera;
    private HashSet<String> completed = new HashSet<string>();
    private Stump[] stumps;
    private int clickedStump = -1;
    private bool havePlayed = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Transform stumpsParent = this.transform.Find("Stumps");
        if (stumpsParent != null)
        {
            stumps = stumpsParent.GetComponentsInChildren<Stump>();
        }
        else
        {
            stumps = new Stump[0];
        }
    }

    public void Interact()
    {
        if (havePlayed) return;
        GameSetup();
        StartCoroutine(PerformGame());
    }

    private IEnumerator PerformGame()
    {
        if (stumps == null || stumps.Length == 0)
        {
            PlayerSetup();
            yield break;
        }
        havePlayed = true;
        foreach (var stump in stumps)
        {
            if (stump != null && stump.IsRune) stump.CanBeInteracted = true;
        }
        while (true)
        {
            clickedStump = -1;
            yield return new WaitUntil(() => clickedStump != -1 &&
                                           stumps != null &&
                                           clickedStump >= 0 && clickedStump < stumps.Length &&
                                           stumps[clickedStump] != null &&
                                           stumps[clickedStump].IsRune);

            if (clickedStump == -1 || stumps == null || clickedStump < 0 || clickedStump >= stumps.Length || stumps[clickedStump] == null)
            {
                continue;
            }
            yield return StartCoroutine(StartPath(stumps[clickedStump].ColorMaterial));
            if (completed.Count == 4)
            {
                OnStumpGameCompleted?.Invoke();
                break;
            }
        }
        PlayerSetup();
        foreach (var stump in stumps)
        {
            if (stump != null) stump.CanBeInteracted = false;
        }
    }

    private IEnumerator StartPath(Material pathMaterial)
    {
        if (pathMaterial == null || stumps == null) yield break;
        ClearPath(pathMaterial);

        int startNodeIndex = clickedStump;
        int currentPathNodeIndex = startNodeIndex;

        if (stumps[startNodeIndex].IsRune) { }
        else { stumps[startNodeIndex].ColorMaterial = pathMaterial; }

        while (true)
        {
            List<int> neighbors = GetNeighbors(currentPathNodeIndex, pathMaterial);
            foreach (int n_idx in neighbors)
            {
                if (n_idx >= 0 && n_idx < stumps.Length && stumps[n_idx] != null)
                    stumps[n_idx].CanBeInteracted = true;
            }
            clickedStump = -1;
            yield return new WaitUntil(() => clickedStump != -1);

            if (stumps == null || clickedStump < 0 || clickedStump >= stumps.Length || stumps[clickedStump] == null)
            {
                ClearPath(pathMaterial);
                DisableAllInteractablesExceptRunes();
                yield break;
            }
            bool clickedIsRune = stumps[clickedStump].IsRune;
            bool clickedIsValidNeighbor = neighbors.Contains(clickedStump);
            DisableAllInteractablesExceptRunes();

            if (clickedIsRune)
            {
                if (stumps[clickedStump].ColorMaterial.name == pathMaterial.name && clickedStump != startNodeIndex && clickedIsValidNeighbor)
                {
                    completed.Add(pathMaterial.name);
                }
                else
                {
                    ClearPath(pathMaterial);
                }
                clickedStump = -1;
                yield break;
            }
            else if (clickedIsValidNeighbor)
            {
                stumps[clickedStump].ColorMaterial = pathMaterial;
                currentPathNodeIndex = clickedStump;
            }
            else
            {
                ClearPath(pathMaterial);
                yield break;
            }
        }
    }

    public void StumpClicked(int index)
    {
        clickedStump = index;
    }

    private void DisableAllInteractablesExceptRunes()
    {
        if (stumps == null) return;
        foreach (var stump in stumps)
        {
            if (stump != null && !stump.IsRune)
            {
                stump.CanBeInteracted = false;
            }
        }
    }

    private void ClearPath(Material material)
    {
        if (stumps == null || material == null) return;
        foreach (var stump in stumps)
        {
            if (stump != null && stump.ColorMaterial != null && stump.ColorMaterial.name == material.name && !stump.IsRune)
            {
                stump.ColorMaterial = null;
            }
        }
    }

    private List<int> GetNeighbors(int index, Material pathMaterial)
    {
        List<int> neighbors = new List<int>();
        if (stumps == null || index < 0 || index >= stumps.Length) return neighbors;
        int row = index / 4;
        int col = index % 4;
        int[] dr = { -1, 1, 0, 0 };
        int[] dc = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int nr = row + dr[i];
            int nc = col + dc[i];
            int neighborIndex = nr * 4 + nc;
            if (nr >= 0 && nr < 4 && nc >= 0 && nc < 4)
            {
                if (stumps[neighborIndex] != null &&
                    (stumps[neighborIndex].ColorMaterial == null ||
                    (stumps[neighborIndex].IsRune && stumps[neighborIndex].ColorMaterial.name == pathMaterial.name)))
                {
                    neighbors.Add(neighborIndex);
                }
            }
        }
        return neighbors;
    }

    private void GameSetup()
    {
        if (cinemachineCamera != null)
        {
            cinemachineCamera.Follow = this.transform;
            var rotationComposer = cinemachineCamera.GetComponent<CinemachineRotationComposer>();
            if (rotationComposer != null) rotationComposer.TargetOffset.y = -1;
        }
        if (GameManager.Instance != null) GameManager.SetState(GameStates.MiniGame);
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) playerObject.transform.localScale = Vector3.zero;
    }

    private void PlayerSetup()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerObject.transform.localScale = Vector3.one;
            if (cinemachineCamera != null) cinemachineCamera.Follow = playerObject.transform;
        }
        if (cinemachineCamera != null)
        {
            var rotationComposer = cinemachineCamera.GetComponent<CinemachineRotationComposer>();
            if (rotationComposer != null) rotationComposer.TargetOffset.y = 0;
        }
        if (GameManager.Instance != null) GameManager.SetState(GameStates.Player);
    }
}