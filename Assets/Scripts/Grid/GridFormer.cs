using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridFormer : MonoBehaviour
{
    [SerializeField]private GameObject _prefab;
    [SerializeField]private Transform _root;
    [SerializeField]private AnchorRect _anchorRect;
    [SerializeField]private LayerMask _mask;    
    private Vector2Int _gridSize = Vector2Int.zero;
    private List<List<CellVertexVisualizer>> _vertices= new List<List<CellVertexVisualizer>>();
    private void Start()
    {
        _anchorRect.OnRectChanged.AddListener(ChangeGrid);
    }
    private void OnEnable()
    {
        ChangeGrid();
    }
    private void OnDisable()
    {
        _anchorRect.OnRectChanged.RemoveListener(ChangeGrid);
    }
    public void CleanAllVerticies()
    {
        foreach (List<CellVertexVisualizer> row in _vertices)
        {
            foreach(CellVertexVisualizer visualizer in row)
            {
                visualizer.vertex.cellState = CellVertex.CellState.Common;
            }
        }
    }
    private void ChangeGrid()
    {
        _root.position = (Vector2)_anchorRect.rect.position + new Vector2(0.5f,0.5f);

        Vector2Int sizeDelta = _anchorRect.rect.size - _gridSize;
        // Debug.Log(sizeDelta);
        if(sizeDelta.y != 0)
        {
            ChangeHeight(sizeDelta.y);
        }
        if(sizeDelta.x != 0)
        {
            ChangeWide(sizeDelta.x);
        }
        _gridSize = _anchorRect.rect.size;
    }
    private void ChangeWide(int delta)
    {
        int absDelta = Mathf.Abs(delta);
        
        if(delta > 0)
        {
            for(int y = 0; y < _vertices.Count; y++)
            {
                for(int x = 0; x < absDelta; x++)
                {
                    // Debug.Log("spawn");
                    _vertices[y].Add(SpawnVertex(new Vector2Int(_vertices[y].Count,y)));
                }
            }
        } 
        else
        {
            for(int y = 0; y < _vertices.Count; y++)
            {
                List<CellVertexVisualizer> row = _vertices[y];
                for(int x = 0; x < absDelta; x++)
                {
                    CellVertexVisualizer vertex = row[row.Count-1];
                    row.Remove(vertex);
                    DestroyVertex(vertex);
                }
            }
        }
        
        
    }
    private void ChangeHeight(int delta)
    {
        int absDelta = Mathf.Abs(delta);
        
        if(delta > 0)
        {
            for(int y = 0; y < absDelta; y++)
            {
                List<CellVertexVisualizer> row = new List<CellVertexVisualizer>();
                _vertices.Add(row);
                for(int x = 0; x < _gridSize.x; x++)
                {
                    // Debug.Log("spawn row");
                    row.Add(SpawnVertex(new Vector2Int(x, _gridSize.y + y)));
                }
            }
        } else
        {
            for(int y = 0; y < absDelta; y++)
            {
                List<CellVertexVisualizer> row = _vertices[_vertices.Count-1];
                for(int x = 0; x < row.Count; x++)
                {
                    DestroyVertex(row[x]);
                }
                _vertices.Remove(row);
            }
        }
        
    }
    private CellVertexVisualizer SpawnVertex(Vector2Int position)
    {
        GameObject vertexGO = Instantiate(_prefab, _root);
        vertexGO.transform.localPosition = (Vector2)position;
        CellVertexVisualizer vertex = vertexGO.GetComponent<CellVertexVisualizer>();
        CellVertex cellVertex = new CellVertex();
        vertex.vertex = cellVertex;
        ConnectVertex(vertex.vertex, position);
        return vertex;
    }
    public void ConnectVertex(Vertex vertex, Vector2Int vertexPosition)
    {
        for(int y = -1; y < 2; y++)
        {
            for(int x = -1; x < 2; x++)
            {
                if(!TryGetVertex(vertexPosition + new Vector2Int(x, y), out CellVertexVisualizer currentVertex))
                    continue;
                if(vertex.HasLinkWith(currentVertex.vertex))
                    continue;
                
                if(currentVertex.vertex.cellState == CellVertex.CellState.Blocked)
                {
                    continue;
                }
                Link link = Vertex.LinkTogether(vertex, currentVertex.vertex);
                if(Mathf.Abs(y) + Mathf.Abs(x) == 2)
                {
                    link.weight = Mathf.Pow(2f, 0.5f);
                }
                else
                {
                    link.weight = 1;
                }
            }
        }
    }
    public bool LocateVertex(CellVertexVisualizer vertexVisualizer, out Vector2Int position)
    {
        for(int y = 0; y < _vertices.Count; y++)
        {
            for(int x = 0; x < _vertices[y].Count; x++)
            {
                if(_vertices[y][x] == vertexVisualizer)
                {
                    position =  new Vector2Int(x, y);
                    return true;
                }
                    
            }
        }
        position =  Vector2Int.zero;
        return false;
    }
    private bool TryGetVertex(Vector2Int position, out CellVertexVisualizer vertex)
    {
        if(position.y < 0 || position.y >= _vertices.Count)
        {
            vertex = null;
            return false;
        }
        if(position.x < 0 || position.x >= _vertices[position.y].Count)
        {
            vertex = null;
            return false;
        }
        vertex = _vertices[position.y][position.x];
        return true;
    }
    private void DestroyVertex(CellVertexVisualizer vertex)
    {
        Destroy(vertex.gameObject);
    }
}
