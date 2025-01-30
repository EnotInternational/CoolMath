using UnityEngine;

public abstract class GraphTool : ITool
{
    [HideInInspector]public GraphToolManager manager = null;
    public virtual void Initialize(IToolManager manager)
    {
        if(this.manager != null)
            return;
        this.manager = manager as GraphToolManager;
    }
    public abstract void Disable();
    public abstract void Enable();
    public abstract void Point();
}