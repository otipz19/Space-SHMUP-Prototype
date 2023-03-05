using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    [Header("Set in Inspector: Enemy_2")]
    public float lifeDuration = 10f;
    public float sinEccentricity = 0.6f;

    [Header("Set Dynamically: Enemy_2")]
    public Vector3 p0; //Начальная точка движения
    public Vector3 p1; //Конечная точка движения
    public float birthTime;

    private void Start()
    {
        //Выбрать случайную точку за левой границей экрана
        p0 = Vector3.zero;
        p0.x = -bndCheck.camWidth - bndCheck.radius;
        p0.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        //Выбрать случайную точку за правой границей экрана
        p1 = Vector3.zero;
        p1.x = bndCheck.camWidth + bndCheck.radius;
        p1.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        //В 50% случаев поменять местами начальную и конечную точки по оси x
        if(Random.value > 0.5f)
        {
            p0.x *= -1;
            p1.x *= -1;
        }

        birthTime = Time.time;
    }

    protected override void Move()
    {
        float u = (Time.time - birthTime) / lifeDuration;

        //Если u > 1, корабль существует дольше, чем lifeDuration 
        if(u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }

        //Функция сглаживания: скорректировать значение u добавлением значения кривой, изменяющейся по синусоиде 
        u = u + sinEccentricity * Mathf.Sin(u * Mathf.PI * 2);

        pos = (1 - u) * p0 + u * p1;
    }
}
