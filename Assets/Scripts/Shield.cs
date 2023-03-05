using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float rotationsPerSecond = 0.1f;

    [Header("Set Dynamically")]
    public int levelShown = 0;

    private Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);

        if (levelShown != currLevel)
        {
            levelShown = currLevel;
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }

        mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);

        //������� �������� ����
        float rotationZ = -(rotationsPerSecond * Time.time);
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    private void OnTriggerEnter(Collider other)
    {
        Hero.S.ShieldTrigger(other);
    }
}
