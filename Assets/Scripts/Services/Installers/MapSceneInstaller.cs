using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class MapSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // ������� Zenject-�: ��� MapUIController � �����, ������ AsSingle
        Container.Bind<MapUIController>().FromComponentInHierarchy().AsSingle();
    }
}
