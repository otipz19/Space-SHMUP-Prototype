using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    [Header("Set in Inspector: Enemy_1")]
    public float waveFrequency = 2f;
    public float waveWidth = 4f;
    public float waveRotY = 45f;

    private float x0;
    private float birthTime;

    private void Start()
    {
        x0 = transform.position.x;
        birthTime = Time.time;
    }

    protected override void Move()
    {
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);

        Vector3 tmpPos = pos;
        tmpPos.x = x0 + sin * waveWidth;
        pos = tmpPos;

        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        transform.rotation = Quaternion.Euler(rot);

        base.Move();
    }
}
