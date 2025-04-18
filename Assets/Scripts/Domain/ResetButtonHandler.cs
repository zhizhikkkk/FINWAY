using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class ResetButtonHandler : MonoBehaviour
{
    [Inject] private PlayerDataManager _dataManager;

    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnResetClicked);
    }

    private void OnResetClicked()
    {
        _dataManager.SaveDefaults();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
