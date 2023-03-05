using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Part
{
    public string name;
    public float health;
    public string[] protectedBy;

    [HideInInspector]
    public GameObject partGO;
    [HideInInspector]
    public Material mat;
}

public class Enemy_4 : Enemy
{
    [Header("Set in Inspector: Enemy_4")]
    public Part[] parts;

    private Vector3 p0, p1;
    private float moveDuration = 4f;
    private float moveStartTime;

    private void Start()
    {
        p0 = p1 = pos;
        InitMovement();

        Transform t;
        foreach(Part part in parts)
        {
            t = transform.Find(part.name);
            if(t != null)
            {
                part.partGO = t.gameObject;
                part.mat = t.GetComponent<Renderer>().material;
            }
        }
    }

    private void InitMovement()
    {
        p0 = p1;

        float height = bndCheck.camHeight - bndCheck.radius;
        float width = bndCheck.camWidth - bndCheck.radius;

        p1.x = Random.Range(-width, width);
        p1.y = Random.Range(-height, height);

        moveStartTime = Time.time;
    }

    protected override void Move()
    {
        float u = (Time.time - moveStartTime) / moveDuration;

        if(u >= 1)
        {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);
        pos = (1 - u) * p0 + u * p1;
    }

    private Part FindPart(GameObject go)
    {
        foreach (Part part in parts)
            if (part.partGO == go)
                return part;
        return null;
    }

    private Part FindPart(string name)
    {
        foreach (Part part in parts)
            if (part.name == name)
                return part;
        return null;
    }

    private bool Destroyed(string name)
    {
        return Destroyed(FindPart(name));
    }

    private bool Destroyed(GameObject go)
    {
        return Destroyed(FindPart(go));
    }

    private bool Destroyed(Part part)
    {
        if (part == null)
            return true;
        return part.health <= 0;
    }

    private void ShowLocalizedDamage(Material m)
    {
        m.color = Color.red;
        damageDoneTime = Time.time;
        showingDamage = true;
    }

    public override bool GetDamage(float damage, Collider hitCollider)
    {
        if (bndCheck.isOnScreen)
        {
            //Ќайти часть корабл€, в которую попал снар€д
            GameObject hitGO = hitCollider.gameObject;
            Part partHit = FindPart(hitGO);

            //ѕроверить части, защищающие часть, в которую попал снар€д
            if (partHit.protectedBy != null)
            {
                foreach (string part in partHit.protectedBy)
                {
                    if (!Destroyed(part)) //≈сли хот€ бы одна защищающа€ часть не разрушена, не наносить урон
                    {
                        return false;
                    }
                }
            }

            //Ќанести урон части корабл€, в которую попал снар€д
            partHit.health -= damage;
            ShowLocalizedDamage(partHit.mat);
            if (partHit.health <= 0)
                partHit.partGO.SetActive(false);

            //ѕроверить все части корабл€ на уничтожение
            bool allPartsDestroyed = true;
            foreach (Part part in parts)
            {
                if (!Destroyed(part))
                {
                    allPartsDestroyed = false;
                    break;
                }
            }

            //≈сли все части уничтожены, уничтожить корабль
            if (allPartsDestroyed)
            {
                Destroy(this.gameObject);
                Main.S.EnemyDestroyed(this);
                return true;
            }
        }
        return false;
    }
}