using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthView : MonoBehaviour
{
    LabyrinthModel labyrinthModel;
    [SerializeField]
    LabyrinthSO labyrinthSettings;
    [SerializeField]
    StartView startObject;
    [SerializeField]
    CollectableView[] collectables;

    [SerializeField]
    InfoPointView[] infoPoints;
    [SerializeField]
    GameObject[] walls;
    [SerializeField]
    GameObject finish;
    [SerializeField]
    GameObject entity;


    [SerializeField]
    float activateRange;

    public StartView StartObject { get => startObject; set => startObject = value; }
    public LabyrinthSO LabyrinthSettings { get => labyrinthSettings; set => labyrinthSettings = value; }
    public InfoPointView[] InfoPoints { get => infoPoints; set => infoPoints = value; }
    public GameObject Entity { get => entity; set => entity = value; }
    public CollectableView[] Collectables { get => collectables; set => collectables = value; }

    private void Start()
    {
        labyrinthModel = LabyrinthModel.Instance;
        labyrinthModel.StartedHandler += ActivateObjects;
        startObject.LabyrinthView = this;

        ToggleObjects(false);
    }

    private void ActivateObjects()
    {
        if (this == labyrinthModel.CurrentSelectedLabyrinth)
        {
            ToggleObjects(true);
        }
    }

    private void ToggleObjects(bool set)
    {
        foreach (var item in Collectables)
        {
            item.gameObject.SetActive(set);
        }

        foreach (var item in InfoPoints)
        {
            item.gameObject.SetActive(set);
        }

        foreach (var item in walls)
        {
            item.SetActive(set);
        }

        finish.SetActive(set);
    }

    private void Update()
    {
        CheckIfObjectIsInRange();
    }

    private void CheckIfObjectIsInRange()
    {
        if (!labyrinthModel.StartedLabyrinth)
        {
            return;
        }
        bool isObjectInRange = false;
        List<GameObject> objectsInRange = new List<GameObject>();
        foreach (var collectable in Collectables)
        {
            if (IsInRange(activateRange, collectable.transform))
            {
                if (collectable.CanBeCollected)
                {
                    isObjectInRange = true;
                    objectsInRange.Add(collectable.gameObject);
                }
            }
        }
        foreach (var infoPoint in InfoPoints)
        {
            if (IsInRange(activateRange, infoPoint.transform))
            {
                if (!infoPoint.Found)
                {
                    isObjectInRange = true;
                    objectsInRange.Add(infoPoint.gameObject);
                }
            }
        }

        if (isObjectInRange)
        {
            InteractiveController.Instance.ShowObjectIsNear(objectsInRange.ToArray());
        }
        else
        {
            InteractiveController.Instance.StopShowingObjectIsNear();
        }
    }

    private bool IsInRange(float range, Transform target)
    {
        if (Vector3.Distance(target.position, labyrinthModel.MainCamera.transform.position) < range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
