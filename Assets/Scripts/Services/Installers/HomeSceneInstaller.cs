using UnityEngine.SceneManagement;
using Zenject;

public class HomeSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {

        if (!SceneManager.GetSceneByName("PersistentUI").isLoaded)
        {
            SceneManager.LoadScene("PersistentUI", LoadSceneMode.Additive);
        }
    }
}
