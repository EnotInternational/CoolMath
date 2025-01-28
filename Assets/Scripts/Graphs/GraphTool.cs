using UnityEngine;
using UnityEngine.InputSystem;

public class GraphTool : MonoBehaviour
{
    [SerializeField]private GameObject _vertexPrefab;
    private Vector2 _mousePosition;
    private Transform _selectedObject;
    private Transform _newLink;
    private void OnPoint(InputValue value)
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
        if(_newLink)
        {
            _newLink.position = _mousePosition;
        }
    }
    private void OnClick(InputValue value)
    {
        Collider2D hit = Physics2D.OverlapPoint(_mousePosition);


        if(value.isPressed)
        {
            if(!hit)
            {
                _selectedObject = null;
                Debug.Log("Not Selected" + _selectedObject);
            }
            else
            {
                Debug.Log("Selected" + _selectedObject);
                _selectedObject = hit.transform;

                _newLink = Instantiate(_vertexPrefab, transform).transform;
                _newLink.position = _mousePosition;
                _newLink.GetComponent<Collider2D>().enabled = false;
            }
        }
        else
        {
            if(!hit)
            {
                if(!_selectedObject)
                    return;

                Debug.Log("New Link" + _selectedObject);

                Vertex vertexA = _selectedObject.GetComponent<Vertex>();
                Vertex vertexB = _newLink.GetComponent<Vertex>();

                if(vertexA.HasLinkWith(vertexB))
                {
                    DropSelections();
                    return;
                }
                Link(vertexA, vertexB);

                _newLink.GetComponent<Collider2D>().enabled = true;
                _newLink = null;

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
    }
    private void DropSelections()
    {
        _selectedObject = null;
        if(_newLink)
        {
            Destroy(_newLink.gameObject);
            _newLink = null;
        }
    }
    private void Link(Vertex vertexA, Vertex vertexB)
    {
        float distance = Vector2.Distance(vertexA.transform.position, vertexB.transform.position);
        Vertex.LinkTogether(vertexA, vertexB, distance);
    }
}
