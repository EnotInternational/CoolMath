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
    [SerializeField]public Transform gameSpace{get => _gameSpace;}
    [SerializeField]private Transform _gameSpace;
    public List<GraphVertexVisualizer> graphVertices = new List<GraphVertexVisualizer>();
    public override void Disable()
    {
        toolMachine.SetTool(null);
    }
    private void Awake()
    {
        alogthmManager = GetComponent<AlgorithmManager>();  
        toolMachine = new(this);

        setStartTool = CreateStartPointTool();
        setGoalTool = CreateGoalPointTool();
        creatingTool.Initialize(this);
        deleteTool.Initialize(this);

        
    }
    private void Start()
    {
        GraphVertexVisualizer vertex1 = creatingTool.CreateSeparateVertex(new Vector3(-3, 0));
        vertex1.vertex.graphState = GraphVertex.GraphState.Start;
        alogthmManager.startVertex = vertex1.vertex;

        GraphVertexVisualizer vertex2 = creatingTool.CreateSeparateVertex(new Vector3(3, 0));
        vertex2.vertex.graphState = GraphVertex.GraphState.Goal;
        alogthmManager.goalVertex = vertex2.vertex;

    }
    #region OnEvents
    protected override void OnEnable()
    {
        base.OnEnable();
        toolMachine.SetTool(creatingTool);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        toolMachine.SetTool(null);
    }
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
        SetTool(creatingTool);
    }
    [Button]
    public void SetMoveTool()
    {
        SetTool(moveTool);
    }
    [Button]
    public void SetDeleteTool()
    {
        SetTool(deleteTool);
    }
    [Button]
    public void SetSetPointToStartTool()
    {
        SetTool(setStartTool);
    }
    [Button]
    public void SetSetPointToGoalTool()
    {
        SetTool(setGoalTool);
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
    private void SetTool(ITool tool)
    {
        if(alogthmManager.inProcess)
            return;
        toolMachine.SetTool(tool);
        alogthmManager.ClearResults();
    }
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
            if(alogthmManager.startVertex != null)
            {
                alogthmManager.startVertex.SetCustomStatesToDefault();
                alogthmManager.startVertex = null;
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
            if(alogthmManager.goalVertex != null)
            {
                alogthmManager.goalVertex.SetCustomStatesToDefault();
                alogthmManager.goalVertex = null;
            }
            vertexVisualizer.vertex.graphState = GraphVertex.GraphState.Goal;
            alogthmManager.goalVertex = vertexVisualizer.vertex;

        });
    }
    #endregion
}
