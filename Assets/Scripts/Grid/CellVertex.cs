using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CellVertex : Vertex
{
    private float _weight = 1;
    public float weight { get => _weight; set => _weight = value; }
    public CellState cellState
    {
        get => _cellState;
        set
        {
            _cellState = value;
            OnCellStateChanged.Invoke(cellState);
            OnCustomStateChanged.Invoke();
        }
    }
    private CellState _cellState = CellState.Common;
    public bool HasDiagonals = true;
    public UnityEvent<CellState> OnCellStateChanged = new();
    public override (Link, Vertex, float)[] GetNeighboursWithWeights()
    {
        (Link, Vertex, float)[] vertices = new (Link, Vertex, float)[_links.Count];

        for(int i = 0; i < vertices.Length; i++)
        {
            CellVertex other = (CellVertex)_links[i].GetOther(this);
            vertices[i] = (_links[i], other, other.weight * _links[i].weight);
        }
        return vertices;
    }
    public override float GetWeightWith(Vertex other, out Link link)
    {
        link = GetLinkWith(other);
        return ((CellVertex)other).weight * link.weight;
    }
    public enum CellState{Start, Goal, Common, Weighted, Blocked}

    public override void SetCustomStatesToDefault()
    {
        cellState = CellState.Common;
    }
}