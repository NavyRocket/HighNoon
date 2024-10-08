using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_UI : MonoBehaviour
{
    public float health = 100.0f; // �÷��̾� ü��

    public Image hpBar; // ü�� �� �̹���
    private readonly Color initColor = new Color(0, 1.0f, 0, 1.0f); // ü�� �� �ʱ� ����
    private Color currColor; // ���� ü�� �� ����

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    void Start()
    {
        // ü�� UI �ʱ� ����
        currColor = initColor;
        hpBar.color = initColor;
        hpBar.fillAmount = health / 100.0f;
    }

    void Update()
    {
        // �׽�Ʈ�� ü�� ����
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(5.0f);
        }
    }

    void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Player HP = " + health.ToString());
        DisplayHpbar(); // ü�� �� ������Ʈ

        if (health <= 0.0f)
        {
            PlayerDie();
        }
    }

    //�÷��̾� ���
    void PlayerDie()
    {
        OnPlayerDie?.Invoke(); // �÷��̾� ��� �̺�Ʈ ȣ��
        Destroy(gameObject); // ���ӿ���
    }

    //ü�� UI����
    void DisplayHpbar()
    {
        // ü���� ���� �̻��� �� ������ ����
        if ((health / 100.0f) > 0.5f)
            currColor.r = (1 - (health / 100.0f)) * 2.0f;
        // ü���� ���� ������ �� ��� ����
        else
            currColor.g = (health / 100.0f) * 2.0f;

        hpBar.color = currColor; // ü�� �� ���� ������Ʈ
        hpBar.fillAmount = health / 100.0f; // ü�� �� ä���
    }
}