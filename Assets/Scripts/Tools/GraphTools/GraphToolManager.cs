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
    private SetVertexType<GraphVertex> setStartTool;
    private SetVertexType<GraphVertex> setGoalTool;

    private ToolMachine toolMachine;
    [SerializeField]public Transform gameSpace{get => _gameSpace;}
    [SerializeField]private Transform _gameSpace;
    public AlgorithmManager alogthmManager{get;private set;}
    public Vector3 mousePosition{get => _mousePosition;}
    private Vector3 _mousePosition;
    public UnityEvent OnClickDown;
    public UnityEvent OnClickUp;
    private void Start()
    {
        alogthmManager = GetComponent<AlgorithmManager>();  
        toolMachine = new(this);

        setStartTool = CreateStartPointTool();
        setGoalTool = CreateGoalPointTool();
    }
    #region OnEvents
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
    #endregion
    #region Buttons
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
    private void SetSetPointToStartTool()
    {
        // SetStartEndPointsTool(StartEndPointTool.PointType.Start);
        toolMachine.SetTool(setStartTool);
    }
    [Button]
    private void SetSetPointToGoalTool()
    {
        toolMachine.SetTool(setGoalTool);
    }
    #endregion
    #region PrivateMethods
    private SetVertexType<GraphVertex> CreateStartPointTool()
    {
        return new SetVertexType<GraphVertex>((vertexVisualizer) => 
        {
            vertexVisualizer.vertex.graphState = GraphVertex.GraphState.Start;
            if(alogthmManager.startVertex != null)
            {
                alogthmManager.startVertex.SetCustomStatesToDefault();
            }
            alogthmManager.startVertex = vertexVisualizer.vertex;

        });
    } 
    private SetVertexType<GraphVertex> CreateGoalPointTool()
    {
        return new SetVertexType<GraphVertex>((vertexVisualizer) => 
        {
            vertexVisualizer.vertex.graphState = GraphVertex.GraphState.Goal;
            if(alogthmManager.goalVertex != null)
            {
                alogthmManager.goalVertex.SetCustomStatesToDefault();
            }
            alogthmManager.goalVertex = vertexVisualizer.vertex;

        });
    }
    #endregion
}
