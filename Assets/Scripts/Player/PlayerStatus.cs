using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] float maxHp;
    [SerializeField] float maxSanity;
    [SerializeField] Image darkenerImage;

    float hp;
    float sanity;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        sanity = maxSanity;

        UpdateDarkness();
    }

    // Update is called once per frame
    void Update()
    {
        // T Ű�� ������ ���ŷ� ���� (�׽�Ʈ��)
        if (Input.GetKeyDown(KeyCode.T))
        {
            DecreaseSanity(5);
        }
    }

    // ���ŷ��� ���ҽ�Ű�� �޼���
    public void DecreaseSanity(float amount)
    {
        // ���ŷ� ����
        sanity -= amount;
        // ���ŷ��� 0�� �ִ� ���ŷ� ���̷� Ŭ����
        sanity = Mathf.Clamp(sanity, 0, maxSanity);
        // ȭ�� ��ο� ������Ʈ
        UpdateDarkness();
    }

    // ȭ���� ��ο��� ������Ʈ�ϴ� �޼���
    void UpdateDarkness()
    {
        // ���� ���ŷ¿� ���� ���İ� ���
        float alphaValue = 1 - (sanity / maxSanity);
        // ��ο� �̹����� ���� ������Ʈ (���İ� ����)
    //  darkenerImage.color = new Color(0, 0, 0, alphaValue);
    }
}
