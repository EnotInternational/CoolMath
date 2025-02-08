using UnityEngine;

public class ToolMachine
{
    private IToolManager _manager;
    public ToolMachine(IToolManager manager)
    {
        _manager = manager;
    }
    public ITool currentTool{get; private set;}
    public virtual void SetTool(ITool tool)
    {
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
