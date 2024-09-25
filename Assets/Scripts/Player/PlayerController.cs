using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Serializable]
    public struct PlayerInfo
    {
        public float moveSpeed;
        public float sprintSpeed;
        public float rollForce;
        public float jumpForce;

        [HideInInspector] public float moveInput;
    }

    [Serializable]
    public struct GunInfo
    {
        public float maxAngle;
        public float minAngle;
    }

    [Serializable]
    public struct BulletInfo
    {
        public float coolTime;
        [HideInInspector] public float timeAcc;
    };

    [SerializeField] public GameObject gun;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletPoolObject;
    [SerializeField] private Transform muzzle;

    [SerializeField] PlayerInfo playerInfo;
    [SerializeField] GunInfo gunInfo;
    [SerializeField] BulletInfo bulletInfo;
    ObjectPool<BulletController> bulletPool;

    public bool roll { get; set; }
    public bool draw { get; set; }
    public bool aim { get; set; }

    public float gunRadian { get; set; }
    Vector3 gunOffset = new Vector3(0f, 0.75f, 0f);


    Animator animator;
    Rigidbody rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        bulletInfo.timeAcc = 0f;

        bulletPool = new ObjectPool<BulletController>(() =>
        {
            var obj = Instantiate(bulletPrefab, bulletPoolObject);
            obj.SetActive(false);
            var bullet = obj.GetComponent<BulletController>();
            bullet.InitBullet(this);
            return bullet;
        }, 6);

        roll = false;
        draw = false;
        aim = false;
        gunRadian = 0f;
    }

    void Update()
    {
        if (aim)
        {
            Vector3 vector = GameInstance.Instance.CursorWorldPosition() - transform.position - gunOffset;
            gunRadian = Mathf.Clamp(Mathf.Atan2(vector.y, transform.eulerAngles.y == 0 ? vector.x : -vector.x),
                gunInfo.minAngle * Mathf.Deg2Rad, gunInfo.maxAngle * Mathf.Deg2Rad);
            gun.transform.localRotation = Quaternion.Euler(0f, 0f, gunRadian * Mathf.Rad2Deg);
        }

        if (bulletInfo.timeAcc < bulletInfo.coolTime)
        {
            bulletInfo.timeAcc += Time.deltaTime;
        }
        if (aim && Input.GetMouseButtonDown(0) && bulletInfo.timeAcc >= bulletInfo.coolTime)
        {
            Invoke("FireBullet", 0f);
        }

        if (!roll)
        {
            if (!draw)
            {
                playerInfo.moveInput = Input.GetKey(KeyCode.LeftShift) ? Input.GetAxisRaw("Horizontal") * playerInfo.sprintSpeed : Input.GetAxisRaw("Horizontal");
            }
            else
            {
                playerInfo.moveInput = 0f;
            }
        }
        else if (!Input.anyKey)
        {
            playerInfo.moveInput = transform.eulerAngles.y == 0 ? playerInfo.rollForce : -playerInfo.rollForce;
        }

    //    if (!draw)
        {
            rb.AddForce(playerInfo.moveInput * playerInfo.moveSpeed * Time.deltaTime, 0f, 0f);
        }
    //  rb.velocity = new Vector2(playerInfo.moveInput * playerInfo.moveSpeed, rb.velocity.y * playerInfo.jumpForce);
    }

    public void RetrieveBullet(BulletController bullet)
    {
        bullet.gameObject.SetActive(false);
        bulletPool.Set(bullet);
    }

    void FireBullet()
    {
        bulletInfo.timeAcc = 0f;

        var bullet = bulletPool.Get();
        bullet.SetMuzzle(muzzle.position);
        bullet.gameObject.SetActive(true);
    }

    void Die()
    {

    }
}
