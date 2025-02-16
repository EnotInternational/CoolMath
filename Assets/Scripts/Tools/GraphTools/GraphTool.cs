using UnityEngine;

public abstract class GraphTool : ITool
{
    [HideInInspector]public GraphToolManager manager = null;

    public bool block { get; set; }

    public virtual void Initialize(ToolManager manager)
    {
        if(this.manager != null)
            return;
        this.manager = manager as GraphToolManager;
    }
    public abstract void Disable();
    public abstract void Enable();
    public abstract void Point();
}