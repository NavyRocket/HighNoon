using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public enum PHASE
{
    PHASE1,
    PHASE2,
    BOSS
}
public enum ItemID
{
    None = -1,
    Ball,
    BowlingBall,
    Bomb,
    Coin,
    Hat,
    Magnet,
    Max
}

public struct ItemData
{
    public ItemID itemID;
    public int icon;
    public int value;
}

public struct ItemInfo
{
    public ItemData data;
    public int count;
}

public class GameInstance : SingletonMonoBehaviour<GameInstance>
{
    static float trainInterval = 17.35f;
    static float railInterval = 15f;
    static float mobSpawnPositionScope = 6.5f;
    static float npcY = 1.14f;

    [ReadOnly]
    public PHASE phase = PHASE.PHASE1;
    [SerializeField, ReadOnly]
    private int _score = 0;
    public int score { get { return _score; } private set { _score = value; } }

    [SerializeField] Camera mainCamera;
    [SerializeField] public PlayerController playerController;
    [SerializeField] float near = 5f;
    public CameraController cameraController;

    [SerializeField] Canvas _canvas;
    [SerializeField] CanvasGroup _heartPanelCG;
    [SerializeField] Inventory _inventory;
    [SerializeField] RebirthMenu _rebirthMenu;
    [HideInInspector] public Inventory myInventory;
    [HideInInspector] public RebirthMenu myRebirthMenu;

    public CanvasGroup heartPanelCG { get { return _heartPanelCG; } }

    [SerializeField] Transform trainObjects;
    [SerializeField] Transform railObjects;
    [SerializeField] GameObject trainPassengerPrefab;
    [SerializeField] GameObject trainCargoPrefab;
    [SerializeField] GameObject trainEmptyPrefab;
    [SerializeField] GameObject trainEnginePrefab;
    [SerializeField] GameObject cargo1;
    [SerializeField] GameObject cargo2;
    [SerializeField] GameObject cargo3;
    [SerializeField] GameObject railPrefab;
    [SerializeField] float railSpeed;

    [SerializeField] public Transform mobPool;
    [SerializeField] GameObject npc_Prefab;
    [SerializeField] GameObject mobA_Prefab;
    [SerializeField] GameObject mobB_Prefab;
    [SerializeField] GameObject mobC_Prefab;
    [SerializeField] GameObject boss_Prefab;
    [SerializeField] Vector2 mobNumScope = Vector2.zero;

    public NPCController npc;
    public BossController boss;
    public GameObject hitPlayerPrefab;
    public GameObject hitMobPrefab;
    public GameObject hitMobCriticalPrefab;
    public GameObject mobB_healPrefab;
    public GameObject mobB_atkPrefab;
    public ObjectPool<ParticleSystem> hitPlayerPool;
    public ObjectPool<ParticleSystem> hitMobPool;
    public ObjectPool<ParticleSystem> hitMobCriticalPool;
    public ObjectPool<ParticleSystem> mobB_healPool;
    public ObjectPool<ParticleSystem> mobB_atkPool;
    [SerializeField] Transform hitPlayerPoolObject;
    [SerializeField] Transform hitMobPoolObject;
    [SerializeField] Transform hitMobCriticalPoolObject;
    [SerializeField] Transform mobB_healPoolObject;
    [SerializeField] Transform mobB_atkPoolObject;

    Dictionary<int, GameObject> trainByIndex = new Dictionary<int, GameObject>();
    LinkedList<GameObject> railDeque = new LinkedList<GameObject>();
    public int currentTrainIndex { get; set; }
    public bool bossSceneReady => _score >= 1;
    [HideInInspector] public bool bossPeak = false;
    private bool spawnRightTrain = true;
    private int engineIndex = 0;

    void Start()
    {
        phase = PHASE.PHASE1;
        score = 0;

        cameraController = mainCamera.GetComponent<CameraController>();

        npc = Instantiate(npc_Prefab).GetComponent<NPCController>();
        npc.transform.localPosition = new Vector3(playerController.transform.position.x + 5f, npcY, 0f);
        npc.speech.Speak(1f, "오랜만이군...");

        hitPlayerPool = new ObjectPool<ParticleSystem>(() =>
        {
            var obj = Instantiate(hitPlayerPrefab, hitPlayerPoolObject);
            obj.SetActive(false);
            var ps = obj.GetComponent<ParticleSystem>();
            return ps;
        }, 1);

        hitMobPool = new ObjectPool<ParticleSystem>(() =>
        {
            var obj = Instantiate(hitMobPrefab, hitMobPoolObject);
            obj.SetActive(false);
            var ps = obj.GetComponent<ParticleSystem>();
            return ps;
        }, 3);

        hitMobCriticalPool = new ObjectPool<ParticleSystem>(() =>
        {
            var obj = Instantiate(hitMobCriticalPrefab, hitMobCriticalPoolObject);
            obj.SetActive(false);
            var ps = obj.GetComponent<ParticleSystem>();
            return ps;
        }, 3);

        mobB_healPool = new ObjectPool<ParticleSystem>(() =>
        {
            var obj = Instantiate(mobB_healPrefab, mobB_healPoolObject);
            obj.SetActive(false);
            var ps = obj.GetComponent<ParticleSystem>();
            return ps;
        }, 3);

        mobB_atkPool = new ObjectPool<ParticleSystem>(() =>
        {
            var obj = Instantiate(mobB_atkPrefab, mobB_atkPoolObject);
            obj.SetActive(false);
            var ps = obj.GetComponent<ParticleSystem>();
            return ps;
        }, 1);

        currentTrainIndex = 0;
        if (trainByIndex.Count == 0)
        {
            trainByIndex.Add(0, Instantiate(trainPassengerPrefab, trainObjects));
            trainByIndex.Add(1, Instantiate(trainPassengerPrefab, trainObjects));
            trainByIndex[1].transform.localPosition = trainByIndex[0].transform.localPosition;
            trainByIndex[1].transform.localPosition = new Vector3(trainByIndex[1].transform.localPosition.x + 1 * trainInterval,
                trainByIndex[1].transform.localPosition.y, trainByIndex[1].transform.localPosition.z);
        }
        if (railDeque.Count == 0)
        {
            railDeque.AddLast(Instantiate(railPrefab, railObjects));
            railDeque.Last.Value.transform.localPosition = new Vector3(-railInterval, -0.5f, 0.8f);
            railDeque.AddLast(Instantiate(railPrefab, railObjects));
            railDeque.Last.Value.transform.localPosition = new Vector3(0f, -0.5f, 0.8f);
            railDeque.AddLast(Instantiate(railPrefab, railObjects));
            railDeque.Last.Value.transform.localPosition = new Vector3(railInterval, -0.5f, 0.8f);
        }

        myInventory = Instantiate(_inventory.gameObject, _canvas.transform).GetComponent<Inventory>();
        myInventory.gameObject.SetActive(false);
        myRebirthMenu = Instantiate(_rebirthMenu.gameObject, _canvas.transform).GetComponent<RebirthMenu>();
        myRebirthMenu.gameObject.SetActive(false);
    }

    void Update()
    {
        RepeatRail();

        if (Input.GetKeyDown(KeyCode.I))
            myInventory.Toggle();
        if (Input.GetKeyDown(KeyCode.O))
            myRebirthMenu.Toggle();

        if (Input.GetKeyDown(KeyCode.Space))
            playerController.Damage(100f);
    }

    //  public Vector3 CursorWorldPosition(float? z = null)
    public Vector3 CursorWorldPosition()
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, near));
        worldPosition.z = 0f;
        return worldPosition;
    }

    public PlayerController GetPlayerController() { return playerController; }

    void RepeatRail()
    {
        for (LinkedListNode<GameObject> node = railDeque.First; node != null; node = node.Next)
        {
            Vector3 newValue = node.Value.transform.localPosition;
            newValue.x -= railSpeed * Time.deltaTime;
            node.Value.transform.localPosition = newValue;
        }

        if (railDeque.First.Value.transform.localPosition.x <= playerController.transform.position.x - railInterval)
        {
            GameObject firstValue = railDeque.First.Value;
            railDeque.RemoveFirst();
            railDeque.AddLast(firstValue);
            Vector3 newValue = railDeque.Last.Value.transform.localPosition;
            newValue.x += railInterval * 2f;
            railDeque.Last.Value.transform.localPosition = newValue;
        }
    }

    public void EnterTrainLeft()
    {
        currentTrainIndex -= 1;
    }

    public void EnterTrainRight()
    {
        currentTrainIndex += 1;

        if (!spawnRightTrain)
            return;

        if (!trainByIndex.ContainsKey(currentTrainIndex + 1))
        {
            int mobCount = 0;
            if (bossSceneReady && !bossPeak)
            {
                spawnRightTrain = false;
                engineIndex = currentTrainIndex + 5;
                mobCount = Random.Range((int)mobNumScope.x, (int)mobNumScope.y);
                trainByIndex.Add(currentTrainIndex + 1, Instantiate(trainCargoPrefab, trainObjects));
                trainByIndex.Add(currentTrainIndex + 2, Instantiate(trainPassengerPrefab, trainObjects));
                trainByIndex.Add(currentTrainIndex + 3, Instantiate(trainEmptyPrefab, trainObjects));
                trainByIndex.Add(currentTrainIndex + 4, Instantiate(trainEmptyPrefab, trainObjects));
                trainByIndex.Add(currentTrainIndex + 5, Instantiate(trainEnginePrefab, trainObjects));

                for (int i = 1; i <= 5; ++i)
                {
                    trainByIndex[currentTrainIndex + i].transform.localPosition = trainByIndex[0].transform.localPosition;
                    trainByIndex[currentTrainIndex + i].transform.localPosition = new Vector3(
                        trainByIndex[currentTrainIndex + i].transform.localPosition.x + (currentTrainIndex + i) * trainInterval,
                        trainByIndex[currentTrainIndex + i].transform.localPosition.y,
                        trainByIndex[currentTrainIndex + i].transform.localPosition.z);
                }
                trainByIndex[currentTrainIndex + 5].transform.localPosition = new Vector3(
                    trainByIndex[currentTrainIndex + 5].transform.localPosition.x - 5.23f,
                    trainByIndex[currentTrainIndex + 5].transform.localPosition.y,
                    trainByIndex[currentTrainIndex + 5].transform.localPosition.z);

                trainByIndex[currentTrainIndex + 2].GetComponent<BoxCollider>().enabled = true;
                trainByIndex[currentTrainIndex + 2].GetComponent<TrainEventController>().enabled = true;

                npc.transform.position = new Vector3(trainByIndex[currentTrainIndex + 2].transform.position.x + 6.5f, npcY, 0f);
                npc.PrepareMessage();
            }
            else
            {
                switch (phase)
                {
                    case PHASE.PHASE1:
                        mobCount = currentTrainIndex;
                        trainByIndex.Add(currentTrainIndex + 1, Instantiate(trainPassengerPrefab, trainObjects));
                        break;
                    case PHASE.PHASE2:
                        mobCount = Random.Range((int)mobNumScope.x, (int)mobNumScope.y);
                        trainByIndex.Add(currentTrainIndex + 1, Instantiate(Random.value < 0.5f ? trainPassengerPrefab : trainCargoPrefab, trainObjects));
                        break;
                }

                trainByIndex[currentTrainIndex + 1].transform.localPosition = trainByIndex[0].transform.localPosition;
                trainByIndex[currentTrainIndex + 1].transform.localPosition = new Vector3(
                    trainByIndex[currentTrainIndex + 1].transform.localPosition.x + (currentTrainIndex + 1) * trainInterval,
                    trainByIndex[currentTrainIndex + 1].transform.localPosition.y,
                    trainByIndex[currentTrainIndex + 1].transform.localPosition.z);
            }

            for (int i = 0; i < mobCount; ++i)
            {
                SpawnEnemy((ENEMY)Random.Range(0, (int)ENEMY.BOSS));
            }
        }
    }

    public void BossPeak()
    {
        bossPeak = true;
        boss = Instantiate(boss_Prefab, mobPool).GetComponent<BossController>();
        boss.transform.position = new Vector3(trainByIndex[currentTrainIndex].transform.position.x, 1.6f, 4f);
        cameraController.SetTarget(boss.transform);
        boss.PhaseIn(0.5f);
        cameraController.SetTargetToPlayer(3f);
        playerController.SlowWalking();
    }

    public void PrepareBoss()
    {
        TimeManager.Instance.TimeScaleEaseOutSine(0.5f, 1f, 1.5f, 0.35f);
        StartCoroutine(TranslateBoss(new Vector3(trainByIndex[engineIndex].transform.position.x - 2f, boss.transform.position.y + 1f, boss.transform.position.z), 5f));
        VolumeManager.Instance.PaniniEffect(0.75f, 0.5f, 1f, 1f);
    }
    IEnumerator TranslateBoss(Vector3 destination, float duration)
    {
        Vector3 original = boss.transform.position;
        float timeAcc = 0f;
        while (timeAcc < duration)
        {
            timeAcc += Time.deltaTime;
            float ratio = timeAcc / duration;
            boss.transform.position = new Vector3(
                EasingFunction.EaseInOutSine(original.x, destination.x, ratio),
                EasingFunction.EaseInOutSine(original.y, destination.y, ratio),
                EasingFunction.EaseInOutSine(original.z, destination.z, ratio));
            yield return null;
        }
        boss.transform.position = destination;
    }

    public void PrepareOutside()
    {
        cameraController.WideView();
        VolumeManager.Instance.DOFOutside();
    }

    private void SpawnEnemy(ENEMY type)
    {
        GameObject mob = null;

        switch (phase)
        {
            case PHASE.PHASE1:
            {
                switch (type)
                {
                    case ENEMY.A:
                        mob = Instantiate(mobA_Prefab, mobPool);
                        break;
                    case ENEMY.B:
                        mob = Instantiate(Random.value < 0.5f ? mobA_Prefab : mobC_Prefab, mobPool);
                        break;
                    case ENEMY.C:
                        mob = Instantiate(mobC_Prefab, mobPool);
                        break;
                }
                mob.transform.localPosition = new Vector3(trainByIndex[currentTrainIndex + 1].transform.position.x + Random.Range(-mobSpawnPositionScope, mobSpawnPositionScope), mob.transform.position.y, 0f);
            }
            break;
            case PHASE.PHASE2:
            {
                switch (type)
                {
                    case ENEMY.A:
                        mob = Instantiate(mobA_Prefab, mobPool);
                        break;
                    case ENEMY.B:
                        mob = Instantiate(mobB_Prefab, mobPool);
                        break;
                    case ENEMY.C:
                        mob = Instantiate(mobC_Prefab, mobPool);
                        break;
                }
                mob.transform.localPosition = new Vector3(trainByIndex[currentTrainIndex + 1].transform.position.x + Random.Range(-mobSpawnPositionScope, mobSpawnPositionScope), mob.transform.position.y, 0f);
            }
            break;
        }
    }

    public int IncreaseScore(int value)
    {
        score += value;
        return score;
    }

    public void ResetLevel()
    {
        spawnRightTrain = true;
        TrainEventController.Reset();
        switch (phase)
        {
            case PHASE.PHASE1:
                npc.speech.Speak(2f, "많이 늙었군...");
                break;
            case PHASE.PHASE2:
                switch (Random.Range(0, 2))
                {
                    case 0:
                        npc.speech.Speak(2f, "그렇게 죽음을 거듭한다면...");
                        npc.speech.Speak(5f, "진짜 죽음이 그대를 덮칠걸세...");
                        break;
                    case 1:
                        npc.speech.Speak(2f, "자네 혹시...");
                        npc.speech.Speak(5f, "구르는 방법을 까먹은 것 아닌가?");
                        break;
                    case 2:
                        npc.speech.Speak(2f, "생명을 대가로 치루고서도...");
                        npc.speech.Speak(5f, "계속 나아갈 가치가 있는가?");
                        break;
                }
                break;
        }
        
        GameInstance.Instance.phase = PHASE.PHASE2;
        VolumeManager.Instance.OpenEye();

        trainByIndex.Clear();
        railDeque.Clear();
        foreach (Transform child in trainObjects)
            Destroy(child.gameObject);
        foreach (Transform child in railObjects)
            Destroy(child.gameObject);
        foreach (Transform child in mobPool)
            Destroy(child.gameObject);

        currentTrainIndex = 0;
        if (trainByIndex.Count == 0)
        {
            trainByIndex.Add(0, Instantiate(trainPassengerPrefab, trainObjects));
            trainByIndex.Add(1, Instantiate(trainPassengerPrefab, trainObjects));
            trainByIndex[1].transform.localPosition = trainByIndex[0].transform.localPosition;
            trainByIndex[1].transform.localPosition = new Vector3(trainByIndex[1].transform.localPosition.x + 1 * trainInterval,
                trainByIndex[1].transform.localPosition.y, trainByIndex[1].transform.localPosition.z);
        }
        if (railDeque.Count == 0)
        {
            railDeque.AddLast(Instantiate(railPrefab, railObjects));
            railDeque.Last.Value.transform.localPosition = new Vector3(-railInterval, -0.5f, 0.8f);
            railDeque.AddLast(Instantiate(railPrefab, railObjects));
            railDeque.Last.Value.transform.localPosition = new Vector3(0f, -0.5f, 0.8f);
            railDeque.AddLast(Instantiate(railPrefab, railObjects));
            railDeque.Last.Value.transform.localPosition = new Vector3(railInterval, -0.5f, 0.8f);
        }
    }
}
