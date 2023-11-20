using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTransitionView : MonoBehaviour
{
    [SerializeField]
    float transitionValue;
    [SerializeField]
    GameObject groundObject;
    LabyrinthModel labyrinthModel;


    bool showingMap = true;

    private void Start()
    {
        labyrinthModel = LabyrinthModel.Instance;
    }

    private void Update()
    {
        CheckTransition();
    }

    private void OnEnable()
    {
        // if (LookingDown(transitionValue))
        // {
        //     Debug.Log("SHOW MAP");
        //     ApplicationController.Instance.ActivateMap();
        //     showingMap = true;
        // }
        // else
        // {
        //     Debug.Log("SHOW AR");
        //     ApplicationController.Instance.ActivateARMap();
        //     showingMap = false;
        // }
    }

    private void CheckTransition()
    {
        var newPos = labyrinthModel.MainCamera.transform.position;
        newPos.y = labyrinthModel.MainCamera.transform.position.y - 1;
        groundObject.transform.position = newPos;

        if (LookingDown(transitionValue))
        {
            if (!showingMap)
            {
                Debug.Log("SHOW MAP");
                ApplicationController.Instance.ActivateMap();
                showingMap = true;
            }

        }
        else
        {
            if (showingMap)
            {
                Debug.Log("SHOW AR");
                ApplicationController.Instance.ActivateARMap();
                showingMap = false;
            }
        }
    }


    public bool LookingDown(float maxRange)
    {
        Vector3 directionToObject = (groundObject.transform.position - labyrinthModel.MainCamera.transform.position).normalized;
        float dotProduct = Vector3.Dot(directionToObject, labyrinthModel.MainCamera.transform.forward);
        return dotProduct > maxRange;
    }
}
