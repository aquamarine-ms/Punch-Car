using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance = null;
    
    [SerializeField] private Text scoreText;

    [SerializeField] private Transform canvas;

    [SerializeField] private GameObject scoreAddAnimation;

    public Combo combo;

    public int maxScoreOnLevel;
    
    [Serializable]
    public class Combo
    {
        public bool canCombo;
        public int multiplier = 1;
        private int maxMultiplier = 5;

        public void IncreaseMultiplier()
        {
            multiplier++;
            Mathf.Clamp(multiplier, 1, maxMultiplier);
        }

        private void ZeroingMultiplier()
        {
            multiplier = 1;
        }

        public void StartCombo()
        {
            canCombo = true;
        }

        public void StopCombo()
        {
            canCombo = false;
            ZeroingMultiplier();
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this; 
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ResetScore();
    }

    public void AddScore(int score = 100)
    {
        int addingScore = score * combo.multiplier;
        int value = GetScoreValue() + addingScore;
        
        if (combo.canCombo)
            combo.IncreaseMultiplier();

        scoreText.text = value.ToString();

        ShowAnimation(addingScore);
    }

    private void ShowAnimation(int score)
    {
        if (scoreAddAnimation != null)
            CreateAddingScoreAnimation(score);
    }

    private void CreateAddingScoreAnimation(int score)
    {
        GameObject go = Instantiate(scoreAddAnimation, canvas);

        go.GetComponent<Text>().text ="+" + score.ToString();
    }

    public int GetScoreValue()
    {
        return Convert.ToInt32(scoreText.text);
    }

    public void ResetScore()
    {
        scoreText.text = 0.ToString();
    }
}
