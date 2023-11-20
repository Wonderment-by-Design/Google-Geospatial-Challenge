using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonView : MonoBehaviour
{
    LabyrinthModel labyrinthModel;

    void Start()
    {
        labyrinthModel = LabyrinthModel.Instance;
        labyrinthModel.ToggleStartHandler += Toggle;
        Toggle();
    }

    void Toggle()
    {
       // gameObject.SetActive(labyrinthModel.AllowedToStart);
    }

}
