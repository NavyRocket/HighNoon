using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemController : MonoBehaviour
{
    [SerializeField] Sprite[] _iconSprites;
    [SerializeField] TextMeshProUGUI _quantity;

    ItemInfo _itemInfo;

    public void SetItem(ItemInfo itemInfo)
    {
        //GameObject quantity = gameObject.transform.parent.transform.GetChild(1).gameObject;

        _itemInfo = itemInfo;

        GetComponent<Image>().sprite = _iconSprites[Random.Range(0, _iconSprites.Length)];

        if (itemInfo.count == 1)
        {
            _quantity.text = "1";
            _quantity.enabled = false;
        }
        else
        {
            _quantity.enabled = true;
            _quantity.text = itemInfo.count.ToString();
        }

    }

    public void UseItem(Inventory inventory)
    {
        if (_itemInfo.count > 2)
        {
            _itemInfo.count--;
            _quantity.text = _itemInfo.count.ToString();

            inventory.ResetTitle();
        }
        else if (_itemInfo.count == 2)
        {
            _itemInfo.count--;
            _quantity.text = _itemInfo.count.ToString();
            _quantity.enabled = false;

            inventory.ResetTitle();
        }
        else if (_itemInfo.count == 1)
        {
            _itemInfo.count--;
            _quantity.text = _itemInfo.count.ToString();
            GetComponent<Image>().sprite = null;
            gameObject.SetActive(false);
            inventory.ItemNum--;

            inventory.ResetTitle();
        }
        else if (_itemInfo.count == 0)
        {
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public Sprite GetIcon(int index)
    {
        return _iconSprites[index];
    }
}
