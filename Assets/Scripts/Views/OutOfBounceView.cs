using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutOfBounceView : MonoBehaviour
{
    [SerializeField]
    RectTransform image;
    [SerializeField]
    float scalingSpeed = 1.0f;
    [SerializeField]
    float maxSize = 2;
    LabyrinthModel labyrinthModel;


    private bool isScaling = false;

    void Start()
    {
        labyrinthModel = LabyrinthModel.Instance;        
    }

    void Update()
    {
        if (labyrinthModel != null)
        {
            UpdateVisual();
        }
    }

    void UpdateVisual()
    {
        if (labyrinthModel.InsideWall)
        {
            if (image.localScale.x > 0)
            {
                image.localScale = image.localScale - Vector3.one * Time.deltaTime * scalingSpeed;
            }
            else
            {
                image.localScale = Vector3.zero;
            }
        }
        else
        {
            if(image.localScale.x < maxSize)
            {
                image.localScale = image.localScale + Vector3.one * Time.deltaTime * scalingSpeed;
            }
        }
    }
}
