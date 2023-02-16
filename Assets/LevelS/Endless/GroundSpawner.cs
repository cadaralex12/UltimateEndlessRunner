using UnityEngine;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    public GameObject[] tilePrefabs3;
    public GameObject[] tilePrefabs5;
    public int lastWidth = 0;
    public int newWidth = 0;
    private Transform playerTransform;
    private float spawnZ = 0.0f;
    private float tileLength = 15.0f;
    private int amnTilesOnScreen = 5000;
    private float safeZone = 15.0f;
    private int lastPrefabIndex = 0;

    private List<GameObject> activeTiles;

    private void Start()
    {
        lastWidth = 0;
        newWidth = 0;
        activeTiles = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < amnTilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    private void Update()
    {
        if ((playerTransform.position.z - safeZone) > (spawnZ - amnTilesOnScreen * tileLength))
        {
            int ran = Random.Range(0, 10);
            if (ran % 2 == 1)
            {
                SpawnTile();
            }
            else
            {
                SpawnTile(5);
            }

            DeleteTile();
        }
    }

    public void SpawnTile(int width = 3, int prefabIndex = -1)
    {
        GameObject temp;

        /*if (width == 3)
        {
            newWidth = 3;*/
            temp = Instantiate(tilePrefabs3[RandomPreFabindex()]) as GameObject;
        /*}
        /*else if (width == 5)
        {
            newWidth = 5;
            temp = Instantiate(tilePrefabs5[RandomPreFabindex()]) as GameObject;
        }*/
        /*else
        {
            return;
        }
    */
        lastWidth = width;
        temp.transform.SetParent(transform);
        temp.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;
        activeTiles.Add(temp);
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    public int RandomPreFabindex(int width = 3)
    {
        if (width == 3)
        {
            if (tilePrefabs3.Length <= 1)
            {
                return 0;
            }
            int randomIndex = lastPrefabIndex;
            while (randomIndex == lastPrefabIndex)
            {
                randomIndex = Random.Range(0, tilePrefabs3.Length);
            }
            lastPrefabIndex = randomIndex;
            return randomIndex;
        }
        else if (width == 3)
        {
            if (tilePrefabs5.Length <= 1)
            {
                return 0;
            }
            int randomIndex = lastPrefabIndex;
            while (randomIndex == lastPrefabIndex)
            {
                randomIndex = Random.Range(0, tilePrefabs5.Length);
            }
            lastPrefabIndex = randomIndex;
            return randomIndex;
        }
        else return 0;
    }
}
