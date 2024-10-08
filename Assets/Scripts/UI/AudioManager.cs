using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public Slider volumeSlider;

    void Start()
    {
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;

        // 슬라이더 초기 값을 중간 값으로 설정
        volumeSlider.value = (volumeSlider.minValue + volumeSlider.maxValue) / 2;

        volumeSlider.onValueChanged.AddListener(OnVolumeChange);

        backgroundMusic.volume = volumeSlider.value;
    }

    void OnVolumeChange(float value)
    {
        backgroundMusic.volume = value;  // 슬라이더 값에 따라 오디오 볼륨 조정
    }
}
