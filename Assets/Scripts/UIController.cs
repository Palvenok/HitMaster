using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject failPanel;
    [Space]
    [SerializeField] private Text counter;
    [SerializeField] private Text levelIndex;
    [SerializeField] private Slider levelProgressBar;

    public void UpdateCounter(string value)
    {
        counter.text = value;
    }

    public void UpdateLevelIndex(string value)
    {
        levelIndex.text = "Level: " + value;
    }

    public void UpdateLevelProgressBar(int enemiesAlive, int enemiesOnLevel)
    {
        levelProgressBar.value = Utils.Map(enemiesAlive, 0, enemiesOnLevel, 0, 1);
    }

    public void ShowPanel(UiPanel panel)
    {
        gamePanel.SetActive(false);
        winPanel.SetActive(false);
        failPanel.SetActive(false);

        switch (panel)
        {
            case UiPanel.GamePanel:
                gamePanel.SetActive(true);
                break;
            case UiPanel.WinPanel:
                winPanel.SetActive(true);
                break;
            case UiPanel.FailPanel:
                failPanel.SetActive(true);
                break;
            case UiPanel.Unknown:
                Debug.LogError("Unknown UI panel");
                break;
        }
    }
}

public enum UiPanel
{
    Unknown,
    GamePanel,
    WinPanel,
    FailPanel
}
