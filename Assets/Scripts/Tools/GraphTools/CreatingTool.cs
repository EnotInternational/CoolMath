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
        manager.OnClickDown.AddListener(ClickDownHandler);
        manager.OnClickUp.AddListener(ClickUpHandler);
    }
    private void ClickDownHandler()
    {
        Collider2D hit = Physics2D.OverlapPoint(manager.mousePosition, _layerMask);
        if(!hit)
        {
            _selectedObject = null;
            Debug.Log("Not Selected" + _selectedObject);
        }
        else
        {
            Debug.Log("Selected" + _selectedObject);
            _selectedObject = hit.transform;

            CreateToMove();
            _toMove.GetComponent<Collider2D>().enabled = false;
        }
    }
    private void ClickUpHandler()
    {
        Collider2D hit = Physics2D.OverlapPoint(manager.mousePosition, _layerMask);
        if(!hit)
        {
            if(!_selectedObject)
                return;

            Debug.Log("New Link" + _selectedObject);

            Vertex vertexA = _selectedObject.GetComponent<Vertex>();
            Vertex vertexB = _toMove.GetComponent<Vertex>();

            if(vertexA.HasLinkWith(vertexB))
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
                Vertex vertexA = _selectedObject.GetComponent<Vertex>();
                Vertex vertexB = hit.transform.GetComponent<Vertex>();
                
                if(vertexA == vertexB)
                {
                    DropSelections();
                    return;
                }

                if(!vertexA.HasLinkWith(vertexB))
                {
                    Debug.Log("Linked" + _selectedObject);
                    Link(vertexA, vertexB);
                }
                else
                {
                    Debug.Log("Unlink");
                    Vertex.UnLink(vertexA, vertexB);
                }
                
                DropSelections();
            }
        }
    }
    public override void Disable()
    {
        manager.OnClickDown.RemoveListener(ClickDownHandler);
        manager.OnClickUp.RemoveListener(ClickUpHandler);
    }

    public override void Point()
    {
        if(_toMove)
        {
            _toMove.GetComponent<Vertex>().UpdateAllLinks();
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
        _toMove.position = manager.mousePosition;
    }
    public void CreateSeparateVertex()
    {
        MonoBehaviour.Instantiate(_vertexPrefab, manager.mousePosition, Quaternion.identity, manager.gameSpace);

    }
    private void Link(Vertex vertexA, Vertex vertexB)
    {
        float distance = Vector2.Distance(vertexA.transform.position, vertexB.transform.position);

        GameObject linkGameObject = MonoBehaviour.Instantiate(_linkPrefab, manager.gameSpace);
        LinkVisualizer linkVisualizer = linkGameObject.GetComponent<LinkVisualizer>();

        Link link = Vertex.LinkTogether(vertexA, vertexB, distance);
        linkVisualizer.SetLink(link);
        linkVisualizer.SyncPosiitons();

    }
}