using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotController : MonoBehaviour, IScrollHandler //https://higatsuryu9975.tistory.com/10
{
    ScrollRect scrollRect;

    public void OnScroll(PointerEventData eventData) { scrollRect.OnScroll(eventData); }

    //자식개체 반환 시 transform.GetChild(int index).gameObject 사용 가능
    [SerializeField] GameObject Item;
    public bool isEmpty { get { return !Item.activeSelf == true; } }
    public void slotEnable() { Item.SetActive(true); }
    public void slotDisable() { Item.SetActive(false); }

    private bool isSlotSelected = false;
    public bool IsSlotSelected { get { return isSlotSelected; } set { isSlotSelected = value; } }

    [SerializeField] Animator _animator;

    public void SlotMouseDown()
    {
        _animator.SetTrigger("slotMouseDown");
    }
    public void SlotMouseUp()
    {
        _animator.SetTrigger("slotMouseUp");
    }
    //public void SlotUnselected() { _animator.SetTrigger("slotUnselected"); }

    Inventory _inventory;

    public void InitSlot(Inventory inventory)
    {
        _inventory = inventory;
        scrollRect = inventory.GetComponentInChildren<ScrollRect>();
    }

    public void OnSelect()
    {
        _inventory.OnSlotSelect(this);
    }

    // Start is called before the first frame update
    void Start()
    {

    }
}
