using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_Text hScoreText;
    [SerializeField] string sceneToLoad = "SampleScene";

    static public int highestScore;
    GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
        highestScore = (highestScore < gm.Score) ? gm.Score : highestScore;
        hScoreText.GetComponent<TextMeshProUGUI>().text = "Highest Score: " + highestScore;
    }

    public void Play() {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void Exit() {
        Application.Quit();
    }

    public void Credits() {

    }
}
