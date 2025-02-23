using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Greedy : GreedyBase
{
    public Greedy(AlgorithmManager manager) : base(manager){}

    protected override float GetWeight(WeightedVertex current, Vertex next)
    {
        return Heuristic(next);
    }
    private float Heuristic(Vertex vertex)
    {
        // return Vector2.Distance(current.vertex.position, goalVertex.position);
        return Vector2.Distance(vertex.position, goalVertex.position);
    }
}