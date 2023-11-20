using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerObstacleView : MonoBehaviour
{
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    public void UpdateObstacleTrigger(bool value)
    {
        _playerController.ToggleTriggerObstacles(value);
    }
}
