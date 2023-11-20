using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotScaleSliderView : MonoBehaviour
{
    private RobotController _robotController;

    private void Awake()
    {
        _robotController = FindObjectOfType<RobotController>();
    }

    public void UpdateScale(float value)
    {
        _robotController.UpdateScale(value);
    }


}
