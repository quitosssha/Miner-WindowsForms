﻿using Miner.Model;
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
                    game.OpenCell(row, column);

                if (args.Button == MouseButtons.Right)
                    game.MarkCell(row, column);
            };

            button.MouseDown += (sender, args) =>
            {
                if (button.DoubleClicked || button.ClickedWithBothMouseButtons)
                {
                    if (game.GameField[row, column] == CellState.Empty)
                    {
                        if (game.CountCellsAround(row, column, CellState.Marked, searchInHidden: false)
                            == game.CountCellsAround(row, column, CellState.Mine))
                            game.OpenCellsAround(row, column);

                        else if (game.CountCellsAround(row, column, CellState.Unknown, CellState.Marked, searchInHidden: false)
                            == game.CountCellsAround(row, column, CellState.Mine))
                            game.MarkCellsAround(row, column);
                    }
                }
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
                if (state == CellState.Mine && !game.GameIsOver)
                    game.GameOver();
            };
        }


        public static void InitHead(this GameModel game, TableLayoutPanel head, Timer timer)
        {
            var leftMinesLabel = (Label)head.GetControlFromPosition(0, 0);
            game.MinesAmountChanged += (amount) =>
            {
                if (amount < 0)
                    leftMinesLabel.ForeColor = Color.Red;
                else
                    leftMinesLabel.ForeColor = Color.Black;
                leftMinesLabel.Text = $"Mines left: {amount}";
            };

            var timeElapsedLabel = (Label)head.GetControlFromPosition(1, 0);
            timer.Interval = 100;
            timer.Tick += (sender, args) =>
            {
                timeElapsedLabel.Text = $"{game.TimeElapsed}";
            };
            timer.Start();
        }
    }
}
