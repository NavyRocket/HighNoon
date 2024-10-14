using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixToPlayer : MonoBehaviour
{
    private GameObject player;
    public bool x;
    public bool y;
    public bool z;

    public bool useLifetime = false;
    public float lifetime = 1f;
    private float timeAcc = 0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameInstance.Instance.playerController.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (useLifetime)
        {
            if (timeAcc < lifetime)
            {
                timeAcc += Time.deltaTime;
                transform.position = new Vector3(
                    x ? player.transform.position.x : transform.position.x,
                    y ? player.transform.position.y : transform.position.y,
                    z ? player.transform.position.z : transform.position.z);
            }
        }
        else
        {
            transform.position = new Vector3(
                x ? player.transform.position.x : transform.position.x,
                y ? player.transform.position.y : transform.position.y,
                z ? player.transform.position.z : transform.position.z);
        }
    }

    public void Reset()
    {
        timeAcc = 0f;
    }
}
