using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class BankSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // ��������� PersistentUI, ���� �� ��������
        if (!SceneManager.GetSceneByName("PersistentUI").isLoaded)
        {
            SceneManager.LoadScene("PersistentUI", LoadSceneMode.Additive);
        }

        // ���� PlayerModel, BankManager, BankTransferService � PlayerDataManager ��� �� ���������������� ���������
        Container.Bind<PlayerModel>().AsSingle().NonLazy();
        //Container.Bind<BankManager>().AsSingle().NonLazy(); // � ������������ ������� 3 ����� � 1 ������
        //Container.Bind<BankTransferService>().AsSingle().NonLazy();
        

        // ���� ����� PlayerController
        // Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
    }
}
