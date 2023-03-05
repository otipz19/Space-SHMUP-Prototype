using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    [HideInInspector]
    static public RocketLauncher S;

    private float delay;
    private float lastShot;
    private GameObject rocketPrefab;

    private void Start()
    {
        S = this;
        WeaponDefinition def = Main.GetWeaponDefinition(WeaponType.rocket);
        delay = def.delayBetweenShots;
        rocketPrefab = def.projectilePrefab;
    }

    public void LaunchRocket()
    {
        if(Time.time - lastShot > delay)
        {
            GameObject rocket = Instantiate<GameObject>(rocketPrefab);
            rocket.transform.position = transform.position;
            lastShot = Time.time;
        }
    }

}
