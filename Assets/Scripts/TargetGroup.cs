using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class TargetGroup : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemiesInGroup;

    public int TargetsCount => enemiesInGroup.Count;
    public UnityEvent OnTargetGroupClear;

    public Enemy GetTarget()
    {
        if (enemiesInGroup.Count == 0)
        {
            OnTargetGroupClear?.Invoke();
            return null;
        }
        var target = enemiesInGroup[Random.Range(0, enemiesInGroup.Count)];
        return target;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (enemiesInGroup.Contains(enemy))
            enemiesInGroup.Remove(enemy);

        if (enemiesInGroup.Count == 0)
        {
            OnTargetGroupClear?.Invoke();
        }
    }

    private void OnDestroy()
    {
        OnTargetGroupClear.RemoveAllListeners();
    }
}
