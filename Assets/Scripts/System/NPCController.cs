using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public SpeechController speech;
    [SerializeField] private float noticeDistance = 3f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrepareMessage()
    {
        StartCoroutine(WaitForPlayer());
    }

    IEnumerator WaitForPlayer()
    {
        yield return new WaitUntil(() => Vector3.Distance(transform.position, GameInstance.Instance.playerController.transform.position) <= noticeDistance);
        speech.Speak(0f, "결국 도달했군...");
        speech.Speak(2f, "무운을 비네...");
    }
}
