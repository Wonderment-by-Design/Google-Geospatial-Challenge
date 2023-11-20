using System.Collections;
using System.Collections.Generic;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using UnityEngine;
using Google.XR.ARCoreExtensions.GeospatialCreator.Internal;

public class MapModel : Singleton<MapModel>
{
    public delegate void EventHandler();
    public event EventHandler SetSelectButtonsHandler;
    public event EventHandler SetDistanceHandler;
    public event EventHandler SwitchToARMapHandler;
    public event EventHandler SwitchToMapHandler;
    public event EventHandler SelectedHandler;
    public event EventHandler DeselectedHandler;

    [SerializeField]
    AbstractMap map;
    [SerializeField]
    Camera mapCam;
    Vector2d[] labyrinthLocations;
    List<GameObject> spawnedLabyrinthObjects;

    [SerializeField]
    float spawnScale = 100f;
    [SerializeField]
    float directionMinDistance = 8f;

    [SerializeField]
    GameObject player;

    [SerializeField]
    DirectionsFactory directions;

    [SerializeField]
    GameObject markerPrefab;
    [SerializeField]
    GameObject infoMarkerPrefab;
    [SerializeField]
    ARGeospatialCreatorAnchor routePointObject;
    [SerializeField]
    LineRenderer routeLine;
    [SerializeField]
    RenderTexture mapARRenderTexture;

    double distanceToTravel;
    Transform camParent;
    StartPointMapView previousSelectedPoint;

    List<Vector3> routeMapPoints = new List<Vector3>();
    List<GameObject> arRoutePoints = new List<GameObject>();
    List<GameObject> directionPoints;
    bool allowedToPlaceARRoute = false;
    bool routingToStart;

    Vector2d[] routePoints;

    Vector2d[] infoPointLocations;
    List<GameObject> spawnedInfoPointObjects;

    public AbstractMap Map { get => map; set => map = value; }
    public Vector2d[] LabyrinthLocations { get => labyrinthLocations; set => labyrinthLocations = value; }
    public float SpawnScale { get => spawnScale; set => spawnScale = value; }
    public GameObject MarkerPrefab { get => markerPrefab; set => markerPrefab = value; }
    public List<GameObject> SpawnedLabyrinthObjects { get => spawnedLabyrinthObjects; set => spawnedLabyrinthObjects = value; }
    public Camera MapCam { get => mapCam; set => mapCam = value; }
    public DirectionsFactory Directions { get => directions; set => directions = value; }
    public GameObject Player { get => player; set => player = value; }
    public Vector2d[] RoutePoints { get => routePoints; set => routePoints = value; }
    public ARGeospatialCreatorAnchor RoutePointObject { get => routePointObject; set => routePointObject = value; }
    public List<GameObject> ArRoutePoints { get => arRoutePoints; set => arRoutePoints = value; }
    public bool AllowedToPlaceARRoute { get => allowedToPlaceARRoute; set => allowedToPlaceARRoute = value; }
    public LineRenderer RouteLine { get => routeLine; set => routeLine = value; }
    public List<Vector3> RouteMapPoints { get => routeMapPoints; set => routeMapPoints = value; }
    public bool RoutingToStart { get => routingToStart; set => routingToStart = value; }
    public StartPointMapView PreviousSelectedPoint { get => previousSelectedPoint; set => previousSelectedPoint = value; }
    public RenderTexture MapARRenderTexture { get => mapARRenderTexture; set => mapARRenderTexture = value; }
    public Transform CamParent { get => camParent; set => camParent = value; }
    public double DistanceToTravel { get => distanceToTravel; set => distanceToTravel = value; }
    public List<GameObject> SpawnedInfoPointObjects { get => spawnedInfoPointObjects; set => spawnedInfoPointObjects = value; }
    public Vector2d[] InfoPointLocations { get => infoPointLocations; set => infoPointLocations = value; }
    public GameObject InfoMarkerPrefab { get => infoMarkerPrefab; set => infoMarkerPrefab = value; }
    public List<GameObject> DirectionPoints { get => directionPoints; set => directionPoints = value; }
    public float DirectionMinDistance { get => directionMinDistance; set => directionMinDistance = value; }

    public void SetSelectButtons()
    {
        SetSelectButtonsHandler?.Invoke();
    }

    public void SetDistance()
    {
        SetDistanceHandler?.Invoke();
    }

    public void SwitchToARMap()
    {
        SwitchToARMapHandler?.Invoke();
    }

    public void SwitchToMap()
    {
        SwitchToMapHandler?.Invoke();
    }

    public void Selected()
    {
        SelectedHandler?.Invoke();
    }

    public void Deselected()
    {
        DeselectedHandler?.Invoke();
    }
}
