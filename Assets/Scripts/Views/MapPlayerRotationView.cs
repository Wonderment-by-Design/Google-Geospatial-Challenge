using System.Collections;
using System.Collections.Generic;
using Google.XR.ARCoreExtensions;
using Google.XR.ARCoreExtensions.Samples.Geospatial;
using UnityEngine;

public class MapPlayerRotationView : MonoBehaviour
{
    GeospatialControllerOriginal geospatialController;

    AREarthManager earthManager;

    private void Start()
    {
        geospatialController = GameObject.FindFirstObjectByType<GeospatialControllerOriginal>();
        earthManager = geospatialController.EarthManager;
    }

    void SetCameraRotation()
    {
        var rot = earthManager.CameraGeospatialPose.EunRotation;
        Vector3 eulerRot = rot.eulerAngles;
        eulerRot.x = 0;
        eulerRot.z = 0;

        this.transform.eulerAngles = eulerRot;
    }

    void Update()
    {
        SetCameraRotation();
    }
}
