using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    [SerializeField]private List<Link> _links= new List<Link>();
    [SerializeField]public List<Link> Links {get => _links;}
    protected void AddLink(Link link)
    {
        _links.Add(link);
    }
    protected void RemoveLink(Link link)
    {
        _links.Remove(link);
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
    private void OnDestroy()
    {
        for(int i = _links.Count-1; i>=0; i--)
        {
            UnLink(_links[i]);
        }
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
        return LinkTogether(vertexA, vertexB, 1);
    }
    public static Link LinkTogether(Vertex vertexA, Vertex vertexB, float weight)
    {
        Link link = new Link(vertexA, vertexB, weight);
        vertexA.AddLink(link);
        vertexB.AddLink(link);
        return link;
    }
}
