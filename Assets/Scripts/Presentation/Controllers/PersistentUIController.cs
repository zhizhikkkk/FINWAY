using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentUIController : MonoBehaviour
{
    [SerializeField]private PortfolioPanel portfolioPanel;
    public void ChooseMap()
    {
        SceneManager.LoadScene("Map");
    }
    public void OnOpenPortfolioClicked()
    {
        portfolioPanel.Show();
    }
}
