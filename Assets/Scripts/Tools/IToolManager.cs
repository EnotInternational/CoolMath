using UnityEngine;

public abstract class ToolManager : MonoBehaviour
{
    protected ToolMachine toolMachine;
    protected AlgorithmManager alogthmManager;
    protected ITool previousTool;
    public abstract void Disable();
    protected virtual void OnEnable()
    {
        alogthmManager.OnProcessChanged.AddListener(ProcessChangedHandler);
    }
    protected virtual void OnDisable()
    {
        alogthmManager.OnProcessChanged.RemoveListener(ProcessChangedHandler);
    }
    public abstract void ResetField();
    private void ProcessChangedHandler(bool processing)
    {
        if(processing)
        {
            DeselectTools();
        } 
        else
        {
            SelectpreviousTool();
        }
    }
    private void DeselectTools()
    {
        previousTool = toolMachine.currentTool;
        toolMachine.SetTool(null);
    }
    private void SelectpreviousTool()
    {
        toolMachine.SetTool(previousTool);
    }
}