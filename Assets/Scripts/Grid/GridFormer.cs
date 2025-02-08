using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridFormer : MonoBehaviour
{
    [SerializeField]private GameObject _prefab;
    [SerializeField]private Transform _root;
    [SerializeField]private AnchorRect _anchorRect;
    [SerializeField]private Transform _draggedObject;    
    [SerializeField]private LayerMask _mask;    
    [SerializeField]private InteractionsManager _interactionsManager;    
    private Vector2Int _gridSize = Vector2Int.zero;
    private List<List<Vertex>> _vertices= new List<List<Vertex>>();
    private void Start()
    {
        _interactionsManager = InteractionsManager.instance;
        _anchorRect.OnRectChanged.AddListener(ChangeGrid);
    }
    private void OnDisable()
    {
        _anchorRect.OnRectChanged.RemoveListener(ChangeGrid);
    }
    public void ChangeGrid()
    {
        _root.position = (Vector2)_anchorRect.rect.position + new Vector2(0.5f,0.5f);

        Vector2Int sizeDelta = _anchorRect.rect.size - _gridSize;
        Debug.Log(sizeDelta);
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
                    Debug.Log("spawn");
                    _vertices[y].Add(SpawnVertex(new Vector2(_vertices[y].Count,y)));
                }
            }
        } 
        else
        {
            for(int y = 0; y < _vertices.Count; y++)
            {
                List<Vertex> row = _vertices[y];
                for(int x = 0; x < absDelta; x++)
                {
                    Vertex vertex = row[row.Count-1];
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
                List<Vertex> row = new List<Vertex>();
                for(int x = 0; x < _gridSize.x; x++)
                {
                    Debug.Log("spawn row");
                    row.Add(SpawnVertex(new Vector2(x, _gridSize.y + y)));
                }
                _vertices.Add(row);
            }
        } else
        {
            for(int y = 0; y < absDelta; y++)
            {
                List<Vertex> row = _vertices[_vertices.Count-1];
                for(int x = 0; x < row.Count; x++)
                {
                    DestroyVertex(row[x]);
                }
                _vertices.Remove(row);
            }
        }
        
    }
    private Vertex SpawnVertex(Vector2 position)
    {
        GameObject vertex = Instantiate(_prefab, _root);
        vertex.transform.localPosition = position;
        return vertex.GetComponent<Vertex>();
    }
    private void DestroyVertex(Vertex vertex)
    {
        Destroy(vertex.gameObject);
    }
}
