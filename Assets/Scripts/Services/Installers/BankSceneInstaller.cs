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

        //Container.Bind<BankManager>().AsSingle().NonLazy(); // � ������������ ������� 3 ����� � 1 ������
        //Container.Bind<BankTransferService>().AsSingle().NonLazy();
        

        // ���� ����� PlayerController
        // Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
    }
}
