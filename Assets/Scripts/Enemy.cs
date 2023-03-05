using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10f;
    public int score = 100;
    public float showingDamageTime = 0.1f;
    public float powerUpDropChance = 0.5f;

    protected BoundsCheck bndCheck;

    protected Material[] materials; //Все материалы этого объекта и его дочерних объектов
    protected Color[] originalColors;
    protected float damageDoneTime;
    protected bool showingDamage = false;
    protected bool notifiedOfDestruction = false;

    public Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();

        materials = Utils.GetAllMaterials(this.gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < originalColors.Length; i++)
            originalColors[i] = materials[i].color;
    }

    private void Update()
    {
        Move();

        if(bndCheck != null && bndCheck.offDown)
        {
            Destroy(this.gameObject);
        }

        if (showingDamage && Time.time - damageDoneTime > showingDamageTime)
        {
            UnShowDamage();
        }
    }

    protected virtual void Move()
    {
        Vector3 newPos = pos;
        newPos.y -= speed * Time.deltaTime;
        pos = newPos;
    }

    /// <returns>True if enemy was destroyed</returns>
    public virtual bool GetDamage(float damage, Collider hitCollider)
    {
        if (bndCheck.isOnScreen)
        {       
            health -= damage;
            ShowDamage();

            if (health <= 0)
            {
                if (!notifiedOfDestruction)
                {
                    Main.S.EnemyDestroyed(this);
                    notifiedOfDestruction = true;
                }
                Destroy(this.gameObject);
                return true;
            }
        }
        return false;
    }

    protected void ShowDamage()
    {
        showingDamage = true;
        damageDoneTime = Time.time;
        for (int i = 0; i < materials.Length; i++)
            materials[i].color = Color.red;
    }

    protected void UnShowDamage()
    {
        showingDamage = false;
        for (int i = 0; i < materials.Length; i++)
            materials[i].color = originalColors[i];
    }
}
