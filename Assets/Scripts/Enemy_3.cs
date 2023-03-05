using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy
{
    [Header("Set in Inspector: Enemy_2")]
    public float lifeDuration = 10f;

    [Header("Set Dynamically: Enemy_2")]
    public float birthTime;
    public Vector3[] points;

    private void Start()
    {
        points = new Vector3[3];

        points[0] = pos;

        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;

        points[1].y = -bndCheck.camHeight * Random.Range(2f, 2.75f);
        points[1].x = Random.Range(xMin, xMax);

        points[2].y = pos.y;
        points[2].x = Random.Range(xMin, xMax);

        birthTime = Time.time;
    }

    protected override void Move()
    {
        float u = (Time.time - birthTime) / lifeDuration;
        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }

        u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2); 
        pos = Utils.BezierRecursive(u, points); 
    }
}
