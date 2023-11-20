using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthModel : Singleton<LabyrinthModel>
{

    public delegate void EventHandler();
    public event EventHandler ToggleStartHandler;
    public event EventHandler StartedHandler;
    public event EventHandler UpdateTimerHandler;

    [SerializeField]
    Camera mainCamera;
    float spawnInDistance;
    [SerializeField]
    float startDistance;
    [SerializeField]
    float finishDistance;
    double minHorizontalAccuracy = 10;
    [SerializeField]
    GameObject entity;
    bool testing = false;
    [SerializeField]
    List<LabyrinthView> labyrinths = new List<LabyrinthView>();
    LabyrinthView currentSelectedLabyrinth;

    bool allowedToStart = false;
    bool startedLabyrinth;
    float startTime;
    float elapsedTime;
    string timeText;
    bool insideWall;
    List<BlockView> blocksToBeRevealed = new List<BlockView>();

    public Camera MainCamera { get => mainCamera; set => mainCamera = value; }
    public float SpawnInDistance { get => spawnInDistance; set => spawnInDistance = value; }
    public bool InsideWall { get => insideWall; set => insideWall = value; }
    public List<BlockView> BlocksToBeRevealed { get => blocksToBeRevealed; set => blocksToBeRevealed = value; }
    public GameObject Entity { get => entity; set => entity = value; }
    public float StartDistance { get => startDistance; set => startDistance = value; }
    public float StartTime { get => startTime; set => startTime = value; }
    public float ElapsedTime { get => elapsedTime; set => elapsedTime = value; }
    public bool StartedLabyrinth { get => startedLabyrinth; set => startedLabyrinth = value; }
    public string TimeText { get => timeText; set => timeText = value; }
    public float FinishDistance { get => finishDistance; set => finishDistance = value; }
    public List<LabyrinthView> Labyrinths { get => labyrinths; set => labyrinths = value; }
    public LabyrinthView CurrentSelectedLabyrinth { get => currentSelectedLabyrinth; set => currentSelectedLabyrinth = value; }
    public double MinHorizontalAccuracy { get => minHorizontalAccuracy; set => minHorizontalAccuracy = value; }
    public bool Testing { get => testing; set => testing = value; }
    public bool AllowedToStart { get => allowedToStart; set => allowedToStart = value; }

    public void Started()
    {
        StartedHandler?.Invoke();
    }

    public void ToggleStart()
    {
        ToggleStartHandler?.Invoke();
    }

    public void TimerUpdate()
    {
        UpdateTimerHandler?.Invoke();
    }
}
