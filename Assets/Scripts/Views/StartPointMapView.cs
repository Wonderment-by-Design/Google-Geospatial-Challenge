using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartPointMapView : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI pointText;
    [SerializeField]
    Image labyrinthIcon;
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    GameObject label;

    MapModel mapModel;
    MapController mapController;
    LabyrinthView labyrinthView;

    bool selected = false;

    public LabyrinthView LabyrinthView { get => labyrinthView; set => labyrinthView = value; }

    private void Start()
    {
        mapController = MapController.Instance;
        mapModel = MapModel.Instance;

        mapModel.SwitchToARMapHandler += HideLabel;
        mapModel.SwitchToMapHandler += ShowLabel;

        canvas.gameObject.SetActive(false);
        canvas.worldCamera = mapModel.MapCam;
    }

    public void Init(LabyrinthView labyrinth)
    {
        labyrinthView = labyrinth;
        pointText.text = labyrinthView.LabyrinthSettings.LabyrinthName;
        labyrinthIcon.sprite = labyrinthView.LabyrinthSettings.MapIcon;
    }

    private void ShowLabel()
    {
        label.SetActive(true);
    }

    private void HideLabel()
    {
        label.SetActive(false);
    }

    public void DirectSelect()
    {
        selected = true;
        mapController.StartPointClicked(this, labyrinthView);
        label.SetActive(true);
        canvas.gameObject.SetActive(selected);
    }

    public void Selected()
    {
        if (!selected)
        {
            selected = true;
            mapController.StartPointClicked(this, labyrinthView);
            label.SetActive(true);
            canvas.gameObject.SetActive(selected);
            
        }
        else
        {
            selected = false;
            mapController.Deselect();
            canvas.gameObject.SetActive(selected);
        }
    }
}
