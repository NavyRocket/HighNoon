using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletObjectTransform;

    ObjectPool<BulletController> bulletPool;

    void Start()
    {

    }

    void Update()
    {

    }
}
