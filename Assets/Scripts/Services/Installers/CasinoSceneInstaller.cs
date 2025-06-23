using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class CasinoSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {

        if (!SceneManager.GetSceneByName("PersistentUI").isLoaded)
        {
            SceneManager.LoadScene("PersistentUI", LoadSceneMode.Additive);
        }
    }
}