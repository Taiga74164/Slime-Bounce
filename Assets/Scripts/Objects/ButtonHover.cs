using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private readonly Vector3 _originalScale = Vector3.one;
    private Vector3 _targetScale;
    private readonly float _scaleFactor = 1.2f;
    private readonly float _lerpSpeed = 10.0f;

    private void Start() => _targetScale = _originalScale;

    public void OnPointerEnter(PointerEventData eventData) => _targetScale = _originalScale * _scaleFactor;
    
    public void OnPointerExit(PointerEventData eventData) => _targetScale = _originalScale;
    
    private void Update()
    {
        if (_targetScale == transform.localScale)
            return;
        
        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, _lerpSpeed * Time.deltaTime);
    }
}
