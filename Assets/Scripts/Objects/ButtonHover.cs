using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private readonly Vector3 _originalScale = Vector3.one;
    private Vector3 _targetScale;
    private readonly float _scaleFactor = 1.2f;
    private readonly float _lerpSpeed = 10.0f;
    private Button _button;
    
    private void Start()
    {
        _targetScale = _originalScale;
        _button = this.GetComponent<Button>();
        _button?.onClick.AddListener(OnClick);
    }

    private void OnClick() => AudioManager.Instance.popClick.Play();
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.otherClick.Play();
        _targetScale = _originalScale * _scaleFactor;
    }
    
    public void OnPointerExit(PointerEventData eventData) => _targetScale = _originalScale;
    
    private void Update()
    {
        if (_targetScale == transform.localScale)
            return;
        
        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, _lerpSpeed * Time.deltaTime);
    }
}
