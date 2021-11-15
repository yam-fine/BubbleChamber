using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] public TMP_Text scoreText;
    [SerializeField] public GameObject spawnPointsParent;
    [SerializeField] public Transform player;
    [SerializeField] public float spawnRadiusFromObj; // spawn outside this radius of all objects (players and enemy)
    [SerializeField] public float sightRadius; // radius to disable spawns.
    [SerializeField] public float validRadius; // limit radius from player to spawn.
    
    private static TextMeshProUGUI uiscore; // CHANGE FROM STATIC
    public static int score; // CHANGE FROM STATIC
    private STAGE gameStage;
    private bool[] spawnsOnBoard;
    
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
        DisableAllFood();
        SpawnPoints();
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

    private void DisableAllFood()
    {
        for (int i = 0; i < spawnPointsParent.transform.childCount; i++)
        {
            spawnPointsParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    //spawn new food to the game. assume there is available object to spawn. 
    private void SpawnPoints()
    {
        Vector3 position = FindPosToSpawn();
        int indexToSpawn = FindIndexToSpawn();
        if (indexToSpawn == -1)
        {
            Debug.Log("No object to spawn in hierarchy! ");
        }
        
        Transform spawn = spawnPointsParent.transform.GetChild(indexToSpawn);
        spawn.transform.position = position;
        spawn.gameObject.SetActive(true);
    }

    
    private int FindIndexToSpawn()
    {
        for (int i = 0; i < spawnPointsParent.transform.childCount; i++)
        {
            if (!spawnPointsParent.transform.GetChild(i).gameObject.activeSelf)
            {
                return i;
            }
        }

        return -1;
    }
    
    //find available pos to spawn.
    private Vector3 FindPosToSpawn()
    {
        var spawnPosition = (player.transform.position + ((validRadius * Random.insideUnitSphere)));
        
        if (Vector3.Distance(spawnPosition, player.transform.position) <= spawnRadiusFromObj)
        {
            return FindPosToSpawn();
        }

        for (int i = 0; i < spawnsOnBoard.Length; i++)
        {
            if (spawnPointsParent.transform.GetChild(i).gameObject.activeSelf &&
                Vector3.Distance(spawnPointsParent.transform.GetChild(i).position, spawnPosition)
                <= spawnRadiusFromObj)
                return FindPosToSpawn();
        }

        return spawnPosition;
    }
}
