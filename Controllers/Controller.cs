using Miner.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Miner.Controllers
{
    public static class Controller
    {
        public static void InitCommands(this GameModel game, DoubleClickableButton button, int row, int column)
        {
            button.MouseDown += (sender, args) =>
            {
                if (args.Button == MouseButtons.Left)
                    if (button.DoubleClicked)
                    {
                        if (game.GameField[row, column] == CellState.Empty)
                            if (game.CountCellsAround(row, column, CellState.Marked, searchInHidden: false)
                                == game.CountCellsAround(row, column, CellState.Mine))
                                game.OpenCellsAround(row, column);
                    }
                    else
                        game.OpenCell(row, column);

                if (args.Button == MouseButtons.Right)
                    game.MarkCell(row, column);
            };
        }

        public static void InitStateChanged(
            this GameModel game,
            TableLayoutPanel table,
            Dictionary<CellState, Color> Colors)
        {
            game.StateChanged += (row, column, state) =>
            {
                var button = (Button)table.GetControlFromPosition(column, row);
                button.BackColor = Colors[state];

                if (state == CellState.Empty)
                {
                    int minesAround = game.CountCellsAround(row, column, CellState.Mine);
                    if (minesAround > 0)
                    {
                        button.Text = minesAround.ToString();
                        button.Font = new Font("Arial", 12f ,FontStyle.Bold);

                    }
                    else
                        game.OpenCellsAround(row, column);
                }
            };

            game.StateChanged += (row, column, state) =>
            {
                if (state == CellState.Mine && !game.IsOver)
                    game.GameOver();
            };
        }
    }
}
