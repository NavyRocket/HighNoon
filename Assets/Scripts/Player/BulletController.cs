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
        //  vector = GameInstance.Instance.CursorWorldPosition() - transform.position;
        //  vector = vector.normalized;
        vector = new Vector3(controller.transform.eulerAngles.y == 0 ? Mathf.Cos(controller.gunRadian) : -Mathf.Cos(controller.gunRadian),
            Mathf.Sin(controller.gunRadian), 0f);
    }
 
    public float Damage { get { return damage; } }

    static int GetIndexOfMinValue(List<float> list)
    {
        int minIndex = 0;
        float minValue = list[0];

        for (int i = 1; i < list.Count; i++)
        {
            if (list[i] < minValue)
            {
                minValue = list[i];
                minIndex = i;
            }
        }

        return minIndex;
    }

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
