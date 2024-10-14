using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//창 드래그: 구글(유니티 창 드래그) 검색 혹은 https://www.youtube.com/watch?v=Mb2oua3FjZg

public class DragWindow : MonoBehaviour, IDragHandler
{
    [SerializeField] RectTransform _rectTransform;

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
