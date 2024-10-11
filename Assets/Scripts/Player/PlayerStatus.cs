using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] float maxHp;
    [SerializeField] float maxSanity;
    [SerializeField] Image darkenerImage;

    float hp;
    float sanity;
    public bool isDead { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        sanity = maxSanity;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Damage(float damage)
    {
        hp -= damage;
        isDead = hp <= 0f;

        return isDead;
    }
}
