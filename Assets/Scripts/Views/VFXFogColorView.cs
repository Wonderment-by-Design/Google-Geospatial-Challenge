using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VFXFogColorView : MonoBehaviour
{
    [SerializeField]
    private Image _fogColorImage;

    private VFXModel _vfxModel;

    private void Awake()
    {
        _vfxModel = FindAnyObjectByType<VFXModel>();
        _vfxModel.SampleFogColorCompleteHandler += SampleFogColorCompleteHandler;
    }

    private void SampleFogColorCompleteHandler()
    {
        _fogColorImage.color = _vfxModel.CurrentFogColor;
    }
}
