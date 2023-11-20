using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationController : Singleton<ApplicationController>
{
    [SerializeField]
    private ApplicationModel applicationModel;

    private void Start()
    {
        ActivateStart();
    }

    public void ActivateStart()
    {
        applicationModel.CurrentState = ApplicationState.Start;
        applicationModel.ChangeState();
    }

    public void ActivateSelect()
    {
        applicationModel.CurrentState = ApplicationState.Select;
        applicationModel.ChangeState();
    }

    public void ActivateMap()
    {
        applicationModel.CurrentState = ApplicationState.Map;
        applicationModel.ChangeState();
        MapController.Instance.SwitchToMap();
    }

    public void ActivateARMap()
    {
        applicationModel.CurrentState = ApplicationState.ARMap;
        applicationModel.ChangeState();
        MapController.Instance.SwitchToARMap();
    }

    public void ActivateLabyrinth()
    {
        applicationModel.CurrentState = ApplicationState.Labyrinth;
        applicationModel.ChangeState();
        MapController.Instance.SwitchToARMap();
    }

    public void ActivateFinish()
    {
        applicationModel.CurrentState = ApplicationState.Finish;
        applicationModel.ChangeState();
    }

    public void ResetScene()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

}
