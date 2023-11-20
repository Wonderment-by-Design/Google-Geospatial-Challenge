using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishView : MonoBehaviour
{

    LabyrinthModel labyrinthModel;
    LabyrinthController labyrinthController;

    private PlayerModel _playerModel;
    private float _distanceThreshold = 50;

    private void Awake()
    {
        _playerModel = FindObjectOfType<PlayerModel>();
    }

    void Start()
    {
        labyrinthModel = LabyrinthModel.Instance;
        labyrinthController = LabyrinthController.Instance;
    }

    float CheckPlayerDistance()
    {
        return Vector3.Distance(labyrinthModel.MainCamera.transform.position, transform.position);
    }

    void CheckFinishDistance()
    {
        if (!labyrinthModel.StartedLabyrinth)
        {
            return;
        }
        if (CheckPlayerDistance() <= labyrinthModel.FinishDistance)
        {
            labyrinthController.FinishLabyrinth();
        }
    }

    void Update()
    {
        //if (_playerModel && _go)
        //{
        //    float distance = Mathf.Abs(Vector3.Distance(transform.position, _playerModel.CurrentPosition));

        //    if (distance > _distanceThreshold)
        //    {
        //        _go.SetActive(false);
        //    }
        //    else
        //    {
        //        _go.SetActive(true);
        //    }
        //}

        CheckFinishDistance();
    }
}
