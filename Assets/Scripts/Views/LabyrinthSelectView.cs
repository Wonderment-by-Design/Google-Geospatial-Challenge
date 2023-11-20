using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabyrinthSelectView : MonoBehaviour
{
    [SerializeField]
    MapModel mapModel;

    [SerializeField]
    List<Button> buttons;

    void Awake()
    {
        mapModel.SetSelectButtonsHandler += SetSelectButtons;
    }

    private void SetSelectButtons()
    {
        for (int i = 0; i < mapModel.SpawnedLabyrinthObjects.Count; i++)
        {
            if (i < buttons.Count)
            {
                int index = i;
                buttons[i].onClick.AddListener(delegate { mapModel.SpawnedLabyrinthObjects[index].GetComponent<StartPointMapView>().DirectSelect(); });
            }
        }
    }

}
