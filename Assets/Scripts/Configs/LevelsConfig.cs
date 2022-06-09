using UnityEngine;

[CreateAssetMenu(fileName = "LevelsConfig", menuName = "Configs/LevelsConfig")]
public class LevelsConfig : ScriptableObject
{
    [SerializeField] private Level[] _levels;

    public int Count => _levels.Length;

    public Level GetLevel(int index)
    {
        if (index < 0) index = 0;
        if (index >= _levels.Length) index = _levels.Length - 1;
        return _levels[index];
    }
}
