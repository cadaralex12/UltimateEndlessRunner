using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class GroundSpawner : MonoBehaviour
{
    public GameObject[] tilePrefabsEasy;
    public GameObject[] tilePrefabsMedium;
    public GameObject[] tilePrefabsHard;
    public GameObject[] tilePrefabs5;
    public int lastWidth = 0;
    public int newWidth = 0;
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
        lastWidth = 0;
        newWidth = 0;
        activeTiles = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        firstFiveEmpty = 5;
        for (int i = 0; i < amnTilesOnScreen; i++)
        {
            SpawnTile(difficulty);
            
        }
    }

    private void Update()
    {
        if ((playerTransform.position.z - safeZone) > (spawnZ - amnTilesOnScreen * tileLength))
        {
            SpawnTile(difficulty);

            if (activeTiles.Count >= 14 && firstFiveEmpty == 0)
            {
                DeleteTile();
            }
            
        }
    }

    public void SpawnTile(int difficulty = 3, int prefabIndex = -1)
    {
        GameObject temp;

        switch(difficulty)
        {
            case 1:
                {
                    if (firstFiveEmpty > 0)
                    {
                        temp = Instantiate(tilePrefabsEasy[0]) as GameObject;
                        firstFiveEmpty--;
                        break;
                    }
                    temp = Instantiate(tilePrefabsEasy[RandomPreFabindex()]) as GameObject;
                    break;
                }
            case 2:
                {
                    if (firstFiveEmpty > 0)
                    {
                        temp = Instantiate(tilePrefabsHard[0]) as GameObject;
                        firstFiveEmpty--;
                        break;
                    }
                    else
                    {
                        int random = UnityEngine.Random.Range(0, 100);
                        UnityEngine.Debug.Log(random);
                        if (random >= 95)
                        {
                            UnityEngine.Debug.Log("Bonus");
                            temp = Instantiate(tilePrefabsEasy[RandomPreFabindex(1)]) as GameObject;
                        }
                        else if (random < 90)
                        {
                            UnityEngine.Debug.Log("Medium");
                            temp = Instantiate(tilePrefabsMedium[RandomPreFabindex(2)]) as GameObject;
                        }
                        else
                        {
                            UnityEngine.Debug.Log("Empty");
                            temp = Instantiate(tilePrefabsHard[RandomPreFabindex(3)]) as GameObject;
                        }
                    }
                    break;
                }
            case 3:
                {
                    if (firstFiveEmpty > 0)
                    {
                        temp = Instantiate(tilePrefabsHard[0]) as GameObject;
                        firstFiveEmpty--;
                        break;
                    }
                    temp = Instantiate(tilePrefabsHard[RandomPreFabindex()]) as GameObject;
                    break;
                }
            default:
                {
                    UnityEngine.Debug.Log("Dicks");
                    if (firstFiveEmpty > 0)
                    {
                        temp = Instantiate(tilePrefabsHard[0]) as GameObject;
                        firstFiveEmpty--;
                        break;
                    }
                    temp = Instantiate(tilePrefabsHard[RandomPreFabindex()]) as GameObject;
                    break;
                }
                
                
        }
        //lastWidth = width;
        temp.transform.SetParent(transform);
        temp.transform.position = Vector3.forward * spawnZ;
        tileLength = temp.gameObject.GetComponent<PrefabInformation>().tileLength;
        spawnZ += tileLength;
        activeTiles.Add(temp);
    }

    private void DeleteTile()
    {
        starsSpawned += activeTiles[5].gameObject.GetComponent<PrefabInformation>().coinsSpawned;
        //UnityEngine.Debug.Log(starsSpawned);
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    /*private void DeleteAllTiles()
    {
        for (int i=0; i<=activeTiles.size(); i++)
        {
            Destroy(activeTiles[i]);
            activeTiles.RemoveAt(i);
        }
        
    }*/

    public int RandomPreFabindex(int type = 1)
    {
        if (type == 1)
        {
            if (tilePrefabsEasy.Length <= 1)
            {
                return 0;
            }
            int randomIndex = lastPrefabIndex;
            while (randomIndex == lastPrefabIndex)
            {
                randomIndex = UnityEngine.Random.Range(0, tilePrefabsEasy.Length);
            }
            lastPrefabIndex = randomIndex;
            return randomIndex;
        }
        else if (type == 2)
        {
            if (tilePrefabsMedium.Length <= 1)
            {
                return 0;
            }
            int randomIndex = lastPrefabIndex;
            while (randomIndex == lastPrefabIndex)
            {
                randomIndex = UnityEngine.Random.Range(0, tilePrefabsMedium.Length);
            }
            lastPrefabIndex = randomIndex;
            return randomIndex;
        }
        else if (type == 3)
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
