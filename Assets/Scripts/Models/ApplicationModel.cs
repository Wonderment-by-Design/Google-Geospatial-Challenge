using UnityEngine;

public class ApplicationModel : Singleton<ApplicationModel>
{
    public delegate void EventHandler();
    public event EventHandler ChangeStateHandler;

    public void ChangeState()
    {
        ChangeStateHandler?.Invoke();
    }

    private ApplicationState currentState;
    private ApplicationState previousState;

    public ApplicationState CurrentState { get => currentState; set => currentState = value; }
    public ApplicationState PreviousState { get => previousState; set => previousState = value; }
}