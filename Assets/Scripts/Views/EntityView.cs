using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EntityView : MonoBehaviour
{
    LabyrinthModel labyrinthModel;
    LabyrinthController labyrinthController;

    [SerializeField]
    float MoveTime = 2.0f;
    [SerializeField]
    float height = 10;
    Vector3 targetObject;
    bool allowedToMoveAgain = true;
    

    void Start()
    {
        labyrinthModel = LabyrinthModel.Instance;
        labyrinthController = LabyrinthController.Instance;
        gameObject.SetActive(false);
    }

    public void SetSize(float size)
    {
        transform.localScale = Vector3.one * size;
    }

    public void SetHeight(float height)
    {
        this.height = height;
        var newPos = transform.position;
        newPos.y = height;
        transform.position = newPos;
    }


    public void SetTarget()
    {
        if (!this.gameObject.activeInHierarchy || labyrinthModel.BlocksToBeRevealed == null)
        {
            return;
        }
        float distance = -1;
        BlockView closestBlock = null;
        foreach (var block in labyrinthModel.BlocksToBeRevealed)
        {
            float newDist = Vector3.Distance(labyrinthModel.MainCamera.transform.position, block.transform.position);
            if(distance == -1 || newDist < distance)
            {
                distance = newDist;
                closestBlock = block;
            }
        }

        if(closestBlock != null)
        {
            targetObject = closestBlock.transform.position;
            if (allowedToMoveAgain)
            {
                MoveToTarget(closestBlock);
            }
        }
    }

    private void MoveToTarget(BlockView block)
    {
        if (targetObject != null)
        {
            allowedToMoveAgain = false;
            Vector3 targetPosition = new Vector3(targetObject.x, targetObject.y+height, targetObject.z);

            this.transform.DOMove(targetPosition, MoveTime).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                labyrinthController.ActivateBlock(block);
                allowedToMoveAgain = true;
                labyrinthModel.BlocksToBeRevealed.Remove(block);

                if(labyrinthModel.BlocksToBeRevealed.Count > 0)
                {
                    SetTarget();
                }
            });
        }
    }
}
