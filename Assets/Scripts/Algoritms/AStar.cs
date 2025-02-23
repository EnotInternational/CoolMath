using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AStar : GreedyBase
{
    public AStar(AlgorithmManager manager) : base(manager){}

    protected override float GetWeight(WeightedVertex current, Vertex next)
    {
        return Heuristic(next) + current.weight + current.vertex.GetWeightWith(next, out Link link);
    }
    private float Heuristic(Vertex vertex)
    {
        // return Vector2.Distance(current.vertex.position, goalVertex.position);
        return Vector2.Distance(vertex.position, goalVertex.position);
    }
}