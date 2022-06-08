using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    [SerializeField] private int ammoCount;
    [SerializeField] private GameObject startTarget;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform startPoint;
    [SerializeField] private TargetGroup[] targetGroups;


    private PlayerController _playerController;
    private int _currentPointNum;

    public UnityEvent OnLevelComplete;

    public PlayerController Initialize()
    {
        _playerController = Instantiate(playerPrefab,
                                       startPoint.position,
                                       startPoint.rotation,
                                       transform).GetComponent<PlayerController>();
        _playerController.Ammo = ammoCount;
        _playerController.OnHit.AddListener(OnPlayerHit);

        foreach (var targetGroup in targetGroups)
            targetGroup.OnTargetGroupClear.AddListener(PlayerNextPoint);

        return _playerController;
    }

    private void OnPlayerHit(string value)
    {
        if (value.Equals("Start"))
        {
            PlayerNextPoint();
            startTarget.GetComponent<Rigidbody>().isKinematic = false;
        }

        if (value.Equals("Finish"))
        {
            OnLevelComplete?.Invoke();
        }
    }

    [ContextMenu("Utils/PlayerNextPoint")]
    public void PlayerNextPoint()
    {
        if (_currentPointNum >= targetGroups.Length)
        {
            return;
        }
        while ( targetGroups[_currentPointNum].TargetsCount == 0)
        {
            _currentPointNum++;
        }
        _playerController.MoveToPoint(targetGroups[_currentPointNum].ShootPoint.position);
        _playerController.SetTargetGroup(targetGroups[_currentPointNum]);
        _currentPointNum++;
    }

    private void OnDestroy()
    {
        OnLevelComplete.RemoveAllListeners();
    }
}
