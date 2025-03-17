using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentBehindObject : MonoBehaviour
{
    [SerializeField] private Transform targetObject;   // Gracz
    [SerializeField] private LayerMask wallMask;       // Warstwa œrodowiska (np. drzewa)
    [SerializeField] private float sphereRadius = 1.0f; // Promieñ "tunelu" od kamery do gracza

    private Camera mainCamera;
    private Material material;

    private void Awake()
    {
        // Pobieramy g³ówn¹ kamerê, jeœli nie przypisano
        mainCamera = Camera.main;
        if (!mainCamera)
        {
            Debug.LogError("Brak kamery!");
        }

        // Zak³adamy, ¿e materia³ jest na obiekcie, który ten skrypt kontroluje
        Renderer rend = GetComponent<Renderer>();
        if (rend)
        {
            material = rend.material;
        }
    }

    private void Update()
    {
        if (!targetObject || !material)
        {
            return;
        }

        // Kierunek i dystans od kamery do gracza
        Vector3 cameraPos = mainCamera.transform.position;
        Vector3 direction = targetObject.position - cameraPos;
        float distance = direction.magnitude;

        // SphereCast – sprawdzamy, czy gracz jest za obiektem
        RaycastHit hit;
        if (Physics.SphereCast(cameraPos, sphereRadius, direction, out hit, distance, wallMask))
        {
            // Gracz jest za obiektem, ustawiamy przezroczystoœæ
            SetOpacity(0.3f);  // Ustawiamy przezroczystoœæ na 30%
        }
        else
        {
            // Gracz nie jest za obiektem, przywracamy pe³n¹ przezroczystoœæ
            SetOpacity(1.0f);  // Pe³na przezroczystoœæ
        }
    }

    private void SetOpacity(float opacity)
    {
        if (material)
        {
            material.SetFloat("_Opacity", opacity);  // Zmieniamy opacnoœæ w materiale
        }
    }
}
