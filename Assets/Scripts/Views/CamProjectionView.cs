using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamProjectionView : MonoBehaviour
{
    [SerializeField]
    Camera mainCam, contentCam;

    void Update()
    {
        //contentCam.projectionMatrix = mainCam.projectionMatrix;
    }
}
