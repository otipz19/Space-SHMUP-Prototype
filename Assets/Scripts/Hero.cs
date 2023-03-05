using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [HideInInspector]
    public static Hero S;
    public delegate void WeaponFireDelegate(bool stopFire = false);
    public WeaponFireDelegate fireDelegate;

    [Header("Set in Inspector")]
    public float speed = 30;
    public float rollMultiplier = -45;
    public float pitchMultiplier = 30;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;
    public Weapon[] weapons;

    private GameObject lastCollided;

    [SerializeField]
    private float _shieldLevel = 1;

    public float shieldLevel
    {
        get { return _shieldLevel; }
        set
        {
            _shieldLevel = Mathf.Min(4, value);
            if (_shieldLevel < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }

    public int CountWeapons()
    {
        int activeWeapons = 0;
        foreach(Weapon w in weapons)
            if (w.type != WeaponType.none)
                activeWeapons++;
        return activeWeapons;
    }

    private void Start()
    {
        if (S == null)
            S = this;
        else
            Debug.LogError("Hero.Start() - Ержан, выключайся!");

        ClearWeapons();
        weapons[0].type = WeaponType.laser;
    }

    private void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(yAxis * pitchMultiplier, xAxis * rollMultiplier, 0);

        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
        if (Input.GetAxis("Jump") == 0 && fireDelegate != null)
        {
            fireDelegate(true);
        }
        if(Input.GetAxis("Fire1") == 1)
        {
            RocketLauncher.S.LaunchRocket();
        }
    }

    public void ShieldTrigger(Collider other)
    {
        GameObject collidedWith = other.transform.root.gameObject;
        if (collidedWith != lastCollided)
        {
            lastCollided = collidedWith;
            if (collidedWith.tag == "Enemy")
            {
                shieldLevel--;
                Destroy(collidedWith);
                Debug.Log("Collided with enemy!");
            }
            else if (collidedWith.tag == "PowerUp")
            {
                AbsorbPowerUp(collidedWith);
                Main.S.ChangeDifficulty();
            }
            else
            {
                Debug.Log("Collided with non-enemy: " + collidedWith.name);
            }
        }
    }

    private void AbsorbPowerUp(GameObject go)
    {
        PowerUp powerUp = go.GetComponent<PowerUp>();

        switch (powerUp.powerUpType)
        {
            case WeaponType.shield:
                shieldLevel++;
                break;
            default:
                if(weapons[0].type == powerUp.powerUpType)
                {
                    Weapon weaponSlot = GetWeaponSlot();
                    if(weaponSlot != null)
                    {
                        weaponSlot.type = powerUp.powerUpType;
                    }
                }
                else
                {
                    ClearWeapons();
                    weapons[0].type = powerUp.powerUpType;
                }
                break;
        }

        powerUp.AbsorbedBy(this.gameObject);
    }

    private Weapon GetWeaponSlot()
    {
        foreach (Weapon weapon in weapons)
            if (weapon.type == WeaponType.none)
                return weapon;
        return null;
    }

    private void ClearWeapons()
    {
        foreach (Weapon weapon in weapons)
            weapon.type = WeaponType.none;
    }
}