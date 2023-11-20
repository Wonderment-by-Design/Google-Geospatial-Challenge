using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraView : MonoBehaviour
{
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (_playerController)
        {
            _playerController.UpdatePosition(this.transform.position);
            _playerController.UpdateRotation(this.transform.rotation);
        }
    }
}
