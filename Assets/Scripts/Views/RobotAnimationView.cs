using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimationView : MonoBehaviour
{
    private ObstacleModel _obstacleModel;
    private PlayerModel _playerModel;

    private RobotController _robotController;

    private Animator _animator;

    void Awake()
    {
        _obstacleModel = FindObjectOfType<ObstacleModel>();
        _obstacleModel.TriggerSpawnHandler += TriggerSpawnHandler;

        //_playerModel = GetComponent<PlayerModel>();
        //_playerModel.FoundObstacleHandler += FoundObstacleHandler;

        _robotController = FindObjectOfType<RobotController>();

        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Invoke("Wave", 20);
    }

    private void Wave()
    {
        _animator.SetTrigger("Wave");
    }

    private void TriggerSpawnHandler(ObstacleView obstacleView)
    {
        _animator.SetTrigger("Throw");

        _robotController.TriggerSpawnAnimation();
    }

    //private void FoundObstacleHandler(ObstacleView obstacleView)
    //{
    //    _robotController.AddToAnimationQueu(obstacleView);
    //}

    //private void AddToQueuHandler(ObstacleView obstacleView)
    //{
    //    //_robotController.AddToQueu(obstacleView);
    //}
}
