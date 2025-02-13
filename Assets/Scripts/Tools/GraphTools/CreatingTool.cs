using UnityEditor;
using UnityEngine;

[System.Serializable]
public class CreatingTool: GraphTool
{
    [SerializeField]private GameObject _vertexPrefab;
    [SerializeField]private GameObject _linkPrefab;
    [SerializeField]private LayerMask _layerMask;

    private Transform _selectedObject;
    private Transform _toMove;

    #region StateChanging
    public override void Enable()
    {
        InteractionsManager.instance.OnObjectClicked.AddListener(ObjectClickedhandler);
        InteractionsManager.instance.OnVoidClicked.AddListener(VoidClickedhandler);
        InteractionsManager.instance.OnClickUp.AddListener(ClickUpHandler);
    }
    private void ObjectClickedhandler(Transform clicked)
    {
        if(!LayerMaskExtensions.Includes(_layerMask, clicked.gameObject.layer))
        {
            return;
        }
        
        Debug.Log("Selected" + _selectedObject);
        _selectedObject = clicked;

        CreateToMove();
        _toMove.GetComponent<Collider2D>().enabled = false;
    }
    private void VoidClickedhandler()
    {
        _selectedObject = null;
        Debug.Log("Not Selected");
    }
    private void ClickUpHandler()
    {
        Collider2D hit = Physics2D.OverlapPoint(manager.mousePosition, _layerMask);
        if(!hit)
        {
            if(!_selectedObject)
                return;

            Debug.Log("New Link" + _selectedObject);

            GraphVertexVisualizer vertexA = _selectedObject.GetComponent<GraphVertexVisualizer>();
            GraphVertexVisualizer vertexB = _toMove.GetComponent<GraphVertexVisualizer>();

            if(vertexA.vertex.HasLinkWith(vertexB.vertex))
            {
                DropSelections();
                return;
            }
            Link(vertexA, vertexB);

            _toMove.GetComponent<Collider2D>().enabled = true;
            _toMove = null;

            DropSelections();
        }
        else
        {
            if(_selectedObject != null)
            {
                GraphVertexVisualizer vertexA = _selectedObject.GetComponent<GraphVertexVisualizer>();
                GraphVertexVisualizer vertexB = hit.transform.GetComponent<GraphVertexVisualizer>();
                
                if(vertexA == vertexB)
                {
                    DropSelections();
                    return;
                }

                if(!vertexA.vertex.HasLinkWith(vertexB.vertex))
                {
                    Debug.Log("Linked" + _selectedObject);
                    Link(vertexA, vertexB);
                }
                else
                {
                    Debug.Log("Unlink");
                    Vertex.UnLink(vertexA.vertex, vertexB.vertex);
                }
                
                DropSelections();
            }
        }
    }
    public override void Disable()
    {
        InteractionsManager.instance.OnObjectClicked.RemoveListener(ObjectClickedhandler);
        InteractionsManager.instance.OnVoidClicked.RemoveListener(VoidClickedhandler);
        InteractionsManager.instance.OnClickUp.RemoveListener(ClickUpHandler);
    }

    public override void Point()
    {
        if(_toMove)
        {
            _toMove.GetComponent<GraphVertexVisualizer>().UpdatePosition();
            _toMove.position = manager.mousePosition;
            
        }
    }
    #endregion
    private void DropSelections()
    {
        _selectedObject = null;
        if(_toMove)
        {
            MonoBehaviour.Destroy(_toMove.gameObject);
            _toMove = null;
        }
    }
    private void CreateToMove()
    {
        _toMove = MonoBehaviour.Instantiate(_vertexPrefab, manager.gameSpace).transform;
        _toMove.GetComponent<GraphVertexVisualizer>().vertex = new GraphVertex();
        _toMove.position = manager.mousePosition;
    }
    public void CreateSeparateVertex()
    {
        var go = MonoBehaviour.Instantiate(_vertexPrefab, manager.mousePosition, Quaternion.identity, manager.gameSpace);
        go.GetComponent<GraphVertexVisualizer>().vertex = new GraphVertex();
    }
    private void Link(GraphVertexVisualizer vertexA, GraphVertexVisualizer vertexB)
    {
        float distance = Vector2.Distance(vertexA.transform.position, vertexB.transform.position);

        GameObject linkGameObject = MonoBehaviour.Instantiate(_linkPrefab, manager.gameSpace);
        LinkVisualizer linkVisualizer = linkGameObject.GetComponent<LinkVisualizer>();

        Link link = Vertex.LinkTogether(vertexA.vertex, vertexB.vertex, distance);
        linkVisualizer.SetLink(link);
        linkVisualizer.SyncPosiitons();

    }
}