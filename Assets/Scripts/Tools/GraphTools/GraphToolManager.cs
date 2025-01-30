using UnityEngine;
using UnityEngine.InputSystem;

public class GraphToolManager : MonoBehaviour, IToolManager
{
    [SerializeField]private CreatingTool creatingTool;
    [SerializeField]private MoveTool moveTool;
    [SerializeField]private DeleteTool deleteTool;
    private ToolMachine<ITool, IToolManager> toolMachine;
    [SerializeField]public Transform gameSpace{get => _gameSpace;}
    [SerializeField]private Transform _gameSpace;
    public Vector3 mousePosition{get => _mousePosition;}
    private Vector3 _mousePosition;
    private void Start()
    {
        toolMachine = new(this);
    }
    private void OnPoint(InputValue value)
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
        _mousePosition.z = _gameSpace.position.z;

        if(toolMachine.currentTool != null)
            toolMachine.currentTool.Point();
    }
    private void OnDoubleClick(InputValue value)
    {
        creatingTool.CreateSeparateVertex();
    }
    private void OnDelete(InputValue value)
    {
        if(value.isPressed)
        {
            toolMachine.SetTool(deleteTool);
        }
        else
        {
            toolMachine.SetTool(null);
        }
    }
    private void OnRightClick(InputValue value)
    {
        if(value.isPressed)
        {
            toolMachine.SetTool(moveTool);
        }
        else
        {
            toolMachine.SetTool(null);
        }
    }
    private void OnClick(InputValue value)
    {

        if(value.isPressed)
        {
            toolMachine.SetTool(creatingTool);
        }
        else
        {
            toolMachine.SetTool(null);
        }
    }
}
