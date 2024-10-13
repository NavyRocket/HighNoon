using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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

    [ShowIf("enemy", ENEMY.A)]
    public Transform meleeCollider;
    [ShowIf("enemy", ENEMY.C)]
    public GameObject mobC_FireballPrefab;
    [HideInInspector]
    public Transform poolObject;
//  public MobC_FireballController fireball;

    private Animator animator;
    private PlayerController playerController;
    private BehaviorTreeRunner btRunner;

    [SerializeField, ReadOnly]
    private float _hp = 0f;
    public float hp
    {
        get {
            if (btRunner != null)
                _hp = btRunner.tree.blackboard.Get<float>("Hp");
            return _hp;
        }
        private set {
            _hp = value;
        }
    }
    public float maxHp = 10f;
    public bool overwriteBtHp = false;
    public bool isDead => hp <= 0f;

    private bool isMelee = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GameInstance.Instance.GetPlayerController();
        btRunner = GetComponent<BehaviorTreeRunner>();

        poolObject = GameObject.Find("Mob_Pool").transform;

        if (null == btRunner)
            _hp = maxHp;
        else
        {
            if (overwriteBtHp)
                btRunner.tree.blackboard.Set<float>("Hp", maxHp);
            else
                maxHp = btRunner.tree.blackboard.Get<float>("Hp");
        }

        hp = Mathf.Min(hp, maxHp);
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

    public void Heal(float amount)
    {
        if (amount <= 0f)
            return;

        if (!isDead)
            btRunner.tree.blackboard.Set<float>("Hp", Mathf.Min(hp + amount, maxHp));
    }

    public void Damage(float damage)
    {
        if (damage <= 0f)
            return;

        if (!isDead)
            btRunner.tree.blackboard.Set<float>("Hp", Mathf.Max(hp - damage, 0f));
    }

    public void DisableObject()
    {
        if (!isDead)
            return;

        switch (enemy)
        {
            case ENEMY.A:
                GameInstance.Instance.IncreaseScore(5);
                break;
            case ENEMY.B:
                GameInstance.Instance.IncreaseScore(15);
                break;
            case ENEMY.C:
                GameInstance.Instance.IncreaseScore(10);
                break;
        }

        DropItem();

        animator.enabled = false;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void MeleeStart()
    {
        isMelee = true;
        meleeCollider.gameObject.SetActive(true);
    }

    public void MeleeEnd()
    {
        isMelee = false;
        meleeCollider.gameObject.SetActive(false);
    }

    public void DropItem()
    {

    }
}
