using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] public TMP_Text scoreText;
    [SerializeField] public float spawnRadiusFromObj; // spawn outside this radius of all objects (players and enemy)
    [SerializeField] public float sightRadius; // radius to disable spawns.
    [SerializeField] public float validRadius; // limit radius from player to spawn.
    
    private static TextMeshProUGUI uiscore; // CHANGE FROM STATIC
    public static int score; // CHANGE FROM STATIC
    private STAGE gameStage;
    private bool[] spawnsOnBoard;
    private Transform player;
    
    private enum STAGE
    {
        TOTURIAL,
        STAGE1
    };

    static GameManager instance;
    public static GameManager Instance {
        get {
            if (!instance) {
                instance = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        uiscore = scoreText.GetComponent<TextMeshProUGUI>();
        gameStage = STAGE.TOTURIAL;
        uiscore.text = "Score: 0";
        score = 0;
        player = Player.Instance.transform;
        // DisableAllFood();
        // SpawnPoints();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameStage)
        {
            case STAGE.TOTURIAL:
                
                
            case STAGE.STAGE1:
                break;
        }
    }

    public static void IncScore()
    {
        score++;
        uiscore.text = ("Score: " + score);
    }

    // private void DisableAllFood()
    // {
    //     GameObject[] allFood = GameObject.FindGameObjectsWithTag("Food");
    //     GameObject[] allEnems = GameObject.FindGameObjectsWithTag("Enemy");
    //     GameObject[] allTargets = allFood.Concat(allEnems).ToArray();
    //
    //     foreach (GameObject enemy in allTargets)
    //     {
    //         enemy.SetActive(false);
    //     }
    // }

    //spawn new food to the game. assume there is available object to spawn. 
    // private void SpawnPoints()
    // {
    //     Vector3 position = FindPosToSpawn();
    //     int indexToSpawn = FindIndexToSpawn();
    //     if (indexToSpawn == -1)
    //     {
    //         Debug.Log("No object to spawn in hierarchy! ");
    //     }
    //     
    //     Transform spawn = spawnPointsParent.transform.GetChild(indexToSpawn);
    //     spawn.transform.position = position;
    //     spawn.gameObject.SetActive(true);
    // }

    
    // private int FindIndexToSpawn()
    // {
    //     GameObject[] allFood = GameObject.FindGameObjectsWithTag("Food");
    //     GameObject[] allEnems = GameObject.FindGameObjectsWithTag("Enemy");
    //     GameObject[] allTargets = allFood.Concat(allEnems).ToArray();
    //
    //     for (int i = 0; i < spawnPointsParent.transform.childCount; i++)
    //     {
    //         if (!spawnPointsParent.transform.GetChild(i).gameObject.activeSelf)
    //         {
    //             return i;
    //         }
    //     }
    //
    //     return -1;
    // }
    
    //find available pos to spawn.
    public Vector3 FindPosToSpawn()
    {
        // Vector3 tmp = validRadius * Random.insideUnitSphere;
        // Vector3 pos = player.transform.position;
        // Vector3 spawnPosition = new Vector3(pos.x + tmp.x, pos.y + tmp.y, pos.z + tmp.z);
        //Vector3 spawnPosition = new Vector3(validRadius * Random.insideUnitCircle)
        
        Vector3 spawnPosition = (player.position + (validRadius * (Random.insideUnitSphere)));
        while (Vector3.Distance(spawnPosition, player.position) <= spawnRadiusFromObj)
        {
            // for (int i = 0; i < spawnsOnBoard.Length; i++)
            // {
            //     if (spawnPointsParent.transform.GetChild(i).gameObject.activeSelf &&
            //         Vector3.Distance(spawnPointsParent.transform.GetChild(i).position, spawnPosition)
            //         <= spawnRadiusFromObj)
            //         return FindPosToSpawn();
            // }
            spawnPosition = (player.position + (validRadius * (Random.insideUnitSphere)));
        }

        return spawnPosition;
    }
}
