using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class GreedyBase : SearchAlgorithm
{
    [SerializeField] private Dictionary<Vertex, WeightedVertex> seen = new Dictionary<Vertex, WeightedVertex>();
    [SerializeField] private List<Vertex> pathVertexes=new List<Vertex>();
    [SerializeField] private PriorityQueues.MappedBinaryPriorityQueue<WeightedVertex> queueVertexes = 
    new PriorityQueues.MappedBinaryPriorityQueue<WeightedVertex>(new System.Comparison<WeightedVertex>((a, b) => a.overhallWeight.CompareTo(b.overhallWeight)));  
    private WeightedVertex currentVertex;
    public GreedyBase(AlgorithmManager manager) : base(manager){}
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
        if(currentVertex != null)
        {
            currentVertex.vertex.processState = Vertex.ProcessState.NotSeen;
            currentVertex = null;
        }
            
        seen.Clear();
        pathVertexes.Clear();
        queueVertexes.Clear();
    }
    protected override void BeforeFirstIteration()
    {
        WeightedVertex startWeightedVertex = new WeightedVertex(startVertex, null, null, 0, Heuristic(startVertex));
        queueVertexes.Enqueue(startWeightedVertex);
        seen.Add(startVertex, startWeightedVertex);
    }
    protected override void Iterate()
    {
        if(queueVertexes.Count == 0)
        {
            Fail();
            foreach(var pathVertex in pathVertexes)
            {
                pathVertex.processState = Vertex.ProcessState.Seen;
            }
            return;
        }
        
        Link[] otherLinks;

        if(currentVertex != null)
            currentVertex.vertex.processState = Vertex.ProcessState.Seen;
        currentVertex = queueVertexes.Dequeue();
        currentVertex.vertex.processState = Vertex.ProcessState.Seen;
        Vertex[] neighpours = currentVertex.vertex.GetNeighbours(out otherLinks);
        for (int j = 0; j < neighpours.Length; j++)
        {
            Vertex neighbour = neighpours[j];
            Vertex next = neighbour;

            otherLinks[j].state = Link.State.Seen;
            
            GetWeight(currentVertex, next, out float heuristicWeight, out float weight);
            weight = weight + currentVertex.weight;
            WeightedVertex nextWeighted;
            if(seen.Keys.Contains(next))
            {
                WeightedVertex weightedVertex = seen[next];
                if(weightedVertex.overhallWeight < heuristicWeight + weight)
                {
                    continue;
                }
                else
                {
                    weightedVertex.weight = weight;
                    weightedVertex.weightHeuristic = heuristicWeight;
                    if(queueVertexes.Contains(weightedVertex))
                    {
                        continue;
                    }

                    // queueVertexes.Enqueue(weightedVertex); 
                    // seen.Add(next, nextWeighted); 
                    // next.processState = Vertex.ProcessState.Processing;
                    continue;
                }
            }
            nextWeighted = new WeightedVertex(next, currentVertex, currentVertex.vertex.GetLinkWith(next), weight, heuristicWeight);
            queueVertexes.Enqueue(nextWeighted); 
            seen.Add(next, nextWeighted); 
            next.processState = Vertex.ProcessState.Processing;
            
            
        } 
        if(CheckVertex(currentVertex)) return;
        ShowPath(currentVertex);
        currentVertex.vertex.processState = Vertex.ProcessState.Current;
        OnStepComplete.Invoke();
    }
 
    [SerializeField] private bool CheckVertex(WeightedVertex current)
    {
        // current.vertex.processState = Vertex.ProcessState.Processing;

        if (current.vertex==goalVertex) //последний vertex
        {
            Debug.Log("Gool");
            RecursiveReturnToStart(current);
            return true;
        }
        return false;
    }
    private void RecursiveReturnToStart(WeightedVertex vertex)
    {
        ClearPathVertexes(Vertex.ProcessState.Seen, Link.State.Seen);;

        ReturnToStart(vertex);
        
        void ReturnToStart(WeightedVertex vertex)
        {
            pathVertexes.Add(vertex.vertex);

            if(vertex.vertex==startVertex)
            {
                foreach(WeightedVertex processed in queueVertexes)
                {
                    if(processed.link.state == Link.State.Path)
                        continue;
                    processed.vertex.processState = Vertex.ProcessState.NotSeen;
                    processed.link.state = Link.State.NotSeen;
                }
                // vertex.link.state = Link.State.Path;
                Complete(pathVertexes);
            }
            else
            {
                WeightedVertex previous = vertex.origin;
                vertex.link.state = Link.State.Path;
                // queueVertexes.Remove(vertex);
                ReturnToStart(previous);
            }
        }
    }
    private void ShowPath(WeightedVertex weightedVertex)
    {
        ClearPathVertexes(Vertex.ProcessState.Seen, Link.State.Seen);

        RecursiveReturn(weightedVertex);
        void RecursiveReturn(WeightedVertex weightedVertex)
        {
            Vertex currentVertex = weightedVertex.vertex;
            pathVertexes.Add(currentVertex);


            

            if(currentVertex==startVertex)
            {
                return;
            }

            currentVertex.GetLinkWith(weightedVertex.origin.vertex).state = Link.State.Path;
            currentVertex.processState = Vertex.ProcessState.Path;

            RecursiveReturn(weightedVertex.origin);
        }
    }
    private void ClearPathVertexes(Vertex.ProcessState vertexState, Link.State linkState)
    {
        if(pathVertexes.Count == 0)
            return;
        Vertex pathVertex = pathVertexes[0];
        pathVertex.processState = vertexState;
        for (int i = 1; i < pathVertexes.Count; i++)
        {
            pathVertex = pathVertexes[i];
            pathVertex.processState = vertexState;
            pathVertex.GetLinkWith(pathVertexes[i-1]).state = linkState;
        }
        pathVertexes.Clear();
    }
    protected abstract void GetWeight(WeightedVertex current, Vertex next, out float euristicWeight, out float weight);
    protected virtual float Heuristic(Vertex current)
    {
        return 0;
    }

    protected class WeightedVertex
    {
        public Vertex vertex;
        public WeightedVertex origin;
        public Link link;
        public float weight;
        public float weightHeuristic;
        public float overhallWeight
        {
            get => weight + weightHeuristic;
        }
        public WeightedVertex(Vertex vertex, WeightedVertex origin, Link link, float weight, float weightHeuristic)
        {
            this.vertex = vertex;
            this.origin = origin;
            this.link = link;
            this.weight = weight;
            this.weightHeuristic = weightHeuristic;
        }
    }
}
