using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentUIController : MonoBehaviour
{
    public void ChooseMap()
    {
        SceneManager.LoadScene("Map");
    }
    
}
