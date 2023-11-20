using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotHandView : MonoBehaviour
{
    private RobotModel _robotModel;
    private RobotController _robotController;

    private void Awake()
    {
        _robotModel = FindObjectOfType<RobotModel>();
        _robotController = FindObjectOfType<RobotController>();
    }

    private void Update()
    {
        if(_robotModel.CurrentAnimationState == RobotModel.AnimationState.PickUp)
        {
            _robotController.UpdateHandTransform(transform.position, transform.rotation);
        }
    }
}
