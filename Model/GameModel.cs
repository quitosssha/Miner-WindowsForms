using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly CellState[,] hiddenField;
        public CellState[,] GameField { get; private set; }
        public int Width { get; }
        public int Height { get; }
        public bool GameIsOver { get; private set; } = false;
        public string TimeElapsed { get => watch.Elapsed.ToString(@"mm\:ss\:ff"); }

        public event Action<int, int, CellState> StateChanged;
        public event Action<int> MinesAmountChanged;

        private readonly Stopwatch watch = new Stopwatch();
        private int emptyFieldsCount;
        private readonly int minesCount;
        private int markedFieldsCount = 0;
        private readonly Form1 form;

        public GameModel(Form1 form, int width, int height, int minesAmount)
        {
            this.form = form;
            Width = width; 
            Height = height;
            minesCount = minesAmount;
            emptyFieldsCount = width * height - minesAmount;
            hiddenField = new CellState[height, width];
            GameField = new CellState[height, width];
        }

        void SetState(int row, int column, CellState state)
        {
            GameField[row, column] = state;
            StateChanged?.Invoke(row, column, state);
        }

        public void Start()
        {
            Random rnd = new Random();
            int minesPlaced = 0;
            while (minesPlaced < minesCount)
            {
                int column = rnd.Next(Width);
                int row = rnd.Next(Height);
                if (hiddenField[row, column] != CellState.Mine)
                {
                    hiddenField[row, column] = CellState.Mine;
                    minesPlaced++;
                }
            }
            MinesAmountChanged?.Invoke(minesCount);

            for (int row = 0; row < Height; row++)
            for (int column = 0; column < Width; column++)
            {
                SetState(row, column, CellState.Unknown);
                if (hiddenField[row, column] != CellState.Mine)
                    hiddenField[row, column] = CellState.Empty;
            }

            watch.Start();
        }

        public void OpenCell(int row, int column)
        {
            if (GameField[row, column] == CellState.Unknown)
            {
                SetState(row, column, hiddenField[row, column]);
                emptyFieldsCount--;
            }

            if (emptyFieldsCount == 0 && !GameIsOver)
                GameWon();

            if (GameIsOver
                && hiddenField[row, column] == CellState.Mine
                && GameField[row, column] == CellState.Marked)
                SetState(row, column, CellState.Mine);
        }

        public void MarkCell(int row, int column)
        {
            if (GameField[row, column] == CellState.Unknown)
            {
                SetState(row, column, CellState.Marked);
                markedFieldsCount++;
            }
            else if (GameField[row, column] == CellState.Marked)
            {
                SetState(row, column, CellState.Unknown);
                markedFieldsCount--;
            }
            MinesAmountChanged?.Invoke(minesCount - markedFieldsCount);
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
            watch.Stop();
            GameIsOver = true;
            for (int row = 0; row < Height; row++)
                for (int column = 0; column < Width; column++)
                {
                    if (GameField[row, column] == CellState.Unknown || GameField[row, column] == CellState.Marked
                        && hiddenField[row, column] == CellState.Mine)
                        OpenCell(row, column);
                }
            form.FinishGameWithMessage("Вы проиграли... Начать сначала?", "Поражение...");
        }

        private void GameWon()
        {
            watch.Stop();
            var t = watch.ElapsedMilliseconds;
            form.FinishGameWithMessage
                ($"Вы победили за {t/1000},{t%1000} сек!\nНачать сначала?", "Победа!");
        }
    }
}
