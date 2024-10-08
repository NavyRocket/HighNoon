using UnityEngine;

[System.Serializable]
public class GameData
{
    public float playTime;  // �÷��� �ð�
    public Vector3 playerPosition;  // �÷��̾� ��ġ
    public int playerScore;  // �÷��̾� ����
}

public class GameDataManager : MonoBehaviour
{
    public Transform player;  // �÷��̾� ������Ʈ
    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    public void SaveGameData(int slot)
    {
        GameData data = new GameData();
        data.playTime = Time.time - startTime;  // ���� �÷��� �ð� ���
        data.playerPosition = player.position;  // �÷��̾� ��ġ ����
        data.playerScore = 100;  // ���÷� ������ ���� ����

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
