using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class StartEndPointTool : ITool
{
    [SerializeField]private LayerMask _layerMask;
    [SerializeField]private AlgorithmManager _algorithmManager;
    private PointType _pointType = PointType.Null;
    public UnityEvent OnEndPlacing = new UnityEvent();
    public void ChangeSettingPoint(PointType type)
    {
        _pointType = type;
    }
    public void Enable()
    {
        InteractionsManager.instance.OnClickDown.AddListener(Click);
    }
    private void Click()
    {
        if(_pointType == PointType.Null)
        {
            Debug.Log("Point type null");
            return;
        }

        Collider2D hit = Physics2D.OverlapPoint(InteractionsManager.instance.mousePosition, _layerMask);
        if(!hit)
        {
            Debug.Log("No hit");
            return;
        }
        Vertex vertex = hit.transform.GetComponent<Vertex>();
        
        AlgorithmManager alogthmManager = _algorithmManager;
        if(_pointType == PointType.Goal)
        {
            Debug.Log("Setting goal");
            ReplaceVertex(alogthmManager.goalVertex, vertex);
            vertex.vertexVisualizer.SetGoal();
        }
        else if(_pointType == PointType.Start)
        {
            Debug.Log("Setting start");
            ReplaceVertex(alogthmManager.startVertex, vertex);
            vertex.vertexVisualizer.SetStart();
        }

        Debug.Log("End");
        _pointType = PointType.Null;
        OnEndPlacing.Invoke();
        InteractionsManager.instance.OnClickDown.RemoveListener(Click);

        void ReplaceVertex(Vertex targetVertex, Vertex newVertex)
        {
            if(targetVertex)
            {
                targetVertex.vertexVisualizer.SetUnseen();
            }
            targetVertex = newVertex;
        }

    }
    public void Disable()
    {
        InteractionsManager.instance.OnClickDown.RemoveListener(Click);
        _pointType = PointType.Null;
    }
    public void Point()
    {
        
    }

    public void Initialize(IToolManager toolManager)
    {
        
    }

    public enum PointType{Null, Goal, Start};
}