using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthController : Singleton<LabyrinthController>
{
    [SerializeField]
    LabyrinthModel labyrinthModel;
    [SerializeField]
    MapModel mapModel;
    [SerializeField]
    PlayerController playerController;

    private void Start()
    {
        labyrinthModel.StartedLabyrinth = false;

#if UNITY_EDITOR
        labyrinthModel.Testing = true;
#endif
    }

    public void StartLabyrinth()
    {
        if (labyrinthModel.StartedLabyrinth)
        {
            return;
        }

        labyrinthModel.Entity = labyrinthModel.CurrentSelectedLabyrinth.Entity;
        labyrinthModel.Entity.gameObject.SetActive(true);

        labyrinthModel.StartTime = Time.time;
        labyrinthModel.StartedLabyrinth = true;
        playerController.ToggleTriggerObstacles(true);
        mapModel.RoutingToStart = false;

        MapController.Instance.Deselect();
        MapController.Instance.SpawnInfoPointsOnMap();
        MapController.Instance.SwitchToARMap();

        labyrinthModel.Started();
        ApplicationController.Instance.ActivateLabyrinth();
    }

    public void StopLabyrinth()
    {
        labyrinthModel.StartedLabyrinth = false;
    }

    public void FinishLabyrinth()
    {
        labyrinthModel.StartedLabyrinth = false;
        ApplicationController.Instance.ActivateFinish();
    }

    public void AddBlockToBeRevealed(BlockView block)
    {
        //labyrinthModel.BlocksToBeRevealed.Add(block);
        //labyrinthModel.Entity.SetTarget();
    }

    public void RemovelockToBeRevealed(BlockView block)
    {
        labyrinthModel.BlocksToBeRevealed.Remove(block);
    }

    public void ActivateBlock(BlockView block)
    {
        block.SpawnBlock();
    }

    private void Update()
    {
        TrackTime();
    }

    public void AddTime(float seconds)
    {
        labyrinthModel.StartTime -= seconds;
    }

    private void TrackTime()
    {
        if (!labyrinthModel.StartedLabyrinth)
        {
            return;
        }

        labyrinthModel.ElapsedTime = Time.time - labyrinthModel.StartTime;

        int minutes = (int)(labyrinthModel.ElapsedTime / 60);
        int seconds = (int)(labyrinthModel.ElapsedTime % 60);

        labyrinthModel.TimeText = string.Format("{0:0}:{1:00}", minutes, seconds);
        labyrinthModel.TimerUpdate();
    }

}
