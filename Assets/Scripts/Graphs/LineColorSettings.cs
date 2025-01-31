using UnityEngine;

[CreateAssetMenu(fileName = "Vertexes", menuName = "ScriptableObjects/LinkColorSettings", order = 1)]
public class LinkColorSettings : ScriptableObject
{
    public Material standartMaterial;
    public Material seenMaterial;
    public Material pathMaterial;

}