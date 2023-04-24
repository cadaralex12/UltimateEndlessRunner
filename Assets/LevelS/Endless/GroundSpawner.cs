using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Collections.Specialized;

public class GroundSpawner : MonoBehaviour
{
    public AudioSource bass, drum1, drum2; 
    public int INDEX = 8;
    public float possibleStylePoints = 0f;
    public float difficultyCounter;
    public GameObject[] tilePrefabsBonus;
    public GameObject[] tilePrefabsNormal;
    public GameObject[] tilePrefabsHard;
    public GameObject[] tilePrefabsVeryHard;
    public GameObject[] tilePrefabsEmpty;
    public int lastWidth = 0;
    public int newWidth = 0;
    public Movement player;
    private Transform playerTransform;
    public float spawnZ = 0.0f;
    private float tileLength = 85.0f;
    private int amnTilesOnScreen = 15;
    public float safeZone = 15f;
    private int lastPrefabIndex = 0;
    public int difficulty = 2;
    private Quaternion spawnRotation;
    public int firstFiveEmpty;

    private float deleteCounter = 0f;

    public int starsSpawned = 0;

    private List<GameObject> activeTiles;

    public bool onTutorial = false;

    public GameObject[] tutorialTilePrefabs;
    public int tutorialIndex = 0;

    private void Start()
    {
        possibleStylePoints = 0;
        difficultyCounter = 20f;
        lastWidth = 0;
        newWidth = 0;
        activeTiles = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        firstFiveEmpty = 10;
        for (int i = 0; i < amnTilesOnScreen; i++)
        {
            SpawnTile(difficulty);
        }
    }

    private void changeMusic()
    {
        if(onTutorial == false)
        {
            switch (difficulty)
            {
                case 1:
                    {
                        bass.volume = 0f;
                        drum1.volume = 0f;
                        drum2.volume = 0f;
                        break;
                    }
                case 2:
                    {
                        bass.volume = 0.1f;
                        drum1.volume = 0f;
                        drum2.volume = 0f;
                        break;
                    }
                case 3:
                    {
                        bass.volume = 0.1f;
                        drum1.volume = 0.1f;
                        drum2.volume = 0f;
                        break;
                    }
                case 4:
                    {
                        bass.volume = 0.1f;
                        drum1.volume = 0f;
                        drum2.volume = 0.1f;
                        break;
                    }
                default:
                    {
                        bass.volume = 0.1f;
                        drum1.volume = 0f;
                        drum2.volume = 0.1f;
                        break;
                    }
            }
        }
    }

    private void Update()
    {
        changeMusic();
        difficultyCounter -= Time.deltaTime;

        if (onTutorial == false)
        {
            if (difficultyCounter <= 0f)
            {
                if (player.stylePoints > 0.50f * possibleStylePoints)
                {
                    if (difficulty < 6)
                    {
                        difficulty++;
                    }
                }
                else if (player.stylePoints < 0.10f * possibleStylePoints)
                {
                    difficultyCounter = 5f;
                }
                else
                {
                    if (difficulty > 1)
                    {
                        difficulty--;
                    }
                }

                difficultyCounter = 20f * difficulty;
                possibleStylePoints = 0;
                player.stylePoints = 0;
            }

            float firstLength = activeTiles[0].GetComponent<PrefabInformation>().tileLength;
            if ((playerTransform.position.z - firstLength - 30f) > activeTiles[0].transform.position.z)
            {
                SpawnTile(difficulty);
                if (activeTiles.Count > 15 && firstFiveEmpty == 0)
                {
                    DeleteTile();
                }
            }
        }
        else
        {
            float firstLength = activeTiles[0].GetComponent<PrefabInformation>().tileLength;
            if ((playerTransform.position.z - firstLength - 30f) > activeTiles[0].transform.position.z)
            {
                SpawnTile(difficulty);
                if (activeTiles.Count > 15 && firstFiveEmpty == 0)
                {
                    DeleteTile();
                }
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

        if(onTutorial == false)
        {
            switch (difficulty)
            {
                //Graphics Debug
                case 0:
                    {
                        SetSpeed(110);
                        if (firstFiveEmpty > 0)
                        {
                            temp = Instantiate(tilePrefabsBonus[RandomPreFabindex(1)]) as GameObject;
                            firstFiveEmpty--;
                            break;
                        }
                        temp = Instantiate(tilePrefabsBonus[RandomPreFabindex(1)]) as GameObject;
                        break;
                    }
                // Easy 1: 120 speed, many bonuses, few normals
                case 1:
                    {
                        SetSpeed(120);
                        if (firstFiveEmpty > 0)
                        {
                            temp = Instantiate(tilePrefabsEmpty[0]) as GameObject;
                            firstFiveEmpty--;
                            break;
                        }
                        else
                        {
                            int random = UnityEngine.Random.Range(0, 100);
                            if (random <= 60)
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
                        SetSpeed(130);
                        if (firstFiveEmpty > 0)
                        {
                            temp = Instantiate(tilePrefabsEmpty[0]) as GameObject;
                            firstFiveEmpty--;
                            break;
                        }
                        else
                        {
                            int random = UnityEngine.Random.Range(0, 100);
                            if (random <= 30)
                            {
                                temp = Instantiate(tilePrefabsBonus[RandomPreFabindex(1)]) as GameObject;
                            }
                            else if (random > 30 && random <= 35)
                            {
                                temp = Instantiate(tilePrefabsEmpty[0]) as GameObject;
                            }
                            else if (random > 35 && random <= 75)
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
                        SetSpeed(130);
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
                            else if (random > 20 && random <= 25)
                            {
                                temp = Instantiate(tilePrefabsEmpty[0]) as GameObject;
                            }
                            else if (random > 25 && random <= 75)
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
                        SetSpeed(140);
                        if (firstFiveEmpty > 0)
                        {
                            temp = Instantiate(tilePrefabsEmpty[0]) as GameObject;
                            firstFiveEmpty--;
                            break;
                        }
                        else
                        {
                            int random = UnityEngine.Random.Range(0, 100);
                            if (random <= 10)
                            {
                                temp = Instantiate(tilePrefabsBonus[RandomPreFabindex(1)]) as GameObject;
                            }
                            else if (random > 10 && random <= 15)
                            {
                                temp = Instantiate(tilePrefabsEmpty[0]) as GameObject;
                            }
                            else if (random > 15 && random <= 75)
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
                //Hard 1: Fuck you.
                case 6:
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
                            if (random <= 10)
                            {
                                temp = Instantiate(tilePrefabsBonus[RandomPreFabindex(1)]) as GameObject;
                            }
                            else if (random > 10 && random <= 50)
                            {
                                temp = Instantiate(tilePrefabsNormal[RandomPreFabindex(2)]) as GameObject;
                            }
                            else if (random > 50 && random <= 75)
                            {
                                temp = Instantiate(tilePrefabsVeryHard[RandomPreFabindex(6)]) as GameObject;
                            }
                            else
                            {
                                temp = Instantiate(tilePrefabsVeryHard[RandomPreFabindex(6)]) as GameObject;
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
            activeTiles.Add(temp);
        }
        else
        {
            SetSpeed(110);
            if (firstFiveEmpty > 0)
            {
                temp = Instantiate(tutorialTilePrefabs[0]) as GameObject;
                firstFiveEmpty--;
                temp.transform.SetParent(transform);
                temp.transform.position = Vector3.forward * spawnZ;
                tileLength = temp.gameObject.GetComponent<PrefabInformation>().tileLength;
                spawnZ += tileLength;
                activeTiles.Add(temp);
            }
            else
            {
                if (tutorialIndex < tutorialTilePrefabs.Length)
                {
                    temp = Instantiate(tutorialTilePrefabs[tutorialIndex]) as GameObject;
                    tutorialIndex++;
                    temp.transform.SetParent(transform);
                    temp.transform.position = Vector3.forward * spawnZ;
                    tileLength = temp.gameObject.GetComponent<PrefabInformation>().tileLength;
                    spawnZ += tileLength;
                    activeTiles.Add(temp);
                }
            }
        }

    }

    private void DeleteTile()
    {
        possibleStylePoints += activeTiles[1].gameObject.GetComponent<PrefabInformation>().possibleStylePoints;
        possibleStylePoints += activeTiles[1].gameObject.GetComponent<PrefabInformation>().coinsSpawned;
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
        else if (type == 6)
        {
            if (tilePrefabsVeryHard.Length <= 1)
            {
                return 0;
            }
            int randomIndex = lastPrefabIndex;
            while (randomIndex == lastPrefabIndex)
            {
                randomIndex = UnityEngine.Random.Range(0, tilePrefabsVeryHard.Length);
            }
            lastPrefabIndex = randomIndex;
            return randomIndex;
        }
        else return 0;
    }
}
