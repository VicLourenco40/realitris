using TMPro;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI level;
    public TextMeshProUGUI score;
    public TextMeshProUGUI lines;
    public TextMeshProUGUI highScore;
    public TextMeshProUGUI gameOver;
    public GameObject darkPanel;

    private ButtonManager play;
    private ButtonManager quit;

    private static GameManager game;

    void Start() {
        game = GameObject.Find("Game Manager").GetComponent<GameManager>();

        game.gameStarted.AddListener(GameStarted);
        game.gameEnded.AddListener(GameEnded);
        game.levelUpdated.AddListener(UpdateLevel);
        game.scoreUpdated.AddListener(UpdateScore);
        game.linesUpdated.AddListener(UpdateLines);

        play = GameObject.Find("Play Button").GetComponent<ButtonManager>();
        quit = GameObject.Find("Quit Button").GetComponent<ButtonManager>();

        gameOver.gameObject.SetActive(false);
        UpdateHighScore();
    }

    void Update() {
        if (game.gameOver) {
            if (play.IsClicked) {
                game.RestartGame();
            }
            if (quit.IsClicked) {
                Application.Quit();
            }
        }
    }

    public void GameStarted() {
        title.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
        darkPanel.SetActive(false);
        play.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        UpdateLevel();
        UpdateScore();
        UpdateLines();
    }

    public void GameEnded() {
        title.gameObject.SetActive(true);
        gameOver.gameObject.SetActive(true);
        darkPanel.SetActive(true);
        play.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
        UpdateHighScore();
    }

    public void UpdateLevel() {
        level.text = "Level: " + game.level;
    }

    public void UpdateScore() {
        score.text = "Score: " + game.score;
    }

    public void UpdateLines() {
        lines.text = "Lines: " + game.linesCleared;
    }

    public void UpdateHighScore() {
        highScore.text = "High Score: " + game.highScore;
    }
}
