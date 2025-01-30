using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Link
{
    [SerializeField]private float _weight;
    public UnityEvent OnUnlink = new();
    public UnityEvent OnChanged = new();
    public float Weight
    {
        get { return _weight; }
        set { _weight = value; }
    }
    [SerializeField]private Vertex _vertexA;
    [SerializeField]private Vertex _vertexB;
    public Vertex VertexA{get => _vertexA;}
    public Vertex VertexB{get => _vertexB;}

    public Vertex GetOther(Vertex vertex)
    {
        if(vertex == _vertexA)
            return _vertexB;
        else
            return _vertexA;
    }
    public Link(Vertex vertexA, Vertex vertexB)
    {
        this._weight = 1;
        this._vertexA = vertexA;
        this._vertexB = vertexB;
    }
    public Link(Vertex vertexA, Vertex vertexB, float weight)
    {
        this._weight = weight;
        this._vertexA = vertexA;
        this._vertexB = vertexB;
    }
}
