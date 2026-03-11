using System;
using TMPro;
using UnityEngine;

public class ScoreChangeListener : MonoBehaviour
{
    [SerializeField] private TMP_Text m_Text;
    private void OnEnable()
    {
        ScoreManager.OnScoreChange += ScoreChanged;
        ScoreManager.OnPlaneDie += PlaneDieScore;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreChange -= ScoreChanged;
        ScoreManager.OnPlaneDie -= PlaneDieScore;
    }

    private void PlaneDieScore(int score)
    {
        
    }

    private void ScoreChanged(int score)
    {
        m_Text.alignment = TextAlignmentOptions.Midline;
        m_Text.SetText($"Score: {score}");
    }
}
