using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_UI : MonoBehaviour
{
    public float health = 100.0f; // 플레이어 체력

    public Image hpBar; // 체력 바 이미지
    private readonly Color initColor = new Color(0, 1.0f, 0, 1.0f); // 체력 바 초기 색상
    private Color currColor; // 현재 체력 바 색상

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    void Start()
    {
        // 체력 UI 초기 설정
        currColor = initColor;
        hpBar.color = initColor;
        hpBar.fillAmount = health / 100.0f;
    }

    void Update()
    {
        // 테스트용 체력 감소
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(5.0f);
        }
    }

    void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Player HP = " + health.ToString());
        DisplayHpbar(); // 체력 바 업데이트

        if (health <= 0.0f)
        {
            PlayerDie();
        }
    }

    //플레이어 사망
    void PlayerDie()
    {
        OnPlayerDie?.Invoke(); // 플레이어 사망 이벤트 호출
        Destroy(gameObject); // 게임오버
    }

    //체력 UI색깔
    void DisplayHpbar()
    {
        // 체력이 절반 이상일 때 빨간색 감소
        if ((health / 100.0f) > 0.5f)
            currColor.r = (1 - (health / 100.0f)) * 2.0f;
        // 체력이 절반 이하일 때 녹색 감소
        else
            currColor.g = (health / 100.0f) * 2.0f;

        hpBar.color = currColor; // 체력 바 색상 업데이트
        hpBar.fillAmount = health / 100.0f; // 체력 바 채우기
    }
}