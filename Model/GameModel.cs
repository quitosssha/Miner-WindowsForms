using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Miner
{
    public class GameModel
    {
        public CellType[,] gameField { get; private set; }
        private CellType[,] hiddenField { get; }
        public int Width { get; }
        public int Height { get; }

        public event Action<int, int, CellType> StateChanged;

        public GameModel(int width, int height)
        {
            Width = width; 
            Height = height;
            hiddenField = new CellType[width, height];
            gameField = new CellType[width, height];
        }

        void SetState(int row, int column, CellType state)
        {
            gameField[row, column] = state;
            StateChanged?.Invoke(row, column, state);
        }

        public void Start()
        {
            Random rnd = new Random();
            for (int row = 0; row < Height; row++)
                for (int column = 0; column < Width; column++)
                {
                    var state = rnd.Next(15) == 0 ? CellType.Mine : CellType.Empty;
                    hiddenField[row, column] = state;
                    SetState(row, column, CellType.Unknown);
                }
        }

        public void OpenCell(int row, int column)
        {
            if (gameField[row, column] == CellType.Unknown)
                SetState(row, column, hiddenField[row, column]);
        }

        public int CountMinesAround(int row, int column)
        {
            int counter = 0;
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    
                    if (!OutOfBounds(row + i, column + j))
                        if (hiddenField[row + i, column + j] == CellType.Mine)
                            counter++;
                }

            return counter;
        }

        public void OpenCellsAround(int row, int column)
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    if (!OutOfBounds(row + i, column + j))
                        OpenCell(row + i, column + j);
                }
        }

        bool OutOfBounds(int row, int column) =>
            row < 0 || row >= Height
            || column < 0 || column >= Width;
        
        public void GameOver(Form1 form)
        {
            StateChanged = null;
            for (int row = 0; row < Height; row++)
                for (int column = 0; column < Width; column++)
                {
                    if (gameField[row, column] == CellType.Unknown)
                        OpenCell(row, column);
                }
            form.ShowGameOverMessage();
        }
    }
}
