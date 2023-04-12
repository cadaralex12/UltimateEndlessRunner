using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

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
        //DeleteAllTiles();
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
            int ran = Random.Range(0, 10);

            SpawnTile(difficulty);

            if (activeTiles.Count >= 12 && firstFiveEmpty == 0)
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
                        temp = Instantiate(tilePrefabsMedium[0]) as GameObject;
                        firstFiveEmpty--;
                        break;
                    }
                    temp = Instantiate(tilePrefabsMedium[RandomPreFabindex(difficulty)]) as GameObject;
                    //temp = Instantiate(tilePrefabsMedium[RandomPreFabindex()], Vector3.forward * spawnZ, spawnRotation) as GameObject;
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
                    if (firstFiveEmpty > 0)
                    {
                        temp = Instantiate(tilePrefabsEasy[0]) as GameObject;
                        firstFiveEmpty--;
                        break;
                    }
                    temp = Instantiate(tilePrefabsEasy[RandomPreFabindex()]) as GameObject;
                    break;
                }
                
                
        }
        //lastWidth = width;
        temp.transform.SetParent(transform);
        temp.transform.position = Vector3.forward * spawnZ;
        
        //temp.transform.position += Vector3.forward * tileLength;
        //temp.transform.rotation = spawnRotation;
        //spawnRotation = temp.transform.GetChild(0).GetChild(1).rotation;
        spawnZ += tileLength;
        activeTiles.Add(temp);
    }

    private void DeleteTile()
    {
        starsSpawned += activeTiles[3].gameObject.GetComponent<StarCounter>().coinsSpawned;
        UnityEngine.Debug.Log(starsSpawned);
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

    public int RandomPreFabindex(int difficulty = 1)
    {
        if (difficulty == 1)
        {
            if (tilePrefabsEasy.Length <= 1)
            {
                return 0;
            }
            int randomIndex = lastPrefabIndex;
            while (randomIndex == lastPrefabIndex)
            {
                randomIndex = Random.Range(0, tilePrefabsEasy.Length);
            }
            lastPrefabIndex = randomIndex;
            return randomIndex;
        }
        else if (difficulty == 2)
        {
            if (tilePrefabsMedium.Length <= 1)
            {
                return 0;
            }
            int randomIndex = lastPrefabIndex;
            while (randomIndex == lastPrefabIndex)
            {
                randomIndex = Random.Range(0, tilePrefabsMedium.Length);
            }
            lastPrefabIndex = randomIndex;
            return randomIndex;
        }
        else if (difficulty == 3)
        {
            if (tilePrefabsHard.Length <= 1)
            {
                return 0;
            }
            int randomIndex = lastPrefabIndex;
            while (randomIndex == lastPrefabIndex)
            {
                randomIndex = Random.Range(0, tilePrefabsHard.Length);
            }
            lastPrefabIndex = randomIndex;
            return randomIndex;
        }
        else return 0;
    }
}
