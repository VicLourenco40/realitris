using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    public GameObject prefabBlock;

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
    private static GameObject[,] nextGrid;
    private static GameObject[,] holdGrid;

    void Start() {
        game = GameObject.Find("Game Manager").GetComponent<GameManager>();
        gameGrid = new GameObject[game.GridRows, game.GridColumns];
        nextGrid = new GameObject[game.PieceSize, game.PieceSize];
        holdGrid = new GameObject[game.PieceSize, game.PieceSize];

        InitializeGrids(game.GridRows, game.GridColumns, "Grid", gameGrid);
        InitializeGrids(game.PieceSize, game.PieceSize, "Next", nextGrid);
        InitializeGrids(game.PieceSize, game.PieceSize, "Hold", holdGrid);
    }

    void Update() {
        int nextPiece = game.pieceSequence[game.pieceIndex + 1];

        UpdateGameGrid();
        UpdateGrids(nextGrid, nextPiece);
        UpdateGrids(holdGrid, game.holdPiece);
    }

    private void InitializeGrids(int rows, int columns, string parentName, GameObject[,] objects) {
        for (int row = 0; row < rows; row++) {
            for (int column = 0; column < columns; column++) {
                GameObject block = Instantiate(prefabBlock, Vector3.zero, Quaternion.identity);
                block.transform.parent = GameObject.Find(parentName).transform;
                Vector3 position = new Vector3(column, (rows - row - 1), 0);
                block.transform.localPosition = position;
                block.SetActive(false);
                objects[row, column] = block;
            }
        }
    }

    private void UpdateGameGrid() {
        for (int row = 0; row < game.GridRows; row++) {
            for (int column = 0; column < game.GridColumns; column++) {
                GameObject block = gameGrid[row, column];
                GameObject mesh = block.transform.GetChild(0).gameObject;
                Renderer renderer = mesh.GetComponent<Renderer>();

                int cell = game.grid[row + game.GridExtraRows, column];
                
                block.SetActive(cell > -1);

                if (cell > -1) {
                    renderer.material.color = colors[cell];
                }

                if (game.IsCellActive(row + game.GridExtraRows, column, true)) {
                    block.SetActive(true);
                    Color colorOpaque = colors[game.active];
                    Color colorTransparent = new(colorOpaque.r, colorOpaque.g, colorOpaque.b, 0.3f);
                    renderer.material.color = colorTransparent;
                }

                if (game.IsCellActive(row + game.GridExtraRows, column)) {
                    block.SetActive(true);
                    renderer.material.color = colors[game.active];
                }
            }
        }
    }

    private void UpdateGrids(GameObject[,] grid, int piece) {
        for (int row = 0; row < grid.GetLength(0); row++) {
            for (int column = 0; column < grid.GetLength(0); column++) {
                GameObject block = grid[row, column];

                block.SetActive(false);

                if (piece == -1) { return; }

                if (game.Pieces[piece][0][row, column] == 1) {
                    GameObject mesh = block.transform.GetChild(0).gameObject;
                    Renderer renderer = mesh.GetComponent<Renderer>();

                    block.SetActive(true);
                    renderer.material.color = colors[piece];
                }
            }
        }
    }
}
