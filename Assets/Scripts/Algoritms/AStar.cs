using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AStar : GreedyBase
{
    public AStar(AlgorithmManager manager) : base(manager){}

    protected override void GetWeight(WeightedVertex current, Vertex next, out float euristicWeight, out float weight)
    {
        weight = current.vertex.GetWeightWith(next, out Link link);
        euristicWeight = Heuristic(next);
    }
    protected override float Heuristic(Vertex vertex)
    {
        // return Vector2.Distance(current.vertex.position, goalVertex.position);
        return Vector2.Distance(vertex.position, goalVertex.position);
        // return Mathf.Abs(vertex.position.x - goalVertex.position.x ) +  Mathf.Abs(vertex.position.y - goalVertex.position.y );
    }
}