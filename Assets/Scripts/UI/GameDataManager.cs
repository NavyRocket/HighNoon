using UnityEngine;

[System.Serializable]
public class GameData
{
    public float playTime;  // 플레이 시간
    public Vector3 playerPosition;  // 플레이어 위치
    public int playerScore;  // 플레이어 점수
}

public class GameDataManager : MonoBehaviour
{
    public Transform player;  // 플레이어 오브젝트
    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    public void SaveGameData(int slot)
    {
        GameData data = new GameData();
        data.playTime = Time.time - startTime;  // 현재 플레이 시간 계산
        data.playerPosition = player.position;  // 플레이어 위치 저장
        data.playerScore = 100;  // 예시로 임의의 점수 저장

        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString($"GameData_{slot}", jsonData);
        PlayerPrefs.Save();

        Debug.Log($"Game data saved in slot {slot}!");
    }

    public GameData LoadGameData(int slot)
    {
        if (PlayerPrefs.HasKey($"GameData_{slot}"))
        {
            string jsonData = PlayerPrefs.GetString($"GameData_{slot}");
            return JsonUtility.FromJson<GameData>(jsonData);
        }

        return null;
    }
}
