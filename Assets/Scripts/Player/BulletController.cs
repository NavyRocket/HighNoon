using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    PlayerController controller;
    PlayerStatus status;
    SpriteRenderer spriteRenderer;
    Material material;
    new Light light;
    Vector3 muzzle;
    Vector3 velocity = Vector2.zero;

    [SerializeField] private float speed = 1f;
    [SerializeField] private float lifeTime = 1f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float criticalDamage = 2f;

    [SerializeField] private Vector3 vfxOffset = Vector3.zero;
    [SerializeField] private float vfxOffsetByVelocityNormalized = 0f;

    private float timeAcc = 0f;
    private bool isCritical = false;

    public delegate void BulletFiredHandler(); // 총알 발사 이벤트 델리게이트
    public static event BulletFiredHandler OnBulletFired; // 총알 이벤트

    void Start()
    {

    }

    void Update()
    {
        timeAcc += Time.deltaTime;
        transform.position += velocity * speed * Time.deltaTime;
    }

    void OnEnable()
    {
        if (IsInvoking("RetrieveBullet"))
            CancelInvoke("RetrieveBullet");
        Invoke("RetrieveBullet", lifeTime);

        if (controller == null)
            controller = GameInstance.Instance.GetPlayerController();
        if (status == null)
            status = controller.GetComponent<PlayerStatus>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (material == null)
            material = GetComponent<Renderer>().material;
        if (light == null)
            light = GetComponent<Light>();

        timeAcc = 0f;
        isCritical = Random.value < status.criticalChance;

        velocity = new Vector3(controller.transform.eulerAngles.y == 0f ? Mathf.Cos(controller.gunRadian) : -Mathf.Cos(controller.gunRadian),
            Mathf.Sin(controller.gunRadian), 0f);

        transform.position = muzzle;
        transform.rotation = Quaternion.Euler(0f, controller.transform.eulerAngles.y, controller.gunRadian * Mathf.Rad2Deg);

        if (isCritical)
        {
            material.SetColor("_EmissionColor", new Color(1f, 0.4f, 0.2f) * 1f);
            light.color = new Color(1f, 0.4f, 0.2f);
            light.range = 1f;
        }
        else
        {
            material.SetColor("_EmissionColor", new Color(0.8f, 0.5f, 0.2f) * 1.05f);
            light.color = new Color(1f, 0.4f, 0.2f);
            light.range = 0f;
        }

        // 총알 발사 시 BulletUIManager 호출
        OnBulletFired?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage(isCritical ? criticalDamage : damage);
                if (enemy.isDead)
                    other.enabled = false;
            }

            var vfx = isCritical ? GameInstance.Instance.hitMobCriticalPool.Get() : GameInstance.Instance.hitMobPool.Get();
            if (null == vfx.GetComponent<PartycleSystemDisactivate>())
                vfx.AddComponent<PartycleSystemDisactivate>();
            vfx.transform.position = transform.position + vfxOffset + velocity.normalized * vfxOffsetByVelocityNormalized;
            vfx.transform.rotation = Quaternion.Euler(0f, controller.transform.eulerAngles.y == 0f ? 0f : 180f, controller.gunRadian * Mathf.Rad2Deg);
            vfx.gameObject.SetActive(true);

            RetrieveBullet();
        }
    }

    public float Damage { get { return damage; } }

    public void SetMuzzle(Vector3 _muzzle)
    {
        muzzle = _muzzle;
    }

    public void InitBullet(PlayerController controller)
    {
        this.controller = controller;
    }

    public void RetrieveBullet()
    {
        if (IsInvoking("RetrieveBullet"))
            CancelInvoke("RetrieveBullet");
        controller.RetrieveBullet(this);
    }
}
