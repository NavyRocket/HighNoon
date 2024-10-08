using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet_UI : MonoBehaviour
{
    public Image[] bulletImages; // �Ѿ� UI �̹��� �迭
    private int currentBulletIndex = 0; // ���� ��Ȱ��ȭ�� �Ѿ� �̹��� �ε���
    private int maxBullets = 6; // �Ѿ� �ִ� ����

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
        // R ��ư�� ������ �� �Ѿ� UI �ʱ�ȭ
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ResetBulletUI());
        }
    }

    // �Ѿ� �߻� �� UI ������Ʈ �Լ�
    void UpdateBulletUI()
    {
        if (currentBulletIndex < maxBullets)
        {
            bulletImages[currentBulletIndex].enabled = false;
            currentBulletIndex++;

            // 6���� �� ��� �ٽ� ��� �Ѿ� �̹����� Ȱ��ȭ
            if (currentBulletIndex >= maxBullets)
            {
                StartCoroutine(ResetBulletUI());
            }
        }
    }

    // 6���� �� �� �Ǵ� R ��ư�� ������ �� ��� �Ѿ� �̹����� �ٽ� Ȱ��ȭ�ϴ� �Լ�
    IEnumerator ResetBulletUI()
    {
        yield return new WaitForSeconds(1f); // 1�� �� ����
        for (int i = 0; i < bulletImages.Length; i++)
        {
            bulletImages[i].enabled = true;
        }
        currentBulletIndex = 0;
    }
}
