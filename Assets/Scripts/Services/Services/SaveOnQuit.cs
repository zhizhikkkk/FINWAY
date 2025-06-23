using UnityEngine;
using Zenject;

public class SaveOnQuit : MonoBehaviour
{
    private PlayerDataManager _dataManager;
    private GameManager _gameManager;

    [Inject]
    public void Construct(PlayerDataManager dataManager, GameManager gameManager)
    {
        _dataManager = dataManager;
        _gameManager = gameManager;
    }

    private void OnApplicationQuit()
    {
        Debug.Log("[SaveOnQuit] OnApplicationQuit: Saving player data...");
        _dataManager.Save(_gameManager.PlayerModel);
        //_dataManager.ResetData();
    }
}
