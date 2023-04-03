using UnityEngine;
using System.Collections.Generic;

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
    private int amnTilesOnScreen = 6;
    public float safeZone = 30.0f;
    private int lastPrefabIndex = 0;
    public int difficulty = 1;
    private Quaternion spawnRotation;

    private List<GameObject> activeTiles;

    private void Start()
    {
        //DeleteAllTiles();
        lastWidth = 0;
        newWidth = 0;
        activeTiles = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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
            /*if (ran % 2 == 1)
            {
                SpawnTile(difficulty);
            }
            else
            {
                SpawnTile(3);
                DeleteTile();
            }*/
            SpawnTile(difficulty);
            if (ran % 3 == 0)
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
                    temp = Instantiate(tilePrefabsEasy[RandomPreFabindex()]) as GameObject;
                    break;
                }
            case 2:
                {
                    temp = Instantiate(tilePrefabsMedium[RandomPreFabindex()]) as GameObject;
                    //temp = Instantiate(tilePrefabsMedium[RandomPreFabindex()], Vector3.forward * spawnZ, spawnRotation) as GameObject;
                    break;
                }
            case 3:
                {
                    temp = Instantiate(tilePrefabsHard[RandomPreFabindex()]) as GameObject;
                    break;
                }
            default:
                {
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
