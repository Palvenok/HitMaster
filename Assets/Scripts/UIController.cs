using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
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
}
