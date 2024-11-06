using System.ComponentModel;
using Zenject;

public class AuthInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<AuthUIController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<AuthService>().AsSingle();
    }
}

