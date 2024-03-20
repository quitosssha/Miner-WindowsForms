using Miner.Controllers;
using Miner.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Miner
{
    public partial class Form1
    {
        GameModel _game;
        TableLayoutPanel _table;

        void StartNewGame()
        {
            _game = new GameModel(this, 10, 10);
            _table = new TableLayoutPanel() { Dock = DockStyle.Fill };
            for (int column = 0; column < _game.Width; column++)
                _table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, Height / _game.Height));
            for (int row = 0; row < _game.Height; row++)
                _table.RowStyles.Add(new RowStyle(SizeType.Percent, Width / _game.Width));

            for (int row = 0; row < _game.Height; row++)
                for (int column = 0; column < _game.Width; column++)
                {
                    var button = new DoubleClickableButton() { Dock = DockStyle.Fill };
                    _table.Controls.Add(button, column, row);
                    _game.InitCommands(button, row, column);
                }
            Controls.Add(_table);

            _game.InitStateChanged(_table, ColorsByCellType);

            _game.Start();
        }
    }
}
