using System;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GraphToolManager : ToolManager
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
    private void Awake()
    {
        alogthmManager = GetComponent<AlgorithmManager>();  
        toolMachine = new(this);

        setStartTool = CreateStartPointTool();
        setGoalTool = CreateGoalPointTool();
        creatingTool.Initialize(this);
        deleteTool.Initialize(this);
    }
    #region OnEvents
    private void OnDoubleClick()
    {
        if(!enabled)
            return;
        creatingTool.CreateSeparateVertex();
    }
    private void OnPoint(InputValue value)
    {
        if(toolMachine.currentTool != null)
            toolMachine.currentTool.Point();
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
