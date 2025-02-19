using UnityEngine;

public class ToolMachine
{
    private ToolManager _manager;
    public ToolMachine(ToolManager manager)
    {
        _manager = manager;
    }
    public ITool currentTool{get; private set;}
    public virtual void SetTool(ITool tool)
    {
        // Debug.Log("Changing to " + tool);
        if(currentTool!=null)
        {
            currentTool.Disable();
        }

        if(tool == null)
        {
            currentTool = tool;  
            return;
        }
        
        tool.Initialize(_manager);
        tool.Enable();
        currentTool = tool;
    }
}
