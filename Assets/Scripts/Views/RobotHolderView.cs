using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// always looks at player
public class RobotLookView : MonoBehaviour
{
    private PlayerModel _playerModel;

    private void Awake()
    {
        _playerModel = FindObjectOfType<PlayerModel>();
    }

    private void Update()
    {
        transform.LookAt(_playerModel.CurrentPosition, Vector3.up);
    }

}
