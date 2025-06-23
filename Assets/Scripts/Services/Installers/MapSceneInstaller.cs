using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class MapSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Говорим Zenject-у: ищи MapUIController в сцене, сделай AsSingle
        Container.Bind<MapUIController>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.Bind<PlayerLocationManager>().AsSingle().NonLazy();
        Container.Bind<PlayerModel>().AsSingle().NonLazy();
        Container.Bind<PlayerDataManager>().AsSingle().NonLazy();
        Container.Bind<ExpenseManager>().AsSingle().NonLazy();
        Container.Bind<LocationAvailabilityService>().AsSingle().NonLazy();
    }
}
