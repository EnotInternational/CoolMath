using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BFS : SearchAlgorithm
{
    [SerializeField] private Dictionary <Vertex,Vertex> vertexHistory = new Dictionary<Vertex,Vertex>();
    [SerializeField] private List<Vertex> processedInPreviousStep = new List<Vertex>();
    [SerializeField] private List<Vertex>pathVertexes=new List<Vertex>();
    [SerializeField] private Queue<Vertex> queueVertexes = new Queue<Vertex>();  

    public BFS(AlgorithmManager manager) : base(manager){}
    protected override void BeforeFirstIteration()
    {
        queueVertexes.Enqueue(startVertex);
    }
    protected override void Iterate()
    {
        int vertexCount= queueVertexes.Count;
        if(vertexCount == 0)
        {
            Fail();
        }
        foreach(Vertex v in processedInPreviousStep)
        {
            v.vertexVisualizer.SetSeen();
        }
        processedInPreviousStep.Clear();
        for(int i=0;i<vertexCount;i++)
        {
            Vertex current = queueVertexes.Dequeue();
            foreach(Link link in current.Links)
            {
                Vertex next = link.GetOther(current);

                link.SetSeen();

                if(vertexHistory.ContainsKey(next))
                    continue;
                
                queueVertexes.Enqueue(next);  
                if(CheckVertex(next,current)) return;
            } 
        } 
        OnStepComplete.Invoke();
    }
 
    [SerializeField] private bool CheckVertex(Vertex current, Vertex privous)
    {
        vertexHistory.Add(current,privous);
        processedInPreviousStep.Add(current);
        current.vertexVisualizer.SetProcessed();

        if (current==goalVertex) //последний vertex
        {
            RecursiveReturnToStart(current);
            return true;
        }
        return false;
    }
     void RecursiveReturnToStart(Vertex vertex)
    {
        pathVertexes.Add(vertex);

        if(vertex==startVertex)
        {
            Complete(pathVertexes);
            foreach(Vertex processed in processedInPreviousStep)
            {
                processed.vertexVisualizer.SetSeen();
            }
        }
        else
        {
            Vertex previous = vertexHistory[vertex];
            vertex.GetLinkWith(previous).SetPath();
            RecursiveReturnToStart(previous);
        }
    }
}
