using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    public class GameModel
    {
        private CellType[,] gameField;
        private readonly CellType[,] hiddenField;
        public readonly int Width;
        public readonly int Height;
        public event Action<int, int, CellType> StateChanged;

        public GameModel(int width, int height)
        {
            Width = width; 
            Height = height;
            hiddenField = new CellType[width, height];
            gameField = new CellType[width, height];
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

        void SetState(int row, int column, CellType state)
        {
            gameField[row, column] = state;
            StateChanged?.Invoke(row, column, state);
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

        private bool OutOfBounds(int row, int column) =>
            row < 0 || row >= Height
            || column < 0 || column >= Width;
        
    }
}
