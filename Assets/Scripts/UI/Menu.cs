using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject menuUI;  // 메뉴 UI
    public GameObject loadUI;  // LOAD UI
    public GameObject soundUI; // Sound UI
    public GameObject saveUI;  // Save UI 추가
    public Button[] saveButtons;  // 3개의 세이브 버튼 배열
    public Button[] loadButtons;  // 3개의 로드 버튼 배열
    public Text[] loadButtonTexts;  // 각 버튼에 해당하는 플레이타임 텍스트
    public GameDataManager gameDataManager;

    public GameObject healthUI;  // 체력 UI
    public GameObject ammoUI;    // 총알 UI

    private bool isGamePaused = false;

    void Start()
    {
        loadUI.SetActive(false);
        soundUI.SetActive(false); // Sound UI 비활성화
        saveUI.SetActive(false);  // Save UI 비활성화
        for (int i = 0; i < loadButtons.Length; i++)
        {
            loadButtons[i].interactable = false;
            loadButtonTexts[i].text = "No Saved Data";
        }
        SetupSaveButtons();  // 세이브 버튼 초기화
        ResumeGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
        {
            if (menuUI.activeSelf || loadUI.activeSelf || soundUI.activeSelf || saveUI.activeSelf)
            {
                ResumeGame();  // ESC 또는 Return 키로 메뉴에서 벗어남
            }
            else
            {
                PauseGame();  // 게임 중일 때 ESC로 메뉴 활성화
            }
        }
    }

    // 세이브 버튼 설정 (각 버튼이 고유한 슬롯을 저장하도록)
    private void SetupSaveButtons()
    {
        for (int i = 0; i < saveButtons.Length; i++)
        {
            int slot = i;  // 로컬 변수로 슬롯 번호 고정
            saveButtons[i].onClick.RemoveAllListeners();  // 기존 리스너 제거
            saveButtons[i].onClick.AddListener(() => SaveGame(slot));  // 클릭 시 SaveGame 호출
        }
    }

    // Save 버튼 클릭 시 Save UI로 이동
    public void ShowSaveMenu()
    {
        menuUI.SetActive(false);  // 기존 메뉴 비활성화
        saveUI.SetActive(true);   // Save UI 활성화
    }

    // 슬롯별로 게임 저장
    public void SaveGame(int slot)
    {
        gameDataManager.SaveGameData(slot);
        UpdateLoadButtons();  // 세이브 후 로드 버튼 업데이트
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
        // 세이브된 데이터를 각 버튼에 업데이트
        for (int i = 0; i < loadButtons.Length; i++)
        {
            GameData savedData = gameDataManager.LoadGameData(i);

            if (savedData != null)
            {
                float playTimeInMinutes = savedData.playTime / 60f;  // 초를 분으로 변환
                loadButtonTexts[i].text = $"Play Time: {playTimeInMinutes:F2} minutes";
                loadButtons[i].interactable = true;
                int slot = i;  // 로컬 변수로 슬롯 저장
                loadButtons[i].onClick.RemoveAllListeners();  // 기존 리스너 제거
                loadButtons[i].onClick.AddListener(() => LoadGame(slot));  // 각 슬롯 로드 기능 추가
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
            Debug.LogError("로드할 데이터가 없습니다.");
        }

        loadUI.SetActive(false);
        menuUI.SetActive(true);
    }

    public void BackToMenu()
    {
        loadUI.SetActive(false);
        soundUI.SetActive(false);
        saveUI.SetActive(false);  // Save UI 비활성화
        menuUI.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isGamePaused = true;
        menuUI.SetActive(true);
        loadUI.SetActive(false);
        soundUI.SetActive(false);
        saveUI.SetActive(false);  // Save UI 비활성화

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
        saveUI.SetActive(false);  // Save UI 비활성화

        if (healthUI != null) healthUI.SetActive(true);
        if (ammoUI != null) ammoUI.SetActive(true);
    }
}
