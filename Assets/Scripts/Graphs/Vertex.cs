using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class Vertex
{
    public ProcessState processState
    {
        get => _processState;
        set
        {
            _processState = value;
            OnProcessStateChanged.Invoke(_processState);
        }
    }
    public Vector2 position
    {
        get => _position;
        set
        {
            _position = value;
            UpdateAllLinks();
        }  
    }
    private Vector2 _position;
    [SerializeField]protected List<Link> _links= new List<Link>();
    private ProcessState _processState;
    public UnityEvent<ProcessState> OnProcessStateChanged = new();
    public UnityEvent OnCustomStateChanged = new();
    
    public List<Link> Links {get => _links;}
    public (Link, Vertex)[] GetNeighbours()
    {
        (Link, Vertex)[] vertices = new (Link, Vertex)[_links.Count];
        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = (_links[i], _links[i].GetOther(this));
        }
        return vertices;
    }
    public virtual (Link, Vertex, float)[] GetNeighboursWithWeights()
    {
        (Link, Vertex, float)[] vertices = new (Link, Vertex, float)[_links.Count];
        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = (_links[i],_links[i].GetOther(this), _links[i].weight);
        }
        return vertices;
    }
    
    public void UpdateAllLinks()
    {
        foreach (Link link in _links)
        {
            link.OnChanged.Invoke();
        }
    }
    public bool HasLinkWith(Vertex vertex)
    {
        if(GetLinkWith(vertex) == null)
            return false;
        return true;
    }
    public Link GetLinkWith(Vertex vertex)
    {
        foreach(Link link in _links)
        {
            if(link.VertexA == vertex || link.VertexB == vertex)
            {
                return link;
            }
        }
        return null;
    }
    public void UnlinkAll()
    {
        for(int i = _links.Count-1; i>=0; i--)
        {
            UnLink(_links[i]);
        }
    }
    // [SerializeField]private Sprite
    protected void AddLink(Link link)
    {
        _links.Add(link);
    }
    public abstract void SetCustomStatesToDefault();
    protected void RemoveLink(Link link)
    {
        _links.Remove(link);
    }

    public static void UnLink(Vertex vertexA, Vertex vertexB)
    {
        Link link = vertexA.GetLinkWith(vertexB);
        if(link == null)
            return;
        UnLink(link);
    }
    public static void UnLink(Link link)
    {
        link.OnUnlink.Invoke();
        link.VertexA.RemoveLink(link);
        link.VertexB.RemoveLink(link);
    }
    public static Link LinkTogether(Vertex vertexA, Vertex vertexB)
    {
        Link link = new Link(vertexA, vertexB);
        vertexA.AddLink(link);
        vertexB.AddLink(link);
        return link;
    }
    public enum ProcessState{NotSeen, Seen, Processing, Path}
}
