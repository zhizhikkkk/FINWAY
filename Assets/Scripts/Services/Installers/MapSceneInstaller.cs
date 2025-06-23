using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class MapSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Говорим Zenject-у: ищи MapUIController в сцене, сделай AsSingle
        Container.Bind<MapUIController>().FromComponentInHierarchy().AsSingle().NonLazy();

        if (!SceneManager.GetSceneByName("PersistentUI").isLoaded)
        {
            SceneManager.LoadScene("PersistentUI", LoadSceneMode.Additive);
        }
    }
}
