using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI gridDisplay;

    private const int GridRows = 20;
    private const int GridExtraRows = 4;
    private const int GridColumns = 10;

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

    private static float dropTime = 1.0f;

    void Start() {
        CreateActive();
    }

    void Update() {
        Movement();
        UpdateDisplay();
    }

    private void ClearRows() {
        for (int row = activeRow; row < Math.Min(activeRow + activeSize, GridRows + GridExtraRows); row++) {
            bool clearRow = true;

            for (int column = 0; column < GridColumns; column++) {
                if (grid[row, column] == 0) {
                    clearRow = false;
                }
            }

            if (clearRow) {
                for (int r = row - 1; r >= GridExtraRows; r--) {
                    for (int c = 0; c < GridColumns; c++) {
                        grid[r + 1, c] = grid[r, c];
                    }
                }

                for (int c = 0; c < GridColumns; c++) {
                    grid[GridExtraRows, c] = 0;
                }
            }
        }
    }

    private void CreateActive() {
        active = Pieces[UnityEngine.Random.Range(0, Pieces.Length)];
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
        CreateActive();
    }

    private int GetNextRotation() {
        int lastRotation = active.Length - 1;

        return activeRotation == lastRotation ? 0 : activeRotation + 1;
    }

    private void Movement() {
        dropTime -= Time.deltaTime;

        if (dropTime <= 0.0f || Input.GetKeyDown(KeyCode.DownArrow)) {
            if (IsPositionValid(activeRow + 1, activeColumn, activeRotation)) {
                activeRow++;
            } else {
                PlaceActive();
            }

            dropTime = 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            while (IsPositionValid(activeRow + 1, activeColumn, activeRotation)) {
                activeRow++;
            }

            dropTime = 1.0f;

            PlaceActive();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && IsPositionValid(activeRow, activeColumn, GetNextRotation())) { activeRotation = GetNextRotation(); }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && IsPositionValid(activeRow, activeColumn - 1, activeRotation)) { activeColumn--; }
        if (Input.GetKeyDown(KeyCode.RightArrow) && IsPositionValid(activeRow, activeColumn + 1, activeRotation)) { activeColumn++; }
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

    private void UpdateDisplay() {
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
