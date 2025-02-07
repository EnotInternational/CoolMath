using EditorAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class LinkVisualizer : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private PolygonCollider2D _collider;
    private Transform _transform;

    [SerializeField]private Link _link;
    [SerializeField]private LinkColorSettings _linkColorSettings;
    private void OnEnable()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _collider = GetComponent<PolygonCollider2D>();
        _transform = transform;
    }
    public void SetLink(Link link)
    {
        _link = link;
        _lineRenderer.material = _linkColorSettings.standartMaterial;
        _link.OnUnlink.AddListener(() => Destroy(gameObject));
        _link.OnChanged.AddListener(()=>SyncPosiitons());
        _link.OnSetAsPath.AddListener(SetPath);
        _link.OnSeen.AddListener(SetSeen);
    }
    private void SetPath()
    {
        _lineRenderer.material = _linkColorSettings.pathMaterial;
    }
    private void SetSeen()
    {
        _lineRenderer.material = _linkColorSettings.seenMaterial;
    }
    [Button]
    private void SyncCollider()
    {
        var lineRenderer = GetComponent<LineRenderer>();

        Vector2 localPosA = transform.InverseTransformPoint(lineRenderer.GetPosition(0));
        Vector2 localPosB = transform.InverseTransformPoint(lineRenderer.GetPosition(1));

        Vector2 side = Vector2.Perpendicular((localPosA-localPosB).normalized);



        Vector2[] points = new Vector2[]
        {
            localPosA - side * lineRenderer.startWidth,
            localPosA + side * lineRenderer.startWidth,
            localPosB + side * lineRenderer.startWidth,
            localPosB - side * lineRenderer.startWidth,
        };

        GetComponent<PolygonCollider2D>().SetPath(0, points);
    }
    public void SyncPosiitons()
    {
        Vector2 posA = _link.VertexA.transform.position;
        Vector2 posB = _link.VertexB.transform.position;

        _lineRenderer.SetPosition(0, posA);
        _lineRenderer.SetPosition(1, posB);

        Vector2 localPosA = _transform.InverseTransformPoint(posA);
        Vector2 localPosB = _transform.InverseTransformPoint(posB);

        Vector2 side = Vector2.Perpendicular((localPosA-localPosB).normalized);



        Vector2[] points = new Vector2[]
        {
            localPosA - side * _lineRenderer.startWidth,
            localPosA + side * _lineRenderer.startWidth,
            localPosB + side * _lineRenderer.startWidth,
            localPosB - side * _lineRenderer.startWidth,
        };

        _collider.SetPath(0, points);
    }
    private void OnDestroy()
    {
        Vertex.UnLink(_link);
    }
}
