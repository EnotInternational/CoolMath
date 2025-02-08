using System;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GraphToolManager : MonoBehaviour, IToolManager
{
    [SerializeField]private CreatingTool creatingTool;
    [SerializeField]private MoveTool moveTool;
    [SerializeField]private DeleteTool deleteTool;
    [SerializeField]private StartEndPointTool startEndPointTool;
    private ToolMachine toolMachine;
    [SerializeField]public Transform gameSpace{get => _gameSpace;}
    [SerializeField]private Transform _gameSpace;
    private Action _setDefaultToolAction;
    public AlgorithmManager alogthmManager{get;private set;}
    public Vector3 mousePosition{get => _mousePosition;}
    private Vector3 _mousePosition;
    public UnityEvent OnClickDown;
    public UnityEvent OnClickUp;
    private void Start()
    {
        alogthmManager = GetComponent<AlgorithmManager>();  
        toolMachine = new(this);
        _setDefaultToolAction = SetMoveTool;
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
        SetDeleteTool();
    }
    private void OnRightClick(InputValue value)
    {
        // SetMoveTool();
    }
    private void OnClick(InputValue value)
    {
        if(value.isPressed)
            OnClickDown.Invoke();
        else
            OnClickUp.Invoke();
        // SetCreatingTool();
    }
    [Button]
    private void SetCreatingTool()
    {
        toolMachine.SetTool(creatingTool);
    }
    [Button]
    private void SetMoveTool()
    {
        toolMachine.SetTool(moveTool);
    }
    [Button]
    private void SetDeleteTool()
    {
        toolMachine.SetTool(deleteTool);
    }
    [Button]
    public void ChangeSettingPointToStart()
    {
        SetStartEndPointsTool(StartEndPointTool.PointType.Start);
    }
    [Button]
    public void ChangeSettingPointToGoal()
    {
        SetStartEndPointsTool(StartEndPointTool.PointType.Goal);
    }
    private void SetStartEndPointsTool(StartEndPointTool.PointType pointType)
    {
        
        startEndPointTool.OnEndPlacing.AddListener(EndPlacingHandler);
        startEndPointTool.ChangeSettingPoint(pointType);
        toolMachine.SetTool(startEndPointTool);
        
        void EndPlacingHandler()
        {
            _setDefaultToolAction();
            startEndPointTool.OnEndPlacing.RemoveListener(EndPlacingHandler);
        }
    }
}
