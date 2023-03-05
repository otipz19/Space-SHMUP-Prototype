using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [HideInInspector]
    public static Main S;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemiesPerSecond = 0.5f;
    public float defaultPadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions;
    public GameObject powerUpPrefab;
    public WeaponType[] powerUpVariants = new WeaponType[] { WeaponType.blaster,
                                                              WeaponType.blaster,
                                                              WeaponType.spread,
                                                              WeaponType.shield};

    private BoundsCheck bndCheck;
    private static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;
    [SerializeField]
    private float dynamicEnemiesDelay;

    private void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        dynamicEnemiesDelay = enemiesPerSecond;

        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
            WEAP_DICT[def.type] = def;

        Invoke("SpawnEnemy", 1f);
    }

    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate<GameObject>(prefabEnemies[Random.Range(0, prefabEnemies.Length)]);

        float enemyPadding = defaultPadding;
        if (bndCheck != null)
            enemyPadding = Mathf.Abs(bndCheck.radius);

        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        enemy.transform.position = pos;

        Invoke("SpawnEnemy", 1f / dynamicEnemiesDelay);
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene_0");
    }

    public static WeaponDefinition GetWeaponDefinition(WeaponType weaponType)
    {
        if (WEAP_DICT.ContainsKey(weaponType))
            return WEAP_DICT[weaponType];
        else
            return new WeaponDefinition();
    }

    public void EnemyDestroyed(Enemy enemy)
    {
        if(Random.value <= enemy.powerUpDropChance)
        {
            WeaponType powerUpType = powerUpVariants[Random.Range(0, powerUpVariants.Length)];

            GameObject powerUpGO = Instantiate<GameObject>(powerUpPrefab);
            powerUpGO.GetComponent<PowerUp>().SetType(powerUpType);
            powerUpGO.transform.position = enemy.transform.position;
        }
    }

    public void ChangeDifficulty()
    {
        int heroWeapons = Hero.S.CountWeapons();
        dynamicEnemiesDelay = enemiesPerSecond + 0.2f * (heroWeapons-1);   
    }
}
