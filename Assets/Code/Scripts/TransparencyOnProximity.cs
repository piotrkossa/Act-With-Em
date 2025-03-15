using UnityEngine;

public class TreeVisibility : MonoBehaviour
{
    public Transform player;
    public Camera playerCamera;
    public Renderer treeRenderer;

    void Update()
    {
        RaycastHit hit;
        // Raycast od kamery w stronê gracza
        Vector3 direction = player.position - playerCamera.transform.position;
        if (Physics.Raycast(playerCamera.transform.position, direction, out hit))
        {
            // Jeœli trafi w drzewo, sprawdŸ, czy to drzewo
            if (hit.transform == transform)
            {
                // Ustaw drzewo jako niewidzialne
                treeRenderer.material.color = new Color(1, 1, 1, 0); // Przezroczysty materia³
            }
            else
            {
                // Przywróæ widocznoœæ drzewa
                treeRenderer.material.color = new Color(1, 1, 1, 1); // Pe³na widocznoœæ
            }
        }
    }
}
