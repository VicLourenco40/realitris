using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

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

    private static int[,] grid = new int[GridRows + GridExtraRows, GridColumns];

    private static int activeRow = 4;
    private static int activeColumn = 0;
    private static int activeRotation = 0;
    private static int[][,] active = TPiece;
    private static int activeSize = active[activeRotation].GetLength(0);

    void Start() { }

    void Update() {
        UpdateDisplay();
    }

    private bool IsCellActive(int row, int column) {
        int boundStartRow = activeRow;
        int boundStartColumn = activeColumn;
        int boundEndRow = activeRow + activeSize - 1;
        int boundEndColumn = activeColumn + activeSize - 1;

        if (row < boundStartRow || column < boundStartColumn || row > boundEndRow || column > boundEndColumn) { return false; }

        int shapeRow = row - activeRow;
        int shapeColumn = column - activeColumn;

        return active[activeRotation][shapeRow, shapeColumn] == 1 ? true : false;
    }

    private bool IsCellInGrid(int row, int column) {
        return (row >= 0 && row < GridRows + GridExtraRows && column >= 0 && column < GridColumns) ? true : false;
    }

    private bool IsPositionInGrid(int row, int column, int rotation) {
        int boundStartColumn = column;
        int boundEndRow = row + activeSize - 1;
        int boundEndColumn = column + activeSize - 1;

        if (boundStartColumn >= 0 && boundEndRow < GridRows + GridExtraRows && boundEndColumn < GridColumns) { return true; }

        for (int shapeRow = 0; shapeRow < activeSize; shapeRow++) {
            for (int shapeColumn = 0; shapeColumn < activeSize; shapeColumn++) {
                if (active[rotation][shapeRow, shapeColumn] == 1) {
                    if (!IsCellInGrid(row + shapeRow, column + shapeColumn)) { return false; }
                }
            }
        }

        return true;
    }

    private bool IsPositionValid(int row, int column, int rotation) {
        return IsPositionInGrid(row, column, rotation);
    }

    private void UpdateDisplay() {
        string displayText = "";

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
