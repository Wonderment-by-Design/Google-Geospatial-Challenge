using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Google.XR.ARCoreExtensions;
using Google.XR.ARCoreExtensions.GeospatialCreator.Internal;
using Mapbox.Examples;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MapController : Singleton<MapController>
{
    [SerializeField]
    MapModel mapModel;
    [SerializeField]
    LabyrinthModel labyrinthModel;

    private void Start()
    {
        mapModel.CamParent = mapModel.MapCam.transform.parent;
        mapModel.LabyrinthLocations = new Vector2d[labyrinthModel.Labyrinths.Count];
        mapModel.SpawnedLabyrinthObjects = new List<GameObject>();
        for (int i = 0; i < labyrinthModel.Labyrinths.Count; i++)
        {
            mapModel.LabyrinthLocations[i] = labyrinthModel.Labyrinths[i].StartObject.GetLatLong();
            var instance = Instantiate(mapModel.MarkerPrefab);
            instance.transform.localPosition = mapModel.Map.GeoToWorldPosition(mapModel.LabyrinthLocations[i], true);
            instance.transform.localScale = new Vector3(mapModel.SpawnScale, mapModel.SpawnScale, mapModel.SpawnScale);
            instance.GetComponent<StartPointMapView>().Init(labyrinthModel.Labyrinths[i]);
            mapModel.SpawnedLabyrinthObjects.Add(instance);
        }

        SwitchToARMap();
        mapModel.SetSelectButtons();
    }

    private void Update()
    {
        UpdateLabyrinthsOnMap();

        UpdateInfoPointsOnMap();

        UpdateMapRoute();


        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = mapModel.MapCam.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.gameObject.GetComponent<StartPointMapView>())
                {
                    hit.transform.gameObject.GetComponent<StartPointMapView>().Selected();
                }
            }
        }
    }

    private void UpdateLabyrinthsOnMap()
    {
        for (int i = 0; i < mapModel.SpawnedLabyrinthObjects.Count; i++)
        {
            var spawnedObject = mapModel.SpawnedLabyrinthObjects[i];
            var location = mapModel.LabyrinthLocations[i];
            spawnedObject.transform.localPosition = mapModel.Map.GeoToWorldPosition(location, true);
            spawnedObject.transform.localScale = new Vector3(mapModel.SpawnScale, mapModel.SpawnScale, mapModel.SpawnScale);
        }
    }

    public void SetDistance(double distance)
    {
        mapModel.DistanceToTravel = distance;
        mapModel.SetDistance();
    }

    public void SwitchToARMap()
    {

        var mainCameraData = labyrinthModel.MainCamera.GetUniversalAdditionalCameraData();

        if (mainCameraData.cameraStack.Contains(mapModel.MapCam))
        {
            mainCameraData.cameraStack.Remove(mapModel.MapCam);
        }

        var mapCameraData = mapModel.MapCam.GetUniversalAdditionalCameraData();
        mapCameraData.renderType = CameraRenderType.Base;
        mapModel.MapCam.fieldOfView = 28f;
        mapModel.MapCam.targetTexture = mapModel.MapARRenderTexture;

        var newRot = Vector3.zero;
        newRot.x = 90;
        newRot.z = -mapModel.Player.transform.eulerAngles.y;
        mapModel.MapCam.transform.eulerAngles = newRot;

        mapModel.MapCam.transform.parent = mapModel.Player.transform;
        mapModel.MapCam.transform.localPosition = new Vector3(0, 40, 0);

        mapModel.SwitchToARMap();
    }

    public void SwitchToMap()
    {
        mapModel.MapCam.transform.parent = mapModel.CamParent;
        mapModel.MapCam.transform.eulerAngles = new Vector3(90, 0, 0);
        mapModel.MapCam.transform.localPosition = new Vector3(0, 200, 0);

        mapModel.MapCam.targetTexture = null;
        var mapCameraData = mapModel.MapCam.GetUniversalAdditionalCameraData();
        mapCameraData.renderType = CameraRenderType.Overlay;

        var mainCameraData = labyrinthModel.MainCamera.GetUniversalAdditionalCameraData();
        if (!mainCameraData.cameraStack.Contains(mapModel.MapCam))
        {
            mainCameraData.cameraStack.Add(mapModel.MapCam);
        }

        mapModel.MapCam.fieldOfView = 60f;

        mapModel.SwitchToMap();
    }

    public void StartPointClicked(StartPointMapView startPoint, LabyrinthView labyrinth)
    {
        mapModel.AllowedToPlaceARRoute = true;

        if (mapModel.RoutingToStart && startPoint != mapModel.PreviousSelectedPoint)
        {
            mapModel.PreviousSelectedPoint.Selected();
        }

        labyrinthModel.CurrentSelectedLabyrinth = labyrinth;

        mapModel.Directions.Waypoints[0] = startPoint.transform;
        mapModel.Directions.Waypoints[1] = mapModel.Player.transform;
        mapModel.Directions.gameObject.SetActive(true);
        mapModel.Directions.CreateRoute();

        mapModel.RouteLine.gameObject.SetActive(true);
        mapModel.Map.UpdateMap(startPoint.transform.GetGeoPosition(mapModel.Map.CenterMercator, mapModel.Map.WorldRelativeScale));

        mapModel.RouteLine.startWidth = 0;
        mapModel.RouteLine.endWidth = 0;
        DOTween.To(() => mapModel.RouteLine.startWidth, x => mapModel.RouteLine.startWidth = x, 1, 0.5f).SetEase(Ease.OutBack);
        DOTween.To(() => mapModel.RouteLine.endWidth, x => mapModel.RouteLine.endWidth = x, 1, 0.5f).SetEase(Ease.OutBack);

        mapModel.RoutingToStart = true;
        mapModel.PreviousSelectedPoint = startPoint;
        mapModel.Selected();
    }

    public void Deselect()
    {
        mapModel.Directions.gameObject.SetActive(false);
        SetARArrowVisibility(false);

        mapModel.RouteLine.gameObject.SetActive(false);
        mapModel.RoutingToStart = false;
        mapModel.Deselected();
    }

    public void SetARArrowVisibility(bool set)
    {
        for (int i = 0; i < mapModel.ArRoutePoints.Count; i++)
        {
            mapModel.ArRoutePoints[i].SetActive(set);
        }
    }

    public void SpawnInfoPointsOnMap()
    {
        int index = -1;
        for (int i = 0; i < labyrinthModel.Labyrinths.Count; i++)
        {
            LabyrinthView item = labyrinthModel.Labyrinths[i];
            if (item == labyrinthModel.CurrentSelectedLabyrinth)
            {
                index = i;
            }
        }

        if (index == -1)
        {
            return;
        }

        mapModel.InfoPointLocations = new Vector2d[labyrinthModel.Labyrinths[index].InfoPoints.Length];
        mapModel.SpawnedInfoPointObjects = new List<GameObject>();

        for (int i = 0; i < labyrinthModel.Labyrinths[index].InfoPoints.Length; i++)
        {
            var geospatialAnchor = labyrinthModel.Labyrinths[index].InfoPoints[i].GetComponent<ARGeospatialCreatorAnchor>();
            mapModel.InfoPointLocations[i] = new Vector2d(geospatialAnchor.Latitude, geospatialAnchor.Longitude);

            var instance = Instantiate(mapModel.InfoMarkerPrefab);
            instance.transform.localPosition = mapModel.Map.GeoToWorldPosition(mapModel.InfoPointLocations[i], true);
            instance.transform.localScale = new Vector3(mapModel.SpawnScale, mapModel.SpawnScale, mapModel.SpawnScale);
            instance.GetComponent<InfoPointMapView>().Init(labyrinthModel.Labyrinths[index].InfoPoints[i]);
            mapModel.SpawnedInfoPointObjects.Add(instance);
        }
    }

    private void UpdateInfoPointsOnMap()
    {
        if (mapModel.SpawnedInfoPointObjects != null)
        {
            for (int i = 0; i < mapModel.SpawnedInfoPointObjects.Count; i++)
            {
                var spawnedObject = mapModel.SpawnedInfoPointObjects[i];
                var location = mapModel.InfoPointLocations[i];
                spawnedObject.transform.localPosition = mapModel.Map.GeoToWorldPosition(location, true);
                spawnedObject.transform.localScale = new Vector3(mapModel.SpawnScale, mapModel.SpawnScale, mapModel.SpawnScale);
            }
        }
    }

    public void UpdateRoutePoints(Vector2d[] route)
    {
        mapModel.RoutePoints = route;
    }

    public void UpdateMapRoute()
    {
        if (mapModel.RoutePoints == null)
        {
            return;
        }

        if (mapModel.RouteMapPoints != null)
        {
            mapModel.RouteMapPoints.Clear();
        }

        foreach (var point in mapModel.RoutePoints)
        {
            mapModel.RouteMapPoints.Add(Conversions.GeoToWorldPosition(point.x, point.y, mapModel.Map.CenterMercator, mapModel.Map.WorldRelativeScale).ToVector3xz());
        }


        mapModel.RouteLine.positionCount = mapModel.RouteMapPoints.Count;
        mapModel.RouteLine.SetPositions(mapModel.RouteMapPoints.ToArray());

    }

    public void UpdateARRoute()
    {
        if (!mapModel.AllowedToPlaceARRoute)
        {
            return;
        }
        if (!mapModel.RoutingToStart)
        {
            SetARArrowVisibility(false);
            return;
        }

        if (mapModel.ArRoutePoints.Count > 0)
        {
            for (int i = 0; i < mapModel.ArRoutePoints.Count; i++)
            {
                if (i >= mapModel.RoutePoints.Length)
                {
                    if (mapModel.ArRoutePoints.Count > mapModel.RoutePoints.Length)
                    {
                        for (int j = mapModel.RoutePoints.Length; j < mapModel.ArRoutePoints.Count; j++)
                        {
                            Debug.Log($"Disabled {mapModel.ArRoutePoints[j].name}");
                            mapModel.ArRoutePoints[j].SetActive(false);
                        }
                    }

                    break;
                }

                GameObject point = mapModel.ArRoutePoints[i];
                point.SetActive(true);

                var anchor = point.GetComponent<ARGeospatialCreatorAnchor>();
                anchor.Latitude = mapModel.RoutePoints[i].x;
                anchor.Longitude = mapModel.RoutePoints[i].y;
            }
        }

        int startingIndex = 0;
        if (mapModel.ArRoutePoints.Count != 0)
        {
            startingIndex = mapModel.ArRoutePoints.Count - 1;
        }

        for (int i = startingIndex; i < MapModel.Instance.RoutePoints.Length; i++)
        {
            Vector2d point = MapModel.Instance.RoutePoints[i];
            var arPoint = Instantiate(mapModel.RoutePointObject);
            mapModel.ArRoutePoints.Add(arPoint.gameObject);
            arPoint.Latitude = point.x;
            arPoint.Longitude = point.y;

        }
        //Rotate towards next point
        for (int i = mapModel.ArRoutePoints.Count - 1; i >= 0; i--)
        {
            GameObject target = null;
            if (mapModel.ArRoutePoints[i].activeInHierarchy == true)
            {
                var ARNavView = mapModel.ArRoutePoints[i].GetComponent<ARNavPointView>();

                if (i - 1 >= 0)
                {
                    if (mapModel.ArRoutePoints[i - 1].activeInHierarchy)
                    {
                        target = mapModel.ArRoutePoints[i - 1];
                        ARNavView.PointsToStart = false;
                    }
                    else
                    {
                        target = labyrinthModel.CurrentSelectedLabyrinth.StartObject.gameObject;
                        ARNavView.PointsToStart = true;
                    }
                }
                else
                {
                    target = labyrinthModel.CurrentSelectedLabyrinth.StartObject.gameObject;
                    ARNavView.PointsToStart = true;
                }
               
                ARNavView.SetTarget(target.transform);
                ARNavView.DeactivateArrows();
            }
        }
        mapModel.DirectionPoints = ActiveARPoints();
    }

    public int GetLastActive()
    {
        int lastActive = -1;
        for (int i = 0; i < mapModel.ArRoutePoints.Count; i++)
        {
            if (mapModel.ArRoutePoints[i].activeInHierarchy)
            {
                lastActive = i;
            }
        }

        return lastActive;
    }

    List<GameObject> ActiveARPoints()
    {
        List<GameObject> activePoints = new List<GameObject>();
        for (int i = 0; i < mapModel.ArRoutePoints.Count; i++)
        {
            if (mapModel.ArRoutePoints[i].activeInHierarchy)
            {
                activePoints.Add(mapModel.ArRoutePoints[i]);
            }
        }
        return activePoints;
    }

    public int GetLastIndexDirectionPoint()
    {
        if(mapModel.DirectionPoints == null)
        {
            return -1;
        }

        for (int i = mapModel.DirectionPoints.Count - 1; i >= 0; i--)
        {
            var point = mapModel.DirectionPoints[i];
            if(point.transform.position != Vector3.zero)
            {
                if (Vector3.Distance(labyrinthModel.MainCamera.transform.position, point.transform.position) < mapModel.DirectionMinDistance)
                {
                    mapModel.DirectionPoints.RemoveAt(i);
                }
            }
        }
        mapModel.DirectionPoints[mapModel.DirectionPoints.Count - 1].GetComponent<ARNavPointView>().ActivateArrows();
        return mapModel.DirectionPoints.Count;
    }

    public string ConvertDistanceToString(double distanceInMeters)
    {
        string distanceString;

        if (distanceInMeters >= 1000)
        {
            double distanceInKilometers = distanceInMeters / 1000;
            distanceString = string.Format("{0:0.0}km", distanceInKilometers);
        }
        else
        {
            distanceString = string.Format("{0}m", (int)distanceInMeters);
        }

        return distanceString;
    }
}
