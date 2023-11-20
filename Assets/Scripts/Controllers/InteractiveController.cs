using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveController : Singleton<InteractiveController>
{
    [SerializeField]
    InteractiveModel interactiveModel;
    [SerializeField]
    LabyrinthModel labyrinthModel;

    public void ShowObjectIsNear(GameObject[] gameobjects)
    {
        interactiveModel.ObjectsInRange = gameobjects;
        interactiveModel.ShowObjectsClose();
    }

    public void StopShowingObjectIsNear()
    {
        interactiveModel.ObjectsInRange = null;
        interactiveModel.HideObjectsClose();
    }

    public bool IsInFrontOfCamera(GameObject obj, float maxRange)
    {
        Vector3 directionToObject = (obj.transform.position - labyrinthModel.MainCamera.transform.position).normalized;
        float dotProduct = Vector3.Dot(directionToObject, labyrinthModel.MainCamera.transform.forward);
        return dotProduct > maxRange;
    }

    private bool IsInBehindOfCamera(GameObject obj, float minRange)
    {
        Vector3 directionToObject = (obj.transform.position - labyrinthModel.MainCamera.transform.position).normalized;
        float dotProduct = Vector3.Dot(directionToObject, labyrinthModel.MainCamera.transform.forward);
        return dotProduct < minRange;
    }

    public void AddAmountCollected()
    {
        interactiveModel.AmountCollected++;
        interactiveModel.UpdateCollection();
    }

    void GazeCheckInteractives()
    {
        if (interactiveModel.ObjectsInRange != null)
        {
            foreach (var item in interactiveModel.ObjectsInRange)
            {
                if (IsInFrontOfCamera(item, 0.970f))
                {
                    if (!interactiveModel.ObjectTimers.ContainsKey(item))
                    {
                        interactiveModel.ObjectTimers.Add(item, Time.time);
                    }

                    if (Time.time - interactiveModel.ObjectTimers[item] >= interactiveModel.TimeToInteract)
                    {
                        if (item.GetComponent<CollectableView>())
                        {
                            var collectableView = item.GetComponent<CollectableView>();
                            if (collectableView.CanBeCollected)
                            {
                                collectableView.Collect();
                                interactiveModel.AmountCollected++;
                                interactiveModel.UpdateCollection();
                            }
                        }

                        if (item.GetComponent<InfoPointView>())
                        {
                            var infoView = item.GetComponent<InfoPointView>();
                            if (!infoView.ShowingInfo)
                            {
                                if (infoView.Found == false)
                                {
                                    interactiveModel.InfoSeen++;
                                }

                                infoView.ShowInfo();
                            }
                        }
                        interactiveModel.ObjectTimers.Remove(item);
                    }
                }
                else
                {
                    if (interactiveModel.ObjectTimers.ContainsKey(item))
                    {
                        interactiveModel.ObjectTimers.Remove(item);
                    }

                    if (IsInBehindOfCamera(item, 0.8f))
                    {
                        if (item.GetComponent<InfoPointView>())
                        {
                            var infoView = item.GetComponent<InfoPointView>();
                            if (infoView.ShowingInfo)
                            {
                                infoView.StopShowingInfo();
                            }
                        }
                    }
                }
            }
        }
        interactiveModel.InteractionVisualValue = GetVisualValue();
    }

    public float GetVisualValue()
    {
        float heighestLookValue = 0;
        if(interactiveModel.ObjectsInRange == null)
        {
            return 0;
        }

        foreach (var item in interactiveModel.ObjectsInRange)
        {
            if (interactiveModel.ObjectTimers.ContainsKey(item))
            {
                float objectTime;
                interactiveModel.ObjectTimers.TryGetValue(item, out objectTime);
                if (objectTime > heighestLookValue)
                {
                    heighestLookValue = objectTime;
                }
            }
        }

        if (Time.time - heighestLookValue >= interactiveModel.TimeToInteract)
        {
            return 0;
        }

        return Time.time - heighestLookValue;
    }

    void Update()
    {
        GazeCheckInteractives();

        // if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        // {
        //     Ray ray = labyrinthModel.MainCamera.ScreenPointToRay(Input.GetTouch(0).position);
        //     RaycastHit hit;
        //     if (Physics.Raycast(ray, out hit))
        //     {
        //         Debug.Log(hit.transform.name);
        //         if (hit.transform.gameObject.GetComponent<CollectableView>())
        //         {
        //             hit.transform.gameObject.GetComponent<CollectableView>().Collect();
        //             interactiveModel.AmountCollected++;
        //             interactiveModel.UpdateCollection();
        //         }

        //         if (hit.transform.gameObject.GetComponent<InfoPointView>())
        //         {
        //             hit.transform.gameObject.GetComponent<InfoPointView>().ShowInfo();
        //         }
        //     }
        // }
    }
}
