using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    [SerializeField]private List<Link> _links= new List<Link>();
    public void AddLink(Link link)
    {
        _links.Add(link);
    }

    private void OnDrawGizmos()
    {
        foreach(Link link in _links)
        {
            Gizmos.DrawLine(link.VertexA.transform.position, link.VertexB.transform.position);
        }
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
