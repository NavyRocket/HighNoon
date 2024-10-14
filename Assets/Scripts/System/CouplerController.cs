using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouplerController : MonoBehaviour
{
    static float engineTrainBossOffsetX = -0.5f;
    static float bossPositionY = 1.8f;
    static float bossPositionZ = 0.8f;

    [SerializeField] TrainEngineController engine;
    private int hp = 5;
    private bool firstHit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Damage()
    {
        if (hp > 0)
            hp -= 1;

        if (!firstHit)
        {
            firstHit = true;
            StartCoroutine(TranslateBoss(GameInstance.Instance.boss.transform.position + Vector3.up * 15f, 2f, 0f));
        }

        if (hp <= 0)
        {
            StartCoroutine(SendForSeconds(10f));
            StartCoroutine(TranslateBoss(new Vector3(engine.transform.position.x + engineTrainBossOffsetX, bossPositionY, bossPositionZ), 2f, 3f));
        }
    }

    IEnumerator SendForSeconds(float time)
    {
        engine.sendForward = true;
        yield return new WaitForSeconds(time);
        engine.sendForward = false;
    }

    IEnumerator TranslateBoss(Vector3 destination, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 original = GameInstance.Instance.boss.transform.position;
        float timeAcc = 0f;
        while (timeAcc < duration)
        {
            timeAcc += Time.deltaTime;
            float ratio = timeAcc / duration;
            GameInstance.Instance.boss.transform.position = new Vector3(
                EasingFunction.EaseInOutSine(original.x, destination.x, ratio),
                EasingFunction.EaseInOutSine(original.y, destination.y, ratio),
                EasingFunction.EaseInOutSine(original.z, destination.z, ratio));
            yield return null;
        }
        GameInstance.Instance.boss.transform.position = destination;
    }
}
