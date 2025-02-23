using UnityEngine;

[CreateAssetMenu(fileName = "Vertexes", menuName = "ScriptableObjects/VertexColorSettings", order = 1)]
public class VertexColorSettings : ScriptableObject
{
    [Header("Process state colors")]
    [SerializeField]private Color unseenColor;
    [SerializeField]private Color seenColor;
    [SerializeField]private Color pathColor;
    [SerializeField]private Color currentColor;
    [SerializeField]private Color processedColor;

    [Header("Common state colors")]
    [SerializeField]private Color startColor;
    [SerializeField]private Color goalColor;

    [Header("Cell state colors")]
    [SerializeField]private Color blockedColor;
    [SerializeField]private Gradient weightGradient;
    // [SerializeField]private Color 

    public Color GetProcessColor(Vertex.ProcessState state)
    {
        switch (state)
        {
            case Vertex.ProcessState.Processing:
                return processedColor;
            case Vertex.ProcessState.Seen:
                return seenColor;
            case Vertex.ProcessState.NotSeen:
                return unseenColor; 
            case Vertex.ProcessState.Path:
                return pathColor; 
            case Vertex.ProcessState.Current:
                return currentColor; 
            default:
                return Color.black;
        }
    }
    public Color GetCellColor(CellVertex.CellState state)
    {
        switch (state)
        {
            case CellVertex.CellState.Common:
                return unseenColor;
            case CellVertex.CellState.Start:
                return startColor;
            case CellVertex.CellState.Goal:
                return goalColor;
            case CellVertex.CellState.Blocked:
                return blockedColor;
            case CellVertex.CellState.Weighted:
                return unseenColor;
            default:
                return Color.black;
        }
    }
    public Color GetColorFromWeightGradient(float value)
    {
        return weightGradient.Evaluate(value);
    }
    public Color GetGraphColor(GraphVertex.GraphState state)
    {
        switch (state)
        {
            case GraphVertex.GraphState.Common:
                return unseenColor;
            case GraphVertex.GraphState.Start:
                return startColor;
            case GraphVertex.GraphState.Goal:
                return goalColor;
            default:
                return Color.black;
        }
    }
}