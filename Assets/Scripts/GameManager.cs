using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float HaxorScore;
    public float hourHaxorScore;
    public float BugSquashScore;
    public float Money;
    public float ScoreMultiplier;
    public TMP_Text MoneyText;
    public TMP_Text HaxorScoreText;
    public TMP_Text BugSquashScoreText;
    public TMP_Text FinalScoreText;

    public static GameManager instance;

    public bool isGamePlaying;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        isGamePlaying = true;
    }

    public void UpdateHaxorScore(float score)
    {
        HaxorScore += (score * ScoreMultiplier);
        if (HaxorScore <= 0)
        {
            HaxorScore = 0;
        }
        hourHaxorScore += (score * ScoreMultiplier);
        HaxorScoreText.text = HaxorScore.ToString();
    }
    
    public void UpdateBugScore(float score)
    {
        BugSquashScore += score;
        BugSquashScoreText.text = BugSquashScore.ToString();
    }

    public void EndNight()
    {
        FinalScoreText.text = "$" + CalculateFinalScore().ToString("F2");
        isGamePlaying = false;
    }

    private float CalculateFinalScore()
    {
        float finalMoney = Money;
        return finalMoney;
    }
    
    public void HourlyPayout()
    {
        UpdateMoney(hourHaxorScore / 10f);
        hourHaxorScore = 0; // Reset HaxorScore for the next hour
    }

    public void UpdateMoney(float newMoney)
    {

        Money += newMoney;
        if (Money <= 0)
        {
            Money = 0;
        }
        MoneyText.text = "$" +Money.ToString("F2"); 

    }

}

