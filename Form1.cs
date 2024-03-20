using Miner.Controllers;
using Miner.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public Form1()
        {
            InitializeComponent();
            StartNewGame();
        }

        private void StartNewGame()
        {
            game = new GameModel(this, 10, 10);
            table = new TableLayoutPanel() { Dock = DockStyle.Fill };
            for (int column = 0; column < game.Width; column++)
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30));
            for (int row = 0; row < game.Height; row++)
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            for (int row = 0; row < game.Height; row++)
                for (int column = 0; column < game.Width; column++)
                {
                    var button = new DoubleClickableButton() { Dock = DockStyle.Fill };
                    table.Controls.Add(button, column, row);
                    game.InitCommands(button, row, column);
                }
            Controls.Add(table);

            game.InitStateChanged(table, Colors);

            game.Start();
        }

        public void FinishGameWithMessage(string message, string caption = "")
        {
            var result = MessageBox.Show(message, caption,
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.No)
                Close();
            else
            {
                Controls.Clear();
                StartNewGame();
            }
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
