using UnityEngine;
using TMPro;
using System;
using UnityEngine.Rendering; // Required for TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public delegate void ScoreReceiver(int score);

    public static event ScoreReceiver OnScoreChange;
    public static GameManager Instance;
    public static event ScoreReceiver OnPlaneDie;
    public int amount = 1;
    private int score = 0;
    private string gate;
    int intgate;
    public void OnEnable()
    {
        PlaneMovementScript.PlaneDied += OnPlaneDeath;
        PlaneMovementScript.ScoreChanged += AddScore;
    }

    private void OnDisable()
    {
        PlaneMovementScript.PlaneDied -= OnPlaneDeath;
        PlaneMovementScript.ScoreChanged -= AddScore; // FIXED: Changed + to -
    }

    private void OnPlaneDeath()
    {
        OnPlaneDie?.Invoke(score);

        // 1. Find the "GameManager" object in the scene
        GameObject managerObj = GameObject.Find("GameManager");

        if (managerObj != null)
        {
            // 2. Get the LeaderboardHandler script attached to that object
            LeaderboardHandler lb = managerObj.GetComponent<LeaderboardHandler>();

            if (lb != null)
            {
                name = PlayerPrefs.GetString("userName");
                gate = PlayerPrefs.GetString("FlightNo");
                intgate = int.Parse(gate);
                lb.UploadScore(name, score, intgate);
            }
            else
            {
                Debug.LogError("The object 'GameManager' does not have a LeaderboardHandler script!");
            }
        }
        else
        {
            Debug.LogError("Could not find a GameObject named 'GameManager' in the scene!");
        }
    }

    private void AddScore()
    {
        score += amount;
        UpdateScoreDisplay();
    }


    public int getScore()
    {
        return score;
    }
    void UpdateScoreDisplay()
    {
        OnScoreChange.Invoke(score);
    }
}
