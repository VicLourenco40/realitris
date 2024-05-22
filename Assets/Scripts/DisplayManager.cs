using TMPro;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    public TextMeshProUGUI levelDisplay;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI linesDisplay;
    public TextMeshProUGUI highScoreDisplay;
    public TextMeshProUGUI gameOverMessage;

    private ButtonManager restart;
    private ButtonManager quit;

    private static GameManager game;

    void Start() {
        game = GameObject.Find("Game Manager").GetComponent<GameManager>();

        game.gameStarted.AddListener(GameStarted);
        game.gameEnded.AddListener(GameEnded);
        game.levelUpdated.AddListener(UpdateLevel);
        game.scoreUpdated.AddListener(UpdateScore);
        game.linesUpdated.AddListener(UpdateLines);

        restart = GameObject.Find("Restart Button").GetComponent<ButtonManager>();
        quit = GameObject.Find("Quit Button").GetComponent<ButtonManager>();

        UpdateHighScore();
        GameStarted();
    }

    void Update() {
        if (game.gameOver) {
            if (restart.IsClicked) {
                game.RestartGame();
            }
            if (quit.IsClicked) {
                Application.Quit();
            }
        }
    }

    public void GameStarted() {
        gameOverMessage.gameObject.SetActive(false);
        restart.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        UpdateLevel();
        UpdateScore();
        UpdateLines();
    }

    public void GameEnded() {
        gameOverMessage.gameObject.SetActive(true);
        restart.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
        UpdateHighScore();
    }

    public void UpdateLevel() {
        levelDisplay.text = "Level: " + game.level;
    }

    public void UpdateScore() {
        scoreDisplay.text = "Score: " + game.score;
    }

    public void UpdateLines() {
        linesDisplay.text = "Lines: " + game.linesCleared;
    }

    public void UpdateHighScore() {
        highScoreDisplay.text = "High Score: " + game.highScore;
    }
}
