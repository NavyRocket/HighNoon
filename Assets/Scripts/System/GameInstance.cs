using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameInstance : SingletonMonoBehaviour<GameInstance>
{
    static float trainInterval = 17.35f;
    static float railInterval = 15.98f;

    [SerializeField] Camera mainCamera;
    [SerializeField] PlayerController playerController;
    [SerializeField] float near = 5f;

    [SerializeField] Transform trainObjects;
    [SerializeField] Transform railObjects;
    [SerializeField] GameObject trainPrefab;
    [SerializeField] GameObject railPrefab;
    [SerializeField] float railSpeed;

    Dictionary<int, GameObject> trainByIndex = new Dictionary<int, GameObject>();
    LinkedList<GameObject> railDeque = new LinkedList<GameObject>();

    public int currentTrainIndex { get; set; }

    void Start()
    {
        currentTrainIndex = 0;
        if (trainByIndex.Count == 0)
        {
            trainByIndex.Add(0, Instantiate(trainPrefab, trainObjects));
            trainByIndex.Add(1, Instantiate(trainPrefab, trainObjects));
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

    void Update()
    {
        RepeatRail();
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
            trainByIndex.Add(currentTrainIndex + 1, Instantiate(trainPrefab, trainObjects));
            trainByIndex[currentTrainIndex + 1].transform.localPosition = trainByIndex[0].transform.localPosition;
            trainByIndex[currentTrainIndex + 1].transform.localPosition = new Vector3(trainByIndex[currentTrainIndex + 1].transform.localPosition.x + (currentTrainIndex + 1) * trainInterval,
                trainByIndex[currentTrainIndex + 1].transform.localPosition.y, trainByIndex[currentTrainIndex + 1].transform.localPosition.z);
        }
    }

    public void SpawnEnemy(ENEMY type, int count)
    {

    }
}
