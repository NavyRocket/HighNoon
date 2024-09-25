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
        // T 키를 누르면 정신력 감소 (테스트용)
        if (Input.GetKeyDown(KeyCode.T))
        {
            DecreaseSanity(5);
        }
    }

    // 정신력을 감소시키는 메서드
    public void DecreaseSanity(float amount)
    {
        // 정신력 감소
        sanity -= amount;
        // 정신력을 0과 최대 정신력 사이로 클램핑
        sanity = Mathf.Clamp(sanity, 0, maxSanity);
        // 화면 어두움 업데이트
        UpdateDarkness();
    }

    // 화면의 어두움을 업데이트하는 메서드
    void UpdateDarkness()
    {
        // 현재 정신력에 따라 알파값 계산
        float alphaValue = 1 - (sanity / maxSanity);
        // 어두운 이미지의 색상 업데이트 (알파값 변경)
    //  darkenerImage.color = new Color(0, 0, 0, alphaValue);
    }
}
