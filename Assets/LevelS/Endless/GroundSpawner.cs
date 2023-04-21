using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class GroundSpawner : MonoBehaviour
{
    public float difficultyCounter;
    public GameObject[] tilePrefabsBonus;
    public GameObject[] tilePrefabsNormal;
    public GameObject[] tilePrefabsHard;
    public GameObject[] tilePrefabsEmpty;
    public GameObject[] tilePrefabs5;
    public int lastWidth = 0;
    public int newWidth = 0;
    public Movement player;
    private Transform playerTransform;
    public float spawnZ = 0.0f;
    private float tileLength = 85.0f;
    private int amnTilesOnScreen = 10;
    public float safeZone = 30.0f;
    private int lastPrefabIndex = 0;
    public int difficulty = 2;
    private Quaternion spawnRotation;
    private int firstFiveEmpty;

    private float deleteCounter = 0f;

    public int starsSpawned = 0;

    private List<GameObject> activeTiles;

    private void Start()
    {
        difficultyCounter = 30f;
        lastWidth = 0;
        newWidth = 0;
        activeTiles = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        firstFiveEmpty = 5;
        for (int i = 0; i < amnTilesOnScreen; i++)
        {
            SpawnTile(difficulty);
        }
    }

    

    private void Update()
    {
        difficultyCounter -= Time.deltaTime;

        if (difficultyCounter <= 0f)
        {
            /*Destroy(activeTiles[13]);
            activeTiles.RemoveAt(13);
            Destroy(activeTiles[12]);
            activeTiles.RemoveAt(12);
            Destroy(activeTiles[11]);
            activeTiles.RemoveAt(11);
            Destroy(activeTiles[10]);
            activeTiles.RemoveAt(10);
            Destroy(activeTiles[9]);
            activeTiles.RemoveAt(9);*/
            if (difficulty < 5)
            {
                difficulty++;
                difficultyCounter = 30f;
                firstFiveEmpty = 5;
            }
        }

        if ((playerTransform.position.z - safeZone) > (spawnZ - amnTilesOnScreen * tileLength))
        {
            SpawnTile(difficulty);

            if (activeTiles.Count >= 14 && firstFiveEmpty == 0)
            {
                DeleteTile();
            }
            
        }
    }

    public void SetSpeed(int speed = 0)
    {
        player.lastFwdSpeed = speed;
        if (!player.inBoost)
        {
            player.fwdSpeed = speed;
        }
    }

    public void SpawnTile(int difficulty = 3, int prefabIndex = -1)
    {
        GameObject temp;

        switch(difficulty)
        {
            //Graphics Debug
            case 0:
                {
                    SetSpeed(90);
                    if (firstFiveEmpty > 0)
                    {
                        temp = Instantiate(tilePrefabsBonus[0]) as GameObject;
                        firstFiveEmpty--;
                        break;
                    }
                    temp = Instantiate(tilePrefabsHard[RandomPreFabindex()]) as GameObject;
                    break;
                }
            // Easy 1: 120 speed, many bonuses, few normals
            case 1:
                {
                    SetSpeed(90);
                    if (firstFiveEmpty > 0)
                    {
                        temp = Instantiate(tilePrefabsEmpty[0]) as GameObject;
                        firstFiveEmpty--;
                        break;
                    }
                    else
                    {
                        int random = UnityEngine.Random.Range(0, 100);
                        if (random <= 80)
                        {
                            temp = Instantiate(tilePrefabsBonus[RandomPreFabindex(1)]) as GameObject;
                        }
                        else
                        {
                            temp = Instantiate(tilePrefabsNormal[RandomPreFabindex(3)]) as GameObject;
                        }
                    }
                    break;
                }
            // Easy 2: 120 speed, half bonuses, few normals, few hards 
            case 2:
                {
                    SetSpeed(110);
                    if (firstFiveEmpty > 0)
                    {
                        temp = Instantiate(tilePrefabsEmpty[0]) as GameObject;
                        firstFiveEmpty--;
                        break;
                    }
                    else
                    {
                        int random = UnityEngine.Random.Range(0, 100);
                        if (random <= 40)
                        {
                            temp = Instantiate(tilePrefabsBonus[RandomPreFabindex(1)]) as GameObject;
                        }
                        else if (random > 40 && random <= 50)
                        {
                            temp = Instantiate(tilePrefabsEmpty[0]) as GameObject;
                        }
                        else if (random > 50 && random <= 75)
                        {
                            temp = Instantiate(tilePrefabsNormal[RandomPreFabindex(2)]) as GameObject;
                        }
                        else
                        {
                            temp = Instantiate(tilePrefabsHard[RandomPreFabindex(4)]) as GameObject;
                        }
                    }
                    break;
                }
            // Medium 1: 120 speed, 20% bonuses, 10% empty 40% normals, 30%  hards 
            case 3:
                {
                    SetSpeed(110);
                    if (firstFiveEmpty > 0)
                    {
                        temp = Instantiate(tilePrefabsEmpty[0]) as GameObject;
                        firstFiveEmpty--;
                        break;
                    }
                    else
                    {
                        int random = UnityEngine.Random.Range(0, 100);
                        if (random <= 20)
                        {
                            temp = Instantiate(tilePrefabsBonus[RandomPreFabindex(1)]) as GameObject;
                        }
                        else if (random > 20 && random <= 30)
                        {
                            temp = Instantiate(tilePrefabsEmpty[0]) as GameObject;
                        }
                        else if (random > 30 && random <= 70)
                        {
                            temp = Instantiate(tilePrefabsNormal[RandomPreFabindex(2)]) as GameObject;
                        }
                        else
                        {
                            temp = Instantiate(tilePrefabsHard[RandomPreFabindex(4)]) as GameObject;
                        }
                    }
                    break;
                }
            // Medium 2:  20% bonuses, 10% empty 40% normals, 30%  hards, 150 fwdSpeed
            case 4:
                {
                    SetSpeed(110);
                    if (firstFiveEmpty > 0)
                    {
                        temp = Instantiate(tilePrefabsEmpty[0]) as GameObject;
                        firstFiveEmpty--;
                        break;
                    }
                    else
                    {
                        int random = UnityEngine.Random.Range(0, 100);
                        if (random <= 40)
                        {
                            temp = Instantiate(tilePrefabsBonus[RandomPreFabindex(1)]) as GameObject;
                        }
                        else if (random > 40 && random <= 50)
                        {
                            temp = Instantiate(tilePrefabsEmpty[0]) as GameObject;
                        }
                        else if (random > 50 && random <= 75)
                        {
                            temp = Instantiate(tilePrefabsNormal[RandomPreFabindex(2)]) as GameObject;
                        }
                        else
                        {
                            temp = Instantiate(tilePrefabsHard[RandomPreFabindex(4)]) as GameObject;
                        }
                    }
                    break;
                }
            // Medium 2: 120 speed, 10% bonuses, 5% empty 45% normals, 40%  hards, 150 fwdSpeed
            case 5:
                {
                    SetSpeed(150);
                    if (firstFiveEmpty > 0)
                    {
                        temp = Instantiate(tilePrefabsEmpty[0]) as GameObject;
                        firstFiveEmpty--;
                        break;
                    }
                    else
                    {
                        int random = UnityEngine.Random.Range(0, 100);
                        if (random <= 1)
                        {
                            temp = Instantiate(tilePrefabsBonus[RandomPreFabindex(1)]) as GameObject;
                        }
                        /*else if (random > 3 && random <= 5)
                        {
                            temp = Instantiate(tilePrefabsEmpty[0]) as GameObject;
                        }*/
                        else if (random > 1 && random <= 95)
                        {
                            temp = Instantiate(tilePrefabsNormal[RandomPreFabindex(2)]) as GameObject;
                        }
                        else
                        {
                            temp = Instantiate(tilePrefabsHard[RandomPreFabindex(4)]) as GameObject;
                        }
                    }
                    break;
                }
            default:
                {
                    SetSpeed(90);
                    if (firstFiveEmpty > 0)
                    {
                        temp = Instantiate(tilePrefabsEmpty[0]) as GameObject;
                        firstFiveEmpty--;
                        break;
                    }
                    temp = Instantiate(tilePrefabsEmpty[0]) as GameObject;
                    break;
                }
                
                
        }
        temp.transform.SetParent(transform);
        temp.transform.position = Vector3.forward * spawnZ;
        tileLength = temp.gameObject.GetComponent<PrefabInformation>().tileLength;
        spawnZ += tileLength;
        if(firstFiveEmpty < 0)
        {
            activeTiles.Add(temp);
        }
    }

    private void DeleteTile()
    {
        starsSpawned += activeTiles[5].gameObject.GetComponent<PrefabInformation>().coinsSpawned;
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    public int RandomPreFabindex(int type = 1)
    {
        if (type == 1)
        {
            if (tilePrefabsBonus.Length <= 1)
            {
                return 0;
            }
            int randomIndex = lastPrefabIndex;
            while (randomIndex == lastPrefabIndex)
            {
                randomIndex = UnityEngine.Random.Range(0, tilePrefabsBonus.Length);
            }
            lastPrefabIndex = randomIndex;
            return randomIndex;
        }
        else if (type == 2)
        {
            if (tilePrefabsNormal.Length <= 1)
            {
                return 0;
            }
            int randomIndex = UnityEngine.Random.Range(0, tilePrefabsNormal.Length);
            return randomIndex;
        }
        else if (type == 3)
        {
            if (tilePrefabsEmpty.Length <= 1)
            {
                return 0;
            }
            int randomIndex = lastPrefabIndex;
            while (randomIndex == lastPrefabIndex)
            {
                randomIndex = UnityEngine.Random.Range(0, tilePrefabsEmpty.Length);
            }
            lastPrefabIndex = randomIndex;
            return randomIndex;
        }
        else if (type == 4)
        {
            if (tilePrefabsHard.Length <= 1)
            {
                return 0;
            }
            int randomIndex = lastPrefabIndex;
            while (randomIndex == lastPrefabIndex)
            {
                randomIndex = UnityEngine.Random.Range(0, tilePrefabsHard.Length);
            }
            lastPrefabIndex = randomIndex;
            return randomIndex;
        }
        else return 0;
    }
}
