﻿using Checkers.Properties;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers {
    public partial class Display : Form {
        public Panel selected;
        public Board board;
        public Display(Board board) {
            this.board = board;
            InitializeComponent();
            foreach (Control c in BoardLayout.Controls)
                if (c is Panel) {
                    c.Click += HandleCellClick;
                    c.Controls.Find("pic", true)[0].Click += HandleCellClick;
                }
        }

        private async void Form1_Load(object sender, EventArgs e) {
            await UpdateBoard();
        }

        //private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) {
        //    UpdateBoard();
        //}
        private async void UpdateTurnLabel() {
            if (board.playersTurn.color == Color.Black) {
                turn.Text = "Black's turn";
            } else {
                turn.Text = "Red's turn";
            }
            if (board.winner != null) {
                turn.Text = "Game over";
            }
        }
        private async Task UpdateBoard() {
            UpdateTurnLabel();
            foreach (Control c in BoardLayout.Controls) {
                if (c is Panel) {
                    var pos = BoardLayout.GetPositionFromControl(c);
                    var pic = (PictureBox)c.Controls.Find("pic", true)[0];
                    if (board.fields[pos.Column, pos.Row].val == null) {
                        pic.Image = null;
                    } else if (board.fields[pos.Column, pos.Row].val.color == Color.Black && board.fields[pos.Column, pos.Row].val is Pawn) {
                        pic.Image = Resources.black;
                    } else if (board.fields[pos.Column, pos.Row].val.color == Color.White && board.fields[pos.Column, pos.Row].val is Pawn) {
                        pic.Image = Resources.red;
                    } else if (board.fields[pos.Column, pos.Row].val.color == Color.Black && board.fields[pos.Column, pos.Row].val is King) {
                        pic.Image = Resources.blackking;
                    } else {
                        pic.Image = Resources.redking;
                    }
                }
            }
        }
        private async void HandleCellClick(object sender, EventArgs e) {
            Panel p;
            if (sender is PictureBox) {
                p = (Panel)((PictureBox)sender).Parent;
            } else {
                p = (Panel)sender;
            }
            if (selected != null &&
                board.fields[BoardLayout.GetPositionFromControl(selected).Column, BoardLayout.GetPositionFromControl(selected).Row].val != null &&
                board.fields[BoardLayout.GetPositionFromControl(selected).Column, BoardLayout.GetPositionFromControl(selected).Row].val.player == board.playersTurn) {
                var pos = BoardLayout.GetPositionFromControl(selected);
                var fieldPos = BoardLayout.GetPositionFromControl(p);
                Move move = board.fields[pos.Column, pos.Row].val.availableMoves.FirstOrDefault(
                    (a) => a.moveTo.x == fieldPos.Column && a.moveTo.y == fieldPos.Row);
                board.MakeMove(move);
            }
            await DeselectAll();
            selected = p;
            selected.BackColor = SystemColors.Highlight;
            await UpdateBoard();
            await HighlightMoves();
        }
        private async Task Deselect() {
            if (selected == null) {
                return;
            }
            var pos = BoardLayout.GetPositionFromControl(selected);
            if ((pos.Column + pos.Row) % 2 == 0) {
                selected.BackColor = SystemColors.ControlDark;
            } else {
                selected.BackColor = SystemColors.ControlLight;
            }
        }
        private async Task DeselectAll() {
            foreach (Control c in BoardLayout.Controls) {
                var pos = BoardLayout.GetPositionFromControl(c);
                if (c is Panel) {
                    if ((pos.Column + pos.Row) % 2 == 0) {
                        c.BackColor = SystemColors.ControlDark;
                    } else {
                        c.BackColor = SystemColors.ControlLight;
                    }
                }
            }
        }
        private async Task HighlightMoves() {
            if (selected == null) {
                return;
            }
            var pos = BoardLayout.GetPositionFromControl(selected);
            if (board.fields[pos.Column, pos.Row].val != null) {
                var list = board.fields[pos.Column, pos.Row].val.GetAvailableMoves();
                foreach (var move in list) {
                    var x = move.moveTo.x;
                    var y = move.moveTo.y;
                    var c = BoardLayout.GetControlFromPosition(x, y);
                    c.BackColor = SystemColors.Highlight;
                }
            }
        }
    }
}
