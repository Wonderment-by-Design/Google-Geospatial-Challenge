using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrowView : MonoBehaviour
{
    [SerializeField]
    float heightOffset;
    float rotationSpeed = 60;
    LabyrinthModel labyrinthModel;
    MapModel mapModel;
    void Start()
    {
        labyrinthModel = LabyrinthModel.Instance;
        mapModel = MapModel.Instance;

        mapModel.SelectedHandler += Activate;
        mapModel.DeselectedHandler += Deactivate;
        Deactivate();
    }

    void Activate()
    {
        gameObject.SetActive(true);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    void UpdateTransform()
    {
        if (!mapModel.RoutingToStart)
        {
            return;
        }

        Vector3 pos = new Vector3();
        pos.x = labyrinthModel.MainCamera.transform.position.x;
        pos.z = labyrinthModel.MainCamera.transform.position.z;
        pos.y = labyrinthModel.MainCamera.transform.position.y + heightOffset;
        transform.position = pos;

        var lastActive = MapController.Instance.GetLastIndexDirectionPoint();
        if (lastActive > 0)
        {
            var targetPos = mapModel.DirectionPoints[lastActive-1].transform.position;
            targetPos.y = transform.position.y;

            Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            transform.rotation = newRotation;
        }
    }

    void Update()
    {
        UpdateTransform();
    }
}
