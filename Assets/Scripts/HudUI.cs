using UnityEngine;
using UnityEngine.UIElements;

public class HudUI : MonoBehaviour
{
    private Label scoreLabel;
    private Label timerLabel;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        scoreLabel = root.Q<Label>("ScoreLabel");
        timerLabel = root.Q<Label>("TimerLabel");
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (scoreLabel == null || timerLabel == null) return;

        scoreLabel.text = "Score: " + GameManager.Instance.Score;
        timerLabel.text = "Time: " + Mathf.CeilToInt(GameManager.Instance.TimeLeft);
    }
}