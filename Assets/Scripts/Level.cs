using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnLevelStarted;
    [HideInInspector] public UnityEvent OnLevelComplete;
    [HideInInspector] public UnityEvent<int, int> OnTargetsOnLevelUpdate;

    [SerializeField] private int ammoCount;
    [SerializeField] private GameObject startTarget;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform finishPoint;
    [SerializeField] private TargetGroup[] targetGroups;

    private PlayerController _playerController;
    private int _currentPointNum;
    private int _enemiesOnLevel;
    private int _enemiesAlive;
    private int _index;

    public int Index => _index;

    public PlayerController Initialize(int index)
    {
        _index = index;
        _playerController = Instantiate(playerPrefab,
                                       startPoint.position,
                                       startPoint.rotation,
                                       transform).GetComponent<PlayerController>();
        _playerController.Ammo = ammoCount;
        _playerController.OnHit.AddListener(OnPlayerHit);

        foreach (var targetGroup in targetGroups)
        {
            targetGroup.OnTargetGroupClear.AddListener(PlayerNextPoint);
            targetGroup.OnTargetGroupUpdate.AddListener(OnTargetGroupUpdate);
        }
        _enemiesOnLevel = EnemiesCount();
        return _playerController;
    }

    private void OnTargetGroupUpdate()
    {
        _enemiesAlive = EnemiesCount();
        OnTargetsOnLevelUpdate?.Invoke(_enemiesAlive, _enemiesOnLevel);
        if(_enemiesAlive == 0) PlayerMoveToFinish();
    }

    private int EnemiesCount()
    {
        int enemies = 0;
        foreach (var targetGroup in targetGroups)
            enemies += targetGroup.TargetsCount;
        return enemies;
    }

    private void OnPlayerHit(string value)
    {
        if (value.Equals("Start"))
        {
            PlayerNextPoint();
            OnLevelStarted?.Invoke();
            startTarget.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    [ContextMenu("Utils/PlayerMoveToFinish")]
    private void PlayerMoveToFinish()
    {
        _playerController.MoveToPoint(finishPoint.position);
        _playerController.SetTargetGroup(null);

        OnLevelComplete?.Invoke();
    }

    [ContextMenu("Utils/PlayerNextPoint")]
    public void PlayerNextPoint()
    {
        if (_currentPointNum >= targetGroups.Length) return;
        while ( targetGroups[_currentPointNum].TargetsCount == 0) _currentPointNum++;

        _playerController.MoveToPoint(targetGroups[_currentPointNum].ShootPoint.position);
        _playerController.SetTargetGroup(targetGroups[_currentPointNum]);
        _currentPointNum++;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnLevelComplete.RemoveAllListeners();
        OnLevelStarted.RemoveAllListeners();
        OnTargetsOnLevelUpdate.RemoveAllListeners();
    }
}
