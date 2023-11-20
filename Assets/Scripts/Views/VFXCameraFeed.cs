using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VFXCameraFeed : MonoBehaviour
{
    [SerializeField]
    private RawImage _debugCameraFeed;

    private VFXModel _vfxModel;

    private void Awake()
    {
        _vfxModel = FindObjectOfType<VFXModel>();
        _vfxModel.UpdateCameraFeedHandler += UpdateCameraFeedHandler;
    }

    private void UpdateCameraFeedHandler()
    {
        _debugCameraFeed.texture = _vfxModel.CurrentCameraFeed;
    }
}
