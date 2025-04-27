using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class ColliderGizmo : MonoBehaviour
{
    void OnDrawGizmos()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        if (box != null)
        {
            Gizmos.color = Color.cyan;

            // Matrix transform - uwzględnia pozycję, rotację i skalę
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position + box.center, transform.rotation, transform.lossyScale);
            Gizmos.matrix = rotationMatrix;

            // Rysowanie gizma
            Gizmos.DrawWireCube(Vector3.zero, box.size);
        }
        else
        {
            Debug.LogWarning("Brak BoxCollider na obiekcie: " + gameObject.name);
        }
    }
}

public class CheckPoint : MonoBehaviour
{
    public UnityEvent onPlayerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            onPlayerEnter?.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}