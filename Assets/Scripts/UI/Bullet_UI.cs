using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet_UI : MonoBehaviour
{
    public Image[] bulletImages; // 총알 UI 이미지 배열
    private int currentBulletIndex = 0; // 현재 비활성화된 총알 이미지 인덱스
    private int maxBullets = 6; // 총알 최대 개수

    void OnEnable()
    {
        BulletController.OnBulletFired += UpdateBulletUI;
    }

    void OnDisable()
    {
        BulletController.OnBulletFired -= UpdateBulletUI;
    }

    void Update()
    {
        // R 버튼이 눌렸을 때 총알 UI 초기화
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ResetBulletUI());
        }
    }

    // 총알 발사 시 UI 업데이트 함수
    void UpdateBulletUI()
    {
        if (currentBulletIndex < maxBullets)
        {
            bulletImages[currentBulletIndex].enabled = false;
            currentBulletIndex++;

            // 6발을 다 쏘면 다시 모든 총알 이미지를 활성화
            if (currentBulletIndex >= maxBullets)
            {
                StartCoroutine(ResetBulletUI());
            }
        }
    }

    // 6발을 쏜 후 또는 R 버튼을 눌렀을 때 모든 총알 이미지를 다시 활성화하는 함수
    IEnumerator ResetBulletUI()
    {
        yield return new WaitForSeconds(1f); // 1초 후 리셋
        for (int i = 0; i < bulletImages.Length; i++)
        {
            bulletImages[i].enabled = true;
        }
        currentBulletIndex = 0;
    }
}
