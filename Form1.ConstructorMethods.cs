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
        TableLayoutPanel _head;
        Timer _timer = new Timer();

        void StartNewGame()
        {
            _game = new GameModel(this, _gameWidth, _gameHeight, _amountOfMines);

            _table = new TableLayoutPanel() { Dock = DockStyle.Fill };
            for (int column = 0; column < _gameWidth; column++)
                _table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, Height / _gameHeight));
            for (int row = 0; row < _gameHeight; row++)
                _table.RowStyles.Add(new RowStyle(SizeType.Percent, Width / _gameWidth));

            for (int row = 0; row < _game.Height; row++)
                for (int column = 0; column < _game.Width; column++)
                {
                    var button = new DoubleClickableButton() { Dock = DockStyle.Fill, Margin = new Padding(1) };
                    _table.Controls.Add(button, column, row);
                    _game.InitCommands(button, row, column);
                }
            Controls.Add(_table);

            BuildHead();

            _game.InitHead(_head, _timer);
            _game.InitStateChanged(_table, ColorsByCellState);

            _game.Start();
        }

        void BuildHead()
        {
            _head = new TableLayoutPanel() { Dock = DockStyle.Top, Height = 20 };
            _head.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            Label leftMinesLabel = new Label()
            {
                AutoSize = true,
                Font = new Font("Arial", 11f)
            };
            _head.Controls.Add(leftMinesLabel, 0, 0);

            Label timeElapsedLabel = new Label()
            {
                AutoSize = true,
                Font = new Font("Arial", 11f)
            };
            _head.Controls.Add(timeElapsedLabel, 1, 0);

            Controls.Add(_head);
        }
    }
}
