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
        _mousePosition = value.Get<Vector2>();
        if(_newLink)
        {
            _newLink.position = Camera.main.ScreenToWorldPoint(_mousePosition);
        }
    }
    private void OnClick(InputValue value)
    {
        Vector2 clickPoint = Camera.main.ScreenToWorldPoint(_mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(clickPoint);


        if(value.isPressed)
        {
            if(!hit)
            {
                _selectedObject = null;
                Debug.Log("Not Selected" + _selectedObject);
                return;
            }
            Debug.Log("Selected" + _selectedObject);
            _selectedObject = hit.transform;

            _newLink = Instantiate(_vertexPrefab, transform).transform;
            _newLink.GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            if(!hit)
            {
                Debug.Log("New Link" + _selectedObject);
                if(!_selectedObject)
                {
                    return;
                }
                Vertex vertexA = _selectedObject.GetComponent<Vertex>();
                Vertex vertexB = _newLink.GetComponent<Vertex>();

                Link(vertexA, vertexB);
                _newLink.GetComponent<Collider2D>().enabled = true;

                _selectedObject = null;

                _newLink = null;

                return;
            }
            Debug.Log("Linked" + _selectedObject);
            if(_selectedObject != null)
            {
                Vertex vertexA = _selectedObject.GetComponent<Vertex>();
                Vertex vertexB = hit.transform.GetComponent<Vertex>();

                Link(vertexA, vertexB);
                if(_newLink)
                {
                    Destroy(_newLink.gameObject);
                    _newLink = null;
                }
                _selectedObject = null;
            }
        }
    }
    private void Link(Vertex vertexA, Vertex vertexB)
    {
        float distance = Vector2.Distance(vertexA.transform.position, vertexB.transform.position);
        Vertex.LinkTogether(vertexA, vertexB, distance);
    }
}
