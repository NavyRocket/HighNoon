using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor.Experimental.GraphView;
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

        public float reboundDuration;
        public Vector3 reboundMagnitude;
    };

    [Serializable]
    public struct DamageInfo
    {
        [SerializeField] public float flashDuration;
        [SerializeField] public float damageCooldown;
        [SerializeField] public Vector2 knockbackForce;
    }

    [SerializeField] public GameObject gun;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform poolObject;
    [SerializeField] private Transform muzzle;

    [SerializeField] PlayerInfo playerInfo;
    [SerializeField] GunInfo gunInfo;
    [SerializeField] BulletInfo bulletInfo;
    [SerializeField] DamageInfo damageInfo;


    ObjectPool<BulletController> bulletPool;
    public bool canTakeDamage { get; set; }

    public bool roll { get; set; }
    public bool draw { get; set; }
    public bool aim { get; set; }

    public float gunRadian { get; set; }
    Vector3 gunOffset = new Vector3(0f, 0.75f, 0f);

    Animator animator;
    Rigidbody rb;
    SpriteRenderer sr;
    PlayerStatus status;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        sr = GetComponent<SpriteRenderer>();
        status = GetComponent<PlayerStatus>();

        bulletInfo.timeAcc = 0f;

        bulletPool = new ObjectPool<BulletController>(() =>
        {
            var obj = Instantiate(bulletPrefab, poolObject);
            obj.SetActive(false);
            var bullet = obj.GetComponent<BulletController>();
            bullet.InitBullet(this);
            return bullet;
        }, 6);

        roll = false;
        draw = false;
        aim = false;
        gunRadian = 0f;
        canTakeDamage = true;
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
            GameInstance.Instance.cameraController.Shake(bulletInfo.reboundDuration, bulletInfo.reboundMagnitude);
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
        if (!status.isDead)
        {
            rb.AddForce(playerInfo.moveInput * playerInfo.moveSpeed * Time.deltaTime, 0f, 0f);
        }
    //  rb.velocity = new Vector2(playerInfo.moveInput * playerInfo.moveSpeed, rb.velocity.y * playerInfo.jumpForce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canTakeDamage)
        {
            if (other.CompareTag("Enemy"))
            {
                Damage(other);
            }
            else if (other.CompareTag("EnemyBullet"))
            {
                var vfx = GameInstance.Instance.hitPlayerPool.Get();
                if (null == vfx.GetComponent<PartycleSystemDisactivate>())
                    vfx.AddComponent<PartycleSystemDisactivate>();
                vfx.transform.position = other.transform.position;
                vfx.gameObject.SetActive(true);

                other.gameObject.SetActive(false);
                Damage(other);
            }
            else if (other.CompareTag("EnemyMelee"))
            {
                var vfx = GameInstance.Instance.hitPlayerPool.Get();
                if (null == vfx.GetComponent<PartycleSystemDisactivate>())
                    vfx.AddComponent<PartycleSystemDisactivate>();
                vfx.transform.position = transform.position + other.GetComponent<MeleeController>().vfxOffset;
                vfx.gameObject.SetActive(true);

                other.gameObject.SetActive(false);
                Damage(other);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (canTakeDamage)
        {
            if (other.CompareTag("Enemy"))
            {
                Damage(other);
            }
            else if (other.CompareTag("EnemyBullet"))
            {
                var vfx = GameInstance.Instance.hitPlayerPool.Get();
                if (null == vfx.GetComponent<PartycleSystemDisactivate>())
                    vfx.AddComponent<PartycleSystemDisactivate>();
                vfx.transform.position = other.transform.position;
                vfx.gameObject.SetActive(true);

                other.gameObject.SetActive(false);
                Damage(other);
            }
        }
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

    public bool Damage(float damage)
    {
        if (!canTakeDamage)
            return false;

        bool org = status.isDead;
        bool isDead = status.Damage(damage);

        if (!isDead)
        {
            DamageEffect();
            KnockBack();
        }
        else if (!org)
            KnockBack();

        return isDead;
    }

    bool Damage(Collider other)
    {
        if (!canTakeDamage)
            return false;

        bool org = status.isDead;
        bool isDead = false;
        switch (other.tag)
        {
            case "Enemy":
                isDead = status.Damage(1f);
                break;
            case "EnemyBullet":
                isDead = status.Damage(1f);
                break;
            case "EnemyMelee":
                isDead = status.Damage(1f);
                break;
            default:
                break;
        }

        if (!isDead)
        {
            DamageEffect();
            KnockBack(other.transform);
        }
        else if (!org)
            KnockBack(other.transform);

        return isDead;
    }

    void DamageEffect()
    {
        StartCoroutine(FlashCoroutine());
    }

    void KnockBack()
    {
        if (rb != null)
        {
            Vector3 knockbackDirection = transform.eulerAngles.y == 180f ? Vector3.right : Vector3.left;
            knockbackDirection.y = damageInfo.knockbackForce.y;
            rb.AddForce(knockbackDirection * damageInfo.knockbackForce.x, ForceMode.Impulse);
        }
    }

    void KnockBack(Transform attacker)
    {
        if (rb != null)
        {
            Vector3 knockbackDirection = (transform.position - attacker.position).normalized;
            knockbackDirection.y = damageInfo.knockbackForce.y;
            rb.AddForce(knockbackDirection * damageInfo.knockbackForce.x, ForceMode.Impulse);
        }
    }

    private IEnumerator FlashCoroutine()
    {
        canTakeDamage = false;
        float elapsedTime = 0f;
        bool isDark = false;

        while (elapsedTime < damageInfo.flashDuration)
        {
            isDark = !isDark;
            sr.color = isDark ? new Color(0.5f, 0.5f, 0.5f, 1f) : Color.white;

            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.1f;
        }

        sr.color = Color.white;
        yield return new WaitForSeconds(damageInfo.damageCooldown);
        canTakeDamage = true;
    }

    public void CanTakeDamage()
    {
        canTakeDamage = true;
    }

    public void CantTakeDamage()
    {
        canTakeDamage = false;
    }
}
