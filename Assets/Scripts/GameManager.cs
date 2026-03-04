using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int winScore = 20;


    public float TimeLeft { get; private set; }
    public int Score { get; private set; }
    public bool Ended { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        TimeLeft = SettingsData.RoundTimeSeconds;
        winScore = Mathf.Clamp(SettingsData.WinScore, 0, 100);
        Score = 0;
        Ended = false;
    }

    private void Update()
    {
        if (Ended) return;

        TimeLeft -= Time.deltaTime;

        if (TimeLeft <= 0f)
        {
            TimeLeft = 0f;
            if (winScore <= 0)
                Win();
            else
                Lose();
            return;
        }

        if (winScore > 0 && Score >= winScore)
        {
            Win();
            return;
        }
    }

    public void AddScore(int amount)
    {
        if (Ended) return;
        Score += amount;
    }

    public void Lose()
    {
        if (Ended) return;
        Ended = true;
        EndData.ResultText = "Je hebt verloren.";
        EndData.FinalScore = Score;
        SceneManager.LoadSceneAsync("End");
    }

    public void Win()
    {
        if (Ended) return;
        Ended = true;
        EndData.ResultText = "Je hebt gewonnen!";
        EndData.FinalScore = Score;
        SceneManager.LoadSceneAsync("End");
    }
}