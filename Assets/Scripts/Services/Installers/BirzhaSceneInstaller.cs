using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class BirzhaSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {

        Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
        if (!SceneManager.GetSceneByName("PersistentUI").isLoaded)
        {
            SceneManager.LoadScene("PersistentUI", LoadSceneMode.Additive);
        }
        Container.Bind<StockMarketManager>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}
