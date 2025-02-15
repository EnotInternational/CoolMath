using UnityEngine;

public interface ITool
{
    public void Initialize(ToolManager toolManager);
    public void Enable();
    public void Disable();   
    public void Point();   
}
