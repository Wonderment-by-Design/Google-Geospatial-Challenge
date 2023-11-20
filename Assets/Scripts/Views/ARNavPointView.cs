using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARNavPointView : MonoBehaviour
{
    [SerializeField]
    GameObject arrows;

    Transform target;
    bool pointsToStart = false;

    public bool PointsToStart { get => pointsToStart; set => pointsToStart = value; }

    public void SetTarget(Transform targetTransform)
    {
        if (pointsToStart)
        {
            arrows.SetActive(false);
        }
        target = targetTransform;
    }

    public void DeactivateArrows()
    {
        arrows.SetActive(false);
    }

    public void ActivateArrows()
    {
        if (pointsToStart)
        {
            arrows.SetActive(false);
            return;
        }
        arrows.SetActive(true);
    }

    private void Update()
    {
        if (target != null)
        {
            transform.LookAt(target);
        }
    }
}
