using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace Miner
{
    public class GameModel
    {
        private CellState[,] hiddenField;
        public CellState[,] GameField { get; private set; }
        public int Width { get; }
        public int Height { get; }
        public bool IsOver { get; private set; }
        private int emptyFieldsCount;
        public event Action<int, int, CellState> StateChanged;
        private Form1 form;

        public GameModel(Form1 form, int width, int height)
        {
            this.form = form;
            Width = width; 
            Height = height;
            IsOver = false;
            hiddenField = new CellState[width, height];
            GameField = new CellState[width, height];
        }

        void SetState(int row, int column, CellState state)
        {
            GameField[row, column] = state;
            StateChanged?.Invoke(row, column, state);
        }

        public void Start()
        {
            Random rnd = new Random();
            for (int row = 0; row < Height; row++)
                for (int column = 0; column < Width; column++)
                {
                    if (rnd.Next(10) == 0)
                        hiddenField[row, column] = CellState.Mine;
                    else
                    {
                        hiddenField[row, column] = CellState.Empty;
                        emptyFieldsCount++;
                    }
                    SetState(row, column, CellState.Unknown);
                }
        }

        public void OpenCell(int row, int column)
        {
            if (GameField[row, column] == CellState.Unknown)
            {
                SetState(row, column, hiddenField[row, column]);
                emptyFieldsCount--;
            }
            if (emptyFieldsCount == 0 && !IsOver)
                GameWon();
        }

        public void MarkCell(int row, int column)
        {
            if (GameField[row, column] == CellState.Unknown)
                SetState(row, column, CellState.Marked);
            else if (GameField[row, column] == CellState.Marked)
                SetState(row, column, CellState.Unknown);
        }

        public int CountCellsAround(int row, int column, CellState state1, CellState state2 = CellState.Null, bool searchInHidden = true)
        {
            var field = searchInHidden ? hiddenField : GameField;
            int counter = 0;
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    
                    if (!OutOfBounds(row + i, column + j))
                        if (field[row + i, column + j] == state1 || field[row + i, column + j] == state2)
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

        public void MarkCellsAround(int row, int column)
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    if (!OutOfBounds(row + i, column + j) && GameField[row + i, column + j] == CellState.Unknown)
                        MarkCell(row + i, column + j);
                }
        }

        bool OutOfBounds(int row, int column) =>
            row < 0 || row >= Height
            || column < 0 || column >= Width;
        
        public void GameOver()
        {
            IsOver = true;
            for (int row = 0; row < Height; row++)
                for (int column = 0; column < Width; column++)
                {
                    if (GameField[row, column] == CellState.Unknown
                        && hiddenField[row, column] == CellState.Mine)
                        OpenCell(row, column);
                }
            form.FinishGameWithMessage("Вы проиграли... Начать сначала?", "Поражение...");
        }

        private void GameWon() =>
            form.FinishGameWithMessage("Вы победили! Начать сначала?", "Победа!");
    }
}
