using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    public class GameModel
    {
        private CellType[,] gameField;
        public readonly CellType[,] hiddenField;
        public readonly int Width;
        public readonly int Height;

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
                    var state = rnd.Next(3) == 0 ? CellType.Mine : CellType.Empty;
                    hiddenField[row, column] = state;
                    SetState(row, column, CellType.Unknown);
                }
        }

        public void OpenCell(int row, int column)
        {
            SetState(row, column, hiddenField[row, column]);
        }

        void SetState(int row, int column, CellType state)
        {
            gameField[row, column] = state;
            StateChanged?.Invoke(row, column, state);
        }
        
        public event Action<int, int, CellType> StateChanged;
    }
}
