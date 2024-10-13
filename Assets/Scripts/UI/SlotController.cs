using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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
    public void SlotUnselected()
    {
        _animator.SetTrigger("slotUnselected");
    }
    public void SlotMouseUpUnselected()
    {
        _animator.SetTrigger("slotMouseUpUnselected");
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

    private RebirthType _rebirthType = RebirthType.END;
    private float _bloodRequire = 1f;
    public void SetRebirthType(RebirthType type)
    {
        ItemController pros = transform.Find("Item_pros").GetComponent<ItemController>();
        ItemController cons = transform.Find("Item_cons").GetComponent<ItemController>();
        Image prosImg = pros.GetComponent<Image>();
        Image consImg = cons.GetComponent<Image>();
        TextMeshProUGUI prosDesc = pros.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI consDesc = cons.transform.Find("Description").GetComponent<TextMeshProUGUI>();

        _rebirthType = type;
        _bloodRequire = (float)Random.Range(1, 3);

        prosImg.sprite = pros.GetIcon((int)type);
        switch (_rebirthType)
        {
            case RebirthType.DMG:
                prosDesc.text = "데미지\n증가";
                break;
            case RebirthType.CRIT_MUL:
                prosDesc.text = "치명타\n배율 증가";
                break;
            case RebirthType.CRIT_CHANCE:
                prosDesc.text = "치명타\n확률 증가";
                break;
            case RebirthType.RELOAD:
                prosDesc.text = "재장전\n속도 증가";
                break;
            case RebirthType.ROLL:
                prosDesc.text = "구르기\n배우기";
                break;
        }

        consImg.sprite = cons.GetIcon((int)RebirthType.END);
        consDesc.text = "생명력\n" + _bloodRequire.ToString() + " 감소";
    }

    public void SetRandomRebirthType()
    {
        if (!GameInstance.Instance.playerController.status.rollExp)
            _rebirthType = (RebirthType)Random.Range(0, (int)(RebirthType.END));
        else while (_rebirthType == RebirthType.ROLL)
            _rebirthType = (RebirthType)Random.Range(0, (int)(RebirthType.END));

        SetRebirthType(_rebirthType);
    }

    public void RebirthSlot()
    {
        switch (_rebirthType)
        {
            case RebirthType.DMG:
                GameInstance.Instance.playerController.status.damage += 0.5f;
                break;
            case RebirthType.CRIT_MUL:
                GameInstance.Instance.playerController.status.criticalDamage += 1f;
                break;
            case RebirthType.CRIT_CHANCE:
                GameInstance.Instance.playerController.status.criticalChance += 0.1f;
                break;
            case RebirthType.RELOAD:
                GameInstance.Instance.playerController.status.reloadSpeed -= 0.05f;
                break;
            case RebirthType.ROLL:
                GameInstance.Instance.playerController.status.rollExp = true;
                break;
        }

        Debug.Log(GameInstance.Instance.playerController.status.maxHp);
        Debug.Log(GameInstance.Instance.playerController.status.hp);
        GameInstance.Instance.playerController.status.maxHp -= _bloodRequire;
        GameInstance.Instance.playerController.status.hp = Mathf.Min(GameInstance.Instance.playerController.status.hp, GameInstance.Instance.playerController.status.maxHp);
        Debug.Log(_bloodRequire);
        Debug.Log(GameInstance.Instance.playerController.status.maxHp);
        Debug.Log(GameInstance.Instance.playerController.status.hp);
    }
}
