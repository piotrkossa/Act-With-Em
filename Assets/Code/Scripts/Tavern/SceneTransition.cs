using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public GameObject[] objectPrefabs;     // Prefabrykaty obiektów, które chcesz przenieœæ

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Zapisz dane obiektów do PlayerPrefs
            for (int i = 0; i < objectPrefabs.Length; i++)
            {
                GameObject prefab = objectPrefabs[i];
                if (prefab != null)
                {
                    PlayerPrefs.SetFloat(prefab.name + "X", prefab.transform.position.x);
                    PlayerPrefs.SetFloat(prefab.name + "Y", prefab.transform.position.y);
                    PlayerPrefs.SetFloat(prefab.name + "Z", prefab.transform.position.z);
                }
            }

            // Za³aduj now¹ scenê
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("TavernScene");
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // W nowej scenie stwórz obiekty
        foreach (var prefab in objectPrefabs)
        {
            if (prefab != null)
            {
                Vector3 newPosition = new Vector3(
                    PlayerPrefs.GetFloat(prefab.name + "X"),
                    PlayerPrefs.GetFloat(prefab.name + "Y"),
                    PlayerPrefs.GetFloat(prefab.name + "Z")
                );

                // Tworzenie obiektów w nowej scenie
                Instantiate(prefab, newPosition, Quaternion.identity);
            }
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
