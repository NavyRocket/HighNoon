using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

public class PlayerStatus : MonoBehaviour
{
//  [SerializeField] private float _maxSanity;
//  [SerializeField] private Image _darkenerImage;

    [SerializeField, ReadOnly]
    private float _hp;
    [SerializeField]
    private float _maxHp;
//  float sanity;
    public float hp { get { return _hp; } set { _hp = value; } }
    public float maxHp { get { return _maxHp; } set { _maxHp = value ; } }
    public float damage { get; set; }
    public float criticalDamage { get; set; }
    public float criticalChance { get; set; }
    public float reloadSpeed { get; set; }
    public bool rollExp { get; set; }
    public bool isDead { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        _hp = _maxHp;
//      sanity = _maxSanity;
        damage = 1f;
        criticalDamage = 2f;
        criticalChance = 0.1f;
        reloadSpeed = 0.2f;
        rollExp = false;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Damage(float damage)
    {
        if (isDead)
            return isDead;

        _hp -= damage;
        isDead = _hp <= 0f;

        if (isDead)
        {
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<PlayerController>().gun.SetActive(false);

            VolumeManager.Instance.CloseEye();
            Invoke("RebirthMenu", 0.75f);
        }

        return isDead;
    }

    public void Rebirth()
    {
        Invoke("RebirthAnimation", 1f);
        isDead = false;
        hp = maxHp;
    }

    void RebirthMenu()
    {
        GameInstance.Instance.myRebirthMenu.Toggle();
    }
    void RebirthAnimation()
    {
        GetComponent<Animator>().SetTrigger("Rebirth");
    }
}
