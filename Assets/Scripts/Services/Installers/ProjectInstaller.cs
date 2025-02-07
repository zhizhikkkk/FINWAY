using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
        Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<InputService>().AsSingle().NonLazy();
        Container.Bind<PlayerModel>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PlayerLocationManager>().AsSingle().NonLazy();
        Container.Bind<MapUIController>().FromComponentInHierarchy().AsSingle();
        
    }
}
