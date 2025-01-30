using UnityEngine;

public class ToolMachine<T, M> where T : ITool where M : IToolManager
{
    private M _manager;
    public ToolMachine(M manager)
    {
        _manager = manager;
    }
    public T currentTool{get; private set;}
    public virtual void SetTool(T tool)
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
