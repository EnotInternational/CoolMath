using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Greedy : SearchAlgorithm
{
    [SerializeField] private List<Vertex> processedInPreviousStep = new List<Vertex>();
    [SerializeField] private List<Vertex> seen = new List<Vertex>();
    [SerializeField] private List<Vertex> pathVertexes=new List<Vertex>();
    [SerializeField] private PriorityQueues.MappedBinaryPriorityQueue<WeightedVertex> queueVertexes = 
    new PriorityQueues.MappedBinaryPriorityQueue<WeightedVertex>(new System.Comparison<WeightedVertex>((a, b) => a.weight.CompareTo(b.weight)));  

    public Greedy(AlgorithmManager manager) : base(manager){}
    public override void Clear()
    {
        foreach (var vertex in seen)
        {
            vertex.processState = Vertex.ProcessState.NotSeen;
            foreach(var link in vertex.Links)
            {
                link.state = Link.State.NotSeen;
            }
        }
        seen.Clear();
        processedInPreviousStep.Clear();
        pathVertexes.Clear();
        queueVertexes.Clear();
    }
    protected override void BeforeFirstIteration()
    {
        queueVertexes.Enqueue(new WeightedVertex(startVertex, null, null, 0f));
        seen.Add(startVertex);
    }
    protected override void Iterate()
    {
        if(queueVertexes.Count == 0)
        {
            Fail();
        }
        foreach(Vertex v in processedInPreviousStep)
        {
            v.processState = Vertex.ProcessState.Seen;
        }
        processedInPreviousStep.Clear();
        Link[] otherLinks;

        WeightedVertex current = queueVertexes.Dequeue();
        Vertex[] neighpours = current.vertex.GetNeighbours(out otherLinks);
        for (int j = 0; j < neighpours.Length; j++)
        {
            Vertex neighbour = neighpours[j];
            Vertex next = neighbour;

            otherLinks[j].state = Link.State.Seen;

            if(seen.Contains(next))
                continue;

            float weight = Heuristic(next);
            WeightedVertex nextWeighted = new WeightedVertex(next, current, next.GetLinkWith(current.vertex), weight);
            queueVertexes.Enqueue(nextWeighted); 
            seen.Add(next); 
            
            if(CheckVertex(nextWeighted,current)) return;
        } 

        OnStepComplete.Invoke();
    }
 
    [SerializeField] private bool CheckVertex(WeightedVertex current, WeightedVertex privous)
    {
        processedInPreviousStep.Add(current.vertex);
        current.vertex.processState = Vertex.ProcessState.Processing;

        if (current.vertex==goalVertex) //последний vertex
        {
            RecursiveReturnToStart(current);
            return true;
        }
        return false;
    }
    private void RecursiveReturnToStart(WeightedVertex vertex)
    {
        pathVertexes.Add(vertex.vertex);

        if(vertex.vertex==startVertex)
        {
            Complete(pathVertexes);
            foreach(Vertex processed in processedInPreviousStep)
            {
                processed.processState = Vertex.ProcessState.Seen;
            }
        }
        else
        {
            WeightedVertex previous = vertex.origin;
            vertex.link.state = Link.State.Path;
            RecursiveReturnToStart(previous);
        }
    }
    private float Heuristic(Vertex vertex)
    {
        return Vector2.Distance(vertex.position, goalVertex.position);
    }
    private class WeightedVertex
    {
        public Vertex vertex;
        public WeightedVertex origin;
        public Link link;
        public float weight;
        public WeightedVertex(Vertex vertex, WeightedVertex origin, Link link, float weight)
        {
            this.vertex = vertex;
            this.origin = origin;
            this.link = link;
            this.weight = weight;
        }
    }
}
