using System.Collections;
using System.Collections.Generic;
using Google.XR.ARCoreExtensions;
using Google.XR.ARCoreExtensions.GeospatialCreator.Internal;
using Google.XR.ARCoreExtensions.Samples.Geospatial;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

//[RequireComponent(typeof(ARGeospatialCreatorAnchor))]
public class StartView : MonoBehaviour
{
    [SerializeField]
    private GameObject _go;

    LabyrinthModel labyrinthModel;
    LabyrinthController labyrinthController;
    [SerializeField]
    GeospatialControllerOriginal geospatialController;
    //[SerializeField]
    //GameObject startPillar;
    ARGeospatialCreatorAnchor aRGeospatialAnchor;
    LabyrinthView labyrinthView;

    private PlayerModel _playerModel;
    private float _distanceThreshold = 50;

    public LabyrinthView LabyrinthView { get => labyrinthView; set => labyrinthView = value; }

    private void Awake()
    {
        aRGeospatialAnchor = GetComponent<ARGeospatialCreatorAnchor>();
        _playerModel = FindObjectOfType<PlayerModel>();
    }

    void Start()
    {
        labyrinthModel = LabyrinthModel.Instance;
        labyrinthController = LabyrinthController.Instance;
    }

    public Vector2d GetLatLong()
    {
        Vector2d latlong = new Vector2d(aRGeospatialAnchor.Latitude, aRGeospatialAnchor.Longitude);
        return latlong;
    }

    public float CheckPlayerDistance()
    {
        return Vector3.Distance(labyrinthModel.MainCamera.transform.position, transform.position);
    }

    void Update()
    {
        if(labyrinthModel.CurrentSelectedLabyrinth == null)
        {
            return;
        }

        if((geospatialController.EarthManager.EarthTrackingState == TrackingState.Tracking && geospatialController.HorizontalAccuracy < labyrinthModel.MinHorizontalAccuracy) || labyrinthModel.Testing)
        {
            if (CheckPlayerDistance() <= labyrinthModel.StartDistance)
            {
                if (labyrinthModel.CurrentSelectedLabyrinth.LabyrinthSettings.name == labyrinthView.LabyrinthSettings.name)
                {
                    if(labyrinthModel.AllowedToStart == false)
                    {
                        labyrinthModel.AllowedToStart = true;
                        labyrinthModel.ToggleStart();
                    }
                }
            }
            else
            {
                if (labyrinthModel.AllowedToStart)
                {
                    labyrinthModel.AllowedToStart = false;
                    labyrinthModel.ToggleStart();
                }
            }
        }

        if (_playerModel && _go)
        {
            float distance = Mathf.Abs(Vector3.Distance(transform.position, _playerModel.CurrentPosition));

            if (distance > _distanceThreshold)
            {
                _go.SetActive(false);
            }
            else
            {
                _go.SetActive(true);
            }

            //Logger.Add("distance: " + distance);
        }

        //else if (CheckPlayerDistance() > labyrinthModel.StartDistance && labyrinthModel.AllowedToStart == true)
        //{
        //    labyrinthModel.AllowedToStart = false;
        //    labyrinthModel.ToggleStart();
        //}
    }
}
