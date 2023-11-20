using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockView : MonoBehaviour
{

    LabyrinthModel labyrinthModel;
    LabyrinthController labyrinthController;
    Vector3 pos;
    bool shouldSpawn = false;
    bool allowedToSpawn = false;

    [SerializeField]
    GameObject blockObject;

    public bool AllowedToSpawn { get => allowedToSpawn; set => allowedToSpawn = value; }

    void Start()
    {
        labyrinthModel = LabyrinthModel.Instance;
        labyrinthController = LabyrinthController.Instance;

        pos = this.transform.position;
        blockObject.SetActive(false);
    }

    public float CheckPlayerDistance()
    {
        return Vector3.Distance(labyrinthModel.MainCamera.transform.position, pos);
    }

    void Update()
    {
        if (CheckPlayerDistance() <= labyrinthModel.SpawnInDistance && shouldSpawn == false)
        {
            labyrinthController.AddBlockToBeRevealed(this);
            shouldSpawn = true;
        }
        else if (CheckPlayerDistance() > labyrinthModel.SpawnInDistance && shouldSpawn == true)
        {
            labyrinthController.RemovelockToBeRevealed(this);
            shouldSpawn = false;
        }
    }

    public void SpawnBlock()
    {
        blockObject.SetActive(true);
    }
}
