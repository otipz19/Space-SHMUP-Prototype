using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody rigid;

    private BoundsCheck bndCheck;
    private Renderer rend;
    [SerializeField]
    private WeaponType _type;

    public WeaponType type
    {
        get
        {
            return _type;
        }
        set
        {
            _type = value;
            WeaponDefinition def = Main.GetWeaponDefinition(value);
            rend.material.color = def.projectileColor;
        }
    }

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (bndCheck != null && bndCheck.isOnScreen == false)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedWith = collision.gameObject;
        if(collidedWith.tag == "Enemy")
        {
            collidedWith.GetComponent<Enemy>().GetDamage(Main.GetWeaponDefinition(type).damageOnHit, collision.collider);
            Destroy(this.gameObject);
        }
    }
}
