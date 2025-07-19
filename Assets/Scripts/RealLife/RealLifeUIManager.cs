
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class RealLifeUIManager : MonoBehaviour
{
    private PlayerModel _playerModel;

    [Inject]
    public void Construct(PlayerModel playerModel)
    {
        _playerModel = playerModel;
    }

    public void GoToSim()
    {
        _playerModel.AddDay();
        SceneManager.LoadScene("Home");
    }
}
