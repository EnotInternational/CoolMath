using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BFS : MonoBehaviour
{
  [SerializeField] private Vertex current;
  [SerializeField] private Vertex previous;
  [SerializeField] private Dictionary <Vertex,Vertex> vertexHistory = new Dictionary<Vertex,Vertex>();
  [SerializeField] private List<Vertex>pathVertexes=new List<Vertex>();
    [SerializeField] private Queue<Vertex> queueVertexes = new Queue<Vertex>();  
  public Vertex goalVertex;
  public Vertex startVertex;
  public bool InProcess; 

    [SerializeField]private void Serch()
    {

        foreach(Link link in startVertex.Links)
        {
            SerchVertex(link.GetOther(startVertex), startVertex);
            queueVertexes.Enqueue(link.GetOther(startVertex));  
        }

        while (InProcess)
        {
            int vertexCount= queueVertexes.Count;
            for(int i=0;i<vertexCount;i++)
            {
                Vertex q=queueVertexes.Dequeue();
                    foreach(Link link in q.Links)
                {
                SerchVertex(link.GetOther(q),q);
                queueVertexes.Enqueue(link.GetOther(q));  
                } 
            } 
        }
    
    }
 
    [SerializeField] private void  SerchVertex(Vertex current, Vertex privous)
    {
      if(privous!=null || current!=null)//все vertex
      {
        vertexHistory.Add(current,previous);
      }
      else if (current==goalVertex ) //последний vertex
      {
        vertexHistory.Add(null,previous);
        RecursiveReturnToStart(current);
      }
      else if (current=null) //первый vertex 
      {
        vertexHistory.Add(current,null);
      }
    }
     void RecursiveReturnToStart(Vertex vertex)
    {
      pathVertexes.Add(vertex);

      if(vertex==startVertex)
      {
        //Конец алгоритма
      }
      else
      {
        Vertex previous = vertexHistory[vertex];
        RecursiveReturnToStart(previous);
      }
    }
}
