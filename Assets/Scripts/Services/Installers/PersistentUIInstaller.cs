using System.ComponentModel;
using Zenject;

public class PersistentUIInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<HUDView>().FromComponentInHierarchy().AsSingle();
    }
}