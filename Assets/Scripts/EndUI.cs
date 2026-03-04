using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndUI : MonoBehaviour
{
    private void OnEnable()
    {
        var doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        root.Q<Label>("ResultLabel").text = EndData.ResultText;
        root.Q<Label>("ScoreLabel").text = "Score: " + EndData.FinalScore;

        root.Q<Button>("RestartButton").clicked += () => SceneManager.LoadSceneAsync("Game");
        root.Q<Button>("MenuButton").clicked += () => SceneManager.LoadSceneAsync("Menu");
    }
}