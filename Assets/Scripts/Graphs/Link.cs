using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Link
{
    [SerializeField]private float _weight = 1;
    [HideInInspector]public UnityEvent OnUnlink = new();
    [HideInInspector]public UnityEvent OnChanged = new();
    [HideInInspector]public UnityEvent<State> OnStateChanged = new();
    [SerializeField]private State _state;
    public State state 
    { 
        get => _state; 
        set 
        {
            _state = value;
            OnStateChanged.Invoke(_state);
        }
    }
    public float weight
    {
        get 
        { 
            return _weight; 
        }
        set 
        { 
            _weight = value; 
            OnWeightChanged.Invoke(_weight);
        }
    }
    [SerializeField]private Vertex _vertexA;
    [SerializeField]private Vertex _vertexB;
    public Vertex VertexA{get => _vertexA;}
    public Vertex VertexB{get => _vertexB;}
    public UnityEvent<float> OnWeightChanged = new UnityEvent<float>();

    public Vertex GetOther(Vertex vertex)
    {
        if(vertex == _vertexA)
            return _vertexB;
        else
            return _vertexA;
    }
    public Link(Vertex vertexA, Vertex vertexB)
    {
        this._vertexA = vertexA;
        this._vertexB = vertexB;
    }
    public enum State{NotSeen, Seen, Path};
}
