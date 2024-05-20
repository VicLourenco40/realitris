using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent gameStarted;
    public UnityEvent gameEnded;
    public UnityEvent pieceCreated;
    public UnityEvent pieceHeld;
    public UnityEvent levelUpdated;
    public UnityEvent scoreUpdated;
    public UnityEvent linesUpdated;

    public int GridRows = 20;
    public int GridExtraRows = 5;
    public int GridColumns = 10;

    private const int RowsPerLevel = 10;
    private readonly int[] ScoreRows = { 100, 300, 500, 800 };
    private const int ScoreSoftDrop = 1;
    private const int ScoreHardDrop = 2;

    private const float DropSpeed = 1.0f;
    private const float PlaceSpeed = 1.0f;
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
    private static int[][,] OPiece = {
        new int[,] {
            { 1, 1 },
            { 1, 1 }
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
    public int[][][,] Pieces = {
        IPiece, JPiece, LPiece, OPiece, SPiece, TPiece, ZPiece
    };

    public int[,] grid;

    public int active;
    public int activeSize;
    private int activeRotation;
    private int activeRow;
    private int activeColumn;

    public int pieceIndex;
    public int[] pieceSequence;
    public int holdPiece;
    private static bool holdUsed;
    private static int lastDirection;

    public int score;
    public int linesCleared;
    public int level;
    public bool gameOver;

    private static float dropTimer = DropSpeed;
    private static float placeTimer = PlaceSpeed;
    private static float dasDelayTimer = DasDelaySpeed;
    private static float dasTimer = DasSpeed;
    private static float dasDownDelayTimer = DasDelaySpeed;
    private static float dasDownTimer = DasSpeed;

    void Start() {
        grid = new int[GridRows + GridExtraRows, GridColumns];
        pieceSequence = new int[Pieces.Length * 2];

        gameStarted.AddListener(GameObject.Find("Display Manager").GetComponent<DisplayManager>().GameStarted);
        gameEnded.AddListener(GameObject.Find("Display Manager").GetComponent<DisplayManager>().GameEnded);
        pieceCreated.AddListener(GameObject.Find("Display Manager").GetComponent<DisplayManager>().UpdateNextGrid);
        pieceHeld.AddListener(GameObject.Find("Display Manager").GetComponent<DisplayManager>().UpdateHoldGrid);
        levelUpdated.AddListener(GameObject.Find("Display Manager").GetComponent<DisplayManager>().UpdateLevel);
        scoreUpdated.AddListener(GameObject.Find("Display Manager").GetComponent<DisplayManager>().UpdateScore);
        linesUpdated.AddListener(GameObject.Find("Display Manager").GetComponent<DisplayManager>().UpdateLines);

        RestartGame();
    }

    void Update() {
        if (gameOver) {
            if (Input.GetKeyDown(KeyCode.R)) {
                RestartGame();
            }
        } else {
            UpdateActive();
        }
    }

    private void RestartGame() {
        score = 0;
        linesCleared = 0;
        level = 1;
        gameOver = false;
        holdPiece = -1;
        holdUsed = false;
        pieceIndex = -1;

        GeneratePieceSequence();
        GeneratePieceSequence();
        ClearGrid();
        CreateActive();

        gameStarted.Invoke();
    }

    private float GetDropSpeed() {
        return Mathf.Max(0.1f, DropSpeed - (level * 0.1f));
    }

    private void ClearGrid() {
        for (int row = 0; row < GridRows + GridExtraRows; row++) {
            for (int column = 0; column < GridColumns; column++) {
                grid[row, column] = -1;
            }
        }
    }

    private void ClearRows() {
        int rowsCleared = 0;

        for (int row = activeRow; row < Mathf.Min(activeRow + activeSize, GridRows + GridExtraRows); row++) {
            bool clearRow = true;

            for (int column = 0; column < GridColumns; column++) {
                if (grid[row, column] == -1) {
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
                    grid[0, c] = -1;
                }

                rowsCleared++;
            }
        }

        if (rowsCleared > 0) {
            linesCleared += rowsCleared;
            score += ScoreRows[rowsCleared - 1] * level;

            if (level < 1 + (linesCleared / RowsPerLevel)) {
                level++;
                levelUpdated.Invoke();
            }
            scoreUpdated.Invoke();
            linesUpdated.Invoke();
        }
    }

    private void CheckLockOut() {
        for (int column = 0; column < GridColumns; column++) {
            if (IsCellActive(GridExtraRows, column)) { return; }
        }
        gameOver = true;
        gameEnded.Invoke();
    }

    private void GeneratePieceSequence() {
        int[] newSequence = new int[Pieces.Length];

        for (int i = 0; i < newSequence.Length; i++) {
            int num;

            newSequence[i] = -1;

            do {
                num = UnityEngine.Random.Range(0, Pieces.Length);
            } while (Array.IndexOf(newSequence, num) > -1);

            newSequence[i] = num;

            pieceSequence[i] = pieceSequence[i + Pieces.Length];
            pieceSequence[i + Pieces.Length] = newSequence[i];
        }
    }

    private int GetNextPiece() {
        if (pieceIndex < Pieces.Length - 1) {
            pieceIndex++;
        } else {
            pieceIndex = 0;
            GeneratePieceSequence();
        }

        return pieceSequence[pieceIndex];
    }

    private void CreateActive(int piece = -1) {
        if (piece == -1) {
            active = GetNextPiece();
            pieceCreated.Invoke();
        }
        else {
            active = piece;
        }

        activeRow = GridExtraRows;
        if (active == 0) { activeRow--; }

        activeRotation = 0;
        activeSize = Pieces[active][0].GetLength(0);
        activeColumn = (GridColumns - activeSize) / 2;

        while(activeRow >= 0 && !IsPositionValid(activeRow, activeColumn, activeRotation)) {
            activeRow--;
        }
    }

    private void PlaceActive() {
        for (int pieceRow = 0; pieceRow < activeSize; pieceRow++) {
            for (int pieceColumn = 0; pieceColumn < activeSize; pieceColumn++) {
                if (Pieces[active][activeRotation][pieceRow, pieceColumn] == 1) {
                    grid[activeRow + pieceRow, activeColumn + pieceColumn] = active;
                }
            }
        }

        holdUsed = false;

        ClearRows();
        if (activeRow < GridExtraRows) { CheckLockOut(); }
        if (!gameOver) { CreateActive(); }
    }

    private int GetNextRotation() {
        int lastRotation = Pieces[active].Length - 1;

        return activeRotation == lastRotation ? 0 : activeRotation + 1;
    }

    private int GetDirection() {
        int direction = 0;

        if (Input.GetKey(KeyCode.LeftArrow)) { direction--; }
        if (Input.GetKey(KeyCode.RightArrow)) { direction++; }

        return direction;
    }

    private void UpdateActive() {
        bool canMoveDown = IsPositionValid(activeRow + 1, activeColumn, activeRotation);
        bool dropScored = false;
        int direction = GetDirection();

        dropTimer -= Time.deltaTime;

        if (canMoveDown) {
            placeTimer = PlaceSpeed;
        } else {
            placeTimer -= Time.deltaTime;

            if (placeTimer <= 0.0f) {
                placeTimer = PlaceSpeed;
                PlaceActive();
            }
        }

        if (!holdUsed && Input.GetKeyDown(KeyCode.C)) {
            int lastActive = active;

            if (holdPiece == -1) {
                CreateActive();
            } else {
                CreateActive(holdPiece);
            }

            holdPiece = lastActive;
            holdUsed = true;

            pieceHeld.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            while (canMoveDown) {
                activeRow++;
                canMoveDown = IsPositionValid(activeRow + 1, activeColumn, activeRotation);
                if (!dropScored) {
                    score += ScoreHardDrop;
                    scoreUpdated.Invoke();
                }
            }

            dropTimer = GetDropSpeed();
            dropScored = true;

            PlaceActive();
        }

        if (dropTimer <= 0.0f) {
            if (canMoveDown) { activeRow++; }
            dropTimer = GetDropSpeed();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (canMoveDown) {
                activeRow++;

                if (!dropScored) {
                    score += ScoreSoftDrop;
                    dropScored = true;
                    scoreUpdated.Invoke();
                }
            }

            dropTimer = GetDropSpeed();
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            dasDownDelayTimer -= Time.deltaTime;

            if (dasDownDelayTimer <= 0.0f) {
                dasDownTimer -= Time.deltaTime;

                if (dasDownTimer <= 0.0f) {
                    if (canMoveDown) {
                        activeRow++;

                        if (!dropScored) {
                            score += ScoreSoftDrop;
                            dropScored = true;
                            scoreUpdated.Invoke();
                        }
                    }

                    dropTimer = GetDropSpeed();
                    dasDownTimer = DasSpeed;
                }
            }
        } else {
            dasDownDelayTimer = DasDelaySpeed;
            dasDownTimer = DasSpeed;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && IsPositionValid(activeRow, activeColumn - 1, activeRotation)) { activeColumn--; }
        if (Input.GetKeyDown(KeyCode.RightArrow) && IsPositionValid(activeRow, activeColumn + 1, activeRotation)) { activeColumn++; }

        if (direction == 0 || direction != lastDirection) {
            dasDelayTimer = DasDelaySpeed;
            dasTimer = DasSpeed;
        } else {
            dasDelayTimer -= Time.deltaTime;
        }

        lastDirection = direction;

        if (dasDelayTimer <= 0.0f) {
            if (dasTimer <= 0.0f) {
                if (IsPositionValid(activeRow, activeColumn + GetDirection(), activeRotation)) { activeColumn += GetDirection(); }
                dasTimer = DasSpeed;
            } else {
                dasTimer -= Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && IsPositionValid(activeRow, activeColumn, GetNextRotation())) {
            activeRotation = GetNextRotation();
        }
    }

    private int GetGhostRow() {
        int ghostRow = activeRow;

        while (IsPositionValid(ghostRow + 1, activeColumn, activeRotation)) {
            ghostRow++;
        }

        return ghostRow;
    }

    public bool IsCellActive(int row, int column, bool ghost = false) {
        int checkRow = ghost ? GetGhostRow() : activeRow;
        int boundStartRow = checkRow;
        int boundStartColumn = activeColumn;
        int boundEndRow = checkRow + activeSize - 1;
        int boundEndColumn = activeColumn + activeSize - 1;

        if (row < boundStartRow || column < boundStartColumn || row > boundEndRow || column > boundEndColumn) { return false; }

        int pieceRow = row - checkRow;
        int pieceColumn = column - activeColumn;

        return Pieces[active][activeRotation][pieceRow, pieceColumn] == 1 ? true : false;
    }

    private bool IsCellFree(int row, int column) {
        return grid[row, column] == -1 ? true : false;
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
                if (Pieces[active][rotation][pieceRow, pieceColumn] == 1) {
                    if (!IsCellInGrid(row + pieceRow, column + pieceColumn)) { return false; }
                }
            }
        }

        return true;
    }

    private bool IsPositionFree(int row, int column, int rotation) {
        for (int pieceRow = 0; pieceRow < activeSize; pieceRow++) {
            for (int pieceColumn = 0; pieceColumn < activeSize; pieceColumn++) {
                if (Pieces[active][rotation][pieceRow, pieceColumn] == 1) {
                    if (!IsCellFree(row + pieceRow, column + pieceColumn)) { return false; }
                }
            }
        }

        return true;
    }

    private bool IsPositionValid(int row, int column, int rotation) {
        return IsPositionInGrid(row, column, rotation) && IsPositionFree(row, column, rotation);
    }
}
