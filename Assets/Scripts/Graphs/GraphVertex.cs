using UnityEngine;
using UnityEngine.Events;

public class GraphVertex : Vertex
{
    public GraphState graphState
    {
        get => _graphState;
        set
        {
            _graphState = value;
            OnGraphStateChanged.Invoke(_graphState);
            OnCustomStateChanged.Invoke();
        }
    }
    private GraphState _graphState = GraphState.Common;
    public UnityEvent<GraphState> OnGraphStateChanged = new();
    public override void SetCustomStatesToDefault()
    {
        graphState = GraphState.Common;
    }
    public enum GraphState{Start, Goal, Common}
}