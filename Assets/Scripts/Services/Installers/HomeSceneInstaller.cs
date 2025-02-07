using UnityEngine.SceneManagement;
using Zenject;

public class HomeSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {

        Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
        if (!SceneManager.GetSceneByName("PersistentUI").isLoaded)
        {
            SceneManager.LoadScene("PersistentUI", LoadSceneMode.Additive);
        }
    }
}
