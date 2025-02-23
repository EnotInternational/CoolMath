using System.Linq;
using EditorAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LinkVisualizer : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private PolygonCollider2D _collider;
    private Transform _transform;

    [SerializeField]private Link _link;
    private Transform _textTransform;
    private TMP_InputField _inputField;
    private Toggle _lockToggle;
    [SerializeField]private LinkColorSettings _linkColorSettings;
    private void OnEnable()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _collider = GetComponent<PolygonCollider2D>();
        _transform = transform;
        if(_inputField)
            _inputField.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        if(_inputField)
            _inputField.gameObject.SetActive(false);
    }
    public void SetLink(Link link, Transform textTransform)
    {
        _link = link;

        _textTransform = textTransform;
        _inputField = _textTransform.GetComponent<TMP_InputField>();
        _lockToggle = _textTransform.GetComponentInChildren<Toggle>();
        
        _inputField.onValueChanged.AddListener(SetWeight);
        _lockToggle.onValueChanged.AddListener(SetLocked);

        _lineRenderer.material = _linkColorSettings.standartMaterial;
        
        _link.OnUnlink.AddListener(LinkDestroyedHandler);
        _link.OnChanged.AddListener(()=>SyncPosiitons());
        _link.OnWeightChanged.AddListener(SetWeightText);
        _link.OnLockWeightChanged.AddListener(LockedLinkHandler);
        _link.OnStateChanged.AddListener(StateChangeHandler);
    }
    private void StateChangeHandler(Link.State state)
    {
        switch (state)
        {
            case Link.State.Seen:
                _lineRenderer.material = _linkColorSettings.seenMaterial;
                break;
            case Link.State.NotSeen:
                _lineRenderer.material = _linkColorSettings.standartMaterial;
                break;
            case Link.State.Path:
                _lineRenderer.material = _linkColorSettings.pathMaterial;
                break;
        }
    }
    private void SetLocked(bool locked)
    {
        _link.SetLockWeightWithoutNotify(locked);
        if(!locked)
        {
            _link.AutoCalcWeight();
        }
    }
    private void LockedLinkHandler()
    {
        _lockToggle.isOn = _link.LockWeight;
        if(!_link.LockWeight)
        {
            _link.AutoCalcWeight();
        }
    }
    private void SetWeightText(float number)
    {
        _inputField.text = number.ToString();
    }
    private void SetWeight(string text)
    {
        // _lineRenderer = _linkColorSettings.
        if(!float.TryParse(text, out float number))
        {
            string result = new string(text.Where(t => char.IsDigit(t)).ToArray());
            _inputField.text = result;
            if(result == string.Empty)
                return;
            number = float.Parse(result);
        }
        if(number > 40)
        {
            number = 40;
            SetWeightText(number);
        }
        _link.SetWeightForceWithoutNotify(number);
    }
    public void SyncPosiitons()
    {
        Vector2 posA = _link.VertexA.position;
        Vector2 posB = _link.VertexB.position;

        _lineRenderer.SetPosition(0, posA);
        _lineRenderer.SetPosition(1, posB);

        Vector2 localPosA = _transform.InverseTransformPoint(posA);
        Vector2 localPosB = _transform.InverseTransformPoint(posB);

        Vector2 side = Vector2.Perpendicular((localPosA-localPosB).normalized);

        _textTransform.position = (posA + posB) /2;

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
        if(_textTransform)
            Destroy(_textTransform.gameObject);
    }
    private void LinkDestroyedHandler()
    {
        // Debug.Log("Handle destroy link");
        Destroy(gameObject);
    }
    
}
