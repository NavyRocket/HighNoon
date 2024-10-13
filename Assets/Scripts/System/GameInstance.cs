using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    static float mobSpawnPositionScope = 8f;

    public PHASE phase { get; set; }

    [SerializeField] Camera mainCamera;
    [SerializeField] public PlayerController playerController;
    [SerializeField] float near = 5f;
    public CameraController cameraController;

    [SerializeField] GameObject _canvas;
    [SerializeField] Inventory _inventory;
    private GameObject myInventory;

    [SerializeField] Transform trainObjects;
    [SerializeField] Transform railObjects;
    [SerializeField] GameObject trainPassengerPrefab;
    [SerializeField] GameObject trainCargoPrefab;
    [SerializeField] GameObject trainEnginePrefab;
    [SerializeField] GameObject cargo1;
    [SerializeField] GameObject cargo2;
    [SerializeField] GameObject cargo3;
    [SerializeField] GameObject railPrefab;
    [SerializeField] float railSpeed;

    [SerializeField] public Transform mobPool;
    [SerializeField] GameObject mobA_Prefab;
    [SerializeField] GameObject mobB_Prefab;
    [SerializeField] GameObject mobC_Prefab;
    [SerializeField] Vector2 mobNumScope = Vector2.zero;

    public GameObject mobB_healPrefab;
    public GameObject mobB_atkPrefab;
    public ObjectPool<ParticleSystem> mobB_healPool;
    public ObjectPool<ParticleSystem> mobB_atkPool;
    [SerializeField] Transform mobB_healPoolObject;
    [SerializeField] Transform mobB_atkPoolObject;

    Dictionary<int, GameObject> trainByIndex = new Dictionary<int, GameObject>();
    LinkedList<GameObject> railDeque = new LinkedList<GameObject>();

    public int currentTrainIndex { get; set; }

    void Start()
    {
        phase = PHASE.PHASE1;

        cameraController = mainCamera.GetComponent<CameraController>();

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

        myInventory = Instantiate(_inventory.gameObject, _canvas.transform);
        myInventory.gameObject.SetActive(false);
    }

    void Update()
    {
        RepeatRail();

        if (Input.GetKeyDown(KeyCode.I))
            myInventory.GetComponent<Inventory>().ToggleInventory();
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

        if (railDeque.First.Value.transform.localPosition.x <= GameInstance.Instance.playerController.transform.position.x - railInterval)
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
        GameInstance.Instance.currentTrainIndex -= 1;
    }

    public void EnterTrainRight()
    {
        GameInstance.Instance.currentTrainIndex += 1;
        if (!trainByIndex.ContainsKey(currentTrainIndex + 1))
        {
            trainByIndex.Add(currentTrainIndex + 1, Instantiate(trainPassengerPrefab, trainObjects));
            trainByIndex[currentTrainIndex + 1].transform.localPosition = trainByIndex[0].transform.localPosition;
            trainByIndex[currentTrainIndex + 1].transform.localPosition = new Vector3(trainByIndex[currentTrainIndex + 1].transform.localPosition.x + (currentTrainIndex + 1) * trainInterval,
                trainByIndex[currentTrainIndex + 1].transform.localPosition.y, trainByIndex[currentTrainIndex + 1].transform.localPosition.z);

            int mobCount = Random.Range((int)mobNumScope.x, (int)mobNumScope.y);
            for (int i = 0; i < mobCount; ++i)
            {
                SpawnEnemy((ENEMY)Random.Range(0, (int)ENEMY.END));
            }
        }
    }

    private void SpawnEnemy(ENEMY type)
    {
        GameObject mob;

        switch (phase)
        {
        case PHASE.PHASE1:
        {
            mob = Instantiate(mobC_Prefab, mobPool);
            mob.transform.localPosition = new Vector3(trainByIndex[currentTrainIndex + 1].transform.position.x + Random.Range(-mobSpawnPositionScope, mobSpawnPositionScope), mob.transform.position.y, 0f);
        }
        break;
        case PHASE.PHASE2:
        {
            switch (type)
            {
                case ENEMY.B:
                    mob = Instantiate(mobB_Prefab, mobPool);
                    mob.transform.localPosition = new Vector3(trainByIndex[currentTrainIndex + 1].transform.position.x + Random.Range(-mobSpawnPositionScope, mobSpawnPositionScope), mob.transform.position.y, 0f);
                    break;

                case ENEMY.C:
                    mob = Instantiate(mobC_Prefab, mobPool);
                    mob.transform.localPosition = new Vector3(trainByIndex[currentTrainIndex + 1].transform.position.x + Random.Range(-mobSpawnPositionScope, mobSpawnPositionScope), mob.transform.position.y, 0f);
                    break;
            }
        }
        break;
        }

        
    }
}
