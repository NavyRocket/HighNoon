using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject menuUI;  // �޴� UI
    public GameObject loadUI;  // LOAD UI
    public GameObject soundUI; // Sound UI
    public GameObject saveUI;  // Save UI �߰�
    public Button[] saveButtons;  // 3���� ���̺� ��ư �迭
    public Button[] loadButtons;  // 3���� �ε� ��ư �迭
    public Text[] loadButtonTexts;  // �� ��ư�� �ش��ϴ� �÷���Ÿ�� �ؽ�Ʈ
    public GameDataManager gameDataManager;

    public GameObject healthUI;  // ü�� UI
    public GameObject ammoUI;    // �Ѿ� UI

    private bool isGamePaused = false;

    void Start()
    {
        loadUI.SetActive(false);
        soundUI.SetActive(false); // Sound UI ��Ȱ��ȭ
        saveUI.SetActive(false);  // Save UI ��Ȱ��ȭ
        for (int i = 0; i < loadButtons.Length; i++)
        {
            loadButtons[i].interactable = false;
            loadButtonTexts[i].text = "No Saved Data";
        }
        SetupSaveButtons();  // ���̺� ��ư �ʱ�ȭ
        ResumeGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
        {
            if (menuUI.activeSelf || loadUI.activeSelf || soundUI.activeSelf || saveUI.activeSelf)
            {
                ResumeGame();  // ESC �Ǵ� Return Ű�� �޴����� ���
            }
            else
            {
                PauseGame();  // ���� ���� �� ESC�� �޴� Ȱ��ȭ
            }
        }
    }

    // ���̺� ��ư ���� (�� ��ư�� ������ ������ �����ϵ���)
    private void SetupSaveButtons()
    {
        for (int i = 0; i < saveButtons.Length; i++)
        {
            int slot = i;  // ���� ������ ���� ��ȣ ����
            saveButtons[i].onClick.RemoveAllListeners();  // ���� ������ ����
            saveButtons[i].onClick.AddListener(() => SaveGame(slot));  // Ŭ�� �� SaveGame ȣ��
        }
    }

    // Save ��ư Ŭ�� �� Save UI�� �̵�
    public void ShowSaveMenu()
    {
        menuUI.SetActive(false);  // ���� �޴� ��Ȱ��ȭ
        saveUI.SetActive(true);   // Save UI Ȱ��ȭ
    }

    // ���Ժ��� ���� ����
    public void SaveGame(int slot)
    {
        gameDataManager.SaveGameData(slot);
        UpdateLoadButtons();  // ���̺� �� �ε� ��ư ������Ʈ
    }

    public void ShowLoadMenu()
    {
        menuUI.SetActive(false);
        loadUI.SetActive(true);
    }

    public void ShowSoundMenu()
    {
        menuUI.SetActive(false);
        soundUI.SetActive(true);
    }

    private void UpdateLoadButtons()
    {
        // ���̺�� �����͸� �� ��ư�� ������Ʈ
        for (int i = 0; i < loadButtons.Length; i++)
        {
            GameData savedData = gameDataManager.LoadGameData(i);

            if (savedData != null)
            {
                float playTimeInMinutes = savedData.playTime / 60f;  // �ʸ� ������ ��ȯ
                loadButtonTexts[i].text = $"Play Time: {playTimeInMinutes:F2} minutes";
                loadButtons[i].interactable = true;
                int slot = i;  // ���� ������ ���� ����
                loadButtons[i].onClick.RemoveAllListeners();  // ���� ������ ����
                loadButtons[i].onClick.AddListener(() => LoadGame(slot));  // �� ���� �ε� ��� �߰�
            }
        }
    }

    public void LoadGame(int slot)
    {
        GameData loadedData = gameDataManager.LoadGameData(slot);
        if (loadedData != null)
        {
            Debug.Log($"Loaded Play Time: {loadedData.playTime:F2} seconds");

            gameDataManager.player.position = loadedData.playerPosition;
            int playerScore = loadedData.playerScore;

            Debug.Log($"Loaded Player Position: {loadedData.playerPosition}");
            Debug.Log($"Loaded Player Score: {playerScore}");
        }
        else
        {
            Debug.LogError("�ε��� �����Ͱ� �����ϴ�.");
        }

        loadUI.SetActive(false);
        menuUI.SetActive(true);
    }

    public void BackToMenu()
    {
        loadUI.SetActive(false);
        soundUI.SetActive(false);
        saveUI.SetActive(false);  // Save UI ��Ȱ��ȭ
        menuUI.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isGamePaused = true;
        menuUI.SetActive(true);
        loadUI.SetActive(false);
        soundUI.SetActive(false);
        saveUI.SetActive(false);  // Save UI ��Ȱ��ȭ

        if (healthUI != null) healthUI.SetActive(false);
        if (ammoUI != null) ammoUI.SetActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isGamePaused = false;
        menuUI.SetActive(false);
        loadUI.SetActive(false);
        soundUI.SetActive(false);
        saveUI.SetActive(false);  // Save UI ��Ȱ��ȭ

        if (healthUI != null) healthUI.SetActive(true);
        if (ammoUI != null) ammoUI.SetActive(true);
    }
}
