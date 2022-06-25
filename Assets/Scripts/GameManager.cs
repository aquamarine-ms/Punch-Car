using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    private ScoreManager scoreManager;
    private TileCreator tileCreator;

    public int scoreToEndRound;

    [SerializeField] private float timeBeforeRoundStarts = 5f;
    private WaitForSeconds tBefore;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this; 
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Initialize();
        
        StartGame();
    }

    private void Initialize()
    {
        scoreManager = FindObjectOfType<ScoreManager>();

        tileCreator = FindObjectOfType<TileCreator>();
        
        tBefore = new WaitForSeconds(timeBeforeRoundStarts);
    }

    public void StartGame()
    {
        StartCoroutine(GameLoop());
    }
    
    public void Reload()
    {
        SceneManager.LoadScene(0);
    }
    
    private IEnumerator GameLoop ()
    {
        yield return StartCoroutine (RoundStarting ());

        yield return StartCoroutine (RoundPlaying());

        yield return StartCoroutine (RoundEnding());

        StartCoroutine(GameLoop());
    }

    private IEnumerator RoundStarting ()
    {
        //before round starts
        
        Debug.Log("Round starts...");
        
        yield return (tBefore);
    }


    private IEnumerator RoundPlaying ()
    {
        tileCreator.StartLooping();
        
        Debug.Log("Round started!");

        while (scoreManager.GetScoreValue() < scoreToEndRound)
        {
            
            yield return null;
        }
    }


    private IEnumerator RoundEnding ()
    {
        Debug.Log("Round ends.");

        yield return new WaitForSeconds(1f);
    }
    
}
