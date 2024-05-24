using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip playGame;
    public AudioClip gameOver;
    public AudioClip pieceMoved;
    public AudioClip pieceRotatedHeld;
    public AudioClip piecePlaced;

    private AudioSource audioSource;
    private static GameManager game;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        game = GameObject.Find("Game Manager").GetComponent<GameManager>();

        game.gameStarted.AddListener(PlayPlayGame);
        game.gameEnded.AddListener(PlayGameOver);
        game.pieceMoved.AddListener(PlayPieceMoved);
        game.pieceRotated.AddListener(PlayPieceRotatedHeld);
        game.pieceHeld.AddListener(PlayPieceRotatedHeld);
        game.piecePlaced.AddListener(PlayPiecePlaced);
    }

    public void PlayPlayGame() {
        audioSource.clip = playGame;
        audioSource.Play();
    }

    public void PlayGameOver() {
        audioSource.clip = gameOver;
        audioSource.Play();
    }

    public void PlayPieceMoved() {
        audioSource.clip = pieceMoved;
        audioSource.Play();
    }

    public void PlayPieceRotatedHeld() {
        audioSource.clip = pieceRotatedHeld;
        audioSource.Play();
    }

    public void PlayPiecePlaced() {
        audioSource.clip = piecePlaced;
        audioSource.Play();
    }
}
