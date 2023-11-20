using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    LabyrinthModel labyrinthModel;
    Collider[] hitColliders;
    bool insideWall = false;

    private void Start()
    {
        labyrinthModel = LabyrinthModel.Instance;
    }


    void Update()
    {
        insideWall = false;
        hitColliders = Physics.OverlapSphere(transform.position, 0f);

        foreach (var collider in hitColliders)
        {
            if (collider.gameObject.tag == "Block")
            {
                insideWall = true;
                //Debug.Log("Inside wall");
            }
        }

        labyrinthModel.InsideWall = insideWall;
    }
}
