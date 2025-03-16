using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCutoutSphere : MonoBehaviour
{
    [SerializeField] private Transform targetObject;   // Gracz
    [SerializeField] private LayerMask wallMask;       // Warstwa œrodowiska (drzewa, ska³y, itd.)
    [SerializeField] private float cutoutSize = 0.2f;  // Rozmiar wyciêcia
    [SerializeField] private float falloffSize = 0.1f; // Rozmycie krawêdzi wyciêcia
    [SerializeField] private float screenOffsetY = 50f;// Przesuniêcie "kó³ka" w pionie (w pikselach)

    [Header("SphereCast ustawienia")]
    [SerializeField] private float sphereRadius = 1.0f; // Promieñ "tunelu" od kamery do gracza

    private Camera mainCamera;

    // S³ownik: Renderer -> oryginalny materia³
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();

    // Obiekty trafione w tej klatce
    private List<Renderer> currentFrameHits = new List<Renderer>();
    // Obiekty trafione w poprzedniej klatce
    private List<Renderer> previousFrameHits = new List<Renderer>();

    private void Awake()
    {
        // Jeœli nie przypiszesz w Inspectorze, pobieramy g³ówn¹ kamerê
        mainCamera = GetComponent<Camera>();
        if (!mainCamera) mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!targetObject)
        {
            Debug.LogError("Brak przypisanego obiektu gracza (targetObject)!");
            return;
        }

        currentFrameHits.Clear();

        // Kierunek i dystans od kamery do gracza
        Vector3 cameraPos = mainCamera.transform.position;
        Vector3 direction = targetObject.position - cameraPos;
        float distance = direction.magnitude;

        // SphereCastAll – szukamy wszystkich obiektów w "tunelu" o promieniu sphereRadius
        RaycastHit[] hits = Physics.SphereCastAll(
            cameraPos,       // punkt startu
            sphereRadius,    // promieñ
            direction,       // kierunek
            distance,        // maks. dystans
            wallMask         // warstwa obiektów
        );

        foreach (RaycastHit hit in hits)
        {
            Renderer rend = hit.transform.GetComponent<Renderer>();
            if (rend)
            {
                // Dodajemy do bie¿¹cej listy
                currentFrameHits.Add(rend);

                // Jeœli to pierwszy raz widzimy ten Renderer, zapisz oryginalny materia³
                if (!originalMaterials.ContainsKey(rend))
                {
                    Material originalMat = rend.material;
                    Material newMat = new Material(originalMat);
                    rend.material = newMat;

                    originalMaterials.Add(rend, originalMat);
                }

                // Ustawiamy (lub aktualizujemy) cutout w tym obiekcie
                UpdateCutout(rend);
            }
        }

        // Obiekty, które zniknê³y z "tunelu" – przywracamy oryginalny materia³
        foreach (Renderer rend in previousFrameHits)
        {
            if (!currentFrameHits.Contains(rend))
            {
                RevertCutout(rend);
            }
        }

        previousFrameHits.Clear();
        previousFrameHits.AddRange(currentFrameHits);
    }

    private void UpdateCutout(Renderer rend)
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetObject.position);
        screenPos.y += screenOffsetY;
        Vector2 cutoutPos = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);

        Material mat = rend.material;
        mat.SetVector("_CutoutPos", cutoutPos);
        mat.SetFloat("_CutoutSize", cutoutSize);
        mat.SetFloat("_FalloffSize", falloffSize);
    }

    private void RevertCutout(Renderer rend)
    {
        if (rend && originalMaterials.ContainsKey(rend))
        {
            rend.material = originalMaterials[rend];
            originalMaterials.Remove(rend);
        }
    }
}