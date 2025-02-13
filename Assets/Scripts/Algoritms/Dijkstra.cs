using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Dijkstra : SearchAlgorithm
{
    [SerializeField] private List<Vertex> pathVertexes=new List<Vertex>();
    // [SerializeField] private List<Vertex> processedInPreviousStep = new List<Vertex>();
    [SerializeField] private LinkedList<WeightedVertex> potentialVertexes = new();  
    [SerializeField] private LinkedList<WeightedVertex> seenVertexes = new();  
    
    public Dijkstra(AlgorithmManager manager) : base(manager){}
    protected override void BeforeFirstIteration()
    {
        var startWeightedVertex = new WeightedVertex(startVertex, null, 0);
        potentialVertexes.AddFirst(startWeightedVertex);
        seenVertexes.AddFirst(startWeightedVertex);
    }
    protected override void Iterate()
    {
        if(potentialVertexes.First == null)
        {
            Fail();
        }
        // foreach(Vertex v in processedInPreviousStep)
        // {
        //     v.processState = Vertex.ProcessState.Seen;
        // }
        // processedInPreviousStep.Clear();
        
        var currentVertex = GetMinimalPotentialVertex();
        Debug.Log("Processing " + currentVertex.vertex);
        // Debug.
        var neighbourInfos = currentVertex.vertex.GetNeighboursWithWeights();
        // processedInPreviousStep.Add(currentVertex.vertex);
        foreach(var neighbour in neighbourInfos)
        {
            WeightedVertex neighbourWeightedVertex = FindWeightedWertex(neighbour.Item2);
            if(neighbourWeightedVertex != null)
            {
                if(neighbourWeightedVertex.weight > currentVertex.weight + neighbour.Item3)
                {
                    neighbourWeightedVertex.weight = currentVertex.weight + neighbour.Item3;
                    neighbourWeightedVertex.origin = currentVertex;
                }
                else
                {
                    // potentialVertexes.Remove(neighbourWeightedVertex);
                    // neighbourWeightedVertex.vertex.processState = Vertex.ProcessState.Seen;
                }
            }
            else
            {
                neighbourWeightedVertex = new WeightedVertex(neighbour.Item2, currentVertex, currentVertex.weight + neighbour.Item3);
                seenVertexes.AddFirst(neighbourWeightedVertex);
                potentialVertexes.AddFirst(neighbourWeightedVertex);
                neighbourWeightedVertex.vertex.processState = Vertex.ProcessState.Processing;
                if(neighbourWeightedVertex.vertex == goalVertex)
                {
                    RecursiveReturnToStart(neighbourWeightedVertex);
                }
            }
            
        }
        
        OnStepComplete.Invoke();
    }
    private WeightedVertex FindWeightedWertex(Vertex vertex)
    {
        foreach(var seenVertex in seenVertexes)
        {
            if(seenVertex.vertex == vertex)
            return seenVertex;
        }
        return null;
    }

    private WeightedVertex GetMinimalPotentialVertex()
    {
        if(potentialVertexes.First == null)
        {
            Fail();
            return null;
        }
        WeightedVertex min = potentialVertexes.First.Value;
        
        // potentialVertexes.
        foreach(var vertex in potentialVertexes)
        {
            if(vertex.weight < min.weight)
            {
                min = vertex;
            }
        }
        potentialVertexes.Remove(min);
        min.vertex.processState = Vertex.ProcessState.Seen;
        return min;
    }
    void RecursiveReturnToStart(WeightedVertex weightedVertex)
    {
        Vertex currentVertex = weightedVertex.vertex;
        pathVertexes.Add(currentVertex);

        if(currentVertex==startVertex)
        {
            Complete(pathVertexes);
            foreach(var potential in potentialVertexes)
        {
                potential.vertex.processState = Vertex.ProcessState.Seen;
            }
        }
        else
        {
            currentVertex.GetLinkWith(weightedVertex.origin.vertex).SetPath();
            RecursiveReturnToStart(weightedVertex.origin);
        }
    }
    // private bool CheckVertex(Vertex current, Vertex privous)
    // {
    //     // vertexHistory.Add(current,privous);
    //     processedInPreviousStep.Add(current);
    //     current.processState = Vertex.ProcessState.Processing;

    //     if (current==goalVertex) //последний vertex
    //     {
    //         RecursiveReturnToStart(current);
    //         return true;
    //     }
    //     return false;
    // }
    // void RecursiveReturnToStart(Vertex vertex)
    // {
    //     pathVertexes.Add(vertex);

    //     if(vertex==startVertex)
    //     {
    //         Complete(pathVertexes);
    //         foreach(Vertex processed in processedInPreviousStep)
    //         {
    //             processed.processState = Vertex.ProcessState.Seen;
    //         }
    //     }
    //     else
    //     {
    //         // Vertex previous = vertexHistory[vertex];
    //         vertex.GetLinkWith(previous).SetPath();
    //         RecursiveReturnToStart(previous);
    //     }
    // }
    private class WeightedVertex
    {
        public Vertex vertex;
        public WeightedVertex origin;
        public float weight;
        public WeightedVertex(Vertex vertex, WeightedVertex origin, float weight)
        {
            this.vertex = vertex;
            this.origin = origin;
            this.weight = weight;
        }
    }
}