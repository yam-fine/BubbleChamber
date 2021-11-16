using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject enemyPref;
    [SerializeField] float validRadius; // limit radius from player to spawn.

    private TextMeshProUGUI uiscore; 
    private int score = 0; 
    private STAGE gameStage;
    Transform player;

    public int Score { get { return score; } }
    
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
        //DisableAllFood();
        //SpawnPoints();
        player = Player.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameStage)
        {
            case STAGE.TOTURIAL:
                //todo : 1. one food on board. 
                //todo : 2. check insight and update.

                
            case STAGE.STAGE1:
                break;
        }
    }

    public void IncScore()
    {
        score++;
        uiscore.text = ("Score: " + score);
    }

    //private void DisableAllFood()
    //{
    //    for (int i = 0; i < spawnPointsParent.transform.childCount; i++)
    //    {
    //        spawnPointsParent.transform.GetChild(i).gameObject.SetActive(false);
    //    }
    //}

    //spawn new food to the game. assume there is available object to spawn. 
    private void SpawnPoints() {
        Vector3 position = FindPosToSpawn();
        GameObject newFood = Instantiate(enemyPref, position, Quaternion.identity);
    }

    private void Reset() {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("SampleScene");
    }


    //private int FindIndexToSpawn()
    //{
    //    for (int i = 0; i < spawnPointsParent.transform.childCount; i++)
    //    {
    //        if (!spawnPointsParent.transform.GetChild(i).gameObject.activeSelf)
    //        {
    //            return i;
    //        }
    //    }

    //    return -1;
    //}

    //find available pos to spawn.
    public Vector3 FindPosToSpawn()
    {
        Vector3 spawnPosition = (player.transform.position + ((validRadius * Random.insideUnitSphere)));
        GameObject[] allFood = GameObject.FindGameObjectsWithTag("Food");
        GameObject[] allEnems = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> allTargets = new List<GameObject>();
        allTargets.AddRange(allFood.Concat(allEnems));
        allTargets.Add(player.gameObject);

        while (true) {
            foreach (GameObject go in allTargets) {
                if (Vector2.Distance(go.transform.position, spawnPosition) < validRadius)
                    spawnPosition = (player.transform.position + ((validRadius * Random.insideUnitSphere)));
            }
            break;
        }

        return spawnPosition;
    }
}
