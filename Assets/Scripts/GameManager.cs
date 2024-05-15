using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI gridDisplay;

    private const int GridRows = 20;
    private const int GridExtraRows = 4;
    private const int GridColumns = 10;

    private bool[,] grid = new bool[GridRows + GridExtraRows, GridColumns];

    void Start() { }

    void Update() {
        updateDisplay();
    }

    void updateDisplay() {
        string displayText = "";

        for (int row = GridExtraRows; row < GridRows + GridExtraRows; row++) {
            for (int column = 0; column < GridColumns; column++) {
                grid[row, column] = true;
                displayText += grid[row, column] ? "# " : "  ";
            }

            displayText += "\n";
        }

        gridDisplay.text = displayText;
    }
}
