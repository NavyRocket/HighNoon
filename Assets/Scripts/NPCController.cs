using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] SpeechController speech;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WelcomeMessage(float delay)
    {
        StartCoroutine(ExecuteAfterDelay(delay));
    }
    IEnumerator ExecuteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        speech.TextTypeWriter("오랜만이군...");
    }
}
