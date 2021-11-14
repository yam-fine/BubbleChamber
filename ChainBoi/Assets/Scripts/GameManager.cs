using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] public TMP_Text scoreText;
    private static TextMeshProUGUI uiscore;
    public static int score;

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
        
        
        //uiscore.SetText("Score: 0");
        
        uiscore.text = "Score: 0";
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void IncScore()
    {
        score++;
        uiscore.text = ("Score: " + score);
    }
    
    
}
