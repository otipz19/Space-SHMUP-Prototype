using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    none, //�� ���������
    blaster, //������� �������
    spread, //������� �����
    shield, //���������� shieldLevel
    laser,
    rocket,
    autoturret,
}

/// <summary>
/// ����� WeaponDefinition ��������� ����������� �������������� ���������� ���� ������ � ���������� Unity.
/// ��� ����� ����� Main ����� ������� ������ �������� ���� WeaponDefinition
/// </summary>
[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter = "N"; //����� �� ������ ������
    public GameObject projectilePrefab;
    public Color weaponColor = Color.white;
    public Color projectileColor = Color.white;
    public float projectileSpeed = 20f;
    public float damageOnHit = 1f;
    public float delayBetweenShots = 0f;
    public float damagePerSecond = 1f; //��� ������
    public float laserWidth;
    public float laserDistance;
}

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR; //������������ GameObject ��� ����������� �������� � ������ ��������

    [SerializeField]
    private WeaponType _type = WeaponType.none;

    private WeaponDefinition weaponDef;
    private GameObject collar;
    private Renderer collarRend;
    private float lastShotTime;
    private Laser laser;
    private Transform laserStartPoint;

    public WeaponType type
    {
        get { return _type; }
        set { SetType(value); }
    }

    private void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.transform.Find("default").GetComponent<Renderer>();

        SetType(_type); //�������� ��� ������ �� ���������

        //���������� PROJECTILE_ANCHOR �����������
        if(PROJECTILE_ANCHOR == null)
        {
            GameObject tmpGO = new GameObject("Projectile Anchor");
            PROJECTILE_ANCHOR = tmpGO.transform;
        } 

        GameObject rootGO = transform.root.gameObject;
        if(rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }

        laser = transform.Find("Laser").gameObject.GetComponent<Laser>();
        laserStartPoint = transform.Find("laserStartPoint");
    }

    public void SetType(WeaponType weaponType)
    {
        if(type == WeaponType.laser && weaponType != WeaponType.laser)
        {
            laser.LaserUnCast();
        }

        _type = weaponType;

        if(weaponType == WeaponType.none)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
            weaponDef = Main.GetWeaponDefinition(weaponType);
            collarRend.material.color = weaponDef.weaponColor;
            lastShotTime = 0f;
        }
    }

    private void Fire(bool stopFire = false)
    {
        if (stopFire)
        {
            laser.LaserUnCast();
            return;
        }

        if (gameObject.activeInHierarchy && Time.time - lastShotTime > weaponDef.delayBetweenShots)
        {
            Vector3 vel = Vector3.up * weaponDef.projectileSpeed; 
            if (transform.up.y < 0) //� ������ ������ ���������� ����
                vel.y = -vel.y;

            switch (type)
            {
                case WeaponType.blaster:
                    MakeProjectileWithRotation(vel, 0f);
                    break;
                case WeaponType.spread:
                    MakeProjectileWithRotation(vel, 0f);
                    MakeProjectileWithRotation(vel, -7.5f);
                    MakeProjectileWithRotation(vel, 7.5f);
                    MakeProjectileWithRotation(vel, 15f);
                    MakeProjectileWithRotation(vel, -15f);
                    break;
                case WeaponType.laser:
                    laser.LaserCast(laserStartPoint.position);
                    break;
            }
        }
    }

    private void MakeProjectileWithRotation(Vector3 speed, float angle)
    {
        Projectile projectile = MakeProjectile();
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
        projectile.rigid.velocity = projectile.transform.rotation * speed;
    }

    private Projectile MakeProjectile()
    {
        GameObject projectileGO = Instantiate<GameObject>(weaponDef.projectilePrefab);

        if(transform.parent.tag == "Hero")
        {
            projectileGO.tag = "ProjectileHero";
            projectileGO.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            projectileGO.tag = "ProjectileEnemy";
            projectileGO.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }

        projectileGO.transform.position = collar.transform.position;
        projectileGO.transform.SetParent(PROJECTILE_ANCHOR, true);

        Projectile projectileScript = projectileGO.GetComponent<Projectile>();
        projectileScript.type = type;
        lastShotTime = Time.time;

        return projectileScript;
    }
}
