using UnityEngine;

[CreateAssetMenu(fileName = "Vertexes", menuName = "ScriptableObjects/VertexColorSettings", order = 1)]
public class VertexColorSettings : ScriptableObject
{
    public Color unseenColor;
    public Color pathColor;
    public Color processedColor;
    public Color startColor;
    public Color goalColor;
    public Color seenColor;
}