using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Greedy : GreedyBase
{
    public Greedy(AlgorithmManager manager) : base(manager){}

    protected override void GetWeight(WeightedVertex current, Vertex next, out float euristicWeight, out float weight)
    {
        // weight = current.vertex.GetWeightWith(next, out Link link);
        weight = 0;
        euristicWeight = Heuristic(next);
    }
    protected override float Heuristic(Vertex vertex)
    {
        // return Vector2.Distance(current.vertex.position, goalVertex.position);
        return Vector2.Distance(vertex.position, goalVertex.position);
    }
}