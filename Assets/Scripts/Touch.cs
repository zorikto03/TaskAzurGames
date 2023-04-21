using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Touch : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject StartUI;

    public static Action TouchedEvent;
    bool _started = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        if (!_started)
        {
            StartUI.SetActive(false);
        }

        TouchedEvent?.Invoke();
    }
}
