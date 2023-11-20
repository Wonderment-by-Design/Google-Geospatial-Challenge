using Google.XR.ARCoreExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

public class PlayerController : MonoBehaviour
{
    private PlayerModel _playerModel;
    private ObstacleModel _obstacleModel;

    void Awake()
    {
        _playerModel = FindObjectOfType<PlayerModel>();
        //_playerModel.FoundObstacleHandler += FoundObstacleHandler;

        _obstacleModel = FindObjectOfType<ObstacleModel>();
        ToggleTriggerObstacles(false);
    }

    internal void UpdatePosition(Vector3 position)
    {
        _playerModel.CurrentPosition = position;
    }

    internal void UpdateRotation(Quaternion rotation)
    {
        _playerModel.CurrentRotation = rotation;
    }

    void Update()
    {
        if(_playerModel.IsTriggeringObstacles)
        {
            CheckForObstacles();
        }
    }

    public void ToggleTriggerObstacles(bool value)
    {
        _playerModel.IsTriggeringObstacles = value;
        Logger.Add("ToggleTriggerObstacles: " + value);
    }

    private void CheckForObstacles()
    {
        // loop through obstacle views and check if the obstacle is hidden and within threshold range
        foreach (ObstacleView obstacleView in _obstacleModel.ObstacleViews)
        {
            obstacleView.VO.Distance = Vector3.Distance(obstacleView.transform.position, _playerModel.CurrentPosition);

            if (obstacleView.VO.CurrentState == ObstacleVO.State.Hidden)
            {
                // check if player position is within threshold range
                if (obstacleView.VO.Distance < _playerModel.ThresholdRange)
                {
                    // player found obstacle
                    _playerModel.FoundObstacle(obstacleView);
                }
            }
        }
    }
}
