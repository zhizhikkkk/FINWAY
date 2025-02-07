using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    private static PersistentManager _instance;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
