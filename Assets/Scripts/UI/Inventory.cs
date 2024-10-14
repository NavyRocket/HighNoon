using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _title;
    [SerializeField] GameObject _slotPrefab;
    [SerializeField] GameObject _slotContainer;
    [SerializeField] int _maxSlot = 40;
    private int _itemNum = 0;
    public int ItemNum { get { return _itemNum; } set { _itemNum = value; } }

    //[SerializeField] ItemController _itemController; (?)

    List<SlotController> _slotList = new List<SlotController>();

    public void ResetTitle()
    {
        _title.text = "가방 (" + _itemNum.ToString() + "/" + _slotList.Count.ToString() + ")";
    }

    public void AddSlot(bool initialSlot)
    {
        var obj = Instantiate(_slotPrefab, _slotContainer.transform);
        //obj.transform.SetParent(_slotContainer.transform); Instantiate의 두 번째 오버로딩 대신 사용가능

        var slot = obj.GetComponent<SlotController>();
        slot.InitSlot(this);
        _slotList.Add(slot);
        if (!initialSlot)
        {
            slot.GetComponent<Animator>().SetTrigger("slotAdd");
            ResetTitle();
        }

    }
    public void AddSlot(int count)
    {
        for (int i = 0; i < count; i++)
            AddSlot(true);
    }

    public void RemoveSlot()
    {
        if (_itemNum < _slotList.Count)
        {
            if (_slotList[_slotList.Count - 1].isEmpty)
            {
                _slotList[_slotList.Count - 1].GetComponent<Animator>().SetTrigger("slotRemove");
                Destroy(_slotList[_slotList.Count - 1].gameObject, 0.25f);
                _slotList.RemoveAt(_slotList.Count - 1);
                ResetTitle();
            }
            else
            {
                for (int i = _slotList.Count - 1; i >= 0; i--)
                {
                    if (_slotList[i].isEmpty)
                    {
                        _slotList[i].GetComponent<Animator>().SetTrigger("slotRemove");
                        Destroy(_slotList[i].gameObject, 0.25f);
                        _slotList.RemoveAt(i);
                        ResetTitle();
                        break;
                    }
                }
            }
        }
        else { /*경고문*/ }
    }

    public void OnSlotSelect(SlotController slot)
    {
        for (int i = 0; i < _slotList.Count; i++)
        {
            if (_slotList[i].IsSlotSelected)
            {
                _slotList[i].IsSlotSelected = false;
                _slotList[i].GetComponent<Animator>().SetTrigger("slotUnselected");
                break;
            }
        }
        slot.IsSlotSelected = !slot.IsSlotSelected;
    }

    // Start is called before the first frame update
    void Start()
    {
        AddSlot(_maxSlot);

        InitItemTable();

        ResetTitle();
    }

    public void Toggle()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        else
        {
            GetComponent<Animator>().Play("Hide");
        }
    }
    Dictionary<ItemID, ItemData> _itemTable = new Dictionary<ItemID, ItemData>();

    void InitItemTable()
    {
        AddItemTable(ItemID.Ball, 1, 10);
        AddItemTable(ItemID.BowlingBall, 2, 10);
        AddItemTable(ItemID.Bomb, 3, 10);
        AddItemTable(ItemID.Coin, 4, 10);
        AddItemTable(ItemID.Hat, 5, 10);
        AddItemTable(ItemID.Magnet, 6, 10);
    }

    void AddItemTable(ItemID itemID, int icon, int value)
    {
        _itemTable.Add(itemID, new ItemData() { itemID = itemID, icon = icon, value = value });
    }

    public void CreateItem()
    {
        if (_itemNum < _slotList.Count)
        {
            var itemID = (ItemID)Random.Range((int)ItemID.Ball, (int)ItemID.Max);
            var itemData = _itemTable[itemID];
            var itemInfo = new ItemInfo() { data = itemData, count = Random.Range(1, 100) };

            _itemNum++;

            for (int i = 0; i < _slotList.Count; i++)
            {
                if (_slotList[i].isEmpty)
                {
                    _slotList[i].slotEnable();
                    _slotList[i].GetComponentInChildren<ItemController>().SetItem(itemInfo);
                    ResetTitle();
                    break;
                }
            }
        }
        else { /*경고문*/ }
    }

    public void UseItem()
    {
        //var result = _slotList.Find(obj => obj.IsSlotSelected);
        //if (result != null)
        //{
        //    result.GetComponentInChildren<ItemController>().UseItem();
        //}

        for (int i = 0; i < _slotList.Count; i++)
        {
            if (_slotList[i].IsSlotSelected && !_slotList[i].isEmpty)
            {
                _slotList[i].GetComponentInChildren<ItemController>().UseItem(this);
            }
        }
    }
}
