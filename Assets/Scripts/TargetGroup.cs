using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class TargetGroup : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnTargetGroupClear;
    
    [SerializeField] private List<Enemy> enemiesInGroup;
    [SerializeField] private Transform shootPoint;

    public Transform ShootPoint => shootPoint;
    public int TargetsCount => enemiesInGroup.Count;

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

    public void MoveEnemy()
    {
        foreach (Enemy enemy in enemiesInGroup)
        {
            enemy.MoveToTarget(shootPoint.position);
        }
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
