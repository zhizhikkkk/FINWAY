using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        /* --------------------------------------------------- */
        /*  Singletons / сервисы                               */
        /* --------------------------------------------------- */

        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();

        Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<InputService>().AsSingle().NonLazy();
        Container.Bind<PlayerModel>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PlayerLocationManager>().AsSingle().NonLazy();
        Container.Bind<MapUIController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerDataManager>().AsSingle().NonLazy();
        Container.Bind<BankManager>().AsSingle().NonLazy();
        Container.Bind<BankTransferService>().AsSingle().NonLazy();
        Container.Bind<ExpenseManager>().AsSingle().NonLazy();
        Container.Bind<ResetButtonHandler>().FromComponentInHierarchy().AsSingle();

        /* --------------------------------------------------- */
        /*  Signals                                            */
        /* --------------------------------------------------- */

        SignalBusInstaller.Install(Container);                 
        Container.DeclareSignal<AgentInteractionSignal>();    
    }
}