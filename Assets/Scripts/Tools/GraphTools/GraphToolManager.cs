using System;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GraphToolManager : ToolManager
{
    [SerializeField]private CreatingTool creatingTool;
    [SerializeField]private MoveTool moveTool;
    [SerializeField]private DeleteTool deleteTool;
    // [SerializeField]private Transform 
    private SetVertexType<GraphVertex> setStartTool;
    private SetVertexType<GraphVertex> setGoalTool;

    private ToolMachine toolMachine;
    [SerializeField]public Transform gameSpace{get => _gameSpace;}
    [SerializeField]private Transform _gameSpace;
    public List<GraphVertexVisualizer> graphVertices = new List<GraphVertexVisualizer>();
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
    private void OnEnable()
    {
        toolMachine.SetTool(creatingTool);
        alogthmManager.OnProcessChanged.AddListener((processing) => {if(processing) DeselectTools();});
    }
    private void OnDisable()
    {
        toolMachine.SetTool(null);
        alogthmManager.OnProcessChanged.RemoveListener((processing) => {if(processing) DeselectTools();});
    }
    private void DeselectTools()
    {
        toolMachine.SetTool(null);
    }
    // private void OnDoubleClick()
    // {
    //     if(!enabled)
    //         return;
    //     creatingTool.CreateSeparateVertex();
    // }
    private void OnPoint(InputValue value)
    {
        if(toolMachine.currentTool != null)
            toolMachine.currentTool.Point();
    }
    #endregion
    #region Buttons
    [Button]
    public void SetCreatingTool()
    {
        if(alogthmManager.inProcess)
            return;
        if(alogthmManager.inProcess)
        {
            return;
        }
        toolMachine.SetTool(creatingTool);
    }
    [Button]
    public void SetMoveTool()
    {
        if(alogthmManager.inProcess)
            return;
        if(alogthmManager.inProcess)
        {
            return;
        }
        toolMachine.SetTool(moveTool);
    }
    [Button]
    public void SetDeleteTool()
    {
        if(alogthmManager.inProcess)
            return;
        if(alogthmManager.inProcess)
        {
            return;
        }
        toolMachine.SetTool(deleteTool);
    }
    [Button]
    public void SetSetPointToStartTool()
    {
        if(alogthmManager.inProcess)
            return;
        if(alogthmManager.inProcess)
        {
            return;
        }
        // SetStartEndPointsTool(StartEndPointTool.PointType.Start);
        toolMachine.SetTool(setStartTool);
    }
    [Button]
    public void SetSetPointToGoalTool()
    {
        if(alogthmManager.inProcess)
            return;
        if(alogthmManager.inProcess)
        {
            return;
        }
        toolMachine.SetTool(setGoalTool);
    }
    [Button]
    public void DestroyAll()
    {
        if(alogthmManager.inProcess)
            return;
        for(int i = graphVertices.Count-1; i>=0; i--)
        {
            if(!graphVertices[i])
                continue;
            Destroy(graphVertices[i].gameObject);
            graphVertices.RemoveAt(i);
        }
    }
    #endregion
    #region PrivateMethods
    private SetVertexType<GraphVertex> CreateStartPointTool()
    {
        return new SetVertexType<GraphVertex>((vertexVisualizer) => 
        {
            if(vertexVisualizer.vertex.graphState == GraphVertex.GraphState.Start)
                return;
            if(vertexVisualizer.vertex.graphState == GraphVertex.GraphState.Goal)
            {
                alogthmManager.goalVertex.SetCustomStatesToDefault();
                alogthmManager.goalVertex = null;
            }
            vertexVisualizer.vertex.graphState = GraphVertex.GraphState.Start;
            alogthmManager.startVertex = vertexVisualizer.vertex;

        });
    } 
    private SetVertexType<GraphVertex> CreateGoalPointTool()
    {
        return new SetVertexType<GraphVertex>((vertexVisualizer) => 
        {
            if(vertexVisualizer.vertex.graphState == GraphVertex.GraphState.Goal)
                return;
            if(vertexVisualizer.vertex.graphState == GraphVertex.GraphState.Start)
            {
                alogthmManager.startVertex.SetCustomStatesToDefault();
                alogthmManager.startVertex = null;
            }
            vertexVisualizer.vertex.graphState = GraphVertex.GraphState.Goal;
            alogthmManager.goalVertex = vertexVisualizer.vertex;

        });
    }
    #endregion
}
