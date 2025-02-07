using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadPersistentUI : MonoBehaviour
{
    private static bool _isLoaded = false;

    void Awake()
    {
        if (_isLoaded)
        {
            Destroy(gameObject);
            return;
        }

        _isLoaded = true;
        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene("PersistentUI", LoadSceneMode.Additive);
    }
}
