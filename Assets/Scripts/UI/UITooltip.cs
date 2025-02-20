using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]private GameObject tooltipPrefab;
    [SerializeField]private float showDelay = 0.6f;
    [SerializeField]private float hideDelay = 0.1f;
    [SerializeField]private string tooltipMessageRU;
    [SerializeField]private string tooltipMessageEN;
    [SerializeField]private Vector2 offset;
    private TextMeshProUGUI tooltipText;
    private Transform tooltipTransform;
    private Coroutine waitCoroutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(tooltipTransform == null)
        {
            CreateToooltip();
        }

        if(waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
        }

        // tooltipText.text = tooltipMessage;
        waitCoroutine = StartCoroutine(Wait(showDelay, Appear));
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
        }
        
        waitCoroutine = StartCoroutine(Wait(hideDelay, Hide));
    }
    private void OnDisable()
    {
        if(waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
        }
        
        Hide();
    }
    private void CreateToooltip()
    {
        tooltipTransform = Instantiate(tooltipPrefab, FindFirstObjectByType<Canvas>().transform).transform;
        tooltipTransform.gameObject.SetActive(false);
        tooltipText = tooltipTransform.GetComponentInChildren<TextMeshProUGUI>(); 

        TextTranslator tooltipTranslator = tooltipTransform.GetComponentInChildren<TextTranslator>(); 
        tooltipTranslator.RuText = tooltipMessageRU;
        tooltipTranslator.EngText = tooltipMessageEN;
        tooltipTranslator.UpdateLanguage();

        tooltipTransform.position = transform.position + (Vector3)offset;
    }

    private void Appear()
    {
        tooltipTransform.position = transform.position + (Vector3)offset;
        tooltipTransform.gameObject.SetActive(true);
    }
    private void Hide()
    {
        tooltipTransform.gameObject.SetActive(false);
    }

    private IEnumerator Wait(float delay, Action performOnWaitEnd)
    {
        yield return new WaitForSeconds(delay);
        performOnWaitEnd();
    }
}
