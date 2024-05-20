using UnityEngine;

public class DisplayManager : MonoBehaviour
{
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
    }

    void Update() {
        int nextPiece = game.pieceSequence[game.pieceIndex + 1];
        
        UpdateGameGrid();
        UpdateGrids("Next", nextPiece);
        UpdateGrids("Hold", game.holdPiece);
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

                GameObject block = Instantiate(prefabBlock, Vector3.zero, Quaternion.identity);
                GameObject mesh = block.transform.GetChild(0).gameObject;
                Renderer renderer = mesh.GetComponent<Renderer>();

                float originX = ((4 - pieceX) / 2f) + column;
                float originY = 3 - ((4 - pieceY) / 2f) - row;
                Vector3 position = new(originX, originY, 0);
                block.transform.parent = parent.transform;
                block.transform.localPosition = position;

                renderer.material.color = colors[piece];
            }
        }
    }

    private void InitializeGameGrid() {
        for (int row = 0; row < game.GridRows; row++) {
            for (int column = 0; column < game.GridColumns; column++) {
                GameObject block = Instantiate(prefabBlock, Vector3.zero, Quaternion.identity);
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
}
