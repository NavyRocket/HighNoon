using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMY
{
    A,
    B,
    C,
    END
}

public class Enemy : MonoBehaviour
{
    public ENEMY enemy = ENEMY.END;

    public Transform poolObject;
    public GameObject mobC_FireballPrefab;
    public MobC_FireballController fireball;

    private Animator animator;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GameInstance.Instance.GetPlayerController();
    }

    // Update is called once per frame
    void Update()
    {
        FacePlayer();
    }

    void FacePlayer()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("MobC_Atk")
        ||  animator.GetCurrentAnimatorStateInfo(0).IsName("MobC_Die")
        ||  animator.GetCurrentAnimatorStateInfo(0).IsName("MobB_Atk1")
        ||  animator.GetCurrentAnimatorStateInfo(0).IsName("MobB_Atk2")
        ||  animator.GetCurrentAnimatorStateInfo(0).IsName("MobB_Die"))
            return;

        bool faceLeft = playerController.transform.position.x <= transform.position.x;
        transform.localRotation = Quaternion.Euler(0f, faceLeft ? 0f : 180f, 0f);
    }
}
