using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXLightView : MonoBehaviour
{
    private VFXModel _vfxModel;

    private void Awake()
    {
        _vfxModel = FindObjectOfType<VFXModel>();
        _vfxModel.UpdateSunDirectionHandler += UpdateSunDirectionHandler;
    }

    private void UpdateSunDirectionHandler()
    {
        transform.localRotation = _vfxModel.CurrentSunDirection;
        transform.Rotate(new Vector3(0,180,0), Space.World); // correction  
    }
}
