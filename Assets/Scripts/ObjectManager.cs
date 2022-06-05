using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject enemyLPrefab;
    public GameObject enemyMPrefab;
    public GameObject enemySPrefab;
    public GameObject BossPrefab;
    public GameObject playerBulletPrefab;
    public GameObject playerBullet2Prefab;
    public GameObject enemyBullet1Prefab;
    public GameObject enemyBullet2Prefab;
    public GameObject bossBulletPrefab;

    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;
    GameObject[] Boss;

    GameObject[] PlayerBullet;
    GameObject[] PlayerBullet2;
    GameObject[] enemyBullet1;
    GameObject[] enemyBullet2;
    GameObject[] bossBullet;

    GameObject[] targetPool;

    void Awake()
    {
        enemyL = new GameObject[10];
        enemyM = new GameObject[10];
        enemyS = new GameObject[10];
        Boss = new GameObject[1];

        PlayerBullet = new GameObject[10000];
        PlayerBullet2 = new GameObject[10000];
        enemyBullet1 = new GameObject[10000];
        enemyBullet2 = new GameObject[10000];
        bossBullet = new GameObject[1000];


        Generate();
    }

    void Generate()
    {
        for (int index = 0; index < enemyL.Length; index++)
        {
            enemyL[index] = Instantiate(enemyLPrefab);
            enemyL[index].SetActive(false);
        }
        for (int index = 0; index < enemyM.Length; index++)
        {
            enemyM[index] = Instantiate(enemyMPrefab);
            enemyM[index].SetActive(false);
        }
        for (int index = 0; index < enemyS.Length; index++)
        {
            enemyS[index] = Instantiate(enemySPrefab);
            enemyS[index].SetActive(false);
        }
        for(int index = 0; index < Boss.Length; index++)
        {
            Boss[index] = Instantiate(BossPrefab);
            Boss[index].SetActive(false);
        }
        for (int index = 0; index < PlayerBullet.Length; index++)
        {
            PlayerBullet[index] = Instantiate(playerBulletPrefab);
            PlayerBullet[index].SetActive(false);
        }
        for(int index = 0; index < PlayerBullet2.Length; index++)
        {
            PlayerBullet2[index] = Instantiate(playerBullet2Prefab);
            PlayerBullet2[index].SetActive(false);
        }
        for (int index = 0; index < enemyBullet1.Length; index++)
        {
            enemyBullet1[index] = Instantiate(enemyBullet1Prefab);
            enemyBullet1[index].SetActive(false);
        }
        for (int index = 0; index < enemyBullet2.Length; index++)
        {
            enemyBullet2[index] = Instantiate(enemyBullet2Prefab);
            enemyBullet2[index].SetActive(false);
        }
        for(int index = 0; index < bossBullet.Length; index++)
        {
            bossBullet[index] = Instantiate(bossBulletPrefab);
            bossBullet[index].SetActive(false);
        }
    }
    public GameObject MakeObj(string type)
    {
        
        switch (type)
        {
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;    
                break;
            case "EnemyB":
                targetPool = Boss;
                break;
            case "PlayerBullet":
                targetPool = PlayerBullet;
                break;
            case "PlayerBullet2":
                targetPool = PlayerBullet2;
                break;
            case "EnemyBullet1":
                targetPool = enemyBullet1;
                break;
            case "EnemyBullet2":
                targetPool = enemyBullet2;
                break;
            case "BossBullet":
                targetPool = bossBullet;
                break;
            
        }
        for (int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
                targetPool[index].SetActive(true);
                return targetPool[index];
            }
        }
        return null;
    }
    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;
                break;
            case "EnemyB":
                targetPool = Boss;
                break;
            case "PlayerBullet":
                targetPool = PlayerBullet;
                break;
            case "PlayerBullet2":
                targetPool = PlayerBullet2;
                break;
            case "EnemyBullet1":
                targetPool = enemyBullet1;
                break;
            case "EnemyBullet2":
                targetPool = enemyBullet2;
                break;
            case "BossBullet":
                targetPool = bossBullet;
                break;
           
        }
                return targetPool;
    }
}
