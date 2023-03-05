using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private GameObject target;
    private WeaponDefinition rocketDef;

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rocketDef = Main.GetWeaponDefinition(WeaponType.rocket);
    }

    private void Update()
    {
        if(bndCheck != null && bndCheck.isOnScreen == false)
        {
            Destroy(this.gameObject);
        }

        if (target == null)
        {
            Vector3 pos = transform.position;
            pos.y += rocketDef.projectileSpeed / 2 * Time.deltaTime;
            transform.position = pos;
        }
        else
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, target.transform.position, rocketDef.projectileSpeed * Time.deltaTime);
            transform.position = pos;

            Vector3 targetDirection = (target.transform.position - transform.position).normalized;
            transform.up = Vector3.Lerp(transform.up, targetDirection, 20 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(target == null)
        {
            GameObject collidedWith = other.transform.root.gameObject;
            if(collidedWith.tag == "Enemy")
            {
                Debug.Log("Rocket: New target found!");
                target = collidedWith;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedWith = collision.transform.root.gameObject;
        if(collidedWith.tag == "Enemy")
        {
            Debug.Log("Rocket: target hit!");
            collidedWith.GetComponent<Enemy>().GetDamage(rocketDef.damageOnHit, collision.collider);
            Destroy(this.gameObject);
        }
    }
}
