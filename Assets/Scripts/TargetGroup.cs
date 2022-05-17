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
        var target = enemiesInGroup[enemiesInGroup.Count - 1];
        enemiesInGroup.RemoveAt(enemiesInGroup.Count - 1);
        return target;
    }
}
