using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class BankSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Загружаем PersistentUI, если не загружен
        if (!SceneManager.GetSceneByName("PersistentUI").isLoaded)
        {
            SceneManager.LoadScene("PersistentUI", LoadSceneMode.Additive);
        }

        // Если PlayerModel, BankManager, BankTransferService и PlayerDataManager ещё не зарегистрированы глобально
        Container.Bind<PlayerModel>().AsSingle().NonLazy();
        //Container.Bind<BankManager>().AsSingle().NonLazy(); // В конструкторе создаст 3 банка с 1 картой
        //Container.Bind<BankTransferService>().AsSingle().NonLazy();
        

        // Если нужен PlayerController
        // Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
    }
}
