using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public delegate void EventObstacleHandler(ObstacleView obstacleView);
    public event EventObstacleHandler FoundObstacleHandler;

    [SerializeField]
    private float _thresholdRange = 50;

    [SerializeField]
    private float _thresholdObstacleRange = 50;

    [SerializeField]
    private bool _isTriggeringObstacles = false;

    private Vector3 _currentPosition = Vector3.zero;
    private Quaternion _currentRotation = Quaternion.identity;

    internal void FoundObstacle(ObstacleView obstacleView)
    {
        FoundObstacleHandler?.Invoke(obstacleView);
        //Logger.Add("Found obstacle: " + obstacleView.name);
    }

    public Vector3 CurrentPosition { get => _currentPosition; set => _currentPosition = value; }
    public Quaternion CurrentRotation { get => _currentRotation; set => _currentRotation = value; }
    public float ThresholdRange { get => _thresholdRange; set => _thresholdRange = value; }
    public bool IsTriggeringObstacles { get => _isTriggeringObstacles; set => _isTriggeringObstacles = value; }
    public float ThresholdObstacleRange { get => _thresholdObstacleRange; set => _thresholdObstacleRange = value; }
}
