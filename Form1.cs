﻿using Miner.Controllers;
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
        readonly int _gameWidth;
        readonly int _gameHeight;
        readonly int _amountOfMines;

        public Form1(Level level)
        {
            _gameWidth = level.Width;
            _gameHeight = level.Height;
            _amountOfMines = level.Mines;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            ClientSize = new Size(_gameWidth * 30, _gameHeight * 30 + 20);
            StartNewGame();
        }

        public void FinishGameWithMessage(string message, string caption = "")
        {
            _timer.Stop();
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

        private static readonly Dictionary<CellState, Color> ColorsByCellState = new Dictionary<CellState, Color>
        {
            { CellState.Mine, Color.Black },
            { CellState.Empty, Color.White },
            { CellState.Unknown, Color.FromArgb(200, 200, 200) },
            { CellState.Marked, Color.Red }
        };
    }
}
