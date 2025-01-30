using UnityEngine;

public interface ITool
{
    public void Initialize(IToolManager toolManager);
    public void Enable();
    public void Disable();   
    public void Point();   
}
