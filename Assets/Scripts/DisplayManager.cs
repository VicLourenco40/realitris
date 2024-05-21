using TMPro;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    public TextMeshProUGUI levelDisplay;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI linesDisplay;
    public TextMeshProUGUI gameOverMessage;

    public GameObject prefabBlock;

    private static int[,] IPiece = {
        { 1, 1, 1, 1 }
    };
    private static int[,] JPiece = {
        { 1, 0, 0 },
        { 1, 1, 1 }
    };
    private static int[,] LPiece = {
        { 0, 0, 1 },
        { 1, 1, 1 }
    };
    private static int[,] OPiece = {
        { 1, 1 },
        { 1, 1 }
    };
    private static int[,] SPiece = {
        { 0, 1, 1 },
        { 1, 1, 0 }
    };
    private static int[,] TPiece = {
        { 0, 1, 0 },
        { 1, 1, 1 }
    };
    private static int[,] ZPiece = {
        { 1, 1, 0 },
        { 0, 1, 1 }
    };
    public int[][,] Pieces = {
        IPiece, JPiece, LPiece, OPiece, SPiece, TPiece, ZPiece
    };

    private static Color colorPiece = new(1f, 1f, 1f, 1f);
    private static Color colorIPiece = new(0f, 1f, 1f, 1f);
    private static Color colorJPiece = new(0f, 0f, 1f, 1f);
    private static Color colorLPiece = new(1f, 0.5f, 0f, 1f);
    private static Color colorOPiece = new(1f, 1f, 0f, 1f);
    private static Color colorSPiece = new(0f, 1f, 0f, 1f);
    private static Color colorTPiece = new(0.75f, 0f, 1f, 1f);
    private static Color colorZPiece = new(1f, 0f, 0f, 1f);

    private static Color[] colors = {
        colorIPiece, colorJPiece, colorLPiece, colorOPiece, colorSPiece, colorTPiece, colorZPiece
    };

    private static GameManager game;
    private static GameObject[,] gameGrid;

    void Start() {
        game = GameObject.Find("Game Manager").GetComponent<GameManager>();
        gameGrid = new GameObject[game.GridRows, game.GridColumns];

        InitializeGameGrid();
        UpdateNextGrid();
    }

    void Update() {
        if (!game.gameOver) {
            UpdateGameGrid();
        }
    }

    public void GameStarted() {
        gameOverMessage.gameObject.SetActive(false);

        UpdateNextGrid();
        UpdateHoldGrid();
        UpdateLevel();
        UpdateScore();
        UpdateLines();
    }

    public void GameEnded() {
        gameOverMessage.gameObject.SetActive(true);
    }

    private void InitializeGameGrid() {
        for (int row = 0; row < game.GridRows; row++) {
            for (int column = 0; column < game.GridColumns; column++) {
                GameObject block = Instantiate(prefabBlock, Vector3.zero, Quaternion.Euler(0, 180, 0));
                block.transform.parent = GameObject.Find("Grid").transform;
                Vector3 position = new(column, game.GridRows - row - 1, 0);
                block.transform.localPosition = position;
                block.SetActive(false);
                gameGrid[row, column] = block;
            }
        }
    }

    private void UpdateGameGrid() {
        for (int row = 0; row < game.GridRows; row++) {
            for (int column = 0; column < game.GridColumns; column++) {
                GameObject block = gameGrid[row, column];
                Renderer renderer = block.GetComponent<Renderer>();

                int cell = game.grid[row + game.GridExtraRows, column];

                block.SetActive(cell > -1);

                if (cell > -1) {
                    renderer.materials[0].color = colors[cell];
                    renderer.materials[1].color = colorPiece;
                }

                if (game.IsCellActive(row + game.GridExtraRows, column, true)) {
                    block.SetActive(true);

                    renderer.material.color = colors[game.active];

                    foreach (Material material in renderer.materials) {
                        Color colorOpaque = material.color;
                        Color colorTransparent = new(colorOpaque.r, colorOpaque.g, colorOpaque.b, 0.3f);
                        material.color = colorTransparent;
                    }
                }

                if (game.IsCellActive(row + game.GridExtraRows, column)) {
                    block.SetActive(true);
                    renderer.materials[0].color = colors[game.active];
                    renderer.materials[1].color = colorPiece;
                }
            }
        }
    }

    public void UpdateNextGrid() {
        if (game != null) {
            int nextPiece = game.pieceSequence[game.pieceIndex + 1];
            UpdateGrids("Next", nextPiece);
        }
    }

    public void UpdateHoldGrid() {
        if (game != null) {
            UpdateGrids("Hold", game.holdPiece);
        }
    }

    private void UpdateGrids(string parentName, int piece) {
        GameObject parent = GameObject.Find(parentName);
        
        foreach (Transform child in parent.transform) {
            Destroy(child.gameObject);
        }

        if (piece == -1) { return; }
        
        int pieceX = Pieces[piece].GetLength(1);
        int pieceY = Pieces[piece].GetLength(0);

        for (int row = 0; row < pieceY; row++) {
            for (int column = 0; column < pieceX; column++) {
                if (Pieces[piece][row, column] == 0) { continue; }

                GameObject block = Instantiate(prefabBlock, Vector3.zero, Quaternion.Euler(0, 180, 0));
                Renderer renderer = block.GetComponent<Renderer>();

                float originX = ((4 - pieceX) / 2f) + column;
                float originY = 3 - ((4 - pieceY) / 2f) - row;
                Vector3 position = new(originX, originY, 0);
                block.transform.parent = parent.transform;
                block.transform.localPosition = position;

                renderer.material.color = colors[piece];
            }
        }
    }

    public void UpdateLevel() {
        if (game != null) {
            levelDisplay.text = "Level: " + game.level;
        }
    }

    public void UpdateScore() {
        if (game != null) {
            scoreDisplay.text = "Score: " + game.score;
        }
    }

    public void UpdateLines() {
        if (game != null) {
            linesDisplay.text = "Lines: " + game.linesCleared;
        }
    }
}
