using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    PlayerController controller;
    Vector3 muzzle;
    Vector3 vector = Vector2.zero;

    [SerializeField] private float speed = 1f;
    [SerializeField] private float lifeTime = 1f;
    [SerializeField] private float damage = 1f;

    private float timeAcc = 0f;

    public delegate void BulletFiredHandler(); // 총알 발사 이벤트 델리게이트
    public static event BulletFiredHandler OnBulletFired; // 총알 이벤트

    void Start()
    {

    }

    void Update()
    {
        timeAcc += Time.deltaTime;
        transform.position += vector * speed * Time.deltaTime;
    }

    void OnEnable()
    {
        if (IsInvoking("RetrieveBullet"))
            CancelInvoke("RetrieveBullet");
        Invoke("RetrieveBullet", lifeTime);

        if (controller == null)
            controller = GameInstance.Instance.GetPlayerController();

        timeAcc = 0f;
        transform.position = muzzle;

        vector = new Vector3(controller.transform.eulerAngles.y == 0 ? Mathf.Cos(controller.gunRadian) : -Mathf.Cos(controller.gunRadian),
            Mathf.Sin(controller.gunRadian), 0f);

        // 총알 발사 시 BulletUIManager 호출
        OnBulletFired?.Invoke();
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
