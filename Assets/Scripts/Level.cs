using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private int ammoCount;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform finishPoint;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private TargetGroup[] targetGroups;


    private PlayerController _playerController;
    private int _currentPointNum;

    public PlayerController Initialize()
    {
        _playerController = Instantiate(playerPrefab,
                                       startPoint.position,
                                       startPoint.rotation,
                                       transform).GetComponent<PlayerController>();

        return _playerController;
    }

    [ContextMenu("Utils/PlayerNextPoint")]
    public void PlayerNextPoint()
    {
        if (_currentPointNum >= wayPoints.Length) return;
        _playerController.MoveToPoint(wayPoints[_currentPointNum].position);
        _playerController.SetTargetGroup(targetGroups[_currentPointNum]);
        _currentPointNum++;
    }
}
