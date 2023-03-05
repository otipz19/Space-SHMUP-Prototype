using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Vector2 rotationMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(0.25f, 2);
    public float lifeTime = 6f;
    public float fadeTime = 4f; //Время исчезнования бонуса

    [Header("Set Dynamically")]
    public GameObject cube;
    public WeaponType powerUpType;
    public TextMesh letter;
    public float birthTime;
    public Vector3 rotationPerSecond;

    private BoundsCheck bndCheck;
    private Renderer cubeRend;
    private Rigidbody rigid;

    private void Awake()
    {
        cube = transform.Find("Cube").gameObject;
        cubeRend = cube.GetComponent<Renderer>();
        bndCheck = GetComponent<BoundsCheck>();
        rigid = GetComponent<Rigidbody>();
        letter = GetComponent<TextMesh>();

        //Выбрать случайную скорость движения
        Vector3 vel = Random.onUnitSphere;
        vel.z = 0;
        vel.Normalize();
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;

        //Выбрать случайную скорость поворота
        transform.rotation = Quaternion.identity; //Quaternion.identity равноценно отсутствию поворота
        rotationPerSecond = new Vector3(Random.Range(rotationMinMax.x, rotationMinMax.y), 
                                        Random.Range(rotationMinMax.x, rotationMinMax.y), 
                                        Random.Range(rotationMinMax.x, rotationMinMax.y));

        birthTime = Time.time;
    }

    private void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotationPerSecond * Time.time);

        //Эффект затухания бонуса
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        if(u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }

        if(u > 0)
        {
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;
            c = letter.color;
            c.a = 1f - u * 0.5f;
            letter.color = c;
        }

        if (bndCheck.isOnScreen == false)
            Destroy(this.gameObject);
    }

    public void SetType(WeaponType weaponType)
    {
        WeaponDefinition def = Main.GetWeaponDefinition(weaponType);
        cubeRend.material.color = def.weaponColor;
        letter.text = def.letter;
        powerUpType = def.type;
    }

    public void AbsorbedBy(GameObject target)
    {
        Destroy(this.gameObject);
    }
}
