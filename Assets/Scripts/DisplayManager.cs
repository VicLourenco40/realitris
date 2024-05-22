using TMPro;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    public TextMeshProUGUI levelDisplay;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI linesDisplay;
    public TextMeshProUGUI highScoreDisplay;
    public TextMeshProUGUI gameOverMessage;

    private static GameManager game;

    void Start() {
        game = GameObject.Find("Game Manager").GetComponent<GameManager>();

        game.gameStarted.AddListener(GameStarted);
        game.gameEnded.AddListener(GameEnded);
        game.levelUpdated.AddListener(UpdateLevel);
        game.scoreUpdated.AddListener(UpdateScore);
        game.linesUpdated.AddListener(UpdateLines);

        UpdateHighScore();
    }

    public void GameStarted() {
        gameOverMessage.gameObject.SetActive(false);
        UpdateLevel();
        UpdateScore();
        UpdateLines();
    }

    public void GameEnded() {
        gameOverMessage.gameObject.SetActive(true);
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
