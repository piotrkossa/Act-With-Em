using UnityEngine;
using System.Collections.Generic;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public class AdjustVisibility : MonoBehaviour
{
    public Camera mainCamera;                   // G³ówna kamera
    public Transform player;                   // Gracz
    public LayerMask environmentLayer;           // Warstwa z obiektami (np. drzewa)
    public float reducedVisibility = 0.5f;      // Zmniejszenie przezroczystoœci do 50%
    public float normalVisibility = 1f;         // Normalna przezroczystoœæ
    public float lerpSpeed = 5f;                // Szybkoœæ przejœcia przezroczystoœci

    private Dictionary<Renderer, float> originalAlphaValues = new Dictionary<Renderer, float>();

    private void Update()
    {
        AdjustObjectsVisibility();
    }

    void AdjustObjectsVisibility()
    {
        // Przywróæ oryginaln¹ przezroczystoœæ dla wszystkich obiektów
        foreach (var pair in originalAlphaValues)
        {
            Renderer renderer = pair.Key;
            if (renderer != null && renderer.material.HasProperty("Opacity")) // Zmieniono na "Opacity"
            {
                float originalAlpha;
                if (originalAlphaValues.TryGetValue(renderer, out originalAlpha))
                {
                    float currentAlpha = renderer.material.GetFloat("Opacity"); // Zmieniono na "Opacity"
                    renderer.material.SetFloat("Opacity", Mathf.Lerp(currentAlpha, originalAlpha, Time.deltaTime * lerpSpeed)); // Zmieniono na "Opacity"
                }
            }
        }
        originalAlphaValues.Clear(); // Wyczyœæ s³ownik przed ponownym sprawdzeniem

        // Oblicz wektor od kamery do gracza
        Vector3 direction = player.position - mainCamera.transform.position;
        float distanceToPlayer = direction.magnitude;

        // Rzucenie promienia (Raycast) od kamery do gracza
        RaycastHit [] hits = Physics.RaycastAll(mainCamera.transform.position, direction.normalized, distanceToPlayer, environmentLayer);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null)
            {
                Renderer objRenderer = hit.collider.GetComponent<Renderer>();
                if (objRenderer != null && objRenderer.material.HasProperty("Opacity")) // Zmieniono na "Opacity"
                {
                    Material material = objRenderer.material;

                    // SprawdŸ, czy gracz jest za obiektem
                    if (IsPlayerBehindObject(hit.point, mainCamera.transform.position, player.position))
                    {
                        // Zapamiêtaj oryginaln¹ wartoœæ alpha, jeœli jeszcze nie zosta³a zapamiêtana
                        if (!originalAlphaValues.ContainsKey(objRenderer))
                        {
                            originalAlphaValues.Add(objRenderer, material.GetFloat("Opacity")); // Zmieniono na "Opacity"
                        }

                        // P³ynnie zmieñ przezroczystoœæ
                        float currentAlpha = material.GetFloat("Opacity"); // Zmieniono na "Opacity"
                        float targetAlpha = reducedVisibility;
                        material.SetFloat("Opacity", Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * lerpSpeed)); // Zmieniono na "Opacity"
                    }
                    // Jeœli gracz nie jest ju¿ za obiektem, przywrócenie nast¹pi na pocz¹tku nastêpnej klatki
                }
            }
        }
    }
    // Sprawdza, czy punkt (trafienia raycasta) znajduje siê miêdzy punktem pocz¹tkowym (kamera) a punktem koñcowym (gracz)
    bool IsPlayerBehindObject(Vector3 hitPoint, Vector3 cameraPos, Vector3 playerPos)
    {
        // Wektory od kamery do punktu trafienia i od punktu trafienia do gracza
        Vector3 cameraToHit = hitPoint - cameraPos;
        Vector3 hitToPlayer = playerPos - hitPoint;
        Vector3 cameraToPlayer = playerPos - cameraPos;

        // SprawdŸ, czy wektory cameraToHit i hitToPlayer s¹ skierowane w podobnym kierunku
        // Mo¿na to sprawdziæ, normalizuj¹c wektory i obliczaj¹c iloczyn skalarny.
        // Jeœli iloczyn skalarny jest bliski 1, oznacza to, ¿e wektory s¹ prawie równoleg³e i skierowane w tym samym kierunku.

        float dotProduct = Vector3.Dot(cameraToHit.normalized, cameraToPlayer.normalized);
        float dotProduct2 = Vector3.Dot(hitToPlayer.normalized, cameraToPlayer.normalized);

        // Obiekt jest miêdzy kamer¹ a graczem, jeœli punkt trafienia le¿y na linii miêdzy nimi
        // Mo¿na to sprawdziæ, porównuj¹c sumê d³ugoœci wektorów cameraToHit i hitToPlayer z d³ugoœci¹ wektora cameraToPlayer.
        float distanceCameraToHit = cameraToHit.magnitude;
        float distanceHitToPlayer = hitToPlayer.magnitude;
        float distanceCameraToPlayer = cameraToPlayer.magnitude;

        // Dodatkowe warunki dla pewnoœci, ¿e obiekt jest "za" graczem z perspektywy kamery
        return dotProduct > 0.1f && dotProduct2 > 0.1f && Mathf.Abs(distanceCameraToHit + distanceHitToPlayer - distanceCameraToPlayer) < 0.1f;
    }
}