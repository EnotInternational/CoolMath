using System;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GridToolManager : MonoBehaviour, IToolManager
{
    private SetVertexType<CellVertex> setCleanTool;
    private SetVertexType<CellVertex> setStartTool;
    private SetVertexType<CellVertex> setGoalTool;
    private SetVertexType<CellVertex> setBlockTool;
    private SetVertexType<CellVertex> setWeightTool;

    private ToolMachine toolMachine;
    [SerializeField]public Transform gameSpace{get => _gameSpace;}
    [SerializeField]private Transform _gameSpace;
    public int settingWeight = 2;
    public AlgorithmManager alogthmManager{get;private set;}
    public GridFormer gridFormer { get => _gridFormer; private set => _gridFormer = value; }
    public Vector3 mousePosition{get => _mousePosition;}
    private Vector3 _mousePosition;
    public UnityEvent OnClickDown;
    public UnityEvent OnClickUp;
    [SerializeField]private GridFormer _gridFormer;

    private void Start()
    {
        alogthmManager = GetComponent<AlgorithmManager>();  
        toolMachine = new(this);

        setStartTool = CreateStartPointTool();
        setGoalTool = CreateGoalPointTool();
        setCleanTool = CreateCleanTool();
        setBlockTool = CreateBlockTool();
        setWeightTool = CreateWeightTool();
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

    }
    private void OnDelete(InputValue value)
    {

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
    private void SetCleanTool()
    {
        // SetStartEndPointsTool(StartEndPointTool.PointType.Start);
        toolMachine.SetTool(setCleanTool);
    }
    [Button]
    private void SetWeightTool()
    {
        // SetStartEndPointsTool(StartEndPointTool.PointType.Start);
        toolMachine.SetTool(setWeightTool);
    }
    [Button]
    private void SetBlockTool()
    {
        // SetStartEndPointsTool(StartEndPointTool.PointType.Start);
        toolMachine.SetTool(setBlockTool);
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
    private SetVertexType<CellVertex> CreateStartPointTool()
    {
        return new SetVertexType<CellVertex>((vertexVisualizer) => 
        {
            vertexVisualizer.vertex.cellState = CellVertex.CellState.Start;
            if(alogthmManager.startVertex != null)
            {
                alogthmManager.startVertex.SetCustomStatesToDefault();
            }
            vertexVisualizer.vertex.weight = 1;
            _gridFormer.LocateVertex(vertexVisualizer as CellVertexVisualizer, out Vector2Int position);
            _gridFormer.ConnectVertex(vertexVisualizer.vertex, position);

            alogthmManager.startVertex = vertexVisualizer.vertex;

        });
    } 
    private SetVertexType<CellVertex> CreateGoalPointTool()
    {
        return new SetVertexType<CellVertex>((vertexVisualizer) => 
        {
            vertexVisualizer.vertex.cellState = CellVertex.CellState.Goal;
            if(alogthmManager.goalVertex != null)
            {
                alogthmManager.goalVertex.SetCustomStatesToDefault();
            }
            vertexVisualizer.vertex.weight = 1;
            _gridFormer.LocateVertex(vertexVisualizer as CellVertexVisualizer, out Vector2Int position);
            _gridFormer.ConnectVertex(vertexVisualizer.vertex, position);
            
            alogthmManager.goalVertex = vertexVisualizer.vertex;

        });
    }
    private SetVertexType<CellVertex> CreateWeightTool()
    {
        return new SetVertexType<CellVertex>((vertexVisualizer) => 
        {
            // if(vertexVisualizer.vertex.cellState ==)
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
            // if(vertexVisualizer.vertex.cellState ==)
            vertexVisualizer.vertex.weight = 1;
            vertexVisualizer.vertex.cellState = CellVertex.CellState.Common;
            _gridFormer.LocateVertex(vertexVisualizer as CellVertexVisualizer, out Vector2Int position);
            _gridFormer.ConnectVertex(vertexVisualizer.vertex, position);
        });
    }
    private SetVertexType<CellVertex> CreateBlockTool()
    {
        return new SetVertexType<CellVertex>((vertexVisualizer) => 
        {
            vertexVisualizer.vertex.cellState = CellVertex.CellState.Blocked;
            vertexVisualizer.vertex.UnlinkAll();
        });
    }
    #endregion
}
