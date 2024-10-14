using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebirthMenu : MonoBehaviour
{
    [SerializeField] SlotController[] slots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            RandomizeSlot();
        }
        else
        {
            GetComponent<Animator>().Play("Hide");
            GameInstance.Instance.playerController.Rebirth();
        }
    }

    void RandomizeSlot()
    {
        foreach (SlotController slot in slots)
        {
            slot.SetRandomRebirthType();
        }
    }
}
