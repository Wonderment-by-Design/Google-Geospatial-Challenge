using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InfoPointMapView : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;

    MapModel mapModel;

    private void Start()
    {
        mapModel = MapModel.Instance;

        canvas.worldCamera = mapModel.MapCam;
    }

    public void Init(InfoPointView infoPoint)
    {
        
    }
}
