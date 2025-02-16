using UnityEngine;

public interface ITool
{
    public bool block{get;set;}
    public void Initialize(ToolManager toolManager);
    public void Enable();
    public void Disable();   
    public void Point();   
}
