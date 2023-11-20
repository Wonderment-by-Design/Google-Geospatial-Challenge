using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveModel : Singleton<InteractiveModel>
{
    public delegate void EventHandler();
    public event EventHandler UpdateCollectionHandler;
    public event EventHandler ShowObjectsCloseHandler;
    public event EventHandler HideObjectsCloseHandler;

    [SerializeField]
    float timeToInteract = 1f;
    int amountCollected = 0;
    int infoSeen = 0;
    float interactionVisualValue;
    GameObject[] objectsInRange;
    private Dictionary<GameObject, float> objectTimers = new Dictionary<GameObject, float>();

    public int AmountCollected { get => amountCollected; set => amountCollected = value; }
    public GameObject[] ObjectsInRange { get => objectsInRange; set => objectsInRange = value; }
    public Dictionary<GameObject, float> ObjectTimers { get => objectTimers; set => objectTimers = value; }
    public float TimeToInteract { get => timeToInteract; set => timeToInteract = value; }
    public int InfoSeen { get => infoSeen; set => infoSeen = value; }
    public float InteractionVisualValue { get => interactionVisualValue; set => interactionVisualValue = value; }

    public void UpdateCollection()
    {
        UpdateCollectionHandler?.Invoke();
    }

    public void ShowObjectsClose()
    {
        ShowObjectsCloseHandler?.Invoke();
    }

    public void HideObjectsClose()
    {
        HideObjectsCloseHandler?.Invoke();
    }
}
