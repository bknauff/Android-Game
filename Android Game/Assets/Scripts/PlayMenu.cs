using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour {
    public Text scoreText;

    public void Start()
    {
        scoreText.text = PlayerPrefs.GetInt("score").ToString();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("GameLevel");
    }
}
