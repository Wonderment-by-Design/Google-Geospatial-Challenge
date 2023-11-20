using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXFogButtonView : MonoBehaviour
{
    private VFXModel _vfxModel;
    private VFXController _vfxController;

    private void Awake()
    {
        _vfxModel = FindObjectOfType<VFXModel>();
        _vfxController = FindObjectOfType<VFXController>();
    }

    public void InitFogSample()
    {
        _vfxController.InitSampleFogColor();
    }
}
