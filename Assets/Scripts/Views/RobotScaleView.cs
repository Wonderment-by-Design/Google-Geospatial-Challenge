using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotScaleView : MonoBehaviour
{
    private RobotModel _robotModel;

    void Awake()
    {
        _robotModel = FindObjectOfType<RobotModel>();
        _robotModel.UpdateScaleHandler += UpdateScaleHandler;
    }

    private void UpdateScaleHandler()
    {
        transform.localScale = new Vector3(_robotModel.CurrentScale, _robotModel.CurrentScale, _robotModel.CurrentScale);
    }
}
