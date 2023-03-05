using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autoturret : MonoBehaviour
{
    [HideInInspector]
    static public Autoturret S;

    public WeaponDefinition def;

    private List<Enemy> EnemiesInTrigger;
    private Enemy target;
    private float lastShot;

    private void Start()
    {
        S = this;
        def = Main.GetWeaponDefinition(WeaponType.autoturret);
        EnemiesInTrigger = new List<Enemy>();
    }

    private void Update()
    {
        if (EnemiesInTrigger.Count > 0 && Time.time - lastShot > def.delayBetweenShots)
        {
            ChooseTarget();
            MakeProjectile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collidedWith = other.transform.root.gameObject;
        if(collidedWith.tag == "Enemy")
            EnemiesInTrigger.Add(collidedWith.GetComponent<Enemy>());
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject collidedWith = other.transform.root.gameObject;
        if(collidedWith.tag == "Enemy")
            EnemiesInTrigger.Remove(collidedWith.GetComponent<Enemy>());
    }

    private void MakeProjectile()
    {
        if(target != null)
        {
            GameObject projectileGO = Instantiate<GameObject>(def.projectilePrefab);
            projectileGO.tag = "ProjectileHero";
            projectileGO.layer = LayerMask.NameToLayer("ProjectileHero");
            projectileGO.transform.parent = Weapon.PROJECTILE_ANCHOR;

            Projectile projectile = projectileGO.GetComponent<Projectile>();
            projectile.type = WeaponType.autoturret;

            projectile.transform.position = transform.position;
            Vector3 targetDirection = (target.transform.position - transform.position).normalized;
            projectile.rigid.velocity = targetDirection * def.projectileSpeed;
            projectile.transform.up = targetDirection;

            lastShot = Time.time;
        }
    }

    private void ChooseTarget()
    {
        if (target == null)
            target = EnemiesInTrigger[0];
        for(int i = 0; i < EnemiesInTrigger.Count; i++)
        {
            if (EnemiesInTrigger[i] == null)
            {
                EnemiesInTrigger.Remove(EnemiesInTrigger[i]);
                continue;
            }
            if ((EnemiesInTrigger[i].transform.position - transform.position).magnitude < (target.transform.position - transform.position).magnitude)
                target = EnemiesInTrigger[i];
        }
    }
}