using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class BankSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        if (!SceneManager.GetSceneByName("PersistentUI").isLoaded)
        {
            SceneManager.LoadScene("PersistentUI", LoadSceneMode.Additive);
        }

      
    }
}
