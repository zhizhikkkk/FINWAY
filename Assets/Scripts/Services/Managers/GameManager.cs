using Zenject;
using UniRx;
using UnityEngine;

public class GameManager : IInitializable
{
    public static GameManager Instance { get; private set; }

    public PlayerModel PlayerModel { get; private set; }

    [Inject]
    public void Construct()
    {
        Instance = this;
        PlayerModel = new PlayerModel();
    }

    public void Initialize()
    {
    }
}
