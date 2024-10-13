using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    GameObject gameController;
    Animator animator;

    void Awake()
    {
        gameController = GameObject.FindWithTag("GameController");
        animator = GetComponent<Animator>();
    }

    public void CloseWindow()
    {
        animator.Play("Close");
    }

    public void HideWindow()
    {
        animator.Play("Hide");
    }

    public void CloseFunc()
    {
        Destroy(gameObject);
    }

    public void HideFunc()
    {
        gameObject.SetActive(false);
        //StartCoroutine(HideAfterDelay(5f));
    }

    IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
