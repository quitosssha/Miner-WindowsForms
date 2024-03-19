using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Miner
{
    public partial class Form1 : Form
    {
        GameModel game;
        TableLayoutPanel table;

        public Form1(GameModel game) 
        {
            this.game = game;
            table = new TableLayoutPanel() { Dock = DockStyle.Fill };
            for (int column = 0; column < game.Width; column++)
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30));
            for (int row = 0; row < game.Height; row++)
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            for (int iRow = 0; iRow < game.Height; iRow++)
                for (int iColumn = 0; iColumn < game.Width; iColumn++)
                {
                    var column = iColumn;
                    var row = iRow;
                    var button = new Button() { Dock = DockStyle.Fill };
                    table.Controls.Add(button, column, row);
                    button.Click += (sender, args) => game.OpenCell(row, column);
                }
            Controls.Add(table);

            game.StateChanged += (row, column, state) =>
                ((Button)table.GetControlFromPosition(column, row)).BackColor = Colors[state];

            game.Start();
        }

        private static readonly Dictionary<CellType, Color> Colors = new Dictionary<CellType, Color>
        {
            { CellType.Mine, Color.Black },
            { CellType.Empty, Color.White },
            { CellType.Unknown, Color.FromArgb(200, 200, 200) },
            { CellType.Marked, Color.Red }
        };
    }
}
