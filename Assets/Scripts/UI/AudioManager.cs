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

        // �����̴� �ʱ� ���� �߰� ������ ����
        volumeSlider.value = (volumeSlider.minValue + volumeSlider.maxValue) / 2;

        volumeSlider.onValueChanged.AddListener(OnVolumeChange);

        backgroundMusic.volume = volumeSlider.value;
    }

    void OnVolumeChange(float value)
    {
        backgroundMusic.volume = value;  // �����̴� ���� ���� ����� ���� ����
    }
}
