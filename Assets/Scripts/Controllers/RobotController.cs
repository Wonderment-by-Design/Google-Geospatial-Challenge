using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    private RobotModel _robotModel;

    //private ObstacleModel _obstacleModel;

    void Awake()
    {
        _robotModel = FindObjectOfType<RobotModel>();
    }

    internal void TriggerSpawnAnimation()
    {
        _robotModel.CurrentAnimationState = RobotModel.AnimationState.PickUp;
        _robotModel.UpdateAnimationState();

        Invoke("TriggerSpawnPickupComplete", _robotModel.SpawnPickupAnimationTime);
        Invoke("TriggerSpawnThrowComplete", _robotModel.SpawnPickupAnimationTime + _robotModel.SpawnThrowAnimationTime);
    }

    private void TriggerSpawnPickupComplete()
    {
        _robotModel.CurrentAnimationState = RobotModel.AnimationState.Throw;
        _robotModel.UpdateAnimationState();
    }

    private void TriggerSpawnThrowComplete()
    {
        _robotModel.CurrentAnimationState = RobotModel.AnimationState.Idle;
        _robotModel.UpdateAnimationState();
        _robotModel.SpawnThrowComplete();
    }

    internal void UpdateHandTransform(Vector3 position, Quaternion rotation)
    {
        _robotModel.CurrentHandPosition = position;
        _robotModel.CurrentHandRotation = rotation;
        _robotModel.UpdateTransformHand();
    }

    internal void UpdateScale(float value)
    {
        _robotModel.CurrentScale = value;
        _robotModel.UpdateScale();
    }
}
