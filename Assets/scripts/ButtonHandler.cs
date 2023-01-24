using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [Header("Colors")]
    [SerializeField] Color activeColor;
    [SerializeField] Color inactiveColor;
    [SerializeField] Color hoverColor;
    [SerializeField] Color pressedColor;
    [SerializeField] Color selectedColor;


    private bool isSelected = false;
    private bool isActive = true;

    public Action onClick;
    public Image _background;
    private Image Background
    {
        get
        {
            if (_background == null)
            {
                _background = GetComponent<Image>();
            }
            return _background;
        }
    }


    private void Awake()
    {
        _background = GetComponent<Image>();
        _background.color = activeColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isActive) return;
        AudioManager.Instance.PlaySound(SoundType.Click);
        onClick?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isActive) return;
        if (isSelected) return;
        Background.color = pressedColor;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isActive) return;
        if (isSelected) return;
        Background.color = hoverColor;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isActive) return;
        Background.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isActive) return;
        Background.color = isSelected ? selectedColor : activeColor;
    }


    public void SetActive(bool active)
    {
        isActive = active;
        Background.color = isActive ? activeColor : inactiveColor;
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        Background.color = isSelected ? selectedColor : activeColor;
    }

}