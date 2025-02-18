using System;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GridToolManager : ToolManager
{
    public static GridToolManager instance{get; private set;}
    private SetVertexType<CellVertex> setCleanTool;
    private SetVertexType<CellVertex> setStartTool;
    private SetVertexType<CellVertex> setGoalTool;
    private SetVertexType<CellVertex> setBlockTool;
    private SetVertexType<CellVertex> setWeightTool;

    private ToolMachine toolMachine;
    [SerializeField]public Transform gameSpace{get => _gameSpace;}
    [SerializeField]private Transform _gameSpace;
    public int settingWeight = 2;
    public bool block
    {
        get
        {
            return _block;
        }
        set
        {
            _block = value;
            if(toolMachine.currentTool != null)
            {
                toolMachine.currentTool.block = _block;
            }
        }
    }
    private bool _block;
    public AlgorithmManager alogthmManager{get;private set;}
    public GridFormer gridFormer { get => _gridFormer; private set => _gridFormer = value; }
    [SerializeField]private GridFormer _gridFormer;

    private void Awake()
    {
        instance = this;
        alogthmManager = GetComponent<AlgorithmManager>();  
        toolMachine = new(this);

        setStartTool = CreateStartPointTool();
        setGoalTool = CreateGoalPointTool();
        setCleanTool = CreateCleanTool();
        setBlockTool = CreateBlockTool();
        setWeightTool = CreateWeightTool();
    }
    private void Start()
    {
        toolMachine.SetTool(setBlockTool);
    }
    #region OnEvents
    private void OnPoint(InputValue value)
    {
        if(toolMachine.currentTool != null)
            toolMachine.currentTool.Point();
    }
    #endregion
    #region Buttons
    [Button]
    public void SetCleanTool()
    {
        // SetStartEndPointsTool(StartEndPointTool.PointType.Start);
        toolMachine.SetTool(setCleanTool);
    }
    [Button]
    public void SetWeightTool()
    {
        // SetStartEndPointsTool(StartEndPointTool.PointType.Start);
        toolMachine.SetTool(setWeightTool);
    }
    [Button]
    public void SetBlockTool()
    {
        // SetStartEndPointsTool(StartEndPointTool.PointType.Start);
        toolMachine.SetTool(setBlockTool);
    }
    [Button]
    public void SetSetPointToStartTool()
    {
        // SetStartEndPointsTool(StartEndPointTool.PointType.Start);
        toolMachine.SetTool(setStartTool);
    }
    [Button]
    public void SetSetPointToGoalTool()
    {
        toolMachine.SetTool(setGoalTool);
    }
    [Button]
    public void CleanAllCells()
    {
        _gridFormer.CleanAllVerticies();
    }
    #endregion
    #region PrivateMethods
    private SetVertexType<CellVertex> CreateStartPointTool()
    {
        return new SetVertexType<CellVertex>((vertexVisualizer) => 
        {
            ClearVertex(vertexVisualizer.vertex);

            if(alogthmManager.startVertex != null)
            {
                alogthmManager.startVertex.SetCustomStatesToDefault();
                alogthmManager.startVertex = null;
            }

            vertexVisualizer.vertex.cellState = CellVertex.CellState.Start;
            _gridFormer.LocateVertex(vertexVisualizer as CellVertexVisualizer, out Vector2Int position);
            _gridFormer.ConnectVertex(vertexVisualizer.vertex, position);

            alogthmManager.startVertex = vertexVisualizer.vertex;

        });
    } 
    private SetVertexType<CellVertex> CreateGoalPointTool()
    {
        return new SetVertexType<CellVertex>((vertexVisualizer) => 
        {
            ClearVertex(vertexVisualizer.vertex);

            if(alogthmManager.goalVertex != null)
            {
                alogthmManager.goalVertex.SetCustomStatesToDefault();
                alogthmManager.goalVertex = null;
            }
            vertexVisualizer.vertex.cellState = CellVertex.CellState.Goal;
            _gridFormer.LocateVertex(vertexVisualizer as CellVertexVisualizer, out Vector2Int position);
            _gridFormer.ConnectVertex(vertexVisualizer.vertex, position);
            
            alogthmManager.goalVertex = vertexVisualizer.vertex;

        });
    }
    private SetVertexType<CellVertex> CreateWeightTool()
    {
        return new SetVertexType<CellVertex>((vertexVisualizer) => 
        {
            ClearVertex(vertexVisualizer.vertex);

            vertexVisualizer.vertex.weight = settingWeight;
            vertexVisualizer.vertex.cellState = CellVertex.CellState.Weighted;
            _gridFormer.LocateVertex(vertexVisualizer as CellVertexVisualizer, out Vector2Int position);
            _gridFormer.ConnectVertex(vertexVisualizer.vertex, position);
        });
    }
    private SetVertexType<CellVertex> CreateCleanTool()
    {
        return new SetVertexType<CellVertex>((vertexVisualizer) => 
        {
            ClearVertex(vertexVisualizer.vertex);

            vertexVisualizer.vertex.cellState = CellVertex.CellState.Common;
            _gridFormer.LocateVertex(vertexVisualizer as CellVertexVisualizer, out Vector2Int position);
            _gridFormer.ConnectVertex(vertexVisualizer.vertex, position);
        });
    }
    private SetVertexType<CellVertex> CreateBlockTool()
    {
        return new SetVertexType<CellVertex>((vertexVisualizer) => 
        {
            ClearVertex(vertexVisualizer.vertex);
            vertexVisualizer.vertex.cellState = CellVertex.CellState.Blocked;
            vertexVisualizer.vertex.UnlinkAll();
        });
    }
    private void ClearVertex(CellVertex vertex)
    {
        vertex.weight = 1;
        if(vertex.cellState == CellVertex.CellState.Start)
        {
            alogthmManager.startVertex = null;
        }
        if(vertex.cellState == CellVertex.CellState.Goal)
        {
            alogthmManager.goalVertex = null;
        }
        vertex.SetCustomStatesToDefault();
    }

    #endregion
}
