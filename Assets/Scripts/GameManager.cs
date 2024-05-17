using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI gridDisplay;

    private const int GridRows = 20;
    private const int GridExtraRows = 4;
    private const int GridColumns = 10;

    private const float DropSpeed = 1.0f;
    private const float DasDelaySpeed = 0.3f;
    private const float DasSpeed = 0.05f;

    private static int[][,] IPiece = {
        new int[,] {
            { 0, 0, 0, 0 },
            { 1, 1, 1, 1 },
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 }
        },
        new int[,] {
            { 0, 0, 1, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 1, 0 }
        },
        new int[,] {
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 },
            { 1, 1, 1, 1 },
            { 0, 0, 0, 0 }
        },
        new int[,] {
            { 0, 1, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 1, 0, 0 }
        }
    };
    private static int[][,] JPiece = {
        new int[,] {
            { 1, 0, 0 },
            { 1, 1, 1 },
            { 0, 0, 0 }
        },
        new int[,] {
            { 0, 1, 1 },
            { 0, 1, 0 },
            { 0, 1, 0 }
        },
        new int[,] {
            { 0, 0, 0 },
            { 1, 1, 1 },
            { 0, 0, 1 }
        },
        new int[,] {
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 1, 1, 0 }
        }
    };
    private static int[][,] LPiece = {
        new int[,] {
            { 0, 0, 1 },
            { 1, 1, 1 },
            { 0, 0, 0 }
        },
        new int[,] {
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 0, 1, 1 }
        },
        new int[,] {
            { 0, 0, 0 },
            { 1, 1, 1 },
            { 1, 0, 0 }
        },
        new int[,] {
            { 1, 1, 0 },
            { 0, 1, 0 },
            { 0, 1, 0 }
        }
    };
    private static int[][,] SPiece = {
        new int[,] {
            { 0, 1, 1 },
            { 1, 1, 0 },
            { 0, 0, 0 }
        },
        new int[,] {
            { 0, 1, 0 },
            { 0, 1, 1 },
            { 0, 0, 1 }
        },
        new int[,] {
            { 0, 0, 0 },
            { 0, 1, 1 },
            { 1, 1, 0 }
        },
        new int[,] {
            { 1, 0, 0 },
            { 1, 1, 0 },
            { 0, 1, 0 }
        }
    };
    private static int[][,] ZPiece = {
        new int[,] {
            { 1, 1, 0 },
            { 0, 1, 1 },
            { 0, 0, 0 }
        },
        new int[,] {
            { 0, 0, 1 },
            { 0, 1, 1 },
            { 0, 1, 0 }
        },
        new int[,] {
            { 0, 0, 0 },
            { 1, 1, 0 },
            { 0, 1, 1 }
        },
        new int[,] {
            { 0, 1, 0 },
            { 1, 1, 0 },
            { 1, 0, 0 }
        }
    };
    private static int[][,] TPiece = {
        new int[,] {
            { 0, 1, 0 },
            { 1, 1, 1 },
            { 0, 0, 0 }
        },
        new int[,] {
            { 0, 1, 0 },
            { 0, 1, 1 },
            { 0, 1, 0 }
        },
        new int[,] {
            { 0, 0, 0 },
            { 1, 1, 1 },
            { 0, 1, 0 }
        },
        new int[,] {
            { 0, 1, 0 },
            { 1, 1, 0 },
            { 0, 1, 0 }
        }
    };
    private static int[][,] OPiece = {
        new int[,] {
            { 1, 1 },
            { 1, 1 },
        }
    };

    private static int[][][,] Pieces = {
        IPiece, JPiece, LPiece, SPiece, ZPiece, TPiece, OPiece
    };

    private static int[,] grid = new int[GridRows + GridExtraRows, GridColumns];

    private static int[][,] active;
    private static int activeRotation;
    private static int activeSize;
    private static int activeRow;
    private static int activeColumn;

    private static float dropTimer = DropSpeed;
    private static float dasDelayTimer = DasDelaySpeed;
    private static float dasTimer = DasSpeed;
    private static float dasDownDelayTimer = DasDelaySpeed;
    private static float dasDownTimer = DasSpeed;

    private static bool gameOver = false;
    private static float gameOverTimer = 5.0f;

    void Start() {
        CreateActive();
    }

    void Update() {
        if (gameOver) {
            gameOverTimer -= Time.deltaTime;

            if (gameOverTimer <= 0.0f) {
                RestartGame();
            }
        } else {
            UpdateActive();
            UpdateGridDisplay();
        }
    }

    private void RestartGame() {
        grid = new int[GridRows + GridExtraRows, GridColumns];
        gameOver = false;
        gameOverTimer = 5.0f;

        CreateActive();
    }

    private void ClearRows() {
        for (int row = activeRow; row < Mathf.Min(activeRow + activeSize, GridRows + GridExtraRows); row++) {
            bool clearRow = true;

            for (int column = 0; column < GridColumns; column++) {
                if (grid[row, column] == 0) {
                    clearRow = false;
                }
            }

            if (clearRow) {
                for (int r = row - 1; r >= 0; r--) {
                    for (int c = 0; c < GridColumns; c++) {
                        grid[r + 1, c] = grid[r, c];
                    }
                }

                for (int c = 0; c < GridColumns; c++) {
                    grid[0, c] = 0;
                }
            }
        }
    }

    private void CheckGridOverflow() {
        for (int row = GridExtraRows - 1; row >= 0; row--) {
            for (int column = 0; column < GridColumns; column++) {
                if (grid[row, column] == 1) {
                    gameOver = true;
                }
            }
        }
    }

    private void CreateActive() {
        active = Pieces[Random.Range(0, Pieces.Length)];
        activeRotation = 0;
        activeSize = active[activeRotation].GetLength(0);
        activeRow = 2;
        activeColumn = (GridColumns - activeSize) / 2;
    }

    private void PlaceActive() {
        for (int pieceRow = 0; pieceRow < activeSize; pieceRow++) {
            for (int pieceColumn = 0; pieceColumn < activeSize; pieceColumn++) {
                if (active[activeRotation][pieceRow, pieceColumn] == 1) {
                    grid[activeRow + pieceRow, activeColumn + pieceColumn] = 1;
                }
            }
        }

        ClearRows();

        if (activeRow < GridExtraRows) {
            CheckGridOverflow();
        }

        CreateActive();
    }

    private int GetNextRotation() {
        int lastRotation = active.Length - 1;

        return activeRotation == lastRotation ? 0 : activeRotation + 1;
    }

    private int GetDirection() {
        int direction = 0;

        if (Input.GetKey(KeyCode.LeftArrow)) { direction--; }
        if (Input.GetKey(KeyCode.RightArrow)) { direction++; }

        return direction;
    }

    private void UpdateActive() {
        dropTimer -= Time.deltaTime;

        if (dropTimer <= 0.0f || Input.GetKeyDown(KeyCode.DownArrow)) {
            if (IsPositionValid(activeRow + 1, activeColumn, activeRotation)) {
                activeRow++;
            } else {
                PlaceActive();
            }

            dropTimer = DropSpeed;
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            dasDownDelayTimer -= Time.deltaTime;

            if (dasDownDelayTimer <= 0.0f) {
                dasDownTimer -= Time.deltaTime;

                if (dasDownTimer <= 0.0f) {
                    if (IsPositionValid(activeRow + 1, activeColumn, activeRotation)) {
                        activeRow++;
                    } else {
                        PlaceActive();
                    }
                    dropTimer = DropSpeed;
                    dasDownTimer = DasSpeed;
                }
            }
        } else {
            dasDownDelayTimer = DasDelaySpeed;
            dasDownTimer = DasSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            while (IsPositionValid(activeRow + 1, activeColumn, activeRotation)) {
                activeRow++;
            }

            dropTimer = DropSpeed;

            PlaceActive();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && IsPositionValid(activeRow, activeColumn - 1, activeRotation)) { activeColumn--; }
        if (Input.GetKeyDown(KeyCode.RightArrow) && IsPositionValid(activeRow, activeColumn + 1, activeRotation)) { activeColumn++; }

        if (GetDirection() == 0) {
            dasDelayTimer = DasDelaySpeed;
            dasTimer = DasSpeed;
        } else {
            dasDelayTimer -= Time.deltaTime;
        }

        if (dasDelayTimer <= 0.0f) {
            if (dasTimer <= 0.0f) {
                if (IsPositionValid(activeRow, activeColumn + GetDirection(), activeRotation)) { activeColumn += GetDirection(); }
                dasTimer = DasSpeed;
            } else {
                dasTimer -= Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && IsPositionValid(activeRow, activeColumn, GetNextRotation())) { activeRotation = GetNextRotation(); }
    }

    private bool IsCellActive(int row, int column) {
        int boundStartRow = activeRow;
        int boundStartColumn = activeColumn;
        int boundEndRow = activeRow + activeSize - 1;
        int boundEndColumn = activeColumn + activeSize - 1;

        if (row < boundStartRow || column < boundStartColumn || row > boundEndRow || column > boundEndColumn) { return false; }

        int pieceRow = row - activeRow;
        int pieceColumn = column - activeColumn;

        return active[activeRotation][pieceRow, pieceColumn] == 1 ? true : false;
    }

    private bool IsCellFree(int row, int column) {
        return grid[row, column] == 1 ? false : true;
    }

    private bool IsCellInGrid(int row, int column) {
        return (row >= 0 && row < GridRows + GridExtraRows && column >= 0 && column < GridColumns) ? true : false;
    }

    private bool IsPositionInGrid(int row, int column, int rotation) {
        int boundStartColumn = column;
        int boundEndRow = row + activeSize - 1;
        int boundEndColumn = column + activeSize - 1;

        if (boundStartColumn >= 0 && boundEndRow < GridRows + GridExtraRows && boundEndColumn < GridColumns) { return true; }

        for (int pieceRow = 0; pieceRow < activeSize; pieceRow++) {
            for (int pieceColumn = 0; pieceColumn < activeSize; pieceColumn++) {
                if (active[rotation][pieceRow, pieceColumn] == 1) {
                    if (!IsCellInGrid(row + pieceRow, column + pieceColumn)) { return false; }
                }
            }
        }

        return true;
    }

    private bool IsPositionFree(int row, int column, int rotation) {
        for (int pieceRow = 0; pieceRow < activeSize; pieceRow++) {
            for (int pieceColumn = 0; pieceColumn < activeSize; pieceColumn++) {
                if (active[rotation][pieceRow, pieceColumn] == 1) {
                    if (!IsCellFree(row + pieceRow, column + pieceColumn)) { return false; }
                }
            }
        }

        return true;
    }

    private bool IsPositionValid(int row, int column, int rotation) {
        return IsPositionInGrid(row, column, rotation) && IsPositionFree(row, column, rotation);
    }

    private void UpdateGridDisplay() {
        string displayText = "<mspace=7>";

        for (int row = GridExtraRows; row < GridRows + GridExtraRows; row++) {
            for (int column = 0; column < GridColumns; column++) {
                if (IsCellActive(row, column)) {
                    displayText += "O ";
                } else {
                    displayText += grid[row, column] == 1 ? "# " : ". ";
                }
            }

            displayText += "\n";
        }

        gridDisplay.text = displayText;
    }
}
