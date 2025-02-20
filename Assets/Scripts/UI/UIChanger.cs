using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent OnPointEnter;
    public UnityEvent OnPointExit;
    [SerializeField]private GameObject[] PointEnterShow;
    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (GameObject gameObject in PointEnterShow)
        {
            gameObject.SetActive(true);
        }
        OnPointEnter.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (GameObject gameObject in PointEnterShow)
        {
            gameObject.SetActive(false);
        }
        OnPointExit.Invoke();
    }
}
