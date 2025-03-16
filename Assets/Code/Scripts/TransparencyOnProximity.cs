using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField] private Transform targetObject;  // Obiekt gracza
    [SerializeField] private LayerMask wallMask;        // Warstwa œrodowiska (np. drzewa)
    [SerializeField] private float cutoutSize = 0.2f;     // Rozmiar wyciêcia
    [SerializeField] private float falloffSize = 0.1f;    // Rozmiar zanikania
    [SerializeField] private float screenOffsetY = 50f;   // Przesuniêcie pozycji kó³ka w pionie (w pikselach)

    private Camera mainCamera;
    private bool isBehind = false;        // Flaga – czy gracz jest za obiektem
    private Material originalMaterial;    // Oryginalny materia³ obiektu
    private Renderer cutoutRenderer;      // Renderer obiektu, na którym efekt jest aktywowany

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (targetObject == null)
        {
            Debug.LogError("Nie przypisano obiektu targetObject!");
            return;
        }

        // Obliczamy kierunek i odleg³oœæ od kamery do gracza
        Vector3 direction = targetObject.position - transform.position;
        float distance = direction.magnitude;
        RaycastHit hit;

        // Wykonujemy raycast w kierunku gracza, sprawdzaj¹c obiekty z warstwy wallMask
        if (Physics.Raycast(transform.position, direction, out hit, distance, wallMask))
        {
            // Trafienie – gracz jest za obiektem
            if (!isBehind)
            {
                isBehind = true;
                // Pobieramy Renderer trafionego obiektu (np. drzewa)
                cutoutRenderer = hit.transform.GetComponent<Renderer>();
                if (cutoutRenderer != null)
                {
                    // Zapisujemy oryginalny materia³
                    originalMaterial = cutoutRenderer.material;
                    // Tworzymy kopiê materia³u, aby modyfikacje nie wp³ywa³y globalnie
                    cutoutRenderer.material = new Material(originalMaterial);
                    // Aktywujemy efekt cutout
                    ActivateCutout(cutoutRenderer);
                }
                else
                {
                    Debug.LogWarning("Brak komponentu Renderer na trafionym obiekcie.");
                }
            }
            else
            {
                // Jeœli efekt ju¿ jest aktywny, aktualizujemy pozycjê cutout
                if (cutoutRenderer != null)
                {
                    UpdateCutoutPosition(cutoutRenderer);
                }
            }
        }
        else
        {
            // Brak trafienia – gracz jest przed obiektem
            if (isBehind)
            {
                isBehind = false;
                if (cutoutRenderer != null)
                {
                    // Wy³¹czamy efekt cutout i przywracamy oryginalny wygl¹d
                    DeactivateCutout(cutoutRenderer);
                    cutoutRenderer = null;
                }
            }
        }
    }

    // Metoda aktywuj¹ca efekt cutout – ustawia parametry shader’a
    private void ActivateCutout(Renderer renderer)
    {
        Material mat = renderer.material;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetObject.position);
        screenPos.y += screenOffsetY; // Dodajemy przesuniêcie w pionie (w pikselach)
        Vector2 cutoutPos = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);
        mat.SetVector("_CutoutPos", cutoutPos);
        mat.SetFloat("_CutoutSize", cutoutSize);
        mat.SetFloat("_FalloffSize", falloffSize);
    }

    // Metoda aktualizuj¹ca pozycjê cutout, gdy efekt jest aktywny
    private void UpdateCutoutPosition(Renderer renderer)
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetObject.position);
        screenPos.y += screenOffsetY;
        Vector2 cutoutPos = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);
        renderer.material.SetVector("_CutoutPos", cutoutPos);
    }

    // Metoda dezaktywuj¹ca efekt – przywraca oryginalny materia³
    private void DeactivateCutout(Renderer renderer)
    {
        renderer.material = originalMaterial;
    }
}