using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AStar : SearchAlgorithm
{
    [SerializeField] private Dictionary<Vertex, WeightedVertex> seen = new Dictionary<Vertex, WeightedVertex>();
    [SerializeField] private List<Vertex> pathVertexes=new List<Vertex>();
    [SerializeField] private PriorityQueues.MappedBinaryPriorityQueue<WeightedVertex> queueVertexes = 
    new PriorityQueues.MappedBinaryPriorityQueue<WeightedVertex>(new System.Comparison<WeightedVertex>((a, b) => a.weight.CompareTo(b.weight)));  

    public AStar(AlgorithmManager manager) : base(manager){}
    public override void Clear()
    {
        foreach (var vertex in seen.Keys)
        {
            vertex.processState = Vertex.ProcessState.NotSeen;
            foreach(var link in vertex.Links)
            {
                link.state = Link.State.NotSeen;
            }
        }
        seen.Clear();
        pathVertexes.Clear();
        queueVertexes.Clear();
    }
    protected override void BeforeFirstIteration()
    {
        WeightedVertex startWeightedVertex = new WeightedVertex(startVertex, null, null, 0f);
        queueVertexes.Enqueue(startWeightedVertex);
        seen.Add(startVertex, startWeightedVertex);
    }
    protected override void Iterate()
    {
        if(queueVertexes.Count == 0)
        {
            Fail();
        }
        
        Link[] otherLinks;

        WeightedVertex current = queueVertexes.Dequeue();
        current.vertex.processState = Vertex.ProcessState.Seen;
        Vertex[] neighpours = current.vertex.GetNeighbours(out otherLinks);
        for (int j = 0; j < neighpours.Length; j++)
        {
            Vertex neighbour = neighpours[j];
            Vertex next = neighbour;

            otherLinks[j].state = Link.State.Seen;

            float newWeight = Heuristic(next) + current.weight + current.vertex.GetWeightWith(next, out Link link);
            WeightedVertex nextWeighted;
            if(seen.Keys.Contains(next))
            {
                if(seen[next].weight < newWeight)
                {
                    continue;
                }
                else
                {
                    nextWeighted = new WeightedVertex(next, current, link, newWeight);
                    queueVertexes.Enqueue(nextWeighted); 
                    // seen.Add(next, nextWeighted); 
                    next.processState = Vertex.ProcessState.Processing;
                    continue;
                }
            }

            nextWeighted = new WeightedVertex(next, current, link, newWeight);
            queueVertexes.Enqueue(nextWeighted); 
            seen.Add(next, nextWeighted); 
            next.processState = Vertex.ProcessState.Processing;
            
            
            if(CheckVertex(nextWeighted,current)) return;
        } 

        OnStepComplete.Invoke();
    }
 
    [SerializeField] private bool CheckVertex(WeightedVertex current, WeightedVertex privous)
    {
        // current.vertex.processState = Vertex.ProcessState.Processing;

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
            foreach(WeightedVertex processed in queueVertexes)
            {
                processed.vertex.processState = Vertex.ProcessState.Seen;
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
