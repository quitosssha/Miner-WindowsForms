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
        public static void InitCommands(this GameModel game, Button button, int row, int column)
        {
            button.Click += (sender, args) => game.OpenCell(row, column);
        }

        public static void InitStateChanged(
            this GameModel game,
            Form1 form,
            TableLayoutPanel table, 
            Dictionary<CellType, Color> Colors)
        {
            game.StateChanged += (row, column, state) =>
            {
                var button = (Button)table.GetControlFromPosition(column, row);
                button.BackColor = Colors[state];

                if (state == CellType.Empty)
                {
                    int minesAround = game.CountMinesAround(row, column);
                    if (minesAround > 0)
                        button.Text = minesAround.ToString();
                    else
                        game.OpenCellsAround(row, column);
                }

                if (state == CellType.Mine)
                {
                    game.GameOver(form);
                }
            };
        }
    }
}
