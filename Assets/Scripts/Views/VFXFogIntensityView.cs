using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXFogIntensityView : MonoBehaviour
{
    private VFXController _vfxController;

    private void Awake()
    {
        _vfxController = FindObjectOfType<VFXController>();
    }

    public void UpdateFogIntensity(float value)
    {
        _vfxController.UpdateFogIntensity(value);
    }
}
