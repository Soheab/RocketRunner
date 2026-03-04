using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuUI : MonoBehaviour
{
    private void OnEnable()
    {
        var doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        var startBtn = root.Q<Button>("StartButton");
        var diff = root.Q<DropdownField>("DifficultyDropdown");
        var time = root.Q<DropdownField>("TimeDropdown");
        var speed = root.Q<DropdownField>("SpeedDropdown");
        var winScore = root.Q<DropdownField>("WinScoreDropdown");

        if (startBtn == null)
        {
            Debug.LogError("MenuUI: StartButton niet gevonden. Check Name in Menu.uxml");
            return;
        }

        // Difficulty
        if (diff != null)
        {
            diff.choices = new List<string> { "Makkelijk", "Normaal", "Moeilijk" };
            if (SettingsData.DifficultyIndex >= 0 && SettingsData.DifficultyIndex < diff.choices.Count)
                diff.value = diff.choices[SettingsData.DifficultyIndex];
            if (string.IsNullOrWhiteSpace(diff.value)) diff.value = "Normaal";
        }

        // Time options
        if (time != null)
        {
            time.choices = new List<string> { "30", "60", "90", "120", "180" };
            if (string.IsNullOrWhiteSpace(time.value)) time.value = "60";
        }

        // Speed options
        if (speed != null)
        {
            speed.choices = new List<string> { "4", "6", "8", "10", "12" };
            if (string.IsNullOrWhiteSpace(speed.value)) speed.value = "6";
        }

        // Win score options (0 disables score-based win)
        if (winScore != null)
        {
            winScore.choices = new List<string>
            {
                "0", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100"
            };

            if (SettingsData.WinScore >= 0 && SettingsData.WinScore <= 100)
                winScore.value = SettingsData.WinScore.ToString();

            if (string.IsNullOrWhiteSpace(winScore.value)) winScore.value = "20";


        }

        startBtn.clicked += () =>
        {
            // difficulty index
            if (diff != null)
            {
                SettingsData.DifficultyIndex = diff.value switch
                {
                    "Makkelijk" => 0,
                    "Moeilijk" => 2,
                    _ => 1
                };
            }

            // time seconds
            if (time != null && float.TryParse(time.value, out var t))
                SettingsData.RoundTimeSeconds = t;

            // player speed
            if (speed != null && float.TryParse(speed.value, out var s))
                SettingsData.PlayerSpeed = s;

            // win score target
            if (winScore != null && int.TryParse(winScore.value, out var ws))
                SettingsData.WinScore = Mathf.Clamp(ws, 0, 100);

            SettingsData.ApplyDifficulty();
            SceneManager.LoadSceneAsync("Game");
        };
    }
}
